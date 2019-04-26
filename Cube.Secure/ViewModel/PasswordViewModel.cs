using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Cube.Secure.Commands;

namespace Cube.Secure.ViewModel
{
    public class PasswordViewModel
    {
        #region Properties
        private string password;
        public string Password
        {
            get => this.password;
            set
            {
                this.password = value;
                this.OnPropertyChanged(nameof(this.Password));
            }
        }
        #endregion

        #region Commands
        private ICommand windowCloseCommand;
        public ICommand WindowCloseCommand
        {
            get
            {
                return this.windowCloseCommand ?? (this.windowCloseCommand = new RelayCommand(
                           x =>
                           {
                               ((Window)x).Close();
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
