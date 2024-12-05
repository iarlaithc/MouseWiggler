using System.Windows.Forms;

namespace Wiggler
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.NonClientAreaEnabled;

            var mainForm = new MainWiggler();
            mainForm.ShowInTaskbar = true;
            Application.Run(mainForm);
        }
    }
}