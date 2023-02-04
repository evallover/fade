using System.Diagnostics;

namespace fade
{
    public delegate void Key(ConsoleKeyInfo key);
    class FileManager
    {
        public event Key kp;
        FilePanel filepanel = new FilePanel();
        public FileManager()
        {
            kp += filepanel.Key;
            filepanel.Borders();
            Console.SetCursorPosition(1, 18);
            Console.WriteLine("Сделайте выбор, управляя стрелками вверх-вниз, нажмите Enter.\n" +
                "Для того, чтобы вернуться на папку выше, нажмите ESC.\n" +
                "Чтобы выйти, нажмите F2.");
        }
        
        public static void PositionString(string str, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(str);
        }
        
        private void Moving()
        {
            FileSystemInfo filesinfo = filepanel.GetActiveObject();
            if (filesinfo != null)
            {
                if (filesinfo is DirectoryInfo)
                {
                    Directory.GetDirectories(filesinfo.FullName);
                    filepanel.Path = filesinfo.FullName;
                    filepanel.SetLists();
                    filepanel.Panel();
                }
                else
                {
                    Process.Start(new ProcessStartInfo(((FileInfo)filesinfo).FullName) { UseShellExecute = true });
                }
            }
            else
            {
                string currentPath = filepanel.Path;
                DirectoryInfo currentDirectory = new(currentPath);
                DirectoryInfo upLevelDirectory = currentDirectory.Parent;
                if (upLevelDirectory != null)
                {
                    filepanel.Path = upLevelDirectory.FullName;
                    filepanel.SetLists();
                    filepanel.Panel();
                }
                else
                {
                    filepanel.SetDiscs();
                    filepanel.Panel();
                }
            }
        }

        public FileManager PCexplorer()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userKey = Console.ReadKey(true);
                    switch (userKey.Key)
                    {
                        case ConsoleKey.Enter:
                            Moving();
                            break;
                        case ConsoleKey.DownArrow:
                            kp(userKey);
                            break;
                        case ConsoleKey.UpArrow:
                            kp(userKey);
                            break;
                        case ConsoleKey.F2:
                            Console.Clear();
                            Environment.Exit(0);
                            break;
                        case ConsoleKey.Escape:
                            PCexplorer();
                            break;
                    }
                }
            }
        }
    }
}
