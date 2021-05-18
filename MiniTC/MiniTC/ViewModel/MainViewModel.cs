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

        public MainViewModel()
        {
            Left = new PanelTCViewModel();
            Right = new PanelTCViewModel();
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
                            var selFile = Left.GetSelectedFile();
                            
                            if (selFile is FileObj srcFile)
                            {
                                string sourceFileName = srcFile.Name;
                                string sourceFilePath = srcFile.Path;
                                string destinationDir = Right.ActualPath;
                                string destinationPath = $@"{destinationDir}\{sourceFileName}";

                                var freeSpace = Right.GetFreeSpaceFromSelectedDirve();
                                // disk error
                                if (freeSpace == null)
                                {
                                    MessageBox.Show("Disk error (check if your disk is connected)!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
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
                                File.Copy(sourceFilePath, destinationPath);
                                Right.UpdateFiles();
                            }
                        },
                        arg => true
                        );
                }
                return copy;
            }
        }
        #endregion

    }
}
