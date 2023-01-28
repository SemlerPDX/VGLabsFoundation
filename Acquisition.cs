using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace VGLabsFoundation
{
    public class Acquisition
    {
        public class VersionData
        {
            public string version { get; set; }
            public string featuredChange { get; set; }
            public string fileHash256 { get; set; }
            public string fileHash512 { get; set; }
            public string zipPackageHash256 { get; set; }
            public string zipPackageHash512 { get; set; }
            public string downloadLink { get; set; }
            public string changelogLink { get; set; }
            public string repositoryLink { get; set; }
        }

        public class ApplicationData
        {
            public string appName { get; set; }
            public VersionData[] versions { get; set; }
        }

        public static string[] GetApplicationData(string appNameToGet, string currentVersion)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    var json = client.DownloadString("https://example-link.com/database.json"); // CHANGE TO URL OF DATABASE JSON!!
                    var appDatas = JsonConvert.DeserializeObject<List<ApplicationData>>(json);
                    var appData = appDatas.Where(a => a.appName == appNameToGet).FirstOrDefault();
                    if (appData != null)
                    {
                        // Check Database for appData matching currentVersion of this Application
                        var matchingVersion = appData.versions.Where(version => version.version == currentVersion).FirstOrDefault();
                        if (matchingVersion != null)
                        {
                            string[] currentApplicationData = {
                                matchingVersion.version.ToString(),
                                matchingVersion.featuredChange,
                                matchingVersion.fileHash256,
                                matchingVersion.fileHash512,
                                matchingVersion.zipPackageHash256,
                                matchingVersion.zipPackageHash512,
                                matchingVersion.downloadLink,
                                matchingVersion.changelogLink,
                                matchingVersion.repositoryLink
                            };
                            return currentApplicationData;
                        }
                    }

                    return null;
                }
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error acquiring application data: {0}", ex.Message);
                ex.LogError("Error acquiring application data for v" + currentVersion);
                return null;
            }
        }


        public static string[] CheckForUpdates(string appNameToCheck, string currentVersion)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    var json = client.DownloadString("https://example-link.com/database.json"); // CHANGE TO URL OF DATABASE JSON!!
                    var appDatas = JsonConvert.DeserializeObject<List<ApplicationData>>(json);
                    var appData = appDatas.Where(a => a.appName == appNameToCheck).FirstOrDefault();
                    if (appData != null)
                    {
                        var latestVersion = appData.versions.OrderByDescending(version => version.version).First();
                        if (latestVersion.version.CompareTo(currentVersion) > 0)
                        {
                            string[] updateData = {
                                latestVersion.version.ToString(),
                                latestVersion.featuredChange,
                                latestVersion.fileHash256,
                                latestVersion.fileHash512,
                                latestVersion.zipPackageHash256,
                                latestVersion.zipPackageHash512,
                                latestVersion.downloadLink,
                                latestVersion.changelogLink,
                                latestVersion.repositoryLink
                            };
                            return updateData;
                        }
                    }

                    return null;
                }
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error checking for updates: {0}", ex.Message);
                ex.LogError("Error checking for updates data");
                return null;
            }
        }


        private static bool DownloadPackage(string downloadPath, string downloadLink, string appName, string updateVersion)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(downloadLink, downloadPath + @"\" + appName + "_v" + updateVersion.Replace(".", "_") + ".zip");
                }
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.NameResolutionFailure ||
                    ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    Exceptions.LogMessage("Update URL error or Application has insufficient internet permissions: " + ex.Message);
                }
                return false;
            }
        }


        public static bool AcquireUpdatePackage(string downloadPath, string appName, string[] downloadData, bool openChangelog)
        {
            // downloadData array contents:
            // [version, featured change, file hash 256, file hash 512, zip hash 256, zip hash 512, zip download link, changelog link, github link]
            // Zip files will have the following example format:  MouseMasterVR_v0_9_1_0.zip with contents MouseMasterVR.exe, LICENSE.txt, README.md
            try
            {
                string errorMessageBase = "UPDATE TERMINATED - Failure at Update Acquisition ";
                string downloadPathTemp = downloadPath + @"\temp";
                string downloadPathFolder = System.IO.Path.GetDirectoryName(downloadPath);

                // Open default web browser to changelog URL from appData
                if (openChangelog)
                    _ = Applications.LaunchApp(downloadData[7]);

                //===========Prepare Download Path=====================

                // Verify the application has permission to create files at update folder path
                if (Extraction.ExtractFolderExists(downloadPathFolder))
                {
                    if (!Applications.OperationHasClearance(downloadPathFolder))
                    {
                        Exceptions.LogMessage(errorMessageBase + " in Download Path write permissions at: " + downloadPathFolder);
                        return false;
                    }
                }

                // Verify the download path exists, create if not, return false on failure for this and any check below
                if (!Extraction.ExtractFolderExists(downloadPath))
                {
                    // Create the folder and check that it was successful
                    Directory.CreateDirectory(downloadPath);
                    if (!Extraction.ExtractFolderExists(downloadPath))
                    {
                        Exceptions.LogMessage(errorMessageBase + "in Create Download Folder Path at: " + downloadPath);
                        return false;
                    }
                }

                // Double-Redundancy Check application has permission to create files at newly created update folder path
                if (!Applications.OperationHasClearance(downloadPath))
                {
                    Exceptions.LogMessage(errorMessageBase + " in Download Path write permissions at: " + downloadPath);
                    return false;
                }

                //===========Prepare Temp Path=====================

                // Verify the application has permission to create files at update temp folder path
                if (Extraction.ExtractFolderExists(downloadPathFolder))
                {
                    if (!Applications.OperationHasClearance(System.IO.Path.GetDirectoryName(downloadPathTemp)))
                    {
                        Exceptions.LogMessage(errorMessageBase + " in Download Temp Path write permissions at: " + downloadPathTemp);
                        return false;
                    }
                }

                // Verify the download temp folder path exists, create if not
                if (!Extraction.ExtractFolderExists(downloadPathTemp))
                {
                    // Create the temp folder and check that it was successful
                    Directory.CreateDirectory(downloadPathTemp);
                    if (!Extraction.ExtractFolderExists(downloadPathTemp))
                    {
                        Exceptions.LogMessage(errorMessageBase + "in Create Temp Download Folder Path at: " + downloadPathTemp);
                        return false;
                    }
                }

                // Double-Redundancy Check application has permission to create files at newly created update folder path
                if (!Applications.OperationHasClearance(downloadPathTemp))
                {
                    Exceptions.LogMessage(errorMessageBase + " in Download Path write permissions at: " + downloadPathTemp);
                    return false;
                }

                //===========Prepare File Target Paths=====================

                // Verify the download file package does not already exist at path, delete if exists
                string downloadPackagePath = downloadPath + @"\" + appName + "_v" + downloadData[0].Replace(".", "_") + ".zip";
                if (Extraction.ExtractFileExists(downloadPackagePath))
                {
                    if (!Extraction.ExtractFileDeleted(downloadPackagePath))
                    {
                        Exceptions.LogMessage(errorMessageBase + "in Delete Existing Download Package at: " + downloadPackagePath);
                        return false;
                    }
                }

                // Verify the download file .exe does not already exist at update temp path, delete if exists
                string updateTempFilePath = downloadPathTemp + @"\" + appName + ".exe";
                if (Extraction.ExtractFileExists(updateTempFilePath))
                {
                    File.Delete(updateTempFilePath);
                    if (Extraction.ExtractFileExists(updateTempFilePath))
                    {
                        Exceptions.LogMessage(errorMessageBase + "in Delete Existing Temp Download File at: " + updateTempFilePath);
                        return false;
                    }
                }

                //===========Acquire and Authenticate Update Package=====================

                // Download file from download link in downloadData[6] to downloadPath
                if (!DownloadPackage(downloadPath, downloadData[6], appName, downloadData[0]))
                {
                    Exceptions.LogMessage(errorMessageBase + "in Download Package from VG website at URL: " + downloadData[6]);
                    return false;
                }
                // Verify the new download file package exists at path
                if (!Extraction.ExtractFileExists(downloadPackagePath))
                {
                    Exceptions.LogMessage(errorMessageBase + "in Validate New Update File Path at: " + downloadPackagePath);
                    return false;
                }

                // Perform File Hash Checks on package, return false on failure
                if (!Authentication.AuthenticPackage(downloadData, downloadPackagePath))
                {
                    Exceptions.LogMessage(errorMessageBase + "in Authenticate New Update Package at: " + downloadPackagePath);
                    return false;
                }

                // Extract file from package to temp folder downloadPath
                if (!Extraction.ExtractZip(downloadPackagePath, downloadPathTemp))
                {
                    Exceptions.LogMessage(errorMessageBase + "in Extract Zip Package Contents to: " + downloadPathTemp);
                    return false;
                }
                // Perform File Hash Checks on extracted application, return false on failure
                if (!Authentication.AuthenticApplication(downloadData, updateTempFilePath))
                {
                    Exceptions.LogMessage(errorMessageBase + "in Authenticate New Update Package at: " + downloadPackagePath);
                    return false;
                }

                // Returning TRUE to signal launch of authenticated updated app from 'temp' & close this app instance to handoff
                return true;

            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error in main Acquisition of Update Package: " + ex.Message);
                ex.LogError("Error in main Acquisition of Update Package");
                return false;
            }
        }

    }
}
