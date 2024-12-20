using System.Runtime.InteropServices;



namespace XTCRecovery
{

    internal static class Program
    {

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}