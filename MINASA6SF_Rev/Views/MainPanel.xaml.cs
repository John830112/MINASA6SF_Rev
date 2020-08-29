using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MINASA6SF_Rev;
using MINASA6SF_Rev.ViewModels;
using Microsoft.Expression.Interactivity.Core;
using System.Windows.Media.Animation;
using System.Threading;

namespace MINASA6SF_Rev.Views
{
    /// <summary>
    /// MainPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPanel : UserControl
    {
        
        static MainPanelViewModel mainPanelViewModel;
        MainWindow mainWindowlocal;

        BlockPara BlockPara = new BlockPara();
        ControlPanel1 ControlPanel1 = new ControlPanel1();
        ServoPara ServoPara;
        Settings Settings;

        public MainPanel() {}

        public MainPanel(MainWindow mainWindow)
        {

            Settings = new Settings();
            mainPanelViewModel = new MainPanelViewModel(Settings, BlockPara, ControlPanel1, this, ServoPara);
            ServoPara = new ServoPara(mainPanelViewModel);

            InitializeComponent();
            mainWindowlocal = mainWindow;
            DataContext = mainPanelViewModel;
            BlockPara.DataContext = mainPanelViewModel;
            ControlPanel1.DataContext = mainPanelViewModel;
            ServoPara.DataContext = mainPanelViewModel;
            Settings.DataContext = mainPanelViewModel;
            mainpanel.DataContext = mainPanelViewModel;
            mainpanel.Navigate(ControlPanel1);

            this.Loaded += UserControl1_Loaded;
        }

        void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing += window_Closing;
        }

        void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
        
        }

        private void PanelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mainWindowlocal.DragMove();
            }
        }

        private void mainpanel_Click(object sender, RoutedEventArgs e)
        {
            mainpanel.Navigate(ControlPanel1);
            if (mainPanelViewModel.mirrtimer.Enabled != true)
            {
                mainPanelViewModel.mirrtimer.Start();
                mainPanelViewModel.timer.Start();
                Debug.WriteLine("Timer Start");
            }
            else
            {
                return;
            }
        }

        private void blockpara_Click(object sender, RoutedEventArgs e)
        {
            mainpanel.Navigate(BlockPara);
            mainPanelViewModel.mirrtimer.Stop();
            mainPanelViewModel.timer.Stop();
            Debug.WriteLine("Timer Stop");
        }

        private void servopara_Click(object sender, RoutedEventArgs e)
        {
            mainpanel.Navigate(ServoPara);
            mainPanelViewModel.mirrtimer.Stop();
            mainPanelViewModel.timer.Stop();
            Debug.WriteLine("Timer Stop");
        }

        private void setting_Click(object sender, RoutedEventArgs e)
        {
            mainpanel.Navigate(Settings);
            mainPanelViewModel.mirrtimer.Stop();
            mainPanelViewModel.timer.Stop();
            Debug.WriteLine("Timer Stop");
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
