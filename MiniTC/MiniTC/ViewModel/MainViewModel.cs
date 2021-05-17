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
                            if (selFile == null) return;
                            if (selFile is FileObj)
                            {
                                Console.WriteLine("test");
                                //var leftFile = selFile as FileObj;
                                string sourceFileName = selFile.Name;
                                string sourceFilePath = selFile.Path;
                                string destinationDir = Right.ActualPath;
                                string destinationPath = $@"{destinationDir}\{sourceFileName}";

                                // add here checking free space on drive
                                if (!File.Exists(destinationPath))
                                {
                                    File.Copy(sourceFilePath, destinationPath);
                                    Right.UpdateFiles();
                                }
                                else
                                    MessageBox.Show("Taki plik juz istnieje!", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Warning);

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
