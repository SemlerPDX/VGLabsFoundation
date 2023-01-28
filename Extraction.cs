using System;
using System.IO;
using System.IO.Compression;

namespace VGLabsFoundation
{
    public static class Extraction
    {
        public static bool ExtractZip(string zipPath, string extractPath)
        {
            try
            {
                if (!ExtractFileExists(zipPath))
                    return false;

                if (!ExtractFolderExists(extractPath))
                    Directory.CreateDirectory(extractPath);

                if (!ExtractFolderExists(extractPath))
                    return false;

                if (!Applications.OperationHasClearance(extractPath))
                    return false;

                using (var archive = ZipFile.OpenRead(zipPath))
                {
                    archive.ExtractToDirectory(extractPath);
                }
                return true;
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error extracting zip file: {0}", ex.Message);
                ex.LogError("Error extracting zip file '" + zipPath + "' to folder at: " + extractPath);
                return false;
            }
        }

        public static bool ExtractFileExists(string filePath)
        {
            try
            {
                // Check if the file path exists
                if (!File.Exists(filePath))
                    return false;

                return true;
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error validating file path: {0}", ex.Message);
                ex.LogError("Error validating file path: " + filePath);
                return false;
            }
        }

        public static bool ExtractFolderExists(string folderPath)
        {
            try
            {
                // Check if the folder path exists
                if (!Directory.Exists(folderPath))
                {
                    return false;
                }

                return true;
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error validating folder path: {0}", ex.Message);
                ex.LogError("Error validating folder path: " + folderPath);
                return false;
            }
        }

        public static bool ExtractFileDeleted(string filePath)
        {
            try
            {
                if (ExtractFileExists(filePath) && Applications.OperationHasClearance(filePath))
                {
                    File.Delete(filePath);
                    if (ExtractFileExists(filePath))
                        return false;

                    return true;
                }
                return false;
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error deleting existing update file(s): {0}", ex.Message);
                ex.LogError("Error deleting existing update file(s) at path: " + filePath);
                return false;
            }
        }

        public static bool ExtractFolderDeleted(string folderPath)
        {
            try
            {
                if (ExtractFolderExists(folderPath) && Applications.OperationHasClearance(folderPath))
                {
                    Directory.Delete(folderPath, true);
                    if (ExtractFolderExists(folderPath))
                        return false;

                    return true;
                }
                return false;
            }
            catch (Exceptions ex)
            {
                Console.WriteLine("Error deleting existing update folder(s): {0}", ex.Message);
                ex.LogError("Error deleting existing update folder(s) at path: " + folderPath);
                return false;
            }
        }

    }
}


