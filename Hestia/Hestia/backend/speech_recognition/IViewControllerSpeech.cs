
namespace Hestia.backend.speech_recognition
{
    /// <summary>
    /// Interface for classes that use speech recognition.
    /// </summary>
    public interface IViewControllerSpeech
    {
        void ProcessSpeech(string result);
    }
}
