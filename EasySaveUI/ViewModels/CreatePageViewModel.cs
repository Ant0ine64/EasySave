using EasySaveUI.Views;
using ReactiveUI;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.ComponentModel;
using EasySaveConsole.ViewModel;

namespace EasySaveUI.ViewModels
{
   public class CreatePageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private MainViewModel mvm = new MainViewModel();
        public ICommand OnClickBrowseFiles { get; private set; }
        public ICommand OnClickBrowseFolder { get; private set; }
        public ICommand OnClickCreate { get; private set; }
        string type = "";
        public CreatePageViewModel()
        {
            OnClickBrowseFiles = ReactiveCommand.Create( async () => {
               
                string _path = await GetPathFiles();
            });
            OnClickBrowseFolder = ReactiveCommand.Create(async () => {

                string _path = await GetPathFolder();
            });
            OnClickCreate = ReactiveCommand.Create(async () => {

               
                Debug.WriteLine(saveName);
                Debug.WriteLine(myValueSource);
                Debug.WriteLine(myValueDest);
                Debug.WriteLine(type);
                mvm.CreateSavingJob(saveName, myValueSource, myValueDest, "test");

            });
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
        private string saveName;
        public string SaveNameField
        {
            get { return saveName; }
            set
            {
                saveName = value;
                RaisePropertyChanged("SaveNameField");
            }
        }
        public async Task<string> GetPathFiles()
        {
            var dialogFolderSource = new OpenFolderDialog();
            var result = await dialogFolderSource.ShowAsync(CreatePage.Instance);

            if (result != null)
            {
                ValueSource = result;
                Trace.WriteLine("DIR IS: " + result);
            }

            return result;

          
        }

        private string myValueDest;
        public string ValueDestination
        {
            get { return myValueDest; }
            set
            {
                myValueDest = value;
                RaisePropertyChanged("ValueDestination");
            }
        }
        public async Task<string> GetPathFolder()
        {
            var dialogFolderDest = new OpenFolderDialog();
            var result = await dialogFolderDest.ShowAsync(CreatePage.Instance);

            if (result != null)
            {
                ValueDestination = result;
                Trace.WriteLine("DIR IS: " + result);
            }

            return result;
        }
        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
