using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Rensoft.Hosting.ManagerClient
{
    static class Program
    {
        private static bool runMainScreen;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if !DEBUG
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif

            // Before running main screen, establish connection.
            ConnectionDialog cd = new ConnectionDialog();
            cd.FormClosing += new FormClosingEventHandler(cd_FormClosing);
            Application.Run(cd);

            if (runMainScreen)
            {
                // Now initialize and run main screen.
                Application.Run(new MainScreen());
            }
        }

        static void cd_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConnectionDialog cd = (ConnectionDialog)sender;
            if (cd.DialogResult == DialogResult.OK)
            {
                LocalContext.Default.ClientContext = cd.CreateClientContext();
                runMainScreen = true;
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new Exception(
                "An error occured outside the windows forms thread.", (Exception)e.ExceptionObject);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            throw new Exception(
                "An error occured outside the windows forms thread.", e.Exception);
        }
    }
}
