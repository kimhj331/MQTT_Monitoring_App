using Caliburn.Micro;
using MqttMonitoringApp.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttMonitoringApp.ViewModels
{
    class RealTimeViewModel : Conductor<object>
    {
        private double livingTempValue;
        public double LivingTempValue
        {
            get => livingTempValue;
            set
            {
                livingTempValue = value;
                NotifyOfPropertyChange(() => LivingTempValue);
            }
        }

        private double diningRoomTempValue;
        public double DiningRoomTempValue
        {
            get => diningRoomTempValue;
            set
            {
                diningRoomTempValue = value;
                NotifyOfPropertyChange(() => DiningRoomTempValue);
            }
        }

        private double bedRoomTempValue;
        public double BedRoomTempValue
        {
            get => bedRoomTempValue;
            set 
            {
                bedRoomTempValue = value;
                NotifyOfPropertyChange(() => BedRoomTempValue);
            }
        }
        private double livingRoomHumidValue;
        public double LivingRoomHumidValue
        {
            get => livingRoomHumidValue;
            set
            {
                livingRoomHumidValue = value;
                NotifyOfPropertyChange(() => LivingRoomHumidValue);
            }
        }
        
        private double diningRoomHumidValue;
        public double DiningRoomHumidValue
        {
            get => diningRoomHumidValue;
            set
            {
                diningRoomHumidValue = value;
                NotifyOfPropertyChange(() => DiningRoomHumidValue);
            }
        }
        private double bedRoomHumidValue;
        public double BedRoomHumidValue
        {
            get => bedRoomHumidValue;
            set
            {
                bedRoomHumidValue = value;
                NotifyOfPropertyChange(() => BedRoomHumidValue);
            }
        }

        private double livingPressureValue;
        public double LivingPressureValue
        {
            get => livingPressureValue;
            set
            {
                livingPressureValue = value;
                NotifyOfPropertyChange(() => LivingPressureValue);
            }
        }
        private double diningPressureValue;
        public double DiningPressureValue
        {
            get => diningPressureValue;
            set
            {
                diningPressureValue = value;
                NotifyOfPropertyChange(() => DiningPressureValue);
            }
        }
        private double bedRoomPressureValue;
        public double BedRoomPressureValue
        {
            get => bedRoomPressureValue;
            set
            {
                bedRoomPressureValue = value;
                NotifyOfPropertyChange(() => BedRoomPressureValue);
            }
        }
        

        public RealTimeViewModel()
        {
            LivingTempValue = 0;
            if (Commons.BROKERCLIENT != null && Commons.BROKERCLIENT.IsConnected)
            {
                Commons.BROKERCLIENT.MqttMsgPublishReceived += BROKERCLIENT_MqttMsgPublishReceived;
            }
        }

        private void BROKERCLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) //데이터를 받을때마다 해당과정 수행
        {
            var message = Encoding.UTF8.GetString(e.Message);
            var currDates = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

            switch (currDates["Dev_Id"].ToString())
            {
                case "LivingRoom":
                    LivingTempValue = double.Parse(currDates["Temp"]);
                    LivingRoomHumidValue = double.Parse(currDates["Humid"]);
                    LivingPressureValue = double.Parse(currDates["Press"]);
                    break;
                case "DiningRoom":
                    DiningRoomTempValue = double.Parse(currDates["Temp"]);
                    DiningRoomHumidValue = double.Parse(currDates["Humid"]);
                    DiningPressureValue = double.Parse(currDates["Press"]);
                    break;
                case "BedRoom":
                    BedRoomTempValue = double.Parse(currDates["Temp"]);
                    BedRoomHumidValue = double.Parse(currDates["Humid"]);
                    BedRoomPressureValue = double.Parse(currDates["Press"]);
                    break;
                default:
                    break;
            }
        }

    }
}
