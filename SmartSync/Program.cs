using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading;
namespace SmartSync
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
            Application.Run(new Form2());

        }



    }


    public static class Helper
    {
        internal class AppHelper
        {
            internal static string AppPath
            {
                get
                {
                    return Application.StartupPath;
                }
            }
        }
            public static void WriteLog(Exception e)
            {
                File.AppendAllText(AppHelper.AppPath + "\\" + "smartsync.log", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine + DateTime.Now.ToShortDateString() + Environment.NewLine + Environment.NewLine);
            }
        }

    }






