﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using WpfSmartHomeMonitoringApp.Helpers;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class DataBaseViewModel : Screen
    {
        private string brokerUrl;
        public string BrokerUrl
        {
            get { return brokerUrl; }
            set
            {
                brokerUrl = value;
                NotifyOfPropertyChange(() => BrokerUrl);
            }
        }

        private string topic;
        public string Topic
        {
            get { return topic; }
            set
            {
                topic = value;
                NotifyOfPropertyChange(() => Topic);
            }
        }

        private string connString;
        public string ConnString
        {
            get { return connString; }
            set
            {
                connString = value;
                NotifyOfPropertyChange(() => ConnString);
            }
        }

        private string dbLog;
        public string DbLog
        {
            get { return dbLog; }
            set
            {
                dbLog = value;
                NotifyOfPropertyChange(() => DbLog);
            }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected; set
            {
                isConnected = value;
                NotifyOfPropertyChange(() => IsConnected);
            }
        }

        public DataBaseViewModel()
        {
            BrokerUrl = Commons.BROCKERHOST = "127.0.0.1"; //MQTT Broker IP 설정
            Topic = Commons.PUB_TOPIC = "home/device/fakedata/";
            ConnString = Commons.CONNSTRING = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";

            if (Commons.IS_CONNECT)
            {
                IsConnected = true;
                ConnectDb();
            }
        }

        // DB연결 MQTT Broker 접속
        public void ConnectDb()
        {
            if (IsConnected)
            {
                Commons.MQTT_CLIENT = new MqttClient(BrokerUrl);

                try
                {
                    if (Commons.MQTT_CLIENT.IsConnected != true)
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived; ;
                        Commons.MQTT_CLIENT.Connect("MONITOR");
                        Commons.MQTT_CLIENT.Subscribe(new string[] { Commons.PUB_TOPIC },
                            new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

                        IsConnected = Commons.IS_CONNECT = true;
                        UpdateText(">>> MQTT Broker Connected");
                    }
                }
                catch (Exception ex)
                {

                    //Pass
                }
            }
            else
            {
                try
                {
                    if(Commons.MQTT_CLIENT.IsConnected)
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived -= MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Disconnect();

                        UpdateText(">>> MQTT Broker Disconnected...");
                        IsConnected = Commons.IS_CONNECT = false;
                    }
                }
                catch (Exception ex)
                {

                    //Pass
                }
            }
        }
        private void UpdateText(string message)
        {
            DbLog += $"{message}\n";
        }

        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            UpdateText(message);
        }
    }
}
