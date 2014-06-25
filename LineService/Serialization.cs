using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace LineService
{
    class Serialization
    {

        private string path;
        //public Serialization()
        //{
        //    this.path = "serialization_user.dat";
        //}

        public Serialization(string path)
        {
            this.path = path;
        }

        public void Backup(object myObj)
        {
            BinaryFormatter BinFormat = new BinaryFormatter();
            using (Stream fStream = new FileStream(this.path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinFormat.Serialize(fStream, myObj);
            }
        }

        public T Restore<T>(T myObj)
        {
            BinaryFormatter BinFormat = new BinaryFormatter();
            if (File.Exists(this.path))
            {
                using (FileStream reStream = new FileStream(this.path, FileMode.Open))
                {
                    myObj = (T)BinFormat.Deserialize(reStream);
                }
            }
            return myObj;
        }

    }
}
