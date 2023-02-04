using System.Text;

namespace fade
{
    class FilePanel
    {
        List<FileSystemInfo> filesystem = new();
        int height = 16;
        int width = 100;
        int top;
        int left;
        string path;
        int oneindex = 0;
        int twoindex = 0;
        int amount = 16;
        public bool discs1;
        bool active;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                DirectoryInfo dir = new(value);
                if (dir.Exists)
                {
                    path = value;
                }
            }
        }
        public FilePanel()
        {
            SetDiscs();
        }
        public FileSystemInfo GetActiveObject()
        {
            if (filesystem != null && filesystem.Count != 0)
            {
                return filesystem[oneindex];
            }
            throw new Exception();
        }
        public void Key(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    Up();
                    break;
                case ConsoleKey.DownArrow:
                    Down();
                    break;
            }
        }
        private void Up()
        {
            if (oneindex <= twoindex)
            {
                twoindex -= 1;
                if (twoindex < 0)
                {
                    twoindex = 0;
                }
                oneindex = twoindex;
                UpdateContent(false);
            }
            else
            {
                DeactivateObject(oneindex);
                oneindex--;
                ActivateObject(oneindex);
            }
        }
        private void Down()
        {
            if (oneindex >= twoindex + amount - 1)
            {
                twoindex += 1;
                if (twoindex + amount >= filesystem.Count)
                {
                    twoindex = filesystem.Count - amount;
                }
                oneindex = twoindex + amount - 1;
                UpdateContent(false);
            }
            else
            {
                if (oneindex >= filesystem.Count - 1)
                {
                    return;
                }
                DeactivateObject(oneindex);
                oneindex++;
                ActivateObject(oneindex);
            }
        }
        public void SetLists()
        {
            if (filesystem.Count != 0)
            {
                filesystem.Clear();
            }
            discs1 = false;
            DirectoryInfo lvlupdir = null;
            filesystem.Add(lvlupdir);
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                DirectoryInfo di = new(directory);
                filesystem.Add(di);
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                FileInfo fls = new(file);
                filesystem.Add(fls);
            }
        }
        public void SetDiscs()
        {
            if (filesystem.Count != 0)
            {
                filesystem.Clear();
            }
            discs1 = true;
            DriveInfo[] discs = DriveInfo.GetDrives();
            foreach (DriveInfo disc in discs)
            {
                if (disc.IsReady)
                {
                    DirectoryInfo dir = new(disc.Name);
                    filesystem.Add(dir);
                }
            }
        }
        
        public void Clear()
        {
            for (int i = 0; i < height; i++)
            {
                string clr = new(' ', width);
                Console.SetCursorPosition(left, top + i);
                Console.Write(clr);
            }
        }
        public void Borders()
        {
            Clear();
            StringBuilder caption = new();
            FileManager.PositionString(caption.ToString(), left + width / 2 - caption.ToString().Length / 2, top);
            PrintContent();
        }
        private void PrintContent()
        {
            if (filesystem.Count == 0)
            {
                return;
            }
            int count = 0;
            int lastelement = twoindex + amount;
            if (lastelement > filesystem.Count)
            {
                lastelement = filesystem.Count;
            }
            if (oneindex >= filesystem.Count)
            {
                oneindex = 0;
            }
            for (int i = twoindex; i < lastelement; i++)
            {
                Console.SetCursorPosition(left + 1, top + count + 1);
                if (i == oneindex && active == true)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                PrintObject(i);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                count++;
            }
        }
        public void Panel()
        {
            twoindex = 0;
            oneindex = 0;
            Borders();
        }
        private void PrintObject(int index)
        {
            int currentCursorTopPosition = Console.CursorTop;
            int currentCursorLeftPosition = Console.CursorLeft;
            if (!discs1 && index == 0)
            {
                Console.Write("...");
                return;
            }
            Console.Write("{0}", filesystem[index].Name);
            Console.SetCursorPosition(currentCursorLeftPosition + width / 2, currentCursorTopPosition);
            if (filesystem[index] is DirectoryInfo)
            {
                Console.Write("{0}", ((DirectoryInfo)filesystem[index]).LastWriteTime);
            }
            else
            {
                Console.Write("{0}", ((FileInfo)filesystem[index]).Length);
            }
        }
        
        public void UpdateContent(bool updatelist)
        {
            if (updatelist)
            {
                SetLists();
            }
            Clear();
            PrintContent();
        }
        private void ActivateObject(int index)
        {
            int os = oneindex - twoindex;
            Console.SetCursorPosition(left + 1, top + os + 1);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            PrintObject(index);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        private void DeactivateObject(int index)
        {
            int os = oneindex - twoindex;
            Console.SetCursorPosition(left + 1, top + os + 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            PrintObject(index);
        }
    }
}
