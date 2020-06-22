﻿using System;
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
        //Block동작 편집 파라미터 VM Instance
        public ObservableCollection<BlockParaModel1> blockParaModel1s { get; set; }
        ObservableCollection<BlockParaModel1> BlockParaModel1s = new ObservableCollection<BlockParaModel1>();

        //Block매개변수 편집 VM Instance
        public ObservableCollection<BlockParaModel2> blockParaModel2s { set; get; }
        ObservableCollection<BlockParaModel2> BlockParaModel2s = new ObservableCollection<BlockParaModel2>();



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

            //Block동작 편집 파라미터, Block매개변수 편집 VM Instance
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
            //Block동작 편집 Instance생성
            for(int i=0; i<256; i++)
            {
                BlockParaModel1s.Add(new BlockParaModel1() { BlockNum= i, BlockData = "설정 안됨" });
                blockParaModel1s = BlockParaModel1s;
            }

            //Block매개변수 Instance생성
            for (int i=0; i<16; i++)
            {
                BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = i, ParameterName = " 블록 동작 속도 " + $"V{i}", Range = "0 - 20000", SettingValue = 0, Unit = " r/min" });
               // blockParaModel2s = BlockParaModel2s;
            }
            int v = 0;
            for (int i = 16; i < 32; i++)
            {
                BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = i, ParameterName = " 블록 동작 가속도 " + $"A{v}", Range = "0 - 10000", SettingValue = 0, Unit = " ms/(3000r/min)" });
               // blockParaModel2s = BlockParaModel2s;
                ++v;
            }
            int b = 0;
            for (int i = 16; i < 32; i++)
            {
                BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = i, ParameterName = " 블록 동작 감속도 " + $"D{b}", Range = "0 - 10000", SettingValue = 0, Unit = " ms/(3000r/min)" });
               // blockParaModel2s = BlockParaModel2s;
                ++b;
            }

            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 블록 동작 방법 설정 ", Range = "0 - 3", SettingValue = 0, Unit = "" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 블록 동작 원점 오프셋 ", Range = "-2147483648 - 2147483647", SettingValue = 0, Unit = " 지령 단위" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 블록 동작 정방향 소프트웨어 한계값 ", Range = "-2147483648 - 2147483647", SettingValue = 0, Unit = " 지령단위" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 블록 동작 부방향 소프트웨어 한계값 ", Range = "-2147483648 - 2147483647", SettingValue = 0, Unit = " 지령단위" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 블록 동작 시 원점 복귀 속도(고속) ", Range = "0 - 20000", SettingValue = 0, Unit = " r/min" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 블록 동작 시 원점 복귀 속도(저속) ", Range = "0 - 20000", SettingValue = 0, Unit = " r/min" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 블록 동작 원점 복귀 가속도 ", Range = "0 - 10000", SettingValue = 0, Unit = " ms/(3000r/min)" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 원점 복귀 무효화 설정 ", Range = "0 - 1", SettingValue = 0, Unit = "" });
            blockParaModel2s = BlockParaModel2s;

            

            //ControlPanel1 컴보박스 인스턴스 생성
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
