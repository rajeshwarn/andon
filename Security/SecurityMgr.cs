using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Principal;
using System.Security.Cryptography;
using System.Xml;
using System.Threading;
using Security.DetroitTableAdapters;

using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.Windows.Forms;
using AppLog;

namespace Security
{
    public enum FormAssessMode { NoAccess = 0, Read = 1, Write = 2 }

    public class FormSecurityModule 
    {
        string readAccessKey = "_R";
        string writeAccessKey = "_W";
        private SecurityMgr sm = new SecurityMgr();
        public FormAssessMode AccessMode = FormAssessMode.NoAccess;
        public FormSecurityModule() 
        {
            // någonting ...
        }
        public void CheckFormAccess(string formName) 
        {
            this.readAccessKey = formName + readAccessKey;
            this.writeAccessKey = formName + writeAccessKey;
            if (sm.CheckPermissions(writeAccessKey))
            {
                //MessageBox.Show("Permission \"" + writeAccessKey + "\" for user: " + User.Name + " - Ok.");
                this.AccessMode = FormAssessMode.Write;
            }
            else if (sm.CheckPermissions(readAccessKey))
            {
                //MessageBox.Show("Permission \"" + readAccessKey + "\" for user: " + User.Name + " - Ok.");
                this.AccessMode = FormAssessMode.Read;
            }
            else
            {
                throw new UnauthorizedAccessException("Permission \"" + readAccessKey + "\" for user: "
                    + User.Name + " - has not been granted."
                    + "\n Please, contact administrator to check your profile.");
            }        
        }
    }


    public class User 
    {
        public static string Name = "NA";
        public static string Password = "";
        public static Hashtable Permissions = new Hashtable();
    }


    public class SecurityMgr
    {
        //private User user;
        private Detroit detroit;
        private UsersTableAdapter usersTableAdapter;
        private UserPermissionsTableAdapter userPermissionsTableAdapter;
        private PermissionsTableAdapter permissionsTableAdapter;
        private LogProvider logProvider;


        // In current realization of SecurityMgr there's no need in constructor cause we're assigning all user info to the current thread.
        // Hence static methods may be used.
        public SecurityMgr()
        {
        }

