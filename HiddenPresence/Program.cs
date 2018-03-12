using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace HiddenPresence
{
    class Program
    {
        //Text File
        private static string filepath = $"{Directory.GetCurrentDirectory()}\\";
        private static string[] SettingsFile = null;
        private static FileSystemWatcher watcher;

        //discord stuff
        private static Discord.EventHandlers handlers;

        static void Main(string[] args)
        {
            CreateFileWatcher();

            while (true)
            {
                Console.WriteLine(watcher.WaitForChanged(WatcherChangeTypes.All).ChangeType);
                Console.WriteLine("Changed");
            }
        }


        public static void CreateFileWatcher()
        {
            // Create FileSystemWatcher
            watcher = new FileSystemWatcher();
            watcher.Path = filepath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "Discord.txt";
            watcher.Renamed += new RenamedEventHandler(OnChanged);
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            OpenSettings();
            Update();
        }

        private static void Update()
        {
            try { Discord.Shutdown(); }
            catch { }
            try
            {
                Discord.RichPresence NewDisplay = new Discord.RichPresence();

                Discord.Initialize(SettingsFile[1], handlers);
                NewDisplay.details = SettingsFile[2];
                NewDisplay.state = SettingsFile[3];
                NewDisplay.largeImageKey = SettingsFile[4];
                NewDisplay.largeImageText = SettingsFile[5];
                NewDisplay.smallImageKey = SettingsFile[6];
                NewDisplay.smallImageText = SettingsFile[7];

                Discord.UpdatePresence(NewDisplay);
            }
            catch { }
        }

        private static void OpenSettings()
        {
            try
            {
                SettingsFile = File.ReadAllLines($"{filepath}\\Discord.txt");
            }
            catch { }
        }
    }
}
