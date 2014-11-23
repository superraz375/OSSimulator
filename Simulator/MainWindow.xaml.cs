using System;
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
        }


        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            //SaveMemoryScreenshots();
        }

        private void SaveMemoryScreenshots()
        {
            Task.Run(() =>
                {
                    for (int i = 0; i < 16; i++)
                    {
                        Thread.Sleep(1000);

                        App.Current.Dispatcher.Invoke((Action)delegate
                          {
                              var bytes = memoryListBox.GetImage(2, 2);
                              File.WriteAllBytes(@"C:\temp\" + i + ".png", bytes);
                          });
                    }
                });
        }
    }
}
