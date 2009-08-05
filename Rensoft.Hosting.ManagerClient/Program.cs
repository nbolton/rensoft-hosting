using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Rensoft.Hosting.ManagerClient
{
    static class Program
    {
        const string eventLogSource = "Rensoft Hosting Manager";
        const string eventLogName = "Rensoft Hosting";

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
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
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
            Exception ex = (Exception)e.ExceptionObject;
            
            writeEventLog(getMessageRecursive(ex, true));

            MessageBox.Show(
                "An application error occured (and was unhandled). " +
                "Full details recorded to event log.\r\n\r\n" +
                getMessageRecursive(ex, false),
                "Error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            
            writeEventLog(getMessageRecursive(ex, true));

            MessageBox.Show(
                "An application error occured (on non-UI thread). " +
                "Full details recorded to event log.\r\n\r\n" +
                getMessageRecursive(ex, false),
                "Error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void writeEventLog(string message)
        {
            try
            {
                if (!EventLog.SourceExists(eventLogSource))
                {
                    EventLog.CreateEventSource(eventLogSource, eventLogName);
                }

                EventLog.WriteEntry(eventLogSource, message, EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not write to event log.\n\n" + ex.Message,
                    "Event Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static string getMessageRecursive(Exception ex, bool stackTrace)
        {
            List<string> messageList = new List<string>();
            Exception current = ex;

            while (current != null)
            {
                messageList.Add(current.Message);

                if (stackTrace)
                {
                    messageList.Add(current.StackTrace);
                }

                current = current.InnerException;
            }

            return string.Join("\r\n\r\n", messageList.ToArray());
        }
    }
}
