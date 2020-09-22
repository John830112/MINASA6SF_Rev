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
        bool isDragging;
        public Point startPoint;
        public Point endPoint;
        public Point currentPosition;

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
            mainPanelViewModel.pageindex = 0;
            currentPosition.X = (System.Windows.SystemParameters.PrimaryScreenWidth/2)-(330/2);
            currentPosition.Y = (System.Windows.SystemParameters.PrimaryScreenHeight/2)-(370/2);

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
                Cursor = Cursors.SizeAll;                
                mainWindowlocal.DragMove();
            }
            if(e.LeftButton == MouseButtonState.Released)
            {
                Cursor = Cursors.Arrow;
                currentPosition.X = mainWindowlocal.Left+(mainWindowlocal.Width/2)-(330 / 2); 
                currentPosition.Y = mainWindowlocal.Top+(mainWindowlocal.Height/2)-(370/2);
                
            }
        }


        private void mainpanel_Click(object sender, RoutedEventArgs e)
        {
            mainPanelViewModel.pageindex = 0;
            mainpanel.Navigate(ControlPanel1);
            if (mainPanelViewModel.mirrtimer.Enabled != true)
            {
                mainPanelViewModel.mirrtimer.Start();
                mainPanelViewModel.timer.Start();
                mainPanelViewModel.MirrTimer_Tick();
                Debug.WriteLine("Timer Start");
            }
            else
            {
                return;
            }
        }

        private void blockpara_Click(object sender, RoutedEventArgs e)
        {
            mainPanelViewModel.pageindex = 1;
            mainpanel.Navigate(BlockPara);
            mainPanelViewModel.timer.Stop();
            mainPanelViewModel.mirrtimer.Stop();
            Debug.WriteLine("Timer Stop");
            return;
        }

        private void servopara_Click(object sender, RoutedEventArgs e)
        {
            mainPanelViewModel.pageindex = 2;
            mainpanel.Navigate(ServoPara);
            mainPanelViewModel.timer.Stop();            
            mainPanelViewModel.mirrtimer.Stop();
            Debug.WriteLine("Timer Stop");
            return;
        }

        private void setting_Click(object sender, RoutedEventArgs e)
        {
            mainPanelViewModel.pageindex = 3;
            mainpanel.Navigate(Settings);
            mainPanelViewModel.timer.Stop();
            mainPanelViewModel.mirrtimer.Stop();
            Debug.WriteLine("Timer Stop");
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void panelHeader_MouseMove(object sender, MouseEventArgs e)
        {
            currentPosition.X = mainWindowlocal.Left + (mainWindowlocal.Width / 2)-(330/2);
            currentPosition.Y = mainWindowlocal.Top + (mainWindowlocal.Height / 2)-(370/2);
        }
    }
}
