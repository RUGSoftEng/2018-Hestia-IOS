using System;
namespace Hestia.backend.speech_recognition
{
    public interface IViewControllerSpeech
    {
        void ProcessSpeech(string result);
    }
}
