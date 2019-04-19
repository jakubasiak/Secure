using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Cube.Secure.Commands;
using Microsoft.Win32;

namespace Cube.Secure.ViewModel
{
    public class AesViewModel: INotifyPropertyChanged
    {
        public AesViewModel()
        {
            this.FolderPath = Properties.Settings.Default.FolderPath;
        }
        #region Properties
        private string folderPath;
        public string FolderPath
        {
            get => this.folderPath;
            set
            {
                this.folderPath = value;
                this.OnPropertyChanged(nameof(this.FolderPath));
            }
        }
        #endregion

        #region Commands
        private ICommand openFolderCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                return this.openFolderCommand ?? (this.openFolderCommand = new RelayCommand(
                           x =>
                           {
                               var ofd = new FolderBrowserDialog();
                               ofd.Description = "Select folder";

                               ofd.ShowDialog();

                               if (!string.IsNullOrEmpty(ofd.SelectedPath))
                               {
                                   this.FolderPath = ofd.SelectedPath;
                               }
                           }
                       ));
            }
        }

        private ICommand encryptCommand;
        public ICommand EncryptCommand
        {
            get
            {
                return this.encryptCommand ?? (this.encryptCommand = new RelayCommand(
                           x =>
                           {

                           }
                       ));
            }
        }

        private ICommand encryptWithFileNamesCommand;
        public ICommand EncryptWithFileNamesCommand
        {
            get
            {
                return this.encryptWithFileNamesCommand ?? (this.encryptWithFileNamesCommand = new RelayCommand(
                           x =>
                           {

                           }
                       ));
            }
        }

        private ICommand decryptCommand;
        public ICommand DecryptCommand
        {
            get
            {
                return this.decryptCommand ?? (this.decryptCommand = new RelayCommand(
                           x =>
                           {

                           }
                       ));
            }
        }

        private ICommand decryptWithFileNamesCommand;
        public ICommand DecryptWithFileNamesCommand
        {
            get
            {
                return this.decryptWithFileNamesCommand ?? (this.decryptWithFileNamesCommand = new RelayCommand(
                           x =>
                           {

                           }
                       ));
            }
        }

        private ICommand windowCloseCommand;
        public ICommand WindowCloseCommand
        {
            get
            {
                return this.windowCloseCommand ?? (this.windowCloseCommand = new RelayCommand(
                           x =>
                           {
                               Properties.Settings.Default.FolderPath = this.FolderPath;
                               Properties.Settings.Default.Save();
                               ((Window) x).Close();
                           }
                       ));
            }
        }
        #endregion

        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));        
        }
        #endregion
    }
}
