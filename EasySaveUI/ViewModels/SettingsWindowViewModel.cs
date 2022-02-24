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
        public ObservableCollection<BlockingApp> BlockingApp { get; set; }
        public ICommand ChangeSettingEvent { get; private set; }
        public Action<string>? SuccessChangedEvent;
        public EasySaveConsole.Model.Settings settings = new EasySaveConsole.Model.Settings(true);
        public SettingsWindowViewModel()
        {
            BlockingApp = new ObservableCollection<BlockingApp>(FetchBlockingApp());

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

        private List<BlockingApp> FetchBlockingApp()
        {
            var list = new List<BlockingApp>
            {
                new BlockingApp("EXCEL.EXE"),
                new BlockingApp("WORD.EXE"),
                new BlockingApp("COUCOU.EXE"),
                new BlockingApp("EXCEL.EXE"),
                new BlockingApp("WORD.EXE"),
                new BlockingApp("COUCOU.EXE"),
                new BlockingApp("EXCEL.EXE"),
                new BlockingApp("WORD.EXE"),
                new BlockingApp("COUCOU.EXE")
            };
            return list;
        }
    }

    public class BlockingApp
    {
        public string App;
        public BlockingApp(string App)
        {
            this.App = App;
        }
    }

}
