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

namespace Kalender
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class DialogBox : Window
    {
            string expectedAnswer;
            public DialogBox(string question, string defaultAnswer = "")
            {
                InitializeComponent();
                lblQuestion.Content = question;
                expectedAnswer = defaultAnswer;
            //<Image Source="" Width="500" Height="500" Grid.RowSpan="2" Margin="20,0" />
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
            {
                this.DialogResult = true;
            }

            private void Window_ContentRendered(object sender, EventArgs e)
            {
                txtAnswer.SelectAll();
                txtAnswer.Focus();
            }

            public Boolean Answer
            {
                get { return txtAnswer.Text.ToLower().Contains(expectedAnswer.ToLower()); }
            }
    }
}
