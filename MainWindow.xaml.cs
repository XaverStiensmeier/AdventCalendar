using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization;
using System.Threading;

namespace Kalender
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath = @"./resources/userdata.conf";
        bool[] answerArray = new bool[24];
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private static SemaphoreSlim blink = new SemaphoreSlim(1, 1);
        ImageBrush second = new ImageBrush(new BitmapImage(new Uri("./resources/img/second/second.png", UriKind.Relative)));
        ImageBrush two = new ImageBrush(new BitmapImage(new Uri("./resources/img/first/first2.png", UriKind.Relative)));
        ImageBrush six = new ImageBrush(new BitmapImage(new Uri("./resources/img/first/first6.png", UriKind.Relative)));
        ImageBrush twelve = new ImageBrush(new BitmapImage(new Uri("./resources/img/first/first12.png", UriKind.Relative)));
        ImageBrush sixteen = new ImageBrush(new BitmapImage(new Uri("./resources/img/second/second16.png", UriKind.Relative)));
        ImageBrush twentytwo = new ImageBrush(new BitmapImage(new Uri("./resources/img/second/second22.png", UriKind.Relative)));
        ImageBrush twentyfour = new ImageBrush(new BitmapImage(new Uri("./resources/img/second/second24.png", UriKind.Relative)));
        ImageBrush night = new ImageBrush(new BitmapImage(new Uri("./resources/img/first/firstnight.png", UriKind.Relative)));
        public MainWindow()
        {
            if(File.Exists(filePath))
            {
                answerArray = ReadFromBinaryFile<bool[]>(filePath);
            }
            else
            {
                MessageBox.Show("Your first time? Enjoy the show! Words are always separated by a space.\nDon't be afraid to ask if anything is weird :)");   //Ergänze! Begrüßung + Erläuterung der Regeln
            }

            //for()
            //WriteToBinaryFile<bool[]>(filePath, arr);
            
            InitializeComponent();
            int today = DateTime.Today.Day;
            if(today>12 && DateTime.Today.Month == 12) //change this later on!
                this.Background = second;
            else if (today == 2)
            {
                this.Background = two;
            }
            else if(today == 6)
            {
                this.Background = six;
            }
            else if (today == 12)
            {
                this.Background = twelve;
            }
            if (today == 16)
            {
                this.Background = sixteen;
            }
            else if (today == 22)
            {
                this.Background = twentytwo;
            }
            else if (today == 24)
            {
                this.Background = twentyfour;
            }
            if(today < 13 || DateTime.Now.Hour > 16)
            {
                this.Background = night;
            }
            CreateMyButtons();
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.PreviewKeyDown += new KeyEventHandler(HandleS);
            IsWin();
        }
        private void CreateMyButton(int x, int y)
        {
            // Create and initialize a Button.
            Button window = new Button();
            int size = 70;
            window.Height = size;
            window.Width = size;
            window.Click += Button_Clicked;
            window.Content = (1+x) + (y*8);
            window.HorizontalAlignment = HorizontalAlignment.Left;
            window.VerticalAlignment = VerticalAlignment.Top;
            window.Margin = new Thickness(25+x*95, 25+y*95, 25, 120);
            GridMain.Children.Add(window);
            int windowDay = (int)window.Content - 1;
            if (answerArray[windowDay])
            {
                window.Background = Brushes.Transparent;
            }  else
            {
                window.Background = Brushes.LightBlue;
            }
            if (12 != DateTime.Today.Month || (int)window.Content > DateTime.Today.Day)
            {
                window.BorderBrush = Brushes.Black;
            }
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
            int windowDay = (int)((Button)sender).Content - 1;
            if (12 == DateTime.Today.Month && windowDay < day)
            {
                int number = (int)((Button)sender).Content - 1;
                if (getPresent(number))
                {
                    ((Button)sender).Background = Brushes.Transparent;
                    IsWin();
                }
                else
                {
                    colorRed(sender);
                }
            } else
            {
                colorRed(sender);
            }
            
        }
        public Boolean getPresent(int day)
        {
            string[,] questions = new string[24, 2] { 
                { "Name the king, who tried to kill Jesus as a child (five letters) ", "Herod" },       //1
                { "How do you call the pickle that can be found in the toolbox (folder) (two words, each six letters)", "German pickle" },     //2
                { "At Christmas some of us celebrate the birth of Jesus... Talking about birthdays:\nAssuming there are sixteen people in a room " +  //3
                "what is the chance that at least two of them share their birthday.\n(assume that there are no leap years; 365 days)(example: write 93.5% as 0.94)",
                    "0.28" }, 
                { "How do we usually call dihydrogenmonoxid when frozen as steam?", "Snow" },           //4
                { "Which famous child was home alone during Christmas?", "Kevin" },             //5
                { "According to the Swedes I live in Dalarna, the Danes are looking for me in Greenland, the Americans think I am at the North Pole. " +        //6
                "\nToday I am not in any of those places. Today I have to work... Who am I?",
                    "Santa Claus" }, 
                { "Which pope introduced the custom of the Christmas tree? (St. Peter's Square; without leading 'pope', two words and a roman number)", "Johan Paul II" },     //7
                { "In 2020 not everything was bad (or was it?), for example there was a really bright comet that will not return for many years.\n" +            //8
                "The approximated next sight will be: 10001001110100. Our questions is: Will this year be a leap year?",
                    "Yes" }, 
                { "Which of the two authors has birthday today? (only firstname, probably the other one :) )", "Michael" },       //9
                { "I am a pioneer in my field. My two children, one of whom bears the name of my home country and the other one is radiantly beautiful, have helped me to achieve this." +
                "\nBut for all the glory my children have brought me, my love for them was to seal my fate . For more than a hundred years I was probably the first they murdered, but by no means the last." +
                "\nToday they are to be banished forever, for too much terror clings to their abilities. Who am I? (Use only latin letters.Three words: five, ten, five letters)", "Maria Sklodowska Curie" },   //10
                { "In the toolbox you can find a well known sweet, probably you know its name. It comes from the latin name ... (ten letters)", "speculator" },        //11
                { "The mistletoe is the holy plant of the goddess... (five letters)", "Frigg" },        //12
                { "How do we usually call Furanoeudesma-1,3-dien? (five letters)", "Myrrh" },       //13
                { "In the toolbox is a .wav file called 'Rhythm of Kyai.mp3'. The music represents a very famous sequence. How is it called? " +  //14
                "\n(without the word sequence)", "Fibonacci" },
                { "In the toolbox is a picture of a circuit called 'circuit.jpg' (OH!). Which diodes would not light up? (Sorted ascending. For example: 'D1,D3')", "D1,D2,D3,D4,D5" },// 15
                { "27! Is a very large number. Please don't compute it! Try to use your awesome math skills and determin the number of zeroes at the end of it. " + //16
                "\n(example 101000 has '3' zeroes at the end)", "6" },
                { "In our beloved toolbox there is an .mp3 file called 'mysterious.mp3'. Try to play with it until you understand the answer.", "8243721" },     //17
                { "I have 52 teeth, but you still gonna eat me. After whom am I namend? (only surename)", "Leibniz" },          //18
                { "In our toolbox (AGAIN?) is a lovely picture of a famous chess game. In which book/movie is it 'shown'? (book/movie title)" +       //19
                "(\nHint: Black had to sacrifice their knight and one rook to lure the queen out and win the game; a great sacrifice...)(Whole title)", "Harry Potter and the Philosopher's Stone" }, 
                { "You've probably heard about the Star of Bethlehem; but do you know which two planets (probably) constituted to the effect? " + //20
                "\n(order the planets alphabetically and separate them using a comma, example: 'Earth,Venus'", "Jupiter,Saturn" }, 
                { "Hello, rare to see me. I only occure twice a year. Tomorrow I'll be gone.. Who am I!", "Solstice" }, //21
                { "What special value did the famous physicist Gauss have for the Germans in the past? (just numbers)", "10" }, //22
                { "My condition changes predominantly four times and a salat is named after me.\nIn the morning I wake up; at noon I have my hottest period." + // 23
                "\nAt the evening I get dirty wet, yet also very colourful. But at night I show you my cold shoulder (4 and 7 letters)", "four seasons" }, 
                { "The last question is a special one. You can find it in the toolbox. It is called: 'RelativeChristmas.pdf'. Please round the answer to four digits (0.0001 for example).", "0.996" }}; //24 !!!!!!!!!!!!!!
            string mp3 = day + 1+".mp3";
            Boolean opened = Dialog(questions[day, 0], questions[day, 1]);
            if (opened)
            {
                mediaPlayer.Open(new Uri("./resources/sound/" + mp3, UriKind.Relative));
                mediaPlayer.Play();
                answerArray[day] = true;
                WriteToBinaryFile<bool[]>(filePath, answerArray);
            }
            return opened;
            //System.Diagnostics.Process.Start(body+questions[day,0]);
        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        private async void HandleS(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {

                Brush t = this.Background;
                int delay = 3000;
                await Task.Delay(delay);
                this.Background = two;
                await Task.Delay(delay);
                this.Background = six;
                await Task.Delay(delay);
                this.Background = twelve;
                await Task.Delay(delay);
                this.Background = second;
                await Task.Delay(delay);
                this.Background = sixteen;
                await Task.Delay(delay);
                this.Background = twentytwo;
                await Task.Delay(delay);
                this.Background = twentyfour;
                await Task.Delay(delay);
                this.Background = night;
                await Task.Delay(delay);
                this.Background = t;
            }
        }
        private async void colorRed(object sender)
        {
            SystemSounds.Exclamation.Play();
            await blink.WaitAsync();
            Brush myBrush = ((Button)sender).Background;
            int delay = 1000;
            int all = 3;
            for (int i = 0; i < all; i++)
            {
                ((Button)sender).Background = Brushes.PaleVioletRed;
                await Task.Delay(delay/all);
                ((Button)sender).Background = myBrush;
                await Task.Delay(delay/all);
            }
            blink.Release();
        }
        public Boolean Dialog(string name, string expectedAnswer)
        {
            DialogBox myBox = new DialogBox(name, expectedAnswer);
            if (myBox.ShowDialog() == true)
            {
                return myBox.Answer;
            }
            return false;

        }
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                try
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(stream);
                }
                catch(SerializationException e)
                {
                    MessageBox.Show(e.Message + "\nDelete the userdata.conf in resource; you damn cheater!");
                    Environment.Exit(-42);
                    return default(T);
                }
            }
        }
        public void IsWin()
        {
            for (int i = 0; i < 24; i++)
            {
                if (answerArray[i])
                {
                    if(i==23)
                    {
                        MessageBox.Show("You won the game! Please contact: riddlemaster-info@protonmain.com! Hope you enjoyed playing :)");
                    }
                }
                else
                    break;
            }
                
        }
    }
}