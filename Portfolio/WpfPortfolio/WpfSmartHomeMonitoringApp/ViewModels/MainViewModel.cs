using Caliburn.Micro;
using System;
using WpfSmartHomeMonitoringApp.Helpers;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class MainViewModel : Conductor<object> //Screen에는 ActivateItem[Async] 메서드 없음
    {
        public MainViewModel()
        {
            DisplayName = "SmartHome Monitoring v2.0"; //윈도우 타이틀 , 제목 
        }

        //TODO

        public void LoadDataBaseView()
        {
            ActivateItemAsync(new DataBaseViewModel());
        }
        public void LoadRealTimeView()
        {
            ActivateItemAsync(new RealTimeViewModel());
        }

        public void LoadHistoryView()
        {
            ActivateItemAsync(new HistoryViewModel());
        }

        public void Exit_Menu()
        {
            ExitProgram();
        }

        public void Exit_Toolbar()
        {
            ExitProgram();
        }

        public void ExitProgram()
        {
            Environment.Exit(0); //프로그램 종료
        }

        // Start메뉴, 아이콘 눌렀을때 처리할 이벤트
        public void PopInfoDialog()
        {
            TaskPopup();
        }

        public void StartSubscribe()
        {
            TaskPopup();
        }

        /*public void Menu_Stop()
        {
            StopSubscribe();
        }

        public void ToolBar_Stop()
        {
            StopSubscribe();
        }

        private void StopSubscribe()
        {
            if(this.ActiveItem is DataBaseViewModel)
            {
                DataBaseViewModel activeModel = (this.ActiveItem as DataBaseViewModel);
                try
                {
                    if(Commons.MQTT_CLIENT.IsConnected)
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived -= activeModel.MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Disconnect();
                        activeModel.IsConnected = Commons.IS_CONNECT = false;
                    }
                }
                catch (Exception ex)
                {
                    //pass
                }
                DeactivateItemAsync(this.ActiveItem, true);
            }
        }*/

        private void TaskPopup()
        {
            //CustomPopupView
            var winManger = new WindowManager();
            var result = winManger.ShowDialogAsync(new CustomPopupViewModel("New Broker"));

            if (result.Result == true)
            {
                ActivateItemAsync(new DataBaseViewModel());
            }
        }
        public void PopInfoView()
        {
            var winManger = new WindowManager();
            winManger.ShowDialogAsync(new CustomInfoViewModel("About"));
        }
    }
}
