using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using ReactiveUI;
using EasySaveConsole.Model;
using System.Diagnostics;

namespace EasySaveUI.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand ChangeSettingEvent { get; private set; }
        public Action<string>? SuccessChangedEvent;
        private Settings settings = new Settings(true);
        public SettingsPageViewModel()
        {
            ChangeSettingEvent = ReactiveCommand.Create((object? arg) => {
                if (!(arg is string type))
                    return;
                settings.LogFormat = type;
                LogFile.selectLogFormat = settings.LogFormat;
                settings.Write();
                SuccessChangedEvent?.Invoke(type.ToUpper());
           });
        }
    }

}
