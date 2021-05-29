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
            var destinationDir = To.GetCurrentDir();

            try
            {
                FileManager.Copy(selFile, destinationDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            To.UpdateFiles();
        }
        #endregion

    }
}
