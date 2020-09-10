using Caliburn.Micro;

namespace MqttMonitoringApp.ViewModels
{
    class ErrorPopupViewModel : Conductor<object>
    {
        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set {
                errorMessage = value;
                NotifyOfPropertyChange(() => errorMessage);
            }
        }

        public ErrorPopupViewModel(string param)
        {
            var tmp = param.Split('|');
            DisplayName = tmp[0];
            ErrorMessage = tmp[1];
        }

        public void ConfirmClose()
        {
            TryClose(true);
        }
    }
}
