using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using MINASA6SF_Rev.Views;

namespace MINASA6SF_Rev
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        LoadingWindow splash;
        BackgroundWorker bg;

        public MainWindow()
        {
            InitializeComponent();
            this.Content = new MainPanel(this);
            
            bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            splash.Close();
            this.Opacity = 1;
        }

        private void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            Debug.WriteLine("....");
        }


        private void Window_Initialized(object sender, EventArgs e)
        {
          
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("종료 하시겠습니까?", "질문", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    System.Windows.Application.Current.Shutdown();
                }
                return;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
         
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0;
            splash = new LoadingWindow();
            splash.Show();
            bg.RunWorkerAsync();
        }
    }
}
