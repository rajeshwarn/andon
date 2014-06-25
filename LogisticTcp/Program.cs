using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LogisticTcp
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
            //Application.Run(new LogisticScreen());

            Application.Run(new LogisticScreenScalable(new System.Drawing.Size(1600, 900)));
        }
    }
}
