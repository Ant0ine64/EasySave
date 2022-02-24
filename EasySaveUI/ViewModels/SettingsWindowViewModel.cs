using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using ReactiveUI;
using EasySaveConsole.Model;
using System.Collections.ObjectModel;


namespace EasySaveUI.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand ChangeSettingEvent { get; private set; }
        public ICommand AddBlockingApp { get; private set; }
        public string NewBlockingApp { get; set; } = "";
        public Action? AddEvent;
        public Action<string>? SuccessChangedEvent;
        public EasySaveConsole.Model.Settings settings = new EasySaveConsole.Model.Settings(true);
        public ObservableCollection<String> BlockingApp { get; set; }
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
            AddBlockingApp = ReactiveCommand.Create((object? arg) =>
            {
                BlockingApp.Add(NewBlockingApp);
                settings.BlockingApp.Add(NewBlockingApp);
                settings.Write();
                AddEvent?.Invoke();
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


        public void DeleteBlockingApp(string? rowDataContext)
        {
            if (rowDataContext == null)
                return;
            settings.BlockingApp.Remove(rowDataContext);
            settings.Write(); 
        }
    }
}
