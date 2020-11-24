using System;
using System.Windows.Forms;

namespace GraphicalApp
{
    public class MyForm : Form
    {
        // No properties.
        public static void getPresent(){
            string body = "https://shorturl.at/"
            string[] links = {"https://www.whatfontis.com","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de",
                "google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de","google.de"};
            DateTime today = DateTime.Today;
            int day = today.Day;
            int month = today.Month;
            Console.WriteLine(today.Day + "/" + today.Month);   
            if(12 == 12) {
                //System.Diagnostics.Process.Start(body+links[day]);
                Console.WriteLine(today.Day + "/" + today.Month + " - Opened present: " + body+links[day]);
            }
        }
        public MyForm ()
        {
            //windows
            //System.Diagnostics.Process.Start("http://google.com");
            // Default constructor
            getPresent();
        }

        public static void Main(string []args) {
            Application.Run(new MyForm());
        }

        // No functions.
        
    }
}
