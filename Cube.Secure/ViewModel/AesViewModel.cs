using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Cube.Secure.Commands;
using Cube.Secure.Model;
using Cube.Secure.ViewModel;
using Cube.Secure.Views;
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
        private AES aes;
        public AES Aes
        {
            get
            {
                if (this.aes == null)
                {
                    this.aes = new AES();
                }
                return this.aes;
            }
        }

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

        private string currentDirectory;
        public string CurrentDirectory
        {
            get => this.currentDirectory;
            set
            {
                this.currentDirectory = value;
                this.OnPropertyChanged(nameof(this.CurrentDirectory));
            }
        }

        private List<FileBrowserItem> fileBrowserItems;
        public List<FileBrowserItem> FileBrowserItems
        {
            get => this.fileBrowserItems;
            set
            {
                this.fileBrowserItems = value;
                this.OnPropertyChanged(nameof(this.FileBrowserItems));
            }
        }

        private FileBrowserItem selectedItem;
        public FileBrowserItem SelectedItem
        {
            get => this.selectedItem;
            set
            {
                this.selectedItem = value;
                this.OnPropertyChanged(nameof(this.SelectedItem));
            }
        }

        private List<FileBrowserItem> selectedItems;
        public List<FileBrowserItem> SelectedItems
        {
            get => this.selectedItems;
            set
            {
                this.selectedItems = value;
                this.OnPropertyChanged(nameof(this.SelectedItems));
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
                               var password = GetPasswordFromUser();

                               if (!string.IsNullOrEmpty(password) && this.SelectedItems.Count > 0)
                               {
                                   var allFilePaths = this.GetAllFilePaths();

                                   foreach (var path in allFilePaths)
                                   {
                                       var file = File.ReadAllBytes(path);
                                       var encryptedFile = this.Aes.Encrypt(file, password);
                                       File.WriteAllBytes(path, encryptedFile);
                                   }
                               }
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
                               var password = GetPasswordFromUser();

                               if (!string.IsNullOrEmpty(password) && this.SelectedItems.Count > 0)
                               {
                                   var allFilePaths = this.GetAllFilePaths();

                                   foreach (var path in allFilePaths)
                                   {
                                       var file = File.ReadAllBytes(path);
                                       var encryptedFile = this.Aes.Encrypt(file, password);
                                       var encryptedFileName = this.GetEncryptedFileName(path, password);
                                       File.WriteAllBytes(encryptedFileName, encryptedFile);
                                       File.Delete(path);

                                       this.OnPropertyChanged(nameof(this.CurrentDirectory));
                                   }
                               }
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
                               var password = GetPasswordFromUser();

                               if (!string.IsNullOrEmpty(password) && this.SelectedItems.Count > 0)
                               {
                                   var allFilePaths = this.GetAllFilePaths();

                                   foreach (var path in allFilePaths)
                                   {
                                       var file = File.ReadAllBytes(path);
                                       var decryptedFile = this.Aes.Decrypt(file, password);
                                       File.WriteAllBytes(path, decryptedFile);
                                   }
                               }
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
                               var password = GetPasswordFromUser();

                               if (!string.IsNullOrEmpty(password) && this.SelectedItems.Count > 0)
                               {
                                   var allFilePaths = this.GetAllFilePaths();

                                   foreach (var path in allFilePaths)
                                   {
                                       var file = File.ReadAllBytes(path);
                                       var decryptedFile = this.Aes.Decrypt(file, password);
                                       var decryptedFileName = this.GetDecryptedFileName(path, password);
                                       File.WriteAllBytes(decryptedFileName, decryptedFile);
                                       File.Delete(path);

                                       this.OnPropertyChanged(nameof(this.CurrentDirectory));
                                   }
                               }
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

        private ICommand fileBorwserDoubleClick;
        public ICommand FileBorwserDoubleClick
        {
            get
            {
                return this.fileBorwserDoubleClick ?? (this.fileBorwserDoubleClick = new RelayCommand(
                           x =>
                           {
                               if(this.SelectedItem.IsDirectory)
                                    this.FolderPath = this.selectedItem.Path;
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

            if (propertyName == nameof(this.CurrentDirectory))
            {
                var directoryInfo = new DirectoryInfo(this.CurrentDirectory);

                var back = new FileBrowserItem()
                {
                    Path = directoryInfo.Parent.FullName,
                    Name = "...",
                    Size = 0,
                    IsDirectory = true
                };

                var folders = directoryInfo.GetDirectories().Select(dir => new FileBrowserItem()
                {
                    Path = dir.FullName,
                    Name = dir.Name,
                    Size = 0,
                    IsDirectory = true
                }).ToList();
                folders.Add(back);

                var files = directoryInfo.GetFiles().Select(file => new FileBrowserItem()
                {
                    Path = file.FullName,
                    Name = file.Name,
                    Size = file.Length,
                    IsDirectory = false
                }).ToList();

                this.FileBrowserItems = folders.Concat(files)
                    .OrderByDescending(x => x.IsDirectory)
                    .ThenBy(x => x.Name)                    
                    .ToList();
            }

            if (propertyName == nameof(this.FolderPath))
            {
                this.CurrentDirectory = this.FolderPath;
            }
        }

        private static string GetPasswordFromUser()
        {
            PasswordWindow passwordWindow = new PasswordWindow();
            passwordWindow.ShowDialog();
            var passwordWindowViewModel = passwordWindow.DataContext as PasswordViewModel;
            string password;
            if (passwordWindowViewModel != null)
            {
                return password = passwordWindowViewModel.Password;
            }

            return null;
        }

        private List<string> GetAllFilePaths()
        {
            var allFilePaths = new List<string>();

            foreach (var item in this.SelectedItems)
            {
                if (item.IsDirectory)
                {
                    allFilePaths.AddRange(Directory.GetFiles(item.Path, "*.*",
                        SearchOption.AllDirectories));
                }
                else
                {
                    allFilePaths.Add(item.Path);
                }
            }

            return allFilePaths;
        }

        private string GetEncryptedFileName(string path, string password)
        {
            var filePath = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var bytesFileName = Encoding.UTF8.GetBytes(fileName);
            var encryptedFileName = Convert.ToBase64String(bytesFileName);

            return filePath + Path.DirectorySeparatorChar + encryptedFileName;
        }

        private string GetDecryptedFileName(string path, string password)
        {
            var filePath = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var bytesFileName = Convert.FromBase64String(fileName);
            var decryptedFileName = Encoding.UTF8.GetString(bytesFileName);

            return filePath + Path.DirectorySeparatorChar + decryptedFileName;
        }
        #endregion
    }
}
