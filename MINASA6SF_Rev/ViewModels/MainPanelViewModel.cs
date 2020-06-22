using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using MINASA6SF_Rev;
using MINASA6SF_Rev.Models;
using MINASA6SF_Rev.Views;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace MINASA6SF_Rev.ViewModels
{
    public class MainPanelViewModel:INotifyPropertyChanged
    {
        //Block동작 편집 파라미터
        public ObservableCollection<BlockParaModel1> blockParaModel1s { get; set; }
        ObservableCollection<BlockParaModel1> BlockParaModel1s = new ObservableCollection<BlockParaModel1>();

        //ControlPanel 콤보박스 변수
        public ObservableCollection<int> selectBlockNum { get; set; }
        public ObservableCollection<int> blockAccSpeed { get; set; }
        public ObservableCollection<int> blockDecSpeed { get; set; }
        public ObservableCollection<int> blockSpeed { get; set; }

        ObservableCollection<int> SelectBlockNum= new ObservableCollection<int>();
        ObservableCollection<int> BlockAccSpeed = new ObservableCollection<int>();
        ObservableCollection<int> BlockDecSpeed = new ObservableCollection<int>();
        ObservableCollection<int> BlockSpeed = new ObservableCollection<int>();

        //MainPanel 리스트뷰 버튼 커맨드...
        string framesource="ControlPanel1.xaml";
        public string FrameSource
        {
            get {return framesource;}
            set
            {
                if(framesource.Equals(value))
                { return; }
                framesource = value;
                OnPropertyRaised("FrameSource");
            }
        }

        public ICommand controlPanel { set; get; }
        public ICommand blockpara { set; get; }
        public ICommand servopara { set; get; }
        public ICommand settings { set; get; }
        public ICommand exit { set; get; }

        //ControlPanel1 제어 버튼
        public ICommand servoOn { set; get; }
        public ICommand stB { set; get; }
        public ICommand a_Clear { set; get; }
        public ICommand s_Stop { set; get; }
        public ICommand h_Stop { get; set; }

        public MainPanelViewModel()
        {

            //MainPanel 버튼 커맨드
            this.controlPanel = new commandModel(ExecuteControlpanel, CanExecuteControlpanel);
            this.blockpara = new commandModel(ExecuteBlockpara, CanExecuteBlockpara);
            this.servopara = new commandModel(ExecuteServopara, CanExecuteServopara);
            this.settings = new commandModel(ExecuteSettings, CanExecuteSettings);
            this.exit = new commandModel(ExecuteExit, CanExecuteExit);

            //ControlPanel 버튼 커맨드
            this.servoOn = new commandModel(ExecuteServoOn, CanexecuteServoOn);
            this.stB = new commandModel(ExecutestB, CanexecutestB);
            this.a_Clear = new commandModel(Executea_Clear, Canexecutea_Clear);
            this.s_Stop = new commandModel(Executes_Stop, Canexecutes_Stop);
            this.h_Stop = new commandModel(Executeh_Stop, Canexecuteh_Stop);

            LoadStudents();
        }

        private void ExecuteServoOn(object parameter)
        {
            throw new NotImplementedException();
        }

        private bool CanexecuteServoOn(object parameter)
        {
            return true;
        }

        private void ExecutestB(object parameter)
        {
            throw new NotImplementedException();
        }

        private bool CanexecutestB(object parameter)
        {
            return true;
        }

        private void Executea_Clear(object parameter)
        {
            throw new NotImplementedException();
        }

        private bool Canexecutea_Clear(object parameter)
        {
            return true;
        }

        private void Executes_Stop(object parameter)
        {
            throw new NotImplementedException();
        }

        private bool Canexecutes_Stop(object parameter)
        {
            return true;
        }

        private void Executeh_Stop(object parameter)
        {
            throw new NotImplementedException();
        }

        private bool Canexecuteh_Stop(object parameter)
        {
            return true;
        }

        private void LoadStudents()
        {
            for(int i=0; i<256; i++)
            {
                BlockParaModel1s.Add(new BlockParaModel1() { BlockNum= i, BlockData = "설정 안됨" });
                blockParaModel1s = BlockParaModel1s;
            }
            
            for (int i = 0; i < 256; i++)
            {
                SelectBlockNum.Add(i);
                selectBlockNum = SelectBlockNum;
            }
           
            for(int i=0; i<16; i++)
            {
                BlockAccSpeed.Add(i);
                BlockDecSpeed.Add(i);
                BlockSpeed.Add(i);
                blockAccSpeed = BlockAccSpeed;
                blockDecSpeed = BlockDecSpeed;
                blockSpeed = BlockSpeed;
            }
    }

    #region MainPanel1제어
        private void ExecuteControlpanel(object parameter)
        {
            FrameSource = "ControlPanel1.xaml";
        }

        private bool CanExecuteControlpanel(object parameter)
        {
            return true;
        }

        private void ExecuteBlockpara(object parameter)
        {
            FrameSource = "BlockPara.xaml";
        }

        private bool CanExecuteBlockpara(object parameter)
        {
            return true;
        }

        private void ExecuteServopara(object parameter)
        {
            FrameSource = "ServoPara.xaml";
        }

        private bool CanExecuteServopara(object parameter)
        {
            return true;
        }

        private void ExecuteSettings(object parameter)
        {
            FrameSource = "Settings.xaml";
        }

        private bool CanExecuteSettings(object parameter)
        {
            return true;
        }

        private void ExecuteExit(object parameter)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private bool CanExecuteExit(object parameter)
        {
            return true;
        }
        #endregion

        #region ControlPanel 제어


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
