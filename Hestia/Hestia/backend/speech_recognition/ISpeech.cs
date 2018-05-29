using System;
namespace Hestia.backend.speech_recognition
{
    public interface ISpeech
    {
        void ProcessSpeech(string result);
    }
}
