using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace MINASA6SF_Rev
{
    /// <summary>
    /// LoadingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingWindow : Window
    {
        MainWindow mainWindow; 
        BackgroundWorker bg;

        public LoadingWindow()
        {
            InitializeComponent();
            bg = new BackgroundWorker();
            bg.WorkerReportsProgress = true;
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.ProgressChanged += bg_ProgressChanged;
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
            mainWindow = new MainWindow();
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
            mainWindow.Show();
        }

        private void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            loadingprogress.Value = e.ProgressPercentage;
        }

        private void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int? maxItems = e.Argument as int?;
            for (int i = 1; i <= maxItems.GetValueOrDefault(); ++i)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                Thread.Sleep(5);
                worker.ReportProgress(i);
            }
        }

        private void Loadingprogress_Loaded(object sender, RoutedEventArgs e)
        {
            int maxItems = 400;
            bg.RunWorkerAsync(maxItems);
        }
    }
}
