using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var api = new WebApiService();
            _ = Task.Run(() => api.Start()); // chạy nền API

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
