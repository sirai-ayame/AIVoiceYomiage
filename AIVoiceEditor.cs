using AI.Talk.Editor.Api;
using System;

namespace AIVoiceYomiage
{
    public class AIVoiceEditor : IDisposable
    {
        private TtsControl _ttsControl;

        public AIVoiceEditor(string voiceName)
        {
            _ttsControl = new TtsControl();
            var hostNames = _ttsControl.GetAvailableHostNames();
            if (hostNames.Length <= 0)
            {
                throw new ErrorException("利用可能なホストが見つかりません。");
            }
            var hostName = hostNames[0];

            var hostIndex = Array.IndexOf(hostNames, hostName);
            if (hostIndex == -1)
            {
                throw new ErrorException("指定されたホスト名が見つかりません。");
            }

            Util.SuccessOrThrow(() => _ttsControl.Initialize(hostName), "A.I.Voice API の初期化に失敗しました。");

            if (_ttsControl.Status == HostStatus.NotRunning)
            {
                Util.SuccessOrThrow(() => _ttsControl.StartHost(), "A.I.Voice ホストの起動に失敗しました。");
            }

            Util.SuccessOrThrow(() => _ttsControl.Connect(), "A.I.Voice ホストへの接続に失敗しました。");

        }

        public void Play(string message)
        {
            if (_ttsControl.Status != HostStatus.Idle)
            {
                return;
            }
            _ttsControl.Text = message;
            _ttsControl.TextSelectionStart = 0;
            _ttsControl.TextSelectionLength = message.Length;

            _ttsControl.Play();
        }

        public void Dispose()
        {
            _ttsControl.Disconnect();
        }
    }
}
