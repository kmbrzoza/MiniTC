using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace MiniTC.ViewModel
{
    using Model;
    using BaseClass;

    public class PanelTCViewModel : BaseViewModel
    {
        //PROPERTIES
        #region PROPERTIES
        private string currentPath;
        public string CurrentPath
        {
            get { return currentPath; }
            set
            {
                currentPath = value;
                onPropertyChanged(nameof(CurrentPath));
                if (FileManager.DirectoryExists(value))
                {
                    // checking if pathroot changed (if yes, update it)
                    var actualDrv = FileManager.GetDriveFromPath(value);
                    if (actualDrv != Drives[SelectedDrive])
                    {
                        for (int i = 0; i < Drives.Length; i++)
                            if (actualDrv == Drives[i])
                            {
                                SelectedDrive = i;
                                break;
                            }
                    }
                    UpdateFiles();
                }
            }
        }
        private string[] drives;
        public string[] Drives
        {
            get { return drives; }
            set { drives = value; onPropertyChanged(nameof(Drives)); }
        }
        private int selectedDrive { get; set; }
        public int SelectedDrive
        {
            get { return selectedDrive; }
            set
            {
                selectedDrive = value;
                onPropertyChanged(nameof(SelectedDrive));
                // if user clicked on combobox and changed drive, update path to drive and files
                if (FileManager.GetDriveFromPath(CurrentPath) != Drives[SelectedDrive])
                    UpdatePathToDriveAndFiles();
            }
        }
        private string[] allFiles;
        public string[] AllFiles
        {
            get { return allFiles; }
            set { allFiles = value; onPropertyChanged(nameof(AllFiles)); }
        }
        public int SelectedFile { get; set; }

        private List<AFile> Files;
        #endregion

        // CTOR
        public PanelTCViewModel()
        {
            GetActiveDrives();
            UpdatePathToDriveAndFiles();
        }

        // ICOMMANDS
        #region ICOMMANDS

        private ICommand fileDblClicked = null;
        public ICommand FileDblClicked
        {
            get
            {
                if (fileDblClicked == null)
                {
                    fileDblClicked = new RelayCommand(
                        arg =>
                        {
                            DirectoryObj selFile = null;
                            // checking if path is not selected drive
                            if (CurrentPath != Drives[SelectedDrive])
                            {
                                // have to check if its ".." (previous directory)
                                if (SelectedFile == 0)
                                {
                                    string parentPath = FileManager.GetParentPath(CurrentPath);
                                    if (FileManager.DirectoryExists(parentPath))
                                        CurrentPath = parentPath;
                                    return;
                                }
                                // -1 because of previous folder ".."
                                selFile = Files[SelectedFile - 1] as DirectoryObj;
                            }
                            else
                                selFile = Files[SelectedFile] as DirectoryObj;

                            // checking if directory exists
                            if (selFile != null)
                                if (FileManager.DirectoryExists(selFile.Path))
                                    CurrentPath = selFile.Path;

                        },
                        arg => true
                        );
                }
                return fileDblClicked;
            }
        }

        private ICommand drivesDropDown = null;
        public ICommand DrivesDropDown
        {
            get
            {
                if (drivesDropDown == null)
                {
                    drivesDropDown = new RelayCommand(
                        arg => { GetActiveDrives(); },
                        arg => true
                        );
                }
                return drivesDropDown;
            }
        }

        private ICommand newDir = null;
        public ICommand NewDir
        {
            get
            {
                if (newDir == null)
                {
                    newDir = new RelayCommand(
                        arg => { CreateNewDir(); },
                        arg => true
                        );
                }
                return newDir;
            }
        }

        #endregion

        // FUNCTIONS
        #region FUNCTIONS
        private void GetActiveDrives()
        {
            Drives = FileManager.GetActiveDrives();
        }

        private void GetFilesFromActualPath()
        {
            Files = new List<AFile>();
            try
            {
                Files = FileManager.GetFilesByPath(CurrentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void SetFilesToAllFiles()
        {
            string[] filesTab;
            // checking if path its not drive, if yes add ".." for previous path
            if (CurrentPath.ToUpper() != Drives[SelectedDrive])
            {
                filesTab = new string[Files.Count + 1];
                filesTab[0] = "..";
                for (int i = 0; i < Files.Count; i++)
                    filesTab[i + 1] = Files[i].ToString();
            }
            else
            {
                filesTab = new string[Files.Count];
                for (int i = 0; i < Files.Count; i++)
                    filesTab[i] = Files[i].ToString();
            }
            AllFiles = filesTab;
        }

        private void UpdatePathToDriveAndFiles()
        {
            CurrentPath = Drives[SelectedDrive];
            GetFilesFromActualPath();
            SetFilesToAllFiles();
        }

        public void UpdateFiles()
        {
            GetFilesFromActualPath();
            SetFilesToAllFiles();
        }

        public AFile GetSelectedFile()
        {
            // if file is not selected
            if (SelectedFile == -1)
                return null;

            AFile selFile;
            if (CurrentPath != Drives[SelectedDrive])
            {
                // have to check if its ".." (previous directory)
                if (SelectedFile == 0)
                    return null;
                // -1 because of previous folder ".."
                selFile = Files[SelectedFile - 1];
            }
            else
                selFile = Files[SelectedFile];

            return selFile;
        }

        public DirectoryObj GetCurrentDir()
        {
            if (FileManager.DirectoryExists(CurrentPath))
                return new DirectoryObj(CurrentPath);
            return null;
        }

        public void CreateNewDir()
        {
            var currentDir = GetCurrentDir();
            string newDir = View.InputBox.Prompt("Creating new directory", "Name of new directory");
            // if is null, just do nothing
            if(newDir != null)
            {
                try
                {
                    FileManager.CreateDirectory(currentDir, newDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                UpdateFiles();
            }
        }
        #endregion

    }
}
