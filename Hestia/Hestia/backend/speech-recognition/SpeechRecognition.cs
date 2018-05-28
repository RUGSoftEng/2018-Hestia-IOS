using System;
using Speech;
using Foundation;
using AVFoundation;
using Plugin.SimpleAudioPlayer;
using System.Threading;
using System.Collections.Generic;
using Hestia.backend.models;
using Hestia.backend.exceptions;

namespace Hestia.backend.speech_recognition
{
    class SpeechRecognition
    {
        /// <summary>
        /// This class can be used to form a speech dialog with the user.
        /// It has several functions for retrieving the voice and excuting
        /// commands based on what was said.
        /// </summary>

        private AVAudioEngine AudioEngine = new AVAudioEngine();
        private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        private SFSpeechRecognitionTask RecognitionTask;
        private string result = null;
        private bool finished = false;
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
                Console.WriteLine("Couldn't start recording.");
		        return;
            }
            
            if (player.IsPlaying) player.Stop();
            player.Load("Sounds/siri_start.mp3");
            player.Play();
            
            RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) =>
            {
                if (err != null)
                {
                    Console.WriteLine("Error while recording.");
                }
                else
                {
                    if (result.Final)
                    {
                        this.result = result.BestTranscription.FormattedString;
                        DecideAction();
                        finished = true;
                    }
                }
            });
        }

        public void DecideAction()
        {
            List<Device> devices = new List<Device>();
            Device device;
            
            if (result.Contains(value: "activate") || 
                (result.Contains(value: "turn") && result.Contains(value: "on")))
            {
                device = GetDevice(devices);
                if (device != null)
                {
                    SetDevice(device, false);
                }
            } else if (result.Contains(value: "deactivate") ||
                (result.Contains(value: "turn") && result.Contains(value: "off")))
            {
                device = GetDevice(devices);
                if(device != null)
                {
                    SetDevice(device, false);
                }
            }
        }

        public void SetDevice(Device device, bool on_off)
        {
            models.Activator act = device.Activators[0];
            if (act.State.Type == "bool")
            {
                try
                {
                    act.State = new ActivatorState(rawState: on_off, type: "bool");
                }
                catch (ServerInteractionException ex)
                {
                    Console.WriteLine("Exception while changing activator state");
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public Device GetDevice(List<Device> list)
        {
            foreach (Device device in list)
            {
                if (result.Contains(value: device.Name)) {
                    return device;
                }
            }
            return null;
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

            // Wait until speech recognition has finished
            SpinWait.SpinUntil(() => finished);

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
