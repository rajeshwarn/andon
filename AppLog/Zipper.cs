using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLog
{
    public class Zipper
    {
        public Zipper() {}

        // C# to convert a string to a byte array.
        public byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public string Zip(string value)
        {
            //Transform string into byte[]  
            byte[] byteArray = this.StrToByteArray(value);

            //Prepare for compress
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(
                ms,
                System.IO.Compression.CompressionMode.Compress);

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            //Transform byte[] zip data to string

            // C# to convert a byte array to a string.
            byteArray = ms.ToArray();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string str = enc.GetString(byteArray);
            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return str;
        }

        public string UnZip(string value)
        {
            //Transform string into byte[]
            byte[] byteArray = this.StrToByteArray(value);

            //Prepare for decompress
            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray);
            System.IO.Compression.GZipStream sr = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Decompress);

            //Reset variable to collect uncompressed result
            byteArray = new byte[byteArray.Length];

            //Decompress
            int rByte = sr.Read(byteArray, 0, byteArray.Length);

            //Transform byte[] unzip data to string
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string str = enc.GetString(byteArray);

            sr.Close();
            ms.Close();
            sr.Dispose();
            ms.Dispose();
            return str;
        }
    }
}
