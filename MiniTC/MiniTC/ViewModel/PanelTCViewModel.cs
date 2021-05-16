using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.ViewModel
{
    class PanelTCViewModel : BaseClass.BaseViewModel
    {
        public string ActualPath { get; set; }
        public string[] Drives { get; set; }
        public int SelectedDrive { get; set; }
        public string[] AllFiles { get; set; }
        public int SelectedFile { get; set; }

        public PanelTCViewModel()
        {

        }

    }
}
