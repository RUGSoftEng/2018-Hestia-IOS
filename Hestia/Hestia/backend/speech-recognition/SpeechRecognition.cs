using System;
using UIKit;
using Speech;
using Foundation;
using AVFoundation;

namespace Hestia.backend.speech_recognition
{
    


    class SpeechRecognition
    {

        #region Private Variables
        private AVAudioEngine AudioEngine = new AVAudioEngine();
        private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        private SFSpeechRecognitionTask RecognitionTask;
        #endregion

        public void StartRecording()
        {
            // Setup audio session
            var node = AudioEngine.InputNode;
            var recordingFormat = node.GetBusOutputFormat(0);
            node.InstallTapOnBus(0, 1024, recordingFormat, (AVAudioPcmBuffer buffer, AVAudioTime when) => {
                // Append buffer to recognition request
                LiveSpeechRequest.Append(buffer);
            });

            // Start recording
            AudioEngine.Prepare();
            NSError error;
            AudioEngine.StartAndReturnError(out error);

            // Did recording start?
            if (error != null)
            {
		// Handle error and return
		...
		return;
            }

            // Start recognition
            RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) => {
                // Was there an error?
                if (err != null)
                {
			// Handle error
			...
		}
                else
                {
                    // Is this the final translation?
                    if (result.Final)
                    {
                        Console.WriteLine("You said \"{0}\".", result.BestTranscription.FormattedString);
                    }
                }
            });
        }

        public void StopRecording()
        {
            AudioEngine.Stop();
            LiveSpeechRequest.EndAudio();
        }

        public void CancelRecording()
        {
            AudioEngine.Stop();
            RecognitionTask.Cancel();
        }
    }
}