using Caliburn.Micro;
using MqttMonitoringApp.Helpers;
using System;

namespace MqttMonitoringApp.ViewModels
{
    class MainViewModel:Conductor<object>
    {
        public MainViewModel()
        {
            DisplayName = "MQTT Monitoring App v0.9";
        }
        protected override void OnDeactivate(bool close) //닫힘버튼 눌럿을때
        {
            if (Commons.BROKERCLIENT.IsConnected)
            {
                Commons.BROKERCLIENT.Disconnect(); //연결 끊어줌
                Commons.BROKERCLIENT = null;
            }
            Environment.Exit(0); //완전 종료
        }
        public void ExitProgram()
        {
            Environment.Exit(0);
        }
        public void ExitApp()
        {
           ExitProgram();
        }
        public void LoadRealTimeView()
        {
            if (Commons.BROKERCLIENT != null)
            {
                ActivateItem(new RealTimeViewModel());
            }
            else
            {
                var wManager = new WindowManager();

                wManager.ShowDialog(new ErrorPopupViewModel("ERROR|MQTT가 실행되지 않았습니다."));
            }
        }
        public void LoadDataBaseView()
        {
            if (Commons.BROKERCLIENT != null)
            {
                ActivateItem(new DataBaseViewModel());
            }
            else 
            { 
                var wManager = new WindowManager();

                wManager.ShowDialog(new ErrorPopupViewModel("ERROR|MQTT가 실행되지 않았습니다."));
            }
           
        }
        public void LoadHistoryView()
        {

            ActivateItem(new HistoryViewModel());
        }
        public void PopInfoDialog()
        {
            TaskStart();
        }

        private void TaskStart()
        {
            var wManager = new WindowManager();
            var result = wManager.ShowDialog(new CustomPopupViewModel("New Broker"));
            if (result == true)
            {
                //MessageBox.Show("Start Subscribe!!"); // Here is in Logics...
                ActivateItem(new DataBaseViewModel());
            }
        }
    }
}
