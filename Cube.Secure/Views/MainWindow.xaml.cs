﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cube.Secure.ViewModel;
using MahApps.Metro.Controls;

namespace Cube.Secure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public AesViewModel AesViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.AesViewModel = (AesViewModel)this.AesTab.DataContext;

        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            this.AesViewModel.WindowCloseCommand.Execute(this);
        }
    }
}
