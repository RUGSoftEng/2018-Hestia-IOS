using System;
using UIKit;
using Speech;
using Foundation;
using AVFoundation;
using System.Threading;
using System.Collections.Generic;

namespace Hestia.backend.speech_recognition
{
    class SpeechRecognition
    {
        private AVAudioEngine AudioEngine = new AVAudioEngine();
        private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        private SFSpeechRecognitionTask RecognitionTask;
        private string result;

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
            // Setup audio session
            var node = AudioEngine.InputNode;
            var recordingFormat = node.GetBusOutputFormat(0);
            node.InstallTapOnBus(0, 1024, recordingFormat, (AVAudioPcmBuffer buffer, AVAudioTime when) => {
                // Append buffer to recognition request
                LiveSpeechRequest.Append(buffer);
            });

            if (!IsAuthorized())
            {
                RequestAuthorization();
                if (!IsAuthorized())
                {
                    return;
                }
            }

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
            AudioEngine.Stop();
            LiveSpeechRequest.EndAudio();

            return result;
        }

        public void CancelRecording()
        {
            AudioEngine.Stop();
            RecognitionTask.Cancel();
        }

        private void ProcessAddDevice(String result) 
        {

        }

        private void ProcessRemoveDevice(String result)
        {
            string deviceName = null;
            string[] words = result.Split(' ');

            for (int i = 0; i < words.Length; i++) 
            {
                if(words[i] == "remove" || words[i] == "delete") 
                {
                    deviceName = words[i + 1];
                }
            }

            //Remove device here
        }

        private void ProcessResult(string result) 
        {
            result = result.ToLower();

            //String result = resultString.ToLower();

            if (result.Contains("add device") || result.Contains("new device"))
            {
                ProcessAddDevice(result);
            }
            else if (result.Contains("remove") || result.Contains("delete"))
            {
                ProcessRemoveDevice(result);
            }
        }
    }
}
