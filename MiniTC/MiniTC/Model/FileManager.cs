using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.Model
{
    using System.IO;
    static class FileManager
    {
        // MESSAGES
        private const string ERR_DISK_CONN = "Disk error (check if your disk is connected)!";
        private const string ERR_FREE_SPACE = "Out of disk space!";
        private const string ERR_FILE_EXISTS = "Such a file already exists!";
        private const string ERR_DIR_EXISTS = "Such a directory already exists!";
        private const string ERR_SEL_FILE = "There is no selected file!";
        private const string ERR_DEST_FILE = "Path error(check your destination path)!";

        public static void Copy(AFile selectedFile, DirectoryObj destination)
        {
            if (selectedFile is null) throw new Exception(ERR_SEL_FILE);
            if (destination is null) throw new Exception(ERR_DEST_FILE);

            string sourceFileName = selectedFile.Name;
            string sourceFilePath = selectedFile.Path;
            string destinationDir = destination.Path;
            string destinationPath = $@"{destinationDir}\{sourceFileName}";
            var freeSpace = GetFreeSpaceFromPath(destinationDir);

            // disk error - not connected
            if (freeSpace is null) throw new Exception(ERR_DISK_CONN);

            // if selectedFile is a File 
            if (selectedFile is FileObj srcFile)
            {
                // checking if file exists in destination path
                if (File.Exists(destinationPath))
                    throw new Exception(ERR_FILE_EXISTS);

                // if there is no space 
                if (freeSpace < srcFile.FileInfo.Length)
                    throw new Exception(ERR_FREE_SPACE);

                // copying
                File.Copy(sourceFilePath, destinationPath);
            }

            // if selectedFile is a Directory
            if (selectedFile is DirectoryObj)
            {
                // checking if directory exists in destination path
                if (Directory.Exists(destinationPath))
                    throw new Exception(ERR_DIR_EXISTS);

                // getting space from all files
                long totalSpace = 0;
                foreach (string filePath in Directory.GetFiles(sourceFilePath, "*.*", SearchOption.AllDirectories))
                    totalSpace += new FileInfo(filePath).Length;

                // if there is no space 
                if (freeSpace < totalSpace)
                    throw new Exception(ERR_FREE_SPACE);

                // at first, creating destination directory
                Directory.CreateDirectory(destinationPath);
                // copying - recursively (first all directories, next all files)
                foreach (string dirPath in Directory.GetDirectories(sourceFilePath, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourceFilePath, destinationPath));
                foreach (string filePath in Directory.GetFiles(sourceFilePath, "*.*", SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(sourceFilePath, destinationPath), true);
            }
        }

        private static long? GetFreeSpaceFromPath(string path)
        {
            var pathDrive = Path.GetPathRoot(path).ToUpper();
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
                if (drive.Name == pathDrive)
                    return drive.AvailableFreeSpace;
            return null;
        }
    }
}
