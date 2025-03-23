using System;

namespace AIVoiceYomiage
{
    public static class Util
    {
        public static string BaseDirectoryPath => AppDomain.CurrentDomain.BaseDirectory;

        public static void SuccessOrThrow(Action action, string message)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                throw new ErrorException($"{message} {ex.Message}");
            }
        }

    }
}
