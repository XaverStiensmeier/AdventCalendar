using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace Kalender
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            if(DateTime.Today.Day>12 && DateTime.Today.Month == 12)
                this.Background = new ImageBrush(new BitmapImage(new Uri("C:/Users/mmast/OneDrive/Desktop/Kalender/img/second.png")));
            CreateMyButtons();
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }
        private void CreateMyButton(int x, int y)
        {
            // Create and initialize a Button.
            Button window = new Button();
            int size = 70;
            window.Background = Brushes.LightBlue;
            window.Height = size;
            window.Width = size;
            window.Click += Button_Clicked;
            window.Content = (1+x) + (y*8);
            window.HorizontalAlignment = HorizontalAlignment.Left;
            window.VerticalAlignment = VerticalAlignment.Top;
            window.Margin = new Thickness(25+x*95, 25+y*95, 25, 120);
            GridMain.Children.Add(window);
        }
        private void CreateCopyright()
        {

        }
        private void CreateMyButtons()
        {
            for(int x = 0; x < 8; x++)
                for (int y = 0; y < 3; y++)
                    CreateMyButton(x, y);
        }
        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Today;
            int day = today.Day;
            int month = today.Month;
            Console.WriteLine(today.Day + "/" + today.Month);
            if (12 == 12)
            {
                ((Button)sender).Background = Brushes.LightCyan;
                int number = (int)((Button)sender).Content - 1;
                getPresent(number);
            } else
            {
                SystemSounds.Exclamation.Play();
                ((Button)sender).Background = Brushes.Red;
            }
            
        }
        public void getPresent(int day)
        {
            string body = "https://shorturl.at/";
            string[] links = {"https://www.whatfontis.com","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de",
                "google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de"};
            Console.WriteLine("Opened present: " + body + links[day]);
            string mp3 = day + 1+".mp3";
            Console.WriteLine(mp3);
            mediaPlayer.Open(new Uri("C:/Users/mmast/OneDrive/Desktop/Kalender/sound/"+mp3));
            mediaPlayer.Play();
            System.Diagnostics.Process.Start(body+links[day]);
        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
 
}
