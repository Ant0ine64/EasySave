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
    public class SettingsPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<string> BlockingApp { get; private set; }
        public ICommand ChangeSettingEvent { get; private set; }
        public Action<string>? SuccessChangedEvent;
        private EasySaveConsole.Model.Settings settings = new EasySaveConsole.Model.Settings(true);
        public SettingsPageViewModel()
        {
            BlockingApp = new ObservableCollection<string>(FetchBlockingApp());

            settings.ReadSettings();
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

        private List<string> FetchBlockingApp()
        {
            List<string> list = new List<string>
            {
                "EXCEL.EXE",
                "WORD.EXE",
                "COUCOU.EXE"
            };
            return list;
        }
    }
}
