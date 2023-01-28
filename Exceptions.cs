using System;
using System.IO;
using System.Windows;

namespace VGLabsFoundation
{
    public class Exceptions : Exception
    {
        /*USAGE_EXAMPLES
        // VG LABS CUSTOM EXCEPTION LOGGING:
        try
        {
            //Your code that may throw exception
        }
        catch (Exceptions ex)
        {
            ex.LogError("Your custom message to appear above ex in errors log plus input: " + exampleOptionalString);
            // -OPTIONAL- Custom Application Exit with error pop-up and log entry of event:
            Exceptions.ExitApplication(1);
        }
        
        // VG LABS CUSTOM MESSAGE LOGGING:
        Exceptions.LogMessage("my error message");
        */


        private const long LOG_MAX_BYTES = 1073741824; // 1GB max log size
        private const string LOG_FILE_NAME = "errors"; // default "errors"

        public Exceptions() : base() { }
        public Exceptions(string message) : base(message) { }
        public Exceptions(string message, Exception inner) : base(message, inner) { }


        public static bool CheckAndRotateErrorsLog(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    if (!Applications.OperationHasClearance(path))
                        return false;

                    FileInfo file = new FileInfo(path);
                    if (file.Length > LOG_MAX_BYTES)
                    {
                        string timestamp = DateTime.Now.ToString("MMddyyyyHHmmss");
                        string newFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), LOG_FILE_NAME + "_" + timestamp + ".log");
                        File.Copy(path, newFilePath);
                        File.Delete(path);
                    }
                    file = null;
                }
                return true;
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error rotating oversized errors log: " + ex.Message);
                return false;
            }
        }


        public static void ExitApplication(int exitCode)
        {
            LogMessage("EXIT CODE(" + exitCode + ") - Application Terminated by Exception. Please report issue!");
            MessageBoxResult result = MessageBox.Show(
                "The application will now safely exit. Please contact support.\n" +
                "\n" +
                "Check errors log in application folder for details.\n",
                System.Reflection.Assembly.GetEntryAssembly().GetName().Name + " is now closing...",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            Application.Current.Shutdown();
            Environment.Exit(exitCode);
        }

        public static void LogMessage(string message)
        {
            try
            {
                string root = AppDomain.CurrentDomain.BaseDirectory;
                string logPath = System.IO.Path.Combine(root, LOG_FILE_NAME + ".log");

                //Ensure errors log is no greater than max size (1GB)
                if (CheckAndRotateErrorsLog(logPath))
                {
                    using (System.IO.StreamWriter log = new System.IO.StreamWriter(logPath, true))
                    {
                        log.WriteLine("================================================");
                        log.WriteLine("================================================");
                        log.WriteLine("Log Entry Date/Time: " + DateTime.Now.ToString());
                        log.WriteLine("Programmer Message: " + message);
                        log.WriteLine();
                    }
                }
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                // Ya know things have gone sideways when Exceptions throw an exception...
                Console.WriteLine("{0} System error at custom exceptions class: {1}", DateTime.Now, ex);
                try
                {
                    MessageBoxResult result = MessageBox.Show(
                        "An application error has been detected!\n" +
                        "\n" +
                        "Unable to log manual exception to file\n" +
                        "\n" +
                        "THIS EXCEPTION:\n" +
                        ex.Message +
                        "\n" +
                        "\n" +
                        "(press OK to copy complete error details to clipboard)" +
                        "\n" +
                        "\n" +
                        "PLEASE CLOSE THE APP AND CONTACT VG SUPPORT!",
                        System.Reflection.Assembly.GetEntryAssembly().GetName().Name + " APPLICATION ERROR!",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Error
                    );
                    if ((int)result == 1)
                        Clipboard.SetText(ex.ToString());
                }
                catch
                {
                    // dispair, for all is lost...
                    ExitApplication(1);
                }
            }
        }

        public void LogError(string notes)
        {
            try
            {
                string root = AppDomain.CurrentDomain.BaseDirectory;
                string logPath = System.IO.Path.Combine(root, LOG_FILE_NAME + ".log");

                //Ensure errors log is no greater than max size (1GB)
                if (CheckAndRotateErrorsLog(logPath))
                {
                    using (System.IO.StreamWriter log = new System.IO.StreamWriter(logPath, true))
                    {
                        log.WriteLine("================================================");
                        log.WriteLine("================================================");
                        log.WriteLine("Log Entry Date/Time: " + DateTime.Now.ToString());
                        log.WriteLine("Programmer Note: " + notes);
                        log.WriteLine("Error: " + this.Message);
                        log.WriteLine("Stack Trace: " + this.StackTrace);
                        if (this.InnerException != null)
                        {
                            log.WriteLine("Inner Exception: " + this.InnerException.Message);
                            log.WriteLine("Inner Exception Stack Trace: " + this.InnerException.StackTrace);
                        }
                        log.WriteLine();
                    }
                }
                Console.WriteLine(notes + ": " + this.Message);
            }
            catch (Exception ex)
            {
                // Ya know things have gone sideways when Exceptions throw an exception...
                Console.WriteLine("{0} System error at custom exceptions class: {1}", DateTime.Now, ex);
                try
                {
                    MessageBoxResult result = MessageBox.Show(
                        "An application error has been detected!\n" +
                        "\n" +
                        "Unable to log another exception to file\n" +
                        "\n" +
                        "THIS EXCEPTION:\n" +
                        ex.Message +
                        "\n" +
                        "\n" +
                        "(press OK to copy complete error details to clipboard)\n" +
                        "\n" +
                        "PLEASE CLOSE THE APP AND CONTACT VG SUPPORT!",
                        System.Reflection.Assembly.GetEntryAssembly().GetName().Name + " APPLICATION ERROR!",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Error
                    );
                    if ((int)result == 1)
                        Clipboard.SetText(ex.ToString());

                }
                catch
                {
                    // dispair, for all is lost...
                    ExitApplication(1);
                }
            }
        }

    }
}


