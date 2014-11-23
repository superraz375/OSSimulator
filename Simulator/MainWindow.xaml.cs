﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SimulatorViewModel vm = new SimulatorViewModel();
        public MainWindow()
        {
            InitializeComponent();

            // Initialize the data context to the viewmodel
            this.DataContext = vm;

            vm.ScreenshotEvent += vm_ScreenshotEvent;
        }

        void vm_ScreenshotEvent(object sender, SimpleMvvmToolkit.NotificationEventArgs e)
        {
            var bytes = memoryListBox.GetImage(2, 2);
            File.WriteAllBytes(e.Message + ".png", bytes);
        }
    }
}
