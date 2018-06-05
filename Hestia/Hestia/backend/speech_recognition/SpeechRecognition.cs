using System;
using Speech;
using Foundation;
using AVFoundation;
using Plugin.SimpleAudioPlayer;
using Hestia.frontend;
using UIKit;
using Hestia.Resources;

namespace Hestia.backend.speech_recognition
{
    /// <summary>
    /// This class can be used to form a speech dialog with the user.
    /// It has several functions for recording voice and excuting
    /// commands based on what was said.
    /// </summary>
    class SpeechRecognition
    {
        private AVAudioEngine AudioEngine = new AVAudioEngine();
        private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        private SFSpeechRecognitionTask RecognitionTask;
        private UIViewController viewController;
        private IViewControllerSpeech viewControllerSpeech;
        private ISimpleAudioPlayer player;

        public SpeechRecognition(UIViewController viewController, IViewControllerSpeech viewControllerSpeech)
        {
            player = CrossSimpleAudioPlayer.Current;
            this.viewController = viewController;
            this.viewControllerSpeech = viewControllerSpeech;
        }

        public static void RequestAuthorization() 
        {
            // Request user authorization
            SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus status) => {
                // Take action based on status
                switch (status)
                {
                    case SFSpeechRecognizerAuthorizationStatus.Authorized:
                        // User has approved speech recognition
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Denied:
                        // User has denied speech recognition
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

        private bool IsAuthorized() 
        {
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
                new WarningMessage(strings.speechAccessDenied, strings.speechAllowAccess, viewController);
                return;
            }
            
            var node = AudioEngine.InputNode;
            var recordingFormat = node.GetBusOutputFormat(0);
            node.InstallTapOnBus(0, 1024, recordingFormat, (AVAudioPcmBuffer buffer, AVAudioTime when) => {
                LiveSpeechRequest.Append(buffer);
            });
            
            AudioEngine.Prepare();
            NSError error;
            AudioEngine.StartAndReturnError(out error);
            
            if (error != null)
            {
                Console.WriteLine(strings.speechStartRecordProblem);
                new WarningMessage(strings.speechStartRecordProblem, strings.tryAgain, viewController);
                return;
            }
            
            if (player.IsPlaying) player.Stop();
            player.Load("Sounds/siri_start.mp3");
            player.Play();
            
            RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) =>
            {
                if (err != null)
                {
                    Console.WriteLine(strings.speechRecordError);
                }
                else
                {
                    if (result.Final)
                    {
                        viewControllerSpeech.ProcessSpeech(result.BestTranscription.FormattedString);
                    }
                }
            });
        }

        public void StopRecording()
        {
            if (!IsAuthorized())
            {
                return;
            }

            AudioEngine.Stop();
            LiveSpeechRequest.EndAudio();

            // Play stop sound
            if (player.IsPlaying) player.Stop();
            player.Load("Sounds/siri_stop.mp3");
            player.Play();
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
