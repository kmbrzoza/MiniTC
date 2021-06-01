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
        private const string ERR_ACCESS_DENIED = "Access denied!";
        private const string ERR_NAME_NOT_DEF = "Name of file not defined!";

        //METHODS
        #region
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

        public static void Delete(AFile selectedFile)
        {
            if (selectedFile is null) throw new Exception(ERR_SEL_FILE);

            if (selectedFile is FileObj srcFile)
            {
                var filePath = srcFile.Path;
                if (!File.Exists(filePath)) throw new Exception(ERR_SEL_FILE);
                File.Delete(filePath);
            }

            if (selectedFile is DirectoryObj srcDir)
            {
                var dirPath = srcDir.Path;
                if(!Directory.Exists(dirPath)) throw new Exception(ERR_SEL_FILE);

                Directory.Delete(dirPath, true);
            }
        }

        public static void Move(AFile selectedFile, DirectoryObj destination)
        {
            if (selectedFile is null) throw new Exception(ERR_SEL_FILE);
            if (destination is null) throw new Exception(ERR_DEST_FILE);

            string sourceFileName = selectedFile.Name;
            string sourceFilePath = selectedFile.Path;
            string destinationDir = destination.Path;
            string destinationPath = $@"{destinationDir}\{sourceFileName}";

            var freeSpace = GetFreeSpaceFromPath(destinationDir);

            if (freeSpace is null) throw new Exception(ERR_DISK_CONN);

            if (selectedFile is FileObj srcFile)
            {
                if (File.Exists(destinationPath))
                    throw new Exception(ERR_FILE_EXISTS);

                if (freeSpace < srcFile.FileInfo.Length)
                    throw new Exception(ERR_FREE_SPACE);

                File.Move(sourceFilePath, destinationPath);
            }

            if (selectedFile is DirectoryObj)
            {
                if (Directory.Exists(destinationPath))
                    throw new Exception(ERR_DIR_EXISTS);

                long totalSpace = 0;
                foreach (string filePath in Directory.GetFiles(sourceFilePath, "*.*", SearchOption.AllDirectories))
                    totalSpace += new FileInfo(filePath).Length;

                if (freeSpace < totalSpace)
                    throw new Exception(ERR_FREE_SPACE);

                Directory.Move(sourceFilePath, destinationPath);
            }
        }

        public static void Rename(AFile selectedFile, string newName)
        {
            if (selectedFile is null) throw new Exception(ERR_SEL_FILE);
            if (newName is null) throw new Exception(ERR_NAME_NOT_DEF);

            string sourceFilePath = selectedFile.Path;
            string parentPath = GetParentPath(sourceFilePath);
            string destinationPath = $@"{parentPath}\{newName}";

            if (selectedFile is FileObj srcFile)
            {
                // adding extension to file
                destinationPath += srcFile.FileInfo.Extension;

                if (File.Exists(destinationPath))
                    throw new Exception(ERR_FILE_EXISTS);

                // Rename
                File.Move(sourceFilePath, destinationPath);
            }

            if (selectedFile is DirectoryObj)
            {
                if (Directory.Exists(destinationPath))
                    throw new Exception(ERR_DIR_EXISTS);

                Directory.Move(sourceFilePath, destinationPath);
            }
        }

        public static void CreateDirectory(DirectoryObj destination, string nameDir)
        {
            if (destination is null) throw new Exception(ERR_DEST_FILE);
            if (nameDir is null) throw new Exception(ERR_NAME_NOT_DEF);

            string destinationPath = $@"{destination.Path}\{nameDir}";

            if (Directory.Exists(destinationPath))
                throw new Exception(ERR_DIR_EXISTS);

            Directory.CreateDirectory(destinationPath);
        }

        public static List<AFile> GetFilesByPath(string path)
        {
            List<AFile> Files = new List<AFile>();
            string[] dirs = null;
            string[] fils = null;

            // if path don't exists, return empty list
            if (Directory.Exists(path))
            {
                try
                {
                    dirs = Directory.GetDirectories(path);
                    fils = Directory.GetFiles(path);
                }
                catch (Exception)
                {
                    // if access denied
                    throw new Exception(ERR_ACCESS_DENIED);
                }
            }

            if(dirs != null)
                foreach (var dir in dirs)
                    Files.Add(DirectoryObj.GetDirectoryObj(dir));

            if(fils != null)
                foreach (var fil in fils)
                    Files.Add(FileObj.GetFileObj(fil));

            return Files;
        }

        public static string[] GetActiveDrives()
        {
            return Directory.GetLogicalDrives();
        }

        public static string GetDriveFromPath(string path)
        {
            string drive = Path.GetPathRoot(path).ToUpper();
            if (drive[drive.Length - 1] != '\\')
                return drive + "\\";
            return drive;
        }

        public static string GetParentPath(string path)
        {
            return Directory.GetParent(path).FullName;
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
        #endregion
    }
}
