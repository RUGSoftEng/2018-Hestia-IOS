using System;
using Speech;
using Foundation;
using AVFoundation;
using Plugin.SimpleAudioPlayer;
using UIKit;
using Hestia.Resources;
using Hestia.Backend.Models;

namespace Hestia.Backend.SpeechRecognition
{
    /// <summary>
    /// This class can be used to form a speech dialog with the user.
    /// It has several functions for recording voice and excuting
    /// commands based on what was said.
    /// </summary>
    class SpeechRecognition
    {
        AVAudioEngine AudioEngine = new AVAudioEngine();
        SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        SFSpeechRecognitionTask RecognitionTask;
        IViewControllerSpeech viewController;
        ISimpleAudioPlayer player;
        enum Warning { AccessDenied = 1, RecordProblem };

        public SpeechRecognition(IViewControllerSpeech viewController)
        {
            player = CrossSimpleAudioPlayer.Current;
            this.viewController = viewController;
        }

        public static void RequestAuthorization() 
        {
            SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus status) => {});
        }

        bool IsAuthorized()
        {
            if (SFSpeechRecognizer.AuthorizationStatus == SFSpeechRecognizerAuthorizationStatus.Authorized)
            {
                return true;
            }
            return false;
        }

        public void StartRecording(out int warningStatus)
        {
            if (!IsAuthorized())
            {
                warningStatus = (int)Warning.AccessDenied;
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
                warningStatus = (int)Warning.RecordProblem;
                return;
            }
            
            // Play start sound
            if (player.IsPlaying) player.Stop();
            player.Load("Sounds/siri_start.mp3");
            player.Play();
            
            RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) =>
            {
                if (err != null)
                {
                    Console.WriteLine(strings.speechRecordError);
                    viewController.ProcessSpeech(null);
                }
                else
                {
                    if (result.Final)
                    {
                        viewController.ProcessSpeech(result.BestTranscription.FormattedString);
                    }
                }
            });

            warningStatus = -1;
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
