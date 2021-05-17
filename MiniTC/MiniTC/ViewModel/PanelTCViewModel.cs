﻿using System;
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
                UpdateFiles();
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

                            if (selFile != null)
                            {
                                if (Directory.Exists(selFile.Path))
                                {
                                    ActualPath = selFile.Path;
                                }
                            }
                        },
                        arg => true
                        );
                }
                return fileDblClicked;
            }
        }

        #endregion

        // FUNCTIONS
        #region FUNCTIONS
        public void GetActiveDrives()
        {
            var drivs = DriveInfo.GetDrives();
            Drives = new string[drivs.Length];
            for (int i = 0; i < drivs.Length; i++)
            {
                Drives[i] = drivs[i].Name;
            }

        }

        public void GetFilesFromActualPath()
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

        public void SetFilesToAllFiles()
        {
            // checking if path its not drive, if yes add ".." for previous path
            if (ActualPath != Drives[SelectedDrive])
            {
                AllFiles = new string[Files.Count + 1];
                AllFiles[0] = "..";
                for (int i = 0; i < Files.Count; i++)
                    AllFiles[i + 1] = Files[i].ToString();
            }
            else
            {
                AllFiles = new string[Files.Count];
                for (int i = 0; i < Files.Count; i++)
                    AllFiles[i] = Files[i].ToString();
            }
        }

        public void UpdatePathToDriveAndFiles()
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
        #endregion

    }
}
