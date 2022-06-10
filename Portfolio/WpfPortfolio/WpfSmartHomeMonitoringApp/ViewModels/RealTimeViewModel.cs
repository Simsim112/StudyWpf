using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSmartHomeMonitoringApp.Helpers;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class RealTimeViewModel : Screen
    {
        private string livingTempVal;
        private string diningTempVal;
        private string bedTempVal;
        private string bathTempVal;

        private string livingHumidVal;
        private string diningHumidVal;
        private string bedHumidVal;
        private string bathHumidVal;

        public string LivingTempVal
        {
            get => livingTempVal; 
            set
            {
                livingTempVal = value;
                NotifyOfPropertyChange(() => livingTempVal);
            }
        }
        public string DiningTempVal
        {
            get => diningTempVal; 
            set
            {
                diningTempVal = value;
                NotifyOfPropertyChange(() => diningTempVal);
            }
        }
        public string BedTempVal
        {
            get => bedTempVal; 
            set
            {
                bedTempVal = value;
                NotifyOfPropertyChange(() => bedTempVal);
            }
        }
        public string BathTempVal
        {
            get => bathTempVal; 
            set
            {
                bathTempVal = value;
                NotifyOfPropertyChange(() => bathTempVal);
            }
        }

        public string LivingHumidVal
        {
            get => livingHumidVal; 
            set
            {
                livingHumidVal = value;
                NotifyOfPropertyChange(() => LivingHumidVal);
            }
        }
        public string DiningHumidVal
        {
            get => diningHumidVal; 
            set
            {
                diningHumidVal = value;
                NotifyOfPropertyChange(() => DiningHumidVal);
            }
        }
        public string BedHumidVal
        {
            get => bedHumidVal; 
            set
            {
                bedHumidVal = value;
                NotifyOfPropertyChange(() => BedHumidVal);
            }
        }
        public string BathHumidVal
        {
            get => bathHumidVal; 
            set
            {
                bathHumidVal = value;
                NotifyOfPropertyChange(() => BathHumidVal);
            }
        }

        public RealTimeViewModel()
        {
            LivingTempVal = DiningTempVal = BedTempVal = BathTempVal = "0";
            
            if(Commons.MQTT_CLIENT != null && Commons.MQTT_CLIENT.IsConnected)
                Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
            else//접속이 안되어 있으면 
            {
                //MQTT Broker에 접속하는 내용
            }
        }

        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            var currDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

            switch (currDatas["DevId"].ToString())
            {
                case "LIVING":
                    LivingTempVal = double.Parse(currDatas["Temp"]).ToString("0.#");
                    break;
                case "DINING":
                    DiningTempVal = double.Parse(currDatas["Temp"]).ToString("0.#");
                    break;
                case "BED":
                    BedTempVal = double.Parse(currDatas["Temp"]).ToString("0.#");
                    break;
                case "BATH":
                    BathTempVal = double.Parse(currDatas["Temp"]).ToString("0.#");
                    break;
                default:
                    break;
                       
                
            }
        }
    }
}
