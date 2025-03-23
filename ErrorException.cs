using System;

namespace AIVoiceYomiage
{
    public class ErrorException : InvalidOperationException
    {
        public ErrorException(string message) : base(message)
        {
        }
    }
}
