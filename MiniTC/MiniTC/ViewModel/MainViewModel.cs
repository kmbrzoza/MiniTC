using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MiniTC.ViewModel
{
    using BaseClass;
    using System.Windows.Input;
    using Model;
    using System.Windows;

    public class MainViewModel : BaseViewModel
    {
        public PanelTCViewModel Left { get; set; }
        public PanelTCViewModel Right { get; set; }

        // XSelectedFiles allow to unchecked file from another panel
        public int LeftSelectedFile
        {
            get { return Left.SelectedFile; }
            set
            {
                Left.SelectedFile = value;
                Right.SelectedFile = -1;
                onPropertyChanged(nameof(LeftSelectedFile), nameof(RightSelectedFile));
            }
        }
        public int RightSelectedFile
        {
            get { return Right.SelectedFile; }
            set
            {
                Right.SelectedFile = value;
                Left.SelectedFile = -1;
                onPropertyChanged(nameof(LeftSelectedFile), nameof(RightSelectedFile));

            }
        }

        public MainViewModel()
        {
            Left = new PanelTCViewModel();
            Right = new PanelTCViewModel();
            LeftSelectedFile = -1;
            RightSelectedFile = -1;
        }

        // ICOMMANDS
        #region ICOMMANDS
        private ICommand copy = null;
        public ICommand Copy
        {
            get
            {
                if (copy == null)
                {
                    copy = new RelayCommand(
                        arg =>
                        {
                            if (LeftSelectedFile >= 0) CopyFileFromTo(Left, Right);
                            else if (RightSelectedFile >= 0) CopyFileFromTo(Right, Left);
                        },
                        arg => true
                        );
                }
                return copy;
            }
        }
        #endregion

        // METHODS
        #region METHODS
        private void CopyFileFromTo(PanelTCViewModel From, PanelTCViewModel To)
        {
            var selFile = From.GetSelectedFile();
            if (selFile == null) return;

            string sourceFileName = selFile.Name;
            string sourceFilePath = selFile.Path;
            string destinationDir = To.ActualPath;
            string destinationPath = $@"{destinationDir}\{sourceFileName}";

            var freeSpace = To.GetFreeSpaceFromSelectedDirve();
            // disk error
            if (freeSpace == null)
            {
                MessageBox.Show("Disk error (check if your disk is connected)!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selFile is FileObj srcFile)
            {
                // checking if file exists
                if (File.Exists(destinationPath))
                {
                    MessageBox.Show("Such a file already exists!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // if there is no space 
                if (freeSpace < srcFile.FileInfo.Length)
                {
                    MessageBox.Show("Out of disk space!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // copying
                File.Copy(sourceFilePath, destinationPath);
            }

            if (selFile is DirectoryObj)
            {
                // checking if directory exists
                if (Directory.Exists(destinationPath))
                {
                    MessageBox.Show("Such a directory already exists!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // getting space from all files
                long totalSpace = 0;
                foreach (string filePath in Directory.GetFiles(sourceFilePath, "*.*", SearchOption.AllDirectories))
                    totalSpace += new FileInfo(filePath).Length;

                // if there is no space 
                if (freeSpace < totalSpace)
                {
                    MessageBox.Show("Out of disk space!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // at first, creating destination directory
                Directory.CreateDirectory(destinationPath);
                // copying - recursively (first all directories, next all files)
                foreach (string dirPath in Directory.GetDirectories(sourceFilePath, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourceFilePath, destinationPath));
                foreach (string filePath in Directory.GetFiles(sourceFilePath, "*.*", SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(sourceFilePath, destinationPath), true);
            }

            To.UpdateFiles();
        }
        #endregion

    }
}
