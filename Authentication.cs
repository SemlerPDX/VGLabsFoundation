// VG Labs Foundation Library
// by SemlerPDX Jan2023

namespace VGLabsFoundation
{
    public static class Authentication
    {

        public static bool AuthenticPackage(string[] packageData, string packagePath)
        {
            try
            {
                // Authenticate the update zip package file using valid package data
                if ((packageData != null) && (packagePath != null))
                {
                    string validPackageHash256 = packageData[4];
                    string validPackageHash512 = packageData[5];
                    string updatedPackageHash256 = Encryption.GetSHA256FileHash(packagePath);
                    string updatedPackageHash512 = Encryption.GetSHA512FileHash(packagePath);
                    return (validPackageHash256 == updatedPackageHash256) && (validPackageHash512 == updatedPackageHash512);
                }
                Exceptions.LogMessage("Update Package Authentication Failure due to null inputs");
                return false;
            }
            catch (Exceptions ex)
            {
                // The updated app package could not be authenticated through the above method
                ex.LogError("Error authenticating update package at folder path: " + packagePath);
                return false;
            }
        }

        public static bool AuthenticApplication(string[] applicationData, string applicationPath)
        {
            try
            {
                // Authenticate the extracted binary application file using valid application data
                if ((applicationData != null) && (applicationPath != null))
                {
                    string validApplicationHash256 = applicationData[2];
                    string validApplicationHash512 = applicationData[3];
                    string updatedApplicationHash256 = Encryption.GetSHA256FileHash(applicationPath);
                    string updatedApplicationHash512 = Encryption.GetSHA512FileHash(applicationPath);
                    return (validApplicationHash256 == updatedApplicationHash256) && (validApplicationHash512 == updatedApplicationHash512);
                }
                Exceptions.LogMessage("Application File Authentication Failure due to null inputs");
                return false;
            }
            catch (Exceptions ex)
            {
                // The extracted binary application could not be authenticated through the above method
                ex.LogError("Error authenticating extracted updated application at folder path: " + applicationPath);
                return false;
            }
        }

        public static bool AuthenticUpdate(string updatePath, string applicationPath)
        {
            try
            {
                // Authenticate the final extracted updated binary application file using already validated current app data
                if ((applicationPath != null) && (updatePath != null))
                {
                    string validApplicationHash256 = Encryption.GetSHA256FileHash(applicationPath);
                    string validApplicationHash512 = Encryption.GetSHA512FileHash(applicationPath);
                    string updatedApplicationHash256 = Encryption.GetSHA256FileHash(updatePath);
                    string updatedApplicationHash512 = Encryption.GetSHA512FileHash(updatePath);
                    return (validApplicationHash256 == updatedApplicationHash256) && (validApplicationHash512 == updatedApplicationHash512);
                }
                Exceptions.LogMessage("Application Update File Authentication Failure due to null inputs");
                return false;
            }
            catch (Exceptions ex)
            {
                // The final extracted updated binary application could not be authenticated through the above method
                ex.LogError("Error authenticating final extracted updated application at folder path: " + updatePath);
                return false;
            }
        }

    }
}
