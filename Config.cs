using System;
using System.IO;
using System.Text.Json;

namespace AIVoiceYomiage
{
    public class Config
    {
        public string WatchFilePath { get; set; } = string.Empty;
        public string Mark { get; set; } = string.Empty;
        public string Voice { get; set; } = string.Empty;

        public Config(string watchFilePath, string mark, string voice)
        {
            WatchFilePath = watchFilePath;
            Mark = mark;
            Voice = voice;
        }

        public static Config Load(string configJsonPath)
        {
            try
            {
                // using var stream = File.OpenRead(configJsonPath);
                //var config = JsonSerializer.Deserialize<Config>(stream);
                var source = File.ReadAllText(configJsonPath);
                var config = JsonSerializer.Deserialize<Config>(source);
                if (config is null)
                {
                    throw new ErrorException("設定ファイルが不正です。");
                }
                if (config.WatchFilePath is null)
                {
                    throw new ErrorException("設定ファイルで WatchFile が設定されていません。");
                }
                if (config.Mark is null)
                {
                    throw new ErrorException("設定ファイルで Mark が設定されていません。");
                }
                if (config.Voice is null)
                {
                    throw new ErrorException("設定ファイルで Chara が設定されていません。");
                }
                return config;
            }

            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException || ex is FileNotFoundException || ex is NotSupportedException)
            {
                throw new ErrorException($"設定ファイルのパスが不正です。{configJsonPath}");
            }
            catch (PathTooLongException)
            {
                throw new ErrorException($"設定ファイルのパスが長すぎます。 {configJsonPath}");
            }
            catch (IOException)
            {
                throw new ErrorException($"設定ファイルの読み込みに失敗しました。{configJsonPath}");
            }
            catch (JsonException ex)
            {
                throw new ErrorException($"設定ファイルの形式が不正です。{ex}");
            }
        }
    }
}
