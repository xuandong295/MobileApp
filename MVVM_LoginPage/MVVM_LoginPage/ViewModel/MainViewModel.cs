using Android.App;
using MVVM_LoginPage.Model;
using MVVM_LoginPage.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Xamarin.Forms;
using MVVM_LoginPage.View;

namespace MVVM_LoginPage.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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
        public Command cmdLed1 { get; set; }
        public Command cmdLed2 { get; set; }
        public Command cmdLed3 { get; set; }
        public Command cmdLed4 { get; set; }
        public Command cmdControlAll { get; set; }
        public Command cmdStartSpeech { get; set; }
        public MainViewModel()

        {
            Form1_Load();
            GetDataFromWebAPI();
            cmdLed1 = new Command(controlLed1);
            cmdLed2 = new Command(controlLed2);
            cmdLed3 = new Command(controlLed3);
            cmdLed4 = new Command(controlLed4);
            cmdControlAll = new Command(controlAll);
            cmdStartSpeech = new Command(gotoSpeech);
                
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    Time = DateTime.Now.ToLongTimeString();
                    Date = DateTime.Now.ToLongDateString();
                }
                );
                return true;
            });
        
        }

        private void gotoSpeech(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(new SpeechPage());
        }

        async void GetDataFromWebAPI()
        {
            LedsList = await ICA.GetLedData();
            if (LedsList[0].isOn == "off")
            {
                Led1 = "OFF";
                Color1Background = "Gray";

            }
            else
            {
                Led1 = "ON";
                Color1Background = "YellowGreen";
            }
            if (LedsList[1].isOn == "off")
            {
                Led2 = "OFF";
                Color2Background = "Gray";
            }
            else
            {
                Color2Background = "YellowGreen";
                Led2 = "ON";
            }
            if (LedsList[2].isOn == "off")
            {
                Led3 = "OFF";
                Color3Background = "Gray";
            }
            else
            {
                Color3Background = "YellowGreen";
                Led3 = "ON";
            }
            if (LedsList[3].isOn == "off")
            {
                Led4 = "OFF";
                Color4Background = "Gray";
            }
            else
            {
                Color4Background = "YellowGreen";
                Led4 = "ON";
            }
            LedAll = "Turn on all";
            ColorAllBackground = "YellowGreen";
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
                    client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                    client.Subscribe(new string[] { "ltnc/ledcontrol" }, new byte[] { 1});
                }

            }

            catch (Exception)
            {
            }
        }
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = System.Text.Encoding.UTF8.GetString(e.Message);
            Receive(message);
        }


        void Receive(string message)
        {

            int status = int.Parse(message);
            LEDControl(status);
            return;
        }

        private void LEDControl(int status)
        {
            switch (status)
            {
                case 00:
                    Led1 = "OFF";
                    Color1Background = "Gray";
                    Led2 = "OFF";
                    Color2Background = "Gray";
                    Led3 = "OFF";
                    Color3Background = "Gray";
                    Led4 = "OFF";
                    Color4Background = "Gray";
                    break;
                case 01:
                    Led1 = "ON";
                    Color1Background = "YellowGreen";
                    Led2 = "ON";
                    Color2Background = "YellowGreen";
                    Led3 = "ON";
                    Color3Background = "YellowGreen";
                    Led4 = "ON";
                    Color4Background = "YellowGreen";
                    break;
                case 10:
                    Led1 = "OFF";
                    Color1Background = "Gray";
                    break;
                case 11:
                    Led1 = "ON";
                    Color1Background = "YellowGreen";
                    break;
                case 20:
                    Led2 = "OFF";
                    Color2Background = "Gray";
                    break;
                case 21:
                    Led2 = "ON";
                    Color2Background = "YellowGreen";
                    break;
                case 30:
                    Led3 = "OFF";
                    Color3Background = "Gray";
                    break;

                case 31:
                    Led3 = "ON";
                    Color3Background = "YellowGreen";
                    break;
                case 40:
                    Led4 = "OFF";
                    Color4Background = "Gray";
                    break;
                case 41:
                    Led4 = "ON";
                    Color4Background = "YellowGreen";
                    break;
                default:
                    break;
            }
        }

        private async void controlAll(object obj)
        {
            if (LedAll == "Turn off all")
            {
                LedAll = "Turn on all";
                ColorAllBackground = "YellowGreen";
                Led1 = "OFF";
                Color1Background = "Gray";
                Led2 = "OFF";
                Color2Background = "Gray";
                Led3 = "OFF";
                Color3Background = "Gray";
                Led4 = "OFF";
                Color4Background = "Gray";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("00"));
                for (int i = 1; i <= 4; i++)
                {
                    id = i;
                    led = new LedModel()
                    {
                        ID = i,
                        isOn = "off"
                    };
                    await ICA.RefreshLed(id, led);
                    history = new HistoryModel()
                    {
                        time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        start = "Mobile",
                        end = i,
                        turn = "off"
                    };
                    await ICD.CreateHistory(history);
                }
            }
            else
            {
                LedAll = "Turn off all";
                ColorAllBackground = "Gray";
                Led1 = "ON";
                Color1Background = "YellowGreen";
                Led2 = "ON";
                Color2Background = "YellowGreen";
                Led3 = "ON";
                Color3Background = "YellowGreen";
                Led4 = "ON";
                Color4Background = "YellowGreen";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("01"));
                for (int i = 1; i <= 4; i++)
                {
                    id = i;
                    led = new LedModel()
                    {
                        ID = i,
                        isOn = "on"
                    };
                    await ICA.RefreshLed(id, led);
                    history = new HistoryModel()
                    {
                        time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        start = "Mobile",
                        end = i,
                        turn = "on"
                    };
                    await ICD.CreateHistory(history);
                }
            }

        }

        private void controlLed4(object obj)
        {
            if (Led4 == "ON")
            {
                Led4 = "OFF";
                Color4Background = "Gray";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("40"));
                LedAction(4, "off");
            }
            else
            {
                Led4 = "ON";
                Color4Background = "YellowGreen";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("41"));
                LedAction(4, "on");
            }
            
        }

        private void controlLed3(object obj)
        {
            if (Led3 == "ON")
            {
                Led3 = "OFF";
                Color3Background = "Gray";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("30"));
                LedAction(3, "off");
            }
            else
            {
                Led3 = "ON";
                Color3Background = "YellowGreen";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("31"));
                LedAction(3, "on");
            }

           
        }

        private void controlLed2(object obj)
        {
            if (Led2 == "ON")
            {
                Led2 = "OFF";
                Color2Background = "Gray";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("20"));
                LedAction(2, "off");
            }
            else
            {
                Led2 = "ON";
                Color2Background = "YellowGreen";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("21"));
                LedAction(2, "on");
            }
  
        }

        private void controlLed1(object obj)
        {
            if (Led1 == "ON")   //tat
            {
                Led1 = "OFF";
                Color1Background = "Gray";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("10"));
                LedAction(1, "off");
            }
            else               //bat
            {
                Led1 = "ON";
                Color1Background = "YellowGreen";
                client.Publish("ltnc/ledcontrol", Encoding.UTF8.GetBytes("11"));
                LedAction(1, "on");
            }
            
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

        private string led1;
        public string Led1
        {
            get { return led1; }
            set
            {
                led1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Led1"));
            }
        }
        private string led2;
        public string Led2
        {
            get { return led2; }
            set
            {
                led2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Led2"));
            }
        }
        private string led3;
        public string Led3
        {
            get { return led3; }
            set
            {
                led3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Led3"));
            }
        }
        private string led4;
        public string Led4
        {
            get { return led4; }
            set
            {
                led4 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Led4"));
            }
        }
        private string ledAll;
        public string LedAll
        {
            get { return ledAll; }
            set
            {
                ledAll = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LedAll"));
            }
        }
        private string color1Background;
        public string Color1Background
        {
            get { return color1Background; }
            set
            {
                color1Background = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color1Background"));
            }
        }
        private string color2Background;
        public string Color2Background
        {
            get { return color2Background; }
            set
            {
                color2Background = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color2Background"));
            }
        }
        private string color3Background;
        public string Color3Background
        {
            get { return color3Background; }
            set
            {
                color3Background = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color3Background"));
            }
        }
        private string color4Background;
        public string Color4Background
        {
            get { return color4Background; }
            set
            {
                color4Background = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color4Background"));
            }
        }
        private string colorAllBackground;
        public string ColorAllBackground
        {
            get { return colorAllBackground; }
            set
            {
                colorAllBackground = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorAllBackground"));
            }
        }
        private string date;
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
            }
        }

        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }

    }
}
