using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows;

namespace VGLabsFoundation
{
    public static class Applications
    {

        public static bool OperationHasClearance(string extractPath)
        {
            try
            {
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, extractPath);
                permission.Demand();
                return true;
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("Application has insufficient permissions for file operations: {0}", ex.Message);
                return false;
            }
        }

        public static bool LaunchApp(string appPath)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(appPath, UriKind.Absolute))
                {
                    if (!File.Exists(appPath))
                    {
                        return false;
                    }
                }

                return (System.Diagnostics.Process.Start(appPath) != null);
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error launching process or application: {0}", ex.Message);
                ex.LogError("Error launching process or application: " + appPath);
                return false;
            }
        }

        public static void CloseApp()
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        public static bool UpdateCleanup(string updateFolderPath)
        {
            try
            {
                if (Extraction.ExtractFolderExists(updateFolderPath) && OperationHasClearance(updateFolderPath))
                {
                    Directory.Delete(updateFolderPath, true);
                    if (!Extraction.ExtractFolderExists(updateFolderPath))
                    {
                        return true;
                    }
                    else
                    {
                        Exceptions.LogMessage("UNKNOWN Error deleting completed update temp folder(s): " + updateFolderPath);
                        return false;
                    }
                }
                return false;
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error deleting completed update temp folder(s): {0}", ex.Message);
                ex.LogError("Error deleting completed update temp folder(s) at path: " + updateFolderPath);
                return false;
            }
        }

        public static bool ApplyUpdate(string updatePackagePath, string appPath, string appName)
        {
            try
            {
                // Verify the previous application version file does not already exist at path, delete if exists
                if (Extraction.ExtractFileExists(appPath))
                {
                    if (!Extraction.ExtractFileDeleted(appPath))
                    {
                        Exceptions.LogMessage("Error in Delete Previous Application Version at: " + appPath);
                        return false;
                    }
                }

                // Folder Path to previous version app location
                string appFolderPath = System.IO.Path.GetDirectoryName(appPath);

                // Remove the old README at app final location if exists
                string newReadmePath = System.IO.Path.GetDirectoryName(updatePackagePath) + @"\temp\README.md";
                string readmeTarget = System.IO.Path.Combine(appFolderPath, "README.md");
                if (Extraction.ExtractFileExists(readmeTarget))
                {
                    if (!Extraction.ExtractFileDeleted(readmeTarget))
                    {
                        Exceptions.LogMessage("Error in Delete Previous Application README at: " + readmeTarget);
                        return false;
                    }
                }

                // Remove the old LICENSE at app final location if exists
                string newLicensePath = System.IO.Path.GetDirectoryName(updatePackagePath) + @"\temp\LICENSE.txt";
                string licenceTarget = System.IO.Path.Combine(appFolderPath, "LICENSE.txt");
                if (Extraction.ExtractFileExists(licenceTarget))
                {
                    if (!Extraction.ExtractFileDeleted(licenceTarget))
                    {
                        Exceptions.LogMessage("Error in Delete Previous Application LICENSE at: " + licenceTarget);
                        return false;
                    }
                }

                // Remove the old CHANGELOG at app final location if exists
                string newChangelogPath = System.IO.Path.GetDirectoryName(updatePackagePath) + @"\temp\CHANGELOG.txt";
                string changelogTarget = System.IO.Path.Combine(appFolderPath, "CHANGELOG.txt");
                if (Extraction.ExtractFileExists(changelogTarget))
                {
                    if (!Extraction.ExtractFileDeleted(changelogTarget))
                    {
                        Exceptions.LogMessage("Error in Delete Previous Application CHANGELOG at: " + changelogTarget);
                        return false;
                    }
                }


                // Using already valid updated instance from update\temp folder, re-extract package to old app location
                if (!Extraction.ExtractZip(updatePackagePath, appFolderPath))
                {
                    Exceptions.LogMessage("Error in Final Extract Zip Package Contents to App Path: " + appFolderPath);
                    return false;
                }

                // Double-Redundancy Validation of Final Extracted Update against already validated current running updated app instance
                string validUpdatePath = System.IO.Path.GetDirectoryName(updatePackagePath) + @"\temp\" + appName + ".exe";
                if (!Authentication.AuthenticUpdate(appPath, validUpdatePath))
                {
                    Exceptions.LogMessage("Error in Authenticate Final Extracted Updated Application at App Path: " + appPath);
                    return false;
                }

                // Returning true will exit this instance of the app, should provide users a pop-up: "App updated - restart app now!"
                return true;

            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error applying final update: {0}", ex.Message);
                ex.LogError("Error applying final update");
                return false;
            }
        }


    }
}
