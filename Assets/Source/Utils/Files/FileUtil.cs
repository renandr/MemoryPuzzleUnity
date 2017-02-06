using System.IO;

namespace GGS.CakeBox.Utils
{
    public class FileUtil
    {
        /// <summary>
        /// Deletes all files and folders in a folder
        /// </summary>
        /// <param name="path">Path to the folder</param>
        public static void DeleteFilesInFolderRecursively(string path)
        {
            DirectoryInfo destinationPathInfo = new DirectoryInfo(path);
            FileInfo[] filesInDestinationPath = destinationPathInfo.GetFiles();

            foreach (FileInfo file in filesInDestinationPath)
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in destinationPathInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Converts all forward slashes to backwards slashes
        /// </summary>
        /// <param name="filePath">The path to fix</param>
        /// <returns>The fixed path</returns>
        public static string FixSlashesInFilePath(string filePath)
        {
            return filePath.Replace('/', '\\');
        }

        /// <summary>
        /// Combines multiple strings to one path
        /// </summary>
        /// <param name="pathParts">The path parts.</param>
        /// <returns>The combined path</returns>
        public static string MultiPathCombine(params string[] pathParts)
        {
            if (pathParts.Length == 0)
            {
                return "";
            }
            string result = pathParts[0];
            for (int i = 1; i < pathParts.Length; i++)
            {
                result = Path.Combine(result, pathParts[i]);
            }
            return result;
        }
    }
}