        private bool InitDataset(string userName, string password) 
        {
            bool result = false;

            System.Data.SqlClient.SqlConnectionStringBuilder builder =
                new System.Data.SqlClient.SqlConnectionStringBuilder(Properties.Settings.Default.DetroitConnectionString);
            builder.UserID = userName;
            builder.Password = password;
            builder.IntegratedSecurity = false;
            Security.Properties.Settings.ChangeConnectionString(builder.ConnectionString);


            this.logProvider.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Security", "builder.ConnectionString = " + builder.ConnectionString);
            this.logProvider.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Security", "DetroitConnectionString = " + Properties.Settings.Default.DetroitConnectionString.ToString());



            // test connection ?!
            //
            try
            {
                string cnStr = builder.ConnectionString;
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = cnStr;
                cn.Open();

                this.detroit = new Detroit();
                this.usersTableAdapter = new UsersTableAdapter();
                this.permissionsTableAdapter = new PermissionsTableAdapter();
                this.userPermissionsTableAdapter = new UserPermissionsTableAdapter();

                result = true;
                return result;
            }
            catch (SqlException ex)
            {
                this.logProvider.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Security", "There is no access to SQL database.");
                //MessageBox.Show("There is no access to SQL database. User \"" + userName + "\". \n Please, try again.", "SQL connecting ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //
        }

        public SecurityMgr(LogProvider logProvider) : this()
        {
            this.logProvider = logProvider;
        }


        public void CreateNewUser(string userName, string userPassword, string salt) 
        {
            string hashPassword = this.ComputePasswordHash(userPassword, salt);

            int userRows = Convert.ToInt32(this.usersTableAdapter.CheckUserName(userName));
            if( userRows == 0 ) 
            {
                this.usersTableAdapter.Insert(userName, hashPassword, salt);
                this.usersTableAdapter.NewUser(userName, userPassword);
            } 
            else 
            {
                throw new System.UnauthorizedAccessException("User with name \"" + userName +  "\" already exists.");
            }
        }
        public bool LogonUser(string userName, string userPassword) 
        {
            bool result = false;

            if (this.InitDataset(userName, userPassword))
            {
                this.usersTableAdapter.FillBy(this.detroit.Users, userName);
                if (this.detroit.Users.Rows.Count == 1)
                {
                    DataRow row = this.detroit.Users.Rows[0];
                    string hashPassword = this.ComputePasswordHash(userPassword, row["Salt"].ToString());
                    string userPasswordFromDatabase = row["Password"].ToString();

                    if (hashPassword == userPasswordFromDatabase)
                    {
                        result = true;
                        User.Name = userName;
                        User.Password = userPassword;
                        this.fillUserPermissions(User.Permissions, User.Name);
                    }
                }
            }
            return result;
        }

        private void fillUserPermissions(Hashtable permissions, string userName)
        {
            permissions.Clear();
            this.permissionsTableAdapter.FillBy(this.detroit.Permissions, userName);
            foreach(DataRow permissionRow in this.detroit.Permissions.Rows) 
            {
                permissions.Add(permissionRow["PermissionKey"], 1);
            }
        }
        public void ReloadUserPermissions() 
        {
            this.fillUserPermissions(User.Permissions, User.Name);
        }

        public bool CheckPermissions(string permissionKey)
        {
            // Here we're using IsInRole method of built-in IPrincipal interface
            //if (Thread.CurrentPrincipal.IsInRole(permissionKey) == true)
            //    return true;
            //return false;

            if (User.Permissions.Contains(permissionKey) == true)
                return true;
            return false;
        }
        public string[] GetPermissionsList() 
        {
            string[] result;
            this.permissionsTableAdapter.Fill(this.detroit.Permissions);
            OrderedEnumerableRowCollection<Detroit.PermissionsRow> rowCollection = this.detroit.Permissions.OrderBy(p => p.PermissionKey);
            result = new string[this.detroit.Permissions.Rows.Count];
            int i = 0;
            foreach (DataRow row in rowCollection) //this.detroit.Permissions.Rows) 
            {
                result[i] = row["PermissionKey"].ToString();
                i++;
            }

            result.OrderBy(p => p.ToString());

            return result;
        }
        public Hashtable GetUserPermissions(string userName) 
        {
            Hashtable result = new Hashtable();
            this.fillUserPermissions(result, userName);
            return result;
        }
        public void SetPermitions(string userName, string[] permissions) 
        { 
            Hashtable oldPermissions = new Hashtable();
            this.fillUserPermissions(oldPermissions, userName);

            for (int i = 0; i < permissions.Count(); i++) 
            {
                if (!oldPermissions.ContainsKey(permissions[i])) 
                {
                    this.userPermissionsTableAdapter.SetUserPermissions(userName, permissions[i].ToString()); 
                }
            }

           
            string[] oldPermissionsArray = new string[oldPermissions.Count];
            oldPermissions.Keys.CopyTo(oldPermissionsArray, 0);
            for (int j = 0; j < oldPermissionsArray.Count(); j++) 
            { 
                if (permissions.FirstOrDefault(p => p.Equals(oldPermissionsArray[j])) == null) 
                {
                    this.userPermissionsTableAdapter.RevokeUserPermissions(userName, oldPermissionsArray[j].ToString()); 
                }
            }

        }

        public string GetCurrentUserName()
        {
            // Here we're using the Name prorerty of built-in IPrincipal interface.
            // When there's no user online, method returns empty string.
            //return Thread.CurrentPrincipal.Identity.Name;
            return User.Name;
        }

        public void LanchNewUserDlg() 
        { 
            NewUser newUser = new NewUser(this);
            newUser.Show();
        }
        public bool LanchLogonDlg() 
        {
            bool result = false;
            bool closeDialog = false;
            string userName = "";
  
            while (!closeDialog) 
            {
                Logon logon = new Logon(this);
                logon.Controls["tbUserName"].Text = userName;
                logon.StartPosition = FormStartPosition.CenterParent;

                if (logon.ShowDialog() == DialogResult.OK)
                {
                    userName = logon.Controls["tbUserName"].Text;
                    result = this.LogonUser(logon.Controls["tbUserName"].Text, logon.Controls["tbUserPassword"].Text);
                    if (result)
                    {
                        closeDialog = true;
                    }
                    else
                    {
                        MessageBox.Show("Login failed! Please, try again.");
                        //this.logProvider.LogAlert(0, "", "Application", this.GetType().ToString(), "User login failed."
                        //    + "User \"" + logon.Controls["tbUserName"].Text + "\"", User.Name);  
                    }
                }
                else 
                {
                    closeDialog = true;
                };
                
            }

            return result;
        }
        public void LanchUserPermissionsDlg() 
        {
            UserPermitions upForm = new UserPermitions(this);
            upForm.Show();        
        }



         // Inner method for converting password to SHA1 hash.
        // Here we're using additional bytes - "salt" - for safety reasons.
        // In db we store hash-code created from particular password with particular salt added. 
        // Salt value is stored in db near hash-code.
        private string ComputePasswordHash(string password, string salt)
        {
            // Getting the byte form of input
            byte[] passwordBytes = UTF8Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = UTF8Encoding.UTF8.GetBytes(salt);

            // Bytes' addition and creating SHA1 instance
            byte[] preHashed = new byte[saltBytes.Length + passwordBytes.Length];
            System.Buffer.BlockCopy(passwordBytes, 0, preHashed, 0, passwordBytes.Length);
            System.Buffer.BlockCopy(saltBytes, 0, preHashed, passwordBytes.Length, saltBytes.Length);
            SHA1 sha1 = SHA1.Create();

            // We want to treat our computed hashcodes as strings
            return BitConverter.ToString(sha1.ComputeHash(preHashed));
        }
        // Inner method for comparing hascodes while logging on.
        // Salt and hash come from db.
        private bool IsPasswordValid(string passwordToValidate, string salt, string correctPasswordHash)
        {

            // Avoiding null input of string salt.
            if (salt == null)
                throw new ArgumentException("Ошибка базы данных при чтении параметра salt");
            string hashedPassword = ComputePasswordHash(passwordToValidate, salt);
            if (hashedPassword == correctPasswordHash)
                return true;
            return false;
        }


    }

}
