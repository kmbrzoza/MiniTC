using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.ViewModel
{
    public class MainViewModel : BaseClass.BaseViewModel
    {
        public PanelTCViewModel Left { get; set; }
        public PanelTCViewModel Right { get; set; }

        public MainViewModel()
        {
            Left = new PanelTCViewModel();
            Right = new PanelTCViewModel();
        }
    }
}
