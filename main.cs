using System;
using System.Windows.Forms;

namespace GraphicalApp
{
    public class MyForm : Form
    {
        // No properties.
        DateTime today = DateTime.Today;
        public MyForm ()
        {
            //windows
            System.Diagnostics.Process.Start("http://google.com");
            // Default constructor
        }

        public static void Main(string []args) {
            Application.Run(new MyForm());
        }

        // No functions.
        
    }
}
