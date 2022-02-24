using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using ReactiveUI;
using EasySaveConsole.Model;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using EasySaveUI.Views;
using System.Collections.ObjectModel;


namespace EasySaveUI.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand ChangeSettingEvent { get; private set; }
        public Action<string>? SuccessChangedEvent;
        public EasySaveConsole.Model.Settings settings = new EasySaveConsole.Model.Settings(true);
        public ObservableCollection<String> BlockingApp;
        public SettingsWindowViewModel()
        {
            settings.ReadSettings();
            BlockingApp = new ObservableCollection<string>(settings.BlockingApp);
            ChangeSettingEvent = ReactiveCommand.Create((object? arg) =>
            {
                if (!(arg is string type))
                    return;
                settings.LogFormat = type;
                LogFile.selectLogFormat = settings.LogFormat;
                settings.Write();
                SuccessChangedEvent?.Invoke(type.ToUpper());
            });
        }

        public string GetLogFormat()
        {
            return settings.LogFormat;
        }

        private string myValueSource;
        public string ValueSource
        {
            get { return myValueSource; }
            set
            {
                myValueSource = value;
                RaisePropertyChanged("ValueSource");
            }
        }

        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        
    }
}
