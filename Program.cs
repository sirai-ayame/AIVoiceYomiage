using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AIVoiceYomiage
{
    class Program
    {
        static void Main(string[] args)
        {
            AIVoiceEditor aiVoiceEditor = null;
            try
            {
                var configFilepath = Path.Combine(Util.BaseDirectoryPath, "config.json");
                var config = Config.Load("config.json");
                var watchDirectory = Path.GetDirectoryName(config.WatchFilePath);
                Debug.Assert(watchDirectory != null);
                var watchFileName = Path.GetFileName(config.WatchFilePath);
                Debug.Assert(watchFileName != null);

                aiVoiceEditor = new AIVoiceEditor(config.Voice);

                var prevLine = CalcInitialMessage(config, config.WatchFilePath);

                var watcher = new FileSystemWatcher(watchDirectory);
                watcher.Filter = watchFileName;
                watcher.Changed += (sender, e) =>
                {
                    if (e.ChangeType == WatcherChangeTypes.Changed)
                    {
                        var newMessage = CalcNewMessage(config, prevLine, e.FullPath, out var nextLine);
                        if (!string.IsNullOrEmpty(newMessage))
                        {
                            Console.WriteLine($"{config.Voice} が「{newMessage}」と言っています。");
                            aiVoiceEditor.Play(newMessage);
                            prevLine = nextLine;
                        }
                    }
                };
                watcher.EnableRaisingEvents = true;
                Console.WriteLine($"now start watching: {config.WatchFilePath}");
                while (true)
                {
                    Thread.Sleep(1000);
                }

            }
            catch (ErrorException ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(1);
            }
            finally
            {
                if (aiVoiceEditor != null)
                {
                    aiVoiceEditor.Dispose();
                }
            }
        }

        static string CalcInitialMessage(Config config, string watchFile)
        {
            var result = string.Empty;
            foreach (var line in File.ReadAllLines(watchFile))
            {
                if (line.StartsWith(config.Mark))
                {
                    result = line;
                }
            }
            return result;
        }

        static string CalcNewMessage(Config config, string prevLine, string watchFile, out string nextLine)
        {
            var sb = new StringBuilder();
            nextLine = string.Empty;
            IEnumerable<string> lines = File.ReadAllLines(watchFile);
            foreach (var line in lines.Reverse())
            {
                if (!line.StartsWith(config.Mark))
                {
                    continue;
                }
                if (prevLine == line)
                {
                    break;
                }
                if (nextLine == string.Empty)
                {
                    nextLine = line;
                }
                sb.AppendLine(line.Substring(config.Mark.Length));

            }
            return sb.ToString().Trim();
        }
    }
}
