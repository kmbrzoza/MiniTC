using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniTC.View
{
    /// <summary>
    /// Interaction logic for PanelTC.xaml
    /// </summary>
    public partial class PanelTC : UserControl
    {
        public PanelTC()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PathStringProperty = DependencyProperty.Register(
        "PathString", typeof(string), typeof(PanelTC), new FrameworkPropertyMetadata(null));
        public string PathString
        {
            get { return (string)GetValue(PathStringProperty); }
            set { SetValue(PathStringProperty, value); }
        }

        public static readonly DependencyProperty DrivesProperty = DependencyProperty.Register(
        "Drives", typeof(string[]), typeof(PanelTC), new FrameworkPropertyMetadata(null));
        public string[] Drives
        {
            get { return (string[])GetValue(DrivesProperty); }
            set { SetValue(DrivesProperty, value); }
        }

        public static readonly DependencyProperty SelectedDriveProperty = DependencyProperty.Register(
        "SelectedDrive", typeof(int), typeof(PanelTC), new FrameworkPropertyMetadata(null));
        public int SelectedDrive
        {
            get { return (int)GetValue(SelectedDriveProperty); }
            set { SetValue(SelectedDriveProperty, value); }
        }

        public static readonly DependencyProperty AllFilesProperty = DependencyProperty.Register(
        "AllFiles", typeof(string[]), typeof(PanelTC), new FrameworkPropertyMetadata(null));
        public string[] AllFiles
        {
            get { return (string[])GetValue(AllFilesProperty); }
            set { SetValue(AllFilesProperty, value); }
        }

        public static readonly DependencyProperty SelectedFileProperty = DependencyProperty.Register(
        "SelectedFile", typeof(int), typeof(PanelTC), new FrameworkPropertyMetadata(null));
        public int SelectedFile
        {
            get { return (int)GetValue(SelectedFileProperty); }
            set { SetValue(SelectedFileProperty, value); }
        }
    }
}
