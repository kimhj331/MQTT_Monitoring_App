using MvvmDialogs.DialogTypeLocators;
using System;
using System.ComponentModel;

namespace MqttMonitoringApp.Helpers
{
    public class DialogTypeLocator : IDialogTypeLocator
    {
        /// <summary>
        /// 특정 뷰모델에다 DialogTypeLocator
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public Type Locate(INotifyPropertyChanged viewModel)
        {
            Type viewModelType = viewModel.GetType();

            string dialogFullName = viewModelType.FullName;
            dialogFullName = dialogFullName.Substring(0, dialogFullName.Length - "Model".Length);

            return viewModelType.Assembly.GetType(dialogFullName);
        }
    }
}
