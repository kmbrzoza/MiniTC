using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiniTC.ViewModel
{
    using Model;
    public class PanelTCViewModel : BaseClass.BaseViewModel
    {
        private string actualPath;
        public string ActualPath
        {
            get { return actualPath; }
            set { actualPath = value; onPropertyChanged(nameof(ActualPath)); }
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
                UpdatePathAndFiles();
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


        public PanelTCViewModel()
        {
            GetActiveDrives();
            UpdatePathAndFiles();
        }


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
            if(ActualPath != Drives[SelectedDrive])
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

        public void UpdatePathAndFiles()
        {
            ActualPath = Drives[SelectedDrive];
            GetFilesFromActualPath();
            SetFilesToAllFiles();
        }

    }
}
