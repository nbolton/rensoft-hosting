using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Rensoft.Hosting.Setup
{
    static class Program
    {
        const string eventLogSource = "Rensoft Hosting Setup";
        const string eventLogName = "Application";

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

            Application.Run(new SetupWizardForm());
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
            if (!EventLog.SourceExists(eventLogSource))
            {
                EventLog.CreateEventSource(eventLogSource, eventLogName);
            }

            EventLog.WriteEntry(eventLogSource, message, EventLogEntryType.Error);
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
