using System;
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
using MINASA6SF_Rev;
using MINASA6SF_Rev.ViewModels;

namespace MINASA6SF_Rev.Views
{
    /// <summary>
    /// MainPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPanel : UserControl
    {
        MainWindow mainWindowlocal;
        MainPanelViewModel MainPanelViewModel = new MainPanelViewModel();

        BlockPara BlockPara = new BlockPara();
        ControlPanel1 ControlPanel1 = new ControlPanel1();
        ServoPara ServoPara = new ServoPara();
        Settings Settings = new Settings();

        public MainPanel()
        {

        }

        public MainPanel(MainWindow mainWindow)
        {
            InitializeComponent();
            mainWindowlocal = mainWindow;
            DataContext = MainPanelViewModel;
            BlockPara.DataContext = MainPanelViewModel;
            ControlPanel1.DataContext = MainPanelViewModel;
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
