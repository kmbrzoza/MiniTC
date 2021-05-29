using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiniTC.ViewModel
{
    using Model;
    using BaseClass;
    using System.Windows.Input;
    using System.Windows;

    public class PanelTCViewModel : BaseViewModel
    {
        //PROPERTIES
        #region PROPERTIES
        private string actualPath;
        public string ActualPath
        {
            get { return actualPath; }
            set
            {
                actualPath = value;
                onPropertyChanged(nameof(ActualPath));
                if (Directory.Exists(value))
                {
                    // checking if pathroot changed (if yes, update it)
                    var actualDrv = Path.GetPathRoot(value).ToUpper();
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
                if (Path.GetPathRoot(ActualPath).ToUpper() != Drives[SelectedDrive])
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
                            if (ActualPath != Drives[SelectedDrive])
                            {
                                // have to check if its ".." (previous directory)
                                if (SelectedFile == 0)
                                {
                                    if (Directory.GetParent(ActualPath).Exists)
                                        ActualPath = Directory.GetParent(ActualPath).FullName;
                                    return;
                                }
                                // -1 because of previous folder ".."
                                selFile = Files[SelectedFile - 1] as DirectoryObj;
                            }
                            else
                                selFile = Files[SelectedFile] as DirectoryObj;

                            // checking if directory exists
                            if (selFile != null)
                                if (Directory.Exists(selFile.Path))
                                    ActualPath = selFile.Path;

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

        #endregion

        // FUNCTIONS
        #region FUNCTIONS
        private void GetActiveDrives()
        {
            Drives = Directory.GetLogicalDrives();
        }

        private void GetFilesFromActualPath()
        {
            Files = new List<AFile>();
            string[] dirs = null;
            string[] fils = null;
            if (Directory.Exists(ActualPath))
            {
                dirs = Directory.GetDirectories(ActualPath);
                fils = Directory.GetFiles(ActualPath);
            }

            foreach (var dir in dirs)
                Files.Add(new DirectoryObj(dir));

            foreach (var fil in fils)
                Files.Add(new FileObj(fil));
        }

        private void SetFilesToAllFiles()
        {
            string[] filesTab;
            // checking if path its not drive, if yes add ".." for previous path
            if (ActualPath.ToUpper() != Drives[SelectedDrive])
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
            ActualPath = Drives[SelectedDrive];
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
            if (ActualPath != Drives[SelectedDrive])
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

        public long? GetFreeSpaceFromSelectedDirve()
        {
            var drivs = DriveInfo.GetDrives();
            foreach (var driv in drivs)
                if (driv.Name == Drives[SelectedDrive])
                    return driv.AvailableFreeSpace;
            return null;
        }

        public string GetActualPathOrNull()
        {
            if (Directory.Exists(ActualPath))
                return ActualPath;
            return null;
        }
        #endregion

    }
}
