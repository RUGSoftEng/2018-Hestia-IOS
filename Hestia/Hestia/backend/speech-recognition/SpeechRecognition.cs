using System;
using Speech;
using Foundation;
using AVFoundation;
using Plugin.SimpleAudioPlayer;

namespace Hestia.backend.speech_recognition
{
    class SpeechRecognition
    {
        private AVAudioEngine AudioEngine = new AVAudioEngine();
        private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        private SFSpeechRecognitionTask RecognitionTask;
        private volatile string result;
        private ISimpleAudioPlayer player;

        public SpeechRecognition()
        {
            player = CrossSimpleAudioPlayer.Current;
        }

        private void RequestAuthorization() {
            // Request user authorization
            SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus status) => {
                // Take action based on status
                switch (status)
                {
                    case SFSpeechRecognizerAuthorizationStatus.Authorized:
                    // User has approved speech recognition
                    break;
                    case SFSpeechRecognizerAuthorizationStatus.Denied:
                    // User has declined speech recognition
                    break;
                    case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
                    // Waiting on approval
                    break;
                    case SFSpeechRecognizerAuthorizationStatus.Restricted:
                    // The device is not permitted
                    break;
                }
            });
        }

        private bool IsAuthorized() {
            if (SFSpeechRecognizer.AuthorizationStatus == SFSpeechRecognizerAuthorizationStatus.Authorized)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public void StartRecording()
        {
            if (!IsAuthorized())
            {
                RequestAuthorization();
                if (!IsAuthorized())
                {
                    return;
                }
            }

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
                Console.WriteLine("Couldn't start recording.");
		        return;
            }

            // Play start sound
            if (player.IsPlaying) player.Stop();
            player.Load("Sounds/siri_start.mp3");
            player.Play();

            // Start recognition
            RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) =>
            {
                // Was there an error?
                if (err != null)
                {
                    Console.WriteLine("Error while recording.");
                }
                else
                {
                    // Is this the final translation?
                    if (result.Final)
                    {
                        this.result = result.BestTranscription.FormattedString;
                    }
                }
            });
        }

        public string StopRecording()
        {
            if (!IsAuthorized())
            {
                return null;
            }

            AudioEngine.Stop();
            LiveSpeechRequest.EndAudio();

            // Play stop sound
            if (player.IsPlaying) player.Stop();
            player.Load("Sounds/siri_stop.mp3");
            player.Play();

            while(result == null) {
                Console.WriteLine("null");
            }

            return result;
        }

        public void CancelRecording()
        {
            if (!IsAuthorized())
            {
                return;
            }

            AudioEngine.Stop();
            RecognitionTask.Cancel();

            // Play cancel sound
            if (player.IsPlaying) player.Stop();
            player.Load("Sounds/siri_cancel.mp3");
            player.Play();
        }
    }
}
