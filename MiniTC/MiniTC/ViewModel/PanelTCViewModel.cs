using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.ViewModel
{
    public class PanelTCViewModel : BaseClass.BaseViewModel
    {
        private string actualPath;
        public string ActualPath { get { return actualPath; } set { actualPath = value; onPropertyChanged(nameof(ActualPath)); } }
        public string[] Drives { get; set; }
        public int SelectedDrive { get; set; }
        public string[] AllFiles { get; set; }
        public int SelectedFile { get; set; }

        public PanelTCViewModel()
        {

        }

    }
}
