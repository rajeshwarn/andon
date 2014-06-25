using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Admin
{
    class SecurityMgr2
    {
        public bool CheckPermissions(string permissionKey) 
        {
            bool result = false;
            if (permissionKey.Length > 5)
            {
                result = true;
            }
            else 
            {
                //throw new UnauthorizedAccessException(permissionKey);
            }
            return result;
        }
    }
}
