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
using MINASA6SF_Rev.ViewModels;

namespace MINASA6SF_Rev.Views
{
    /// <summary>
    /// ServoPara.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ServoPara : UserControl
    {
       public para0 Ppara0 = new para0();
       public para1 Ppara1 = new para1();
       public para2 Ppara2 = new para2();
       public para3 Ppara3 = new para3();
       public para4 Ppara4 = new para4();
       public para5 Ppara5 = new para5();
       public para6 Ppara6 = new para6();
       public para7 Ppara7 = new para7();

        public ServoPara(MainPanelViewModel mainPanelViewModel)
        {
            InitializeComponent();
            DataContext = mainPanelViewModel;
            Ppara0.DataContext = mainPanelViewModel;
            Ppara1.DataContext = mainPanelViewModel;
            Ppara2.DataContext = mainPanelViewModel;
            Ppara3.DataContext = mainPanelViewModel;
            Ppara4.DataContext = mainPanelViewModel;
            Ppara5.DataContext = mainPanelViewModel;
            Ppara6.DataContext = mainPanelViewModel;
            Ppara7.DataContext = mainPanelViewModel;
            ParaTreeView.SelectedIndex = 0;
        }

        private void ParaTreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ParaTreeView.SelectedIndex)
            {
                case 0:
                    Para_Count.Navigate(Ppara0);
                    break;
                case 1:
                    Para_Count.Navigate(Ppara1);
                    break;
                case 2:
                    Para_Count.Navigate(Ppara2);
                    break;
                case 3:
                    Para_Count.Navigate(Ppara3);
                    break;
                case 4:
                    Para_Count.Navigate(Ppara4);
                    break;
                case 5:
                    Para_Count.Navigate(Ppara5);
                    break;
                case 6:
                    Para_Count.Navigate(Ppara6);
                    break;
                case 7:
                    Para_Count.Navigate(Ppara7);
                    break;
            }
        }
    }
}
