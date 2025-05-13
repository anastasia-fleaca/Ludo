using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ludo
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 mainForm = new Form1();

            float scaleX = Screen.PrimaryScreen.Bounds.Width / 1920f;
            float scaleY = Screen.PrimaryScreen.Bounds.Height / 1080f;

            mainForm.Scale(new SizeF(scaleX, scaleY));

            Application.Run(mainForm);
        }
    }
}
