using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;

namespace AutoPlay
{
    class Program
    {
        private static String username = Environment.UserName;
        private static String path = @"C:\Users\" + username + @"\Music\";

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        private enum WindowShowStyle:uint
        {
            Hide = 0,
            ShowMinimized = 2, 
            Minimize = 6
        }

        static void Main(string[] args)
        {
            Run();
        }

        public static void Run()
        {        
            username = Environment.UserName;
            username = char.ToUpper(username[0]) + username.Substring(1);
            Console.WriteLine("Hello " + username);
            Thread.Sleep(2000);
            Console.WriteLine("Opening AutoPlay...");
            Thread.Sleep(2000);
            OpenPlayer(path);            
            Thread.Sleep(2000);
            Console.WriteLine("Minimizing Player...");
            Thread.Sleep(2000);
            MinimizePlayer();
            Thread.Sleep(2000);
            Console.WriteLine("Happy Listening.");
            Thread.Sleep(1000);
            Console.WriteLine("Goodbye...");
            Thread.Sleep(2000);            
            Environment.Exit(0);

        }
        public static void MinimizePlayer()
        {
            Process[] ps = Process.GetProcesses();
            foreach(Process p in ps)
            {
                if(p.ProcessName.Contains("wmplayer"))
                {
                    IntPtr h = p.MainWindowHandle;

                    ShowWindow(h, WindowShowStyle.Minimize);
                }
            }
        }    
        public static void OpenPlayer(String path)
        {
            string directory = @"C:\Users\" + username + @"\Music";
            DirectoryInfo dir = new DirectoryInfo(path);
            string firstFile = dir.GetFiles().OrderBy(fi => fi.Name).Select(fi => fi.Name).FirstOrDefault();
            path += firstFile;
            if (IsDirectoryEmpty(directory))
            {
                Console.WriteLine("Your Music folder is empty.\nConsider adding music files.");
                Thread.Sleep(3000);
                Console.WriteLine("Goodbye...");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
            else
            {
                Process.Start("wmplayer.exe", path);
            }
        }
        public static bool IsDirectoryEmpty(String path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
}
