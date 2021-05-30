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


        // EVENT - Double click on the list
        //rejestracja zdarzenia tak, żeby możliwe było jego bindowanie
        public static readonly RoutedEvent DblClickedEvent =
        EventManager.RegisterRoutedEvent("DblClicked",
                     RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                     typeof(PanelTC));

        //definicja zdarzenia NumberChanged
        public event RoutedEventHandler DblClicked
        {
            add { AddHandler(DblClickedEvent, value); }
            remove { RemoveHandler(DblClickedEvent, value); }
        }

        //Metoda pomocnicza wywołująca zdarzenie
        //przy okazji metoda ta tworzy obiekt argument przekazywany przez to zdarzenie
        void RaiseDblClicked()
        {
            //argument zdarzenia
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(PanelTC.DblClickedEvent);
            //wywołanie zdarzenia
            RaiseEvent(newEventArgs);
        }


        public static readonly RoutedEvent DropDownCBEvent =
        EventManager.RegisterRoutedEvent("DropDownCB",
                     RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                     typeof(PanelTC));

        public event RoutedEventHandler DropDownCB
        {
            add { AddHandler(DropDownCBEvent, value); }
            remove { RemoveHandler(DropDownCBEvent, value); }
        }
        void RaiseDropDownCB()
        {
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(PanelTC.DropDownCBEvent);
            RaiseEvent(newEventArgs);
        }


        public static readonly RoutedEvent BtnClickedEvent =
        EventManager.RegisterRoutedEvent("BtnClicked",
             RoutingStrategy.Bubble, typeof(RoutedEventHandler),
             typeof(PanelTC));

        public event RoutedEventHandler BtnClicked
        {
            add { AddHandler(BtnClickedEvent, value); }
            remove { RemoveHandler(BtnClickedEvent, value); }
        }
        void RaiseBtnClicked()
        {
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(PanelTC.BtnClickedEvent);
            RaiseEvent(newEventArgs);
        }


        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RaiseDblClicked();
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            RaiseDropDownCB();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseBtnClicked();
        }
    }
}
