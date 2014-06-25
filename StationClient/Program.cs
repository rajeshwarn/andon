using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StationClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //x Application.Run(new BlackScreen1920());

            Application.Run(new BlackScreen());
        }
    }
}
