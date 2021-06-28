using Android.App;
using Android.OS;
using Android.Runtime;
using MVVM_LoginPage.Model;
using MVVM_LoginPage.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace MVVM_LoginPage.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpeechPage : ContentPage
    {
        private ISpeechToText _speechRecongnitionInstance;
        static MqttClient client;
        ILedService ICA = DependencyService.Get<ILedService>();
        IHistoryService ICD = DependencyService.Get<IHistoryService>();
        private List<LedModel> ledsList;
        public List<LedModel> LedsList
        {
            get
            {
                return ledsList;
            }
            set
            {
                ledsList = value;
            }
        }
        public LedModel led { get; set; }
        public int id { get; set; }
        public HistoryModel history { get; set; }
        public SpeechPage()
        {
            InitializeComponent();
        
            try
            {
                _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }


            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });

            MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
            {
                start.IsEnabled = true;
            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });
            Form1_Load();
        }

  

        private void Form1_Load()
        {
            try
            {
                client = new MqttClient("broker.emqx.io", 1883, false, MqttSslProtocols.None, null, null);
                client.ProtocolVersion = MqttProtocolVersion.Version_3_1;
                byte code = client.Connect(Guid.NewGuid().ToString());
                if (code == 0)
                {
                    //Subcribe Topic
                    client.Subscribe(new string[] { "ltnc/ledcontrol" }, new byte[] { 1 });
                }

            }

            catch (Exception)
            {
            }
        }
        private void SpeechToTextFinalResultRecieved(string args)
        {
            recon.Text = args;
        }

        private void Start_Clicked(object sender, EventArgs e)
        {
            try
            {
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                start.IsEnabled = false;
            }

            stop_btn.IsVisible = true;
            start.IsVisible = false;
        }

        private void stop_btn_Clicked(object sender, EventArgs e)
        {
            switch (recon.Text.ToLower())
            {
                case "turn on switch one":
                case "turn on switch 1":
                case "turn on device one":
                case "turn on device 1":
                case "turn on lamp one":
                case "turn on lamp 1":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("11"));
                    LedAction(1, "on");
                    break;
                case "turn off switch one":
                case "turn off switch 1":
                case "turn off device one":
                case "turn off device 1":
                case "turn off lamp one":
                case "turn off lamp 1":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("10"));
                    LedAction(1, "off");
                    break;
                case "turn on switch two":
                case "turn on switch 2":
                case "turn on device two":
                case "turn on device 2":
                case "turn on lamp two":
                case "turn on lamp 2":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("21"));
                    LedAction(2, "on");
                    break;
                case "turn off switch two":
                case "turn off switch 2":
                case "turn off device two":
                case "turn off device 2":
                case "turn off lamp two":
                case "turn off lamp 2":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("20"));
                    LedAction(2, "off");
                    break;
                case "turn on switch three":
                case "turn on switch 3":
                case "turn on device three":
                case "turn on device 3":
                case "turn on lamp three":
                case "turn on lamp 3":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("31"));
                    LedAction(3, "on");
                    break;
                case "turn off switch three":
                case "turn off switch 3":
                case "turn off device three":
                case "turn off device 3":
                case "turn off lamp three":
                case "turn off lamp 3":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("30"));
                    LedAction(3, "off");
                    break;
                case "turn on switch four":
                case "turn on switch 4":
                case "turn on device four":
                case "turn on device 4":
                case "turn on lamp four":
                case "turn on lamp 4":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("41"));
                    LedAction(4, "on");
                    break;
                case "turn off switch four":
                case "turn off switch 4":
                case "turn off device four":
                case "turn off device 4":
                case "turn off lamp four":
                case "turn off lamp 4":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("40"));
                    LedAction(4, "off");
                    break;
                case "turn on all":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("01"));
                    for (int i = 1; i <= 4; i++)
                    {
                        id = i;
                        led = new LedModel()
                        {
                            ID = i,
                            isOn = "on"
                        };
                        ICA.RefreshLed(id, led);
                        history = new HistoryModel()
                        {
                            time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            start = "Mobile",
                            end = i,
                            turn = "on"
                        };
                        ICD.CreateHistory(history);
                    }
                    break;
                case "turn off all":
                    client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("00"));
                    for (int i = 1; i <= 4; i++)
                    {
                        id = i;
                        led = new LedModel()
                        {
                            ID = i,
                            isOn = "off"
                        };
                        ICA.RefreshLed(id, led);
                        history = new HistoryModel()
                        {
                            time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            start = "Mobile",
                            end = i,
                            turn = "off"
                        };
                        ICD.CreateHistory(history);
                    }
                    break;
                default:
                    fault("Don't understand, please try again!");
                    break;
                
            }
            stop_btn.IsVisible = false;
            start.IsVisible = true;
            
        }

        private async void fault(string faultText)
        {
            await TextToSpeech.SpeakAsync(faultText, new SpeechOptions
            {
                Volume = 0.75f
            });
        }

        private async void LedAction(int _ID, string state)
        {
            id = _ID;
            led = new LedModel()
            {
                ID = _ID,
                isOn = state
            };
            await ICA.RefreshLed(id, led);
            history = new HistoryModel()
            {
                time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                start = "Mobile",
                end = _ID,
                turn = state
            };
            await ICD.CreateHistory(history);
        }
    }
    
}