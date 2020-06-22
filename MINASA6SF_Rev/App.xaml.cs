using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MINASA6SF_Rev.ViewModels;
using MINASA6SF_Rev.Views;

namespace MINASA6SF_Rev
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        MainPanelViewModel mainPanelViewModel = new MainPanelViewModel();
    }
}
