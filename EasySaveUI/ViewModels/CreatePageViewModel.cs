﻿using EasySaveUI.Views;
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
using Avalonia.Data.Converters;
using System.Globalization;
using Avalonia.Data;
using EasySaveUI.Enum;

namespace EasySaveUI.ViewModels
{
   
    public class CreatePageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        SaveType saveType { get; set; }
        private MainViewModel mvm = new MainViewModel();
        public Action CloseAction { get; set; }
        public ICommand OnClickBrowseFiles { get; private set; }
        public ICommand OnClickBrowseFolder { get; private set; }
        public ICommand OnClickCreate { get; private set; }
        public bool errormessage { get;  set; }
        string type = "";
        private bool errorMessage;
        public bool ErrorMessage
        {
            get { return errormessage; }
            set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
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

        public CreatePageViewModel()
        {
            OnClickBrowseFiles = ReactiveCommand.Create( async () => { string _path = await GetPathFiles(); });
            OnClickBrowseFolder = ReactiveCommand.Create(async () => { string _path = await GetPathFolder(); });
            
            OnClickCreate = ReactiveCommand.Create(async () => {
                // CreatePage createPage = new CreatePage();
                Debug.WriteLine(saveName);
                Debug.WriteLine(myValueSource);
                Debug.WriteLine(myValueDest);
                Debug.WriteLine(saveType);
                if (saveName == null || myValueSource == null || myValueDest == null)
                {
                    Debug.WriteLine("errormessage");
                    errorMessage = true;
                    Debug.WriteLine(errormessage);
                }
                else
                {
                    var typeSave = saveType == SaveType.Complete ? "c" : "d";
                    mvm.CreateSavingJob(saveName, myValueSource, myValueDest, typeSave);
                }
            });
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
