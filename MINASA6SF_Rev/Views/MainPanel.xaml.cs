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

        public MainPanel()
        {

        }

        public MainPanel(MainWindow mainWindow)
        {
            InitializeComponent();
            mainWindowlocal = mainWindow;

        }

        private void PanelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mainWindowlocal.DragMove();
            }
        }
        
    }
}
