using System;
using System.IO;
using System.Windows.Forms;

namespace ServerInfoApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Catch unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            File.WriteAllText("error_log.txt", ex.ToString()); // Log the error
            MessageBox.Show("An unexpected error occurred. Please check error_log.txt for details.", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
