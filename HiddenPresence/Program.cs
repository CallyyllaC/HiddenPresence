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

        //discord stuff
        private static Discord.EventHandlers handlers;

        static void Main(string[] args)
        {
            try { Discord.Shutdown(); }
            catch { }

            SettingsFile = OpenSettings();
            Update();

            while (true)
            {
                Thread.Sleep(30000);
                if (!Enumerable.SequenceEqual(SettingsFile, OpenSettings()))
                {
                    SettingsFile = OpenSettings();
                    Update();
                }
            }
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

        private static string[] OpenSettings()
        {
            string[] Settings = null;
            try
            {
                Settings = File.ReadAllLines($"{filepath}\\Discord.txt");
            }
            catch { }

            return Settings;
        }
    }
}
