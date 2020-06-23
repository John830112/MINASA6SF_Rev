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
    public class MainPanelViewModel:ViewModelBase, IWindowService
    {
        //Block동작 편집 파라미터 VM Instance
        public ObservableCollection<BlockParaModel1> blockParaModel1s { get; set; }
        ObservableCollection<BlockParaModel1> BlockParaModel1s = new ObservableCollection<BlockParaModel1>();
        BlockSettingDialogs blockSettingDialog;

        public ObservableCollection<BlockFunction> blockFunctions { set; get; }


        //Block매개변수 편집 VM Instance
        public ObservableCollection<BlockParaModel2> blockParaModel2s { set; get; }
        ObservableCollection<BlockParaModel2> BlockParaModel2s = new ObservableCollection<BlockParaModel2>();

        //ControlPanel 콤보박스 변수
        public ObservableCollection<int> SelectBlockNum { get; set; }
        public ObservableCollection<int> BlockAccSpeed { get; set; }
        public ObservableCollection<int> BlockDecSpeed { get; set; }
        public ObservableCollection<int> BlockSpeed { get; set; }

        ObservableCollection<int> selectBlockNum= new ObservableCollection<int>();
        ObservableCollection<int> blockAccSpeed = new ObservableCollection<int>();
        ObservableCollection<int> blockDecSpeed = new ObservableCollection<int>();
        ObservableCollection<int> blockSpeed = new ObservableCollection<int>();

        int selectedBlockNum;
        public int Selected_BlockNum
        {
            get
            {
                return selectedBlockNum;
            }
            set
            {
                SetProperty(ref selectedBlockNum, value);
            }
        }

        int selectedBlockSpeed;
        public int Selected_BlockSpeed
        {
            get
            {
                return selectedBlockSpeed;
            }
            set
            {
                SetProperty(ref selectedBlockSpeed, value);
            }
        }

        int selectedBlockAccSpeed;
        public int Selected_BlockAccSpeed
        {
            get
            {
                return selectedBlockAccSpeed;
            }
            set
            {
                SetProperty(ref selectedBlockAccSpeed, value);
            }
        }

        int selectedBlockDecSpeed;
        public int Selected_BlockDecSpeed
        {
            get
            {
                return selectedBlockDecSpeed;
            }
            set
            {
                SetProperty(ref selectedBlockDecSpeed, value);
            }
        }

        //MainPanel 리스트뷰 버튼 커맨드...
        string framesource="ControlPanel1.xaml";
        public string FrameSource
        {
            get {return framesource;}
            set
            {
                if(framesource.Equals(value))
                { return; }
                SetProperty(ref framesource, value);                
            }
        }

        //MainPanel Frame선택 버튼
        //public ICommand controlPanel { set; get; }
        //public ICommand blockpara { set; get; }
        //public ICommand servopara { set; get; }
        //public ICommand settings { set; get; }
        //public ICommand exit { set; get; }

        //ControlPanel1 제어 버튼
        public ICommand servoOn { set; get; }
        public ICommand stB { set; get; }
        public ICommand a_Clear { set; get; }
        public ICommand s_Stop { set; get; }
        public ICommand h_Stop { get; set; }


        //블럭 동작 편집 커맨드
        public ICommand BlockActDouClick { set; get; }
        public ICommand Setting_Reset { set; get; }
        public ICommand Confirm { set; get; }
        public ICommand Cancel { set; get; }


        public MainPanelViewModel()
        {

            //MainPanel 버튼 커맨드
            //this.controlPanel = new commandModel(ExecuteControlpanel, CanExecuteControlpanel);
            //this.blockpara = new commandModel(ExecuteBlockpara, CanExecuteBlockpara);
            //this.servopara = new commandModel(ExecuteServopara, CanExecuteServopara);
            //this.settings = new commandModel(ExecuteSettings, CanExecuteSettings);
            //this.exit = new commandModel(ExecuteExit, CanExecuteExit);

            //ControlPanel 버튼 커맨드
            this.servoOn = new commandModel(ExecuteServoOn, CanexecuteServoOn);
            this.stB = new commandModel(ExecutestB, CanexecutestB);
            this.a_Clear = new commandModel(Executea_Clear, Canexecutea_Clear);
            this.s_Stop = new commandModel(Executes_Stop, Canexecutes_Stop);
            this.h_Stop = new commandModel(Executeh_Stop, Canexecuteh_Stop);

            //블럭 동작 편집 커맨드
            this.BlockActDouClick = new commandModel(ExecuteBlockActDouClick, CanexecuteBlockActDuoClick);

            this.Setting_Reset = new commandModel(ExecuteSetting_reset, CanexecuteSetting_Rset);
            this.Confirm = new commandModel(ExecuteConfirm, CanexecuteConfirm);
            this.Cancel = new commandModel(ExecuteCancel, CanexecuteCancel);

            //Block동작 편집 파라미터, Block매개변수 편집 VM Instance
            LoadStudents();

            blockFunctions = new ObservableCollection<BlockFunction>()
            {
                new BlockFunction(){Id=0, Name="상대 위치 결정"}
                ,new BlockFunction(){Id=1, Name="절대 위치 결정"}
                ,new BlockFunction(){Id=2, Name="JOG"}
                ,new BlockFunction(){Id=3, Name="원점 복귀"}
                ,new BlockFunction(){Id=4, Name="감속 정지"}
                ,new BlockFunction(){Id=5, Name="속도 갱신"}
                ,new BlockFunction(){Id=6, Name="디크리멘트 카운터 기동"}
                ,new BlockFunction(){Id=6, Name="출력 신호 조작"}
                ,new BlockFunction(){Id=7, Name="점프"}
                ,new BlockFunction(){Id=8, Name="조건 분기(=)"}
                ,new BlockFunction(){Id=9, Name="조건 분기(>)"}
                ,new BlockFunction(){Id=10, Name="조건 분기(<)"}
            };
        }

        private void ExecuteSetting_reset(object parameter)
        {
            Debug.WriteLine("블럭 셋팅 창 리셋 테스트");
        }

        private bool CanexecuteSetting_Rset(object parameter)
        {
            return true;
        }

        private void ExecuteConfirm(object parameter)
        {
            Debug.WriteLine("블럭 셋팅 창 확인 테스트");

        }

        private bool CanexecuteConfirm(object parameter)
        {
            return true;
        }

        private void ExecuteCancel(object parameter)
        {
            Debug.WriteLine("블럭 셋팅 창 취소 테스트");

        }

        private bool CanexecuteCancel(object parameter)
        {
            return true;
        }

        private void ExecuteBlockActDouClick(object parameter)
        {
            Debug.WriteLine("그리드 버튼 테스트");
            showWindow(this);
        }

        private bool CanexecuteBlockActDuoClick(object parameter)
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
                selectBlockNum.Add(i);
                SelectBlockNum = selectBlockNum;
            }
            for(int i=0; i<16; i++)
            {
                blockAccSpeed.Add(i);
                blockDecSpeed.Add(i);
                blockSpeed.Add(i);
                BlockAccSpeed = blockAccSpeed;
                BlockDecSpeed = blockDecSpeed;
                BlockSpeed = blockSpeed;
            }         
        }
    //#region MainPanel1제어
    //    public void ExecuteControlpanel(object parameter)
    //    {
    //        FrameSource = "ControlPanel1.xaml";

    //    }

    //    private bool CanExecuteControlpanel(object parameter)
    //    {
    //        return true;
    //    }

    //    public void ExecuteBlockpara(object parameter)
    //    {
    //        FrameSource = "BlockPara.xaml";
    //    }

    //    private bool CanExecuteBlockpara(object parameter)
    //    {
    //        return true;
    //    }

    //    private void ExecuteServopara(object parameter)
    //    {
    //        FrameSource = "ServoPara.xaml";

    //    }

    //    private bool CanExecuteServopara(object parameter)
    //    {
    //        return true;
    //    }

    //    private void ExecuteSettings(object parameter)
    //    {
    //        FrameSource = "Settings.xaml";
    //    }

    //    private bool CanExecuteSettings(object parameter)
    //    {
    //        return true;
    //    }

    //    private void ExecuteExit(object parameter)
    //    {
    //        System.Windows.Application.Current.Shutdown();
    //    }

    //    private bool CanExecuteExit(object parameter)
    //    {
    //        return true;
    //    }
    //    #endregion

        #region controlpanel button제어
        private void ExecuteServoOn(object parameter)
        {
            //ControlPanel combobox 바인딩 테스트
            Debug.WriteLine(Selected_BlockNum.ToString());
            Debug.WriteLine(Selected_BlockSpeed.ToString());
            Debug.WriteLine(Selected_BlockAccSpeed.ToString());
            Debug.WriteLine(Selected_BlockDecSpeed.ToString());
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

        //블록셋팅 창 오픈
        public void showWindow(object dataContext)
        {
            blockSettingDialog = new BlockSettingDialogs();
            blockSettingDialog.DataContext = this;
            blockSettingDialog.FunctionSelect1.ItemsSource = blockFunctions;
            blockSettingDialog.Show();
            
        }
        #endregion
    }
}
