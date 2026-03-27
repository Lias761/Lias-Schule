using System;
using System.Windows.Forms;

namespace SnakeGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Hier wird dein Snake-Fenster gestartet:
            Application.Run(new Form1()); 
        }
    }
}