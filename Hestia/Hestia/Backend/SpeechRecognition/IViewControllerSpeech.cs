
namespace Hestia.Backend.SpeechRecognition
{
    /// <summary>
    /// Interface for classes that use speech recognition.
    /// </summary>
    public interface IViewControllerSpeech
    {
        void ProcessSpeech(string result);
    }
}
