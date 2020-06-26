using System;
using System.Collections.Generic;
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
using MINASA6SF_Rev;
using MINASA6SF_Rev.ViewModels;

namespace MINASA6SF_Rev.Views
{
    /// <summary>
    /// MainPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPanel : UserControl
    {
        static MainPanelViewModel MainPanelViewModel;
        MainWindow mainWindowlocal;

        BlockPara BlockPara = new BlockPara();
        ControlPanel1 ControlPanel1 = new ControlPanel1();
        ServoPara ServoPara = new ServoPara(MainPanelViewModel);
        Settings Settings = new Settings();
        
        //Abs_Position_Page2 Abs_Position_Page2 = new Abs_Position_Page2();
        //ConditionDiv_Page10 ConditionDiv_Page10 = new ConditionDiv_Page10();
        //ConditionDiv_Page11 ConditionDiv_Page11 = new ConditionDiv_Page11();
        //ConditionDiv_Page12 ConditionDiv_Page12 = new ConditionDiv_Page12();
        //DecrementCount_Page7 DecrementCount_Page7 = new DecrementCount_Page7();
        //DecStop_Page5 DecStop_Page5 = new DecStop_Page5();
        //HomeReturn_Page4 HomeReturn_Page4 = new HomeReturn_Page4();
        //IncPosition_Page1 IncPosition_Page1 = new IncPosition_Page1();
        //JOG_Operation_Page3 JOG_Operation_Page3 = new JOG_Operation_Page3();
        //Jump_Page9 Jump_Page9 = new Jump_Page9();
        //OutPutSignal_Page8 OutPutSignal_Page8 = new OutPutSignal_Page8();
        //SpeedUpdate_Page6 SpeedUpdate_Page6 = new SpeedUpdate_Page6();

        public MainPanel() {}

        public MainPanel(MainWindow mainWindow)
        {
            InitializeComponent();
            MainPanelViewModel = new MainPanelViewModel(Settings);
            mainWindowlocal = mainWindow;
            DataContext = MainPanelViewModel;
            BlockPara.DataContext = MainPanelViewModel;
            ControlPanel1.DataContext = MainPanelViewModel;
            ServoPara.DataContext = MainPanelViewModel;
            Settings.DataContext = MainPanelViewModel;
            mainpanel.Navigate(ControlPanel1);
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
        }

        private void blockpara_Click(object sender, RoutedEventArgs e)
        {
            mainpanel.Navigate(BlockPara);
        }

        private void servopara_Click(object sender, RoutedEventArgs e)
        {
            mainpanel.Navigate(ServoPara);
        }

        private void setting_Click(object sender, RoutedEventArgs e)
        {
            mainpanel.Navigate(Settings);
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
