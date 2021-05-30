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
using System.Windows.Shapes;

namespace MiniTC.View
{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        private string Response { get { return responseTextBox.Text; } }
        private bool ClickedOK;
        private InputBox(string title = "title", string question = "question")
        {
            InitializeComponent();
            this.Title = title;
            this.questionLabel.Content = question;
            ClickedOK = false;
        }
        public static string Prompt(string title, string question)
        {
            InputBox inputBox = new InputBox(title, question);
            inputBox.ShowDialog();
            if(inputBox.ClickedOK)
                return inputBox.Response;
            return null;
        }

        private void leftBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Response))
            {
                ClickedOK = true;
                Close();
            }
            else
                MessageBox.Show("You did not answer!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void rightBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
