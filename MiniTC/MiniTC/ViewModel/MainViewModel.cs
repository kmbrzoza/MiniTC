using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private ICommand delete = null;
        public ICommand Delete
        {
            get
            {
                if (delete == null)
                {
                    delete = new RelayCommand(
                        arg =>
                        {
                            if (LeftSelectedFile >= 0) DeleteSelectedFileIn(Left);
                            else if (RightSelectedFile >= 0) DeleteSelectedFileIn(Right);
                        },
                        arg => true
                        );
                }
                return delete;
            }
        }

        private ICommand move = null;
        public ICommand Move
        {
            get
            {
                if (move == null)
                {
                    move = new RelayCommand(
                        arg =>
                        {
                            if (LeftSelectedFile >= 0) MoveFileFromTo(Left, Right);
                            else if (RightSelectedFile >= 0) MoveFileFromTo(Right, Left);
                        },
                        arg => true
                        );
                }
                return move;
            }
        }

        private ICommand rename = null;
        public ICommand Rename
        {
            get
            {
                if (rename == null)
                {
                    rename = new RelayCommand(
                        arg =>
                        {
                            if (LeftSelectedFile >= 0) RenameSelectedFileIn(Left);
                            else if (RightSelectedFile >= 0) RenameSelectedFileIn(Right);
                        },
                        arg => true
                        );
                }
                return rename;
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

        private void DeleteSelectedFileIn(PanelTCViewModel In)
        {
            var selFile = In.GetSelectedFile();

            try
            {
                FileManager.Delete(selFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            In.UpdateFiles();
        }

        private void MoveFileFromTo(PanelTCViewModel From, PanelTCViewModel To)
        {
            var selFile = From.GetSelectedFile();
            var destinationDir = To.GetCurrentDir();

            try
            {
                FileManager.Move(selFile, destinationDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            From.UpdateFiles();
            To.UpdateFiles();
        }

        private void RenameSelectedFileIn(PanelTCViewModel In)
        {
            var selFile = In.GetSelectedFile();

            // if is null, just do nothing
            if (selFile != null)
            {
                var newName = View.InputBox.Prompt("Rename a file", "New name of file");

                if (newName != null)
                {
                    try
                    {
                        FileManager.Rename(selFile, newName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    In.UpdateFiles();

                }
            }
        }

        #endregion

    }
}
