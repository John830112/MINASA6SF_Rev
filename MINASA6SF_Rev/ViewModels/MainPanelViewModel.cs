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
        
        //ServoPara para0~para1의 객체생성
        public ObservableCollection<ServoParaModel> para0 { set; get; }
        public ObservableCollection<ServoParaModel> para1 { set; get; }
        public ObservableCollection<ServoParaModel> para2 { set; get; }
        public ObservableCollection<ServoParaModel> para3 { set; get; }
        public ObservableCollection<ServoParaModel> para4 { set; get; }
        public ObservableCollection<ServoParaModel> para5 { set; get; }
        public ObservableCollection<ServoParaModel> para6 { set; get; }
        public ObservableCollection<ServoParaModel> para7 { set; get; }


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

        int blockFunction;
        public int BlockFunctionID
        {
            get { return blockFunction; }
            set { SetProperty(ref blockFunction, value);
                Debug.WriteLine(blockFunction.ToString());
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

        #region 각종 ICommand객체 생성
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

        //블럭 파라미터 수신, 송신, EEP 커맨드
        public ICommand RecCommand { set; get; }
        public ICommand TranCommand { set; get; }
        public ICommand EepCommand { set; get; }
        #endregion

        #region viewmodel 생성자
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

            //블럭 동작 파라미터 설정 창 커맨드
            this.Setting_Reset = new commandModel(ExecuteSetting_reset, CanexecuteSetting_Rset);
            this.Confirm = new commandModel(ExecuteConfirm, CanexecuteConfirm);
            this.Cancel = new commandModel(ExecuteCancel, CanexecuteCancel);

            //블럭 파라미터 수신,송신,EEP 커맨드
            this.RecCommand = new commandModel(ExecuteRecCommand, CanexecuteRecCommand);
            this.TranCommand = new commandModel(ExecuteTransCommand, CanexecuteTransCommand);
            this.EepCommand = new commandModel(ExecuteEepCommand, CanexecuteEepCommand);

            //Block동작 편집 파라미터, Block매개변수 편집 VM Instance
            LoadObjectViewModel();
        }
        #endregion

        #region 블럭 동작 편집, 블럭 매개변수 객체 생성 함수
        private void LoadObjectViewModel()
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
            para0 = new ObservableCollection<ServoParaModel>()
            {
                 new ServoParaModel() { MainIndex = "0", SubIndex = 0, ParaName = "회전 방향 설정", range = "0- 1", SetVal = 1, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 1, ParaName = "제어 모드 설정", range = "0- 6", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 3, ParaName = "실시간 오토뉴닝 기계 강성", range = "0- 31", SetVal = 13, unitVal = "%" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 2, ParaName = "실시간 오토튜닝 설정", range = "0- 6", SetVal = 1, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 4, ParaName = "관성비", range = "0- 10000", SetVal = 250, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 5, ParaName = "지령 펄스 입력 선택", range = "0- 2", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 6, ParaName = "지령 펄스 회전 방향 설정", range = "0- 1", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 7, ParaName = "지령 펄스 입력 모드 설정", range = "0- 3", SetVal = 1, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 8, ParaName = "모터 1회전당 지령 펄스 수", range = "0- 8388608", SetVal = 10000, unitVal = "4체배 후" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 9, ParaName = "제1 지령 분주 체배 분자", range = "0- 1073741824", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 10, ParaName = "지령 분주 체배 분모", range = "0- 1073741824", SetVal = 10000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 11, ParaName = "모터 1회전당 출력 펄스", range = "0- 2907152", SetVal = 2500, unitVal = "4체배 전" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 12, ParaName = "펄스 출력 논리 반전/출력 소스 선택", range = "0- 3", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 13, ParaName = "제1 토크 한계", range = "0- 500", SetVal = 500, unitVal = "%" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 14, ParaName = "위치 편차 과대 설정", range = "0- 1073741824", SetVal = 100000, unitVal = "단위설정에 의존" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 15, ParaName = "앱솔루트 인코더 설정", range = "0- 4", SetVal = 1, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 16, ParaName = "회생 저항 외장 설정", range = "0- 4", SetVal = 3, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 17, ParaName = "외장 회생 저항 부하율 선택", range = "0- 4", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "0", SubIndex = 18, ParaName = "제조사 사용", range = "0- 1", SetVal = 0, unitVal = "---" }
            };
            para1 = new ObservableCollection<ServoParaModel>()
            {
                 new ServoParaModel() { MainIndex = "1", SubIndex = 0, ParaName = "제1 위치 루프 게인", range = "0- 30000", SetVal = 48, unitVal = "1/s" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 1, ParaName = "제1 속도 루프 게인", range = "0.1- 3276.7", SetVal = 27.0, unitVal = "Hz" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 2, ParaName = "제1 속도 루프 적분 시정수", range = "0.1- 1000.0", SetVal = 21.0, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 3, ParaName = "제1 속도 검출 필터", range = "0- 5", SetVal = 0, unitVal = "--" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 4, ParaName = "제1 토크 필터 시정수", range = "0.00- 25.00", SetVal = 0.84, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 5, ParaName = "제2 위치 루프 게인", range = "0.0- 3000.0", SetVal = 48.0, unitVal = "1/s" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 6, ParaName = "제2 속도 루프 게인", range = "0.1- 3276.7", SetVal = 27.0, unitVal = "Hz" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 7, ParaName = "제2 속도 루프 적분 시정수", range = "0.1- 1000.0", SetVal = 21.0, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 8, ParaName = "제2 속도 검출 필터", range = "0- 5", SetVal = 0, unitVal = "--" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 9, ParaName = "제2 토크 필터 시정수", range = "0.00- 25.00", SetVal = 0.84, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 10, ParaName = "속도 피드포워드 게인", range = "0.0- 400.0", SetVal = 100.0, unitVal = "%" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 11, ParaName = "속도 피드포워드 필터", range = "0.00- 64.00", SetVal = 0.00, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 12, ParaName = "토크 피드포워드 게인", range = "0.0- 200.0", SetVal = 100.0, unitVal = "%" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 13, ParaName = "토크 피드포워드 필터", range = "0.00- 64.00", SetVal = 0.00, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 14, ParaName = "제2 게인 설정", range = "0- 1", SetVal = 1, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 15, ParaName = "위치 제어 전환 모드", range = "0- 10", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 16, ParaName = "위치 제어 전환 지연 시간", range = "0.0- 1000.0", SetVal = 1.0, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 17, ParaName = "위치 제어 전환 레벨", range = "0.0- 1000.0", SetVal = 0, unitVal = "모드" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 18, ParaName = "위치 제어 전환 시 히스테리", range = "0- 20000", SetVal = 0, unitVal = "모드" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 19, ParaName = "위치 게인 전환 시간", range = "0.0- 1000.0", SetVal = 1.0, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 20, ParaName = "속도 제어 전환 모드", range = "0- 5", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 21, ParaName = "속도 제어 전환 시간", range = "0.0- 1000.0", SetVal = 480, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 22, ParaName = "속도 제어 전환 레벨", range = "0- 20000", SetVal = 0, unitVal = "모드" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 23, ParaName = "속도 제어 전환 시 히스테리", range = "0- 20000", SetVal = 0, unitVal = "모드" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 24, ParaName = "토크 제어 전환 모드", range = "0- 3", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 25, ParaName = "토크 제어 전환 시간", range = "0.0- 1000.0", SetVal = 0.0, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 26, ParaName = "토크 제어 전환 레벨", range = "0- 20000", SetVal = 0, unitVal = "모드" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 27, ParaName = "토크 제어 전환 시 히스테리", range = "0- 20000", SetVal = 0, unitVal = "모드" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 28, ParaName = "제조사 사용", range = "0- 4000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 29, ParaName = "제조사 사용", range = "0- 2000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 30, ParaName = "제조사 사용", range = "0- 10000", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 31, ParaName = "제조사 사용", range = "0- 30000", SetVal = 480, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 32, ParaName = "제조사 사용", range = "0- 32767", SetVal = 270, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 33, ParaName = "제조사 사용", range = "0- 10000", SetVal = 210, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 34, ParaName = "제조사 사용", range = "0- 2500", SetVal = 84, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 35, ParaName = "제조사 사용", range = "0- 10000", SetVal = 250, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 36, ParaName = "제조사 사용", range = "0- 4000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 37, ParaName = "제조사 사용", range = "0- 2000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 38, ParaName = "제조사 사용", range = "0- 10000", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 39, ParaName = "제조사 사용", range = "0- 30000", SetVal = 480, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 40, ParaName = "제조사 사용", range = "0- 32767", SetVal = 270, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 41, ParaName = "제조사 사용", range = "0- 10000", SetVal = 210, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 42, ParaName = "제조사 사용", range = "0- 2500", SetVal = 84, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 43, ParaName = "제조사 사용", range = "0- 10000", SetVal = 250, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 44, ParaName = "제조사 사용", range = "0- 4000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 45, ParaName = "제조사 사용", range = "0- 2000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 46, ParaName = "제조사 사용", range = "0- 10000", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 47, ParaName = "제조사 사용", range = "0- 30000", SetVal = 480, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 48, ParaName = "제조사 사용", range = "0- 32767", SetVal = 270, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 49, ParaName = "제조사 사용", range = "0- 10000", SetVal = 210, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 50, ParaName = "제조사 사용", range = "0- 2500", SetVal = 84, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 51, ParaName = "제조사 사용", range = "0- 10000", SetVal = 250, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 52, ParaName = "제조사 사용", range = "0- 4000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 53, ParaName = "제조사 사용", range = "0- 2000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 54, ParaName = "제조사 사용", range = "0- 10000", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 55, ParaName = "제조사 사용", range = "0- 30000", SetVal = 480, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 56, ParaName = "제조사 사용", range = "0- 32767", SetVal = 270, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 57, ParaName = "제조사 사용", range = "0- 10000", SetVal = 210, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 58, ParaName = "제조사 사용", range = "0- 2500", SetVal = 84, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 59, ParaName = "제조사 사용", range = "0- 10000", SetVal = 250, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 60, ParaName = "제조사 사용", range = "0- 4000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 61, ParaName = "제조사 사용", range = "0- 2000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 62, ParaName = "제조사 사용", range = "0- 10000", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 63, ParaName = "제조사 사용", range = "0- 30000", SetVal = 480, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 64, ParaName = "제조사 사용", range = "0- 32767", SetVal = 270, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 65, ParaName = "제조사 사용", range = "0- 10000", SetVal = 210, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 66, ParaName = "제조사 사용", range = "0- 2500", SetVal = 84, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 67, ParaName = "제조사 사용", range = "0- 10000", SetVal = 250, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 68, ParaName = "제조사 사용", range = "0- 4000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 69, ParaName = "제조사 사용", range = "0- 2000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 70, ParaName = "제조사 사용", range = "0- 10000", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 71, ParaName = "제조사 사용", range = "0- 30000", SetVal = 480, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 72, ParaName = "제조사 사용", range = "0- 32767", SetVal = 270, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 73, ParaName = "제조사 사용", range = "0- 10000", SetVal = 210, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 74, ParaName = "제조사 사용", range = "0- 2500", SetVal = 84, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 75, ParaName = "제조사 사용", range = "0- 10000", SetVal = 250, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 76, ParaName = "제조사 사용", range = "0- 4000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 77, ParaName = "제조사 사용", range = "0- 2000", SetVal = 1000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "1", SubIndex = 77, ParaName = "제조사 사용", range = "0- 10000", SetVal = 0, unitVal = "---" }
            };
            para2 = new ObservableCollection<ServoParaModel>()
            {
                new ServoParaModel() { MainIndex = "2", SubIndex = 0, ParaName = "적응 필터 모드 설정", range = "0- 6", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 1, ParaName = "제1 노치 주파수", range = "50- 5000", SetVal = 5000, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 2, ParaName = "제1 노치 폭 선택", range = "0- 20", SetVal = 2, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 3, ParaName = "제1 노치 깊이 선택", range = "0- 99", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 4, ParaName = "제2 노치 주파수", range = "50- 5000", SetVal = 5000, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 5, ParaName = "제2 노치 폭 선택", range = "0- 20", SetVal = 2, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 6, ParaName = "제2 노치 깊이 선택", range = "0- 99", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 7, ParaName = "제3 노치 주파수", range = "50- 5000", SetVal = 5000, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 8, ParaName = "제3 노치 폭 선택", range = "0- 20", SetVal = 2, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 9, ParaName = "제3 노치 깊이 선택", range = "0- 99", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 10, ParaName = "제4 노치 주파수", range = "50- 5000", SetVal = 5000, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 11, ParaName = "제4 노치 폭 선택", range = "0- 20", SetVal = 2, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 12, ParaName = "제4 노치 깊이 선택", range = "0- 99", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 13, ParaName = "제진 필터 전환 선택", range = "0- 6", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 14, ParaName = "제1 제진 주파수", range = "0.0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 15, ParaName = "제1 제진 필터 설정", range = "0.0- 150.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 16, ParaName = "제2 제진 주파수", range = "0.0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 17, ParaName = "제2 제진 필터 설정", range = "0.0- 150.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 18, ParaName = "제3 제진 주파수", range = "0.0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 19, ParaName = "제3 제진 필터 설정", range = "0.0- 150.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 20, ParaName = "제4 제진 주파수", range = "0.0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 21, ParaName = "제4 제진 필터 설정", range = "0.0- 150.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 22, ParaName = "지령 스무딩 필터", range = "0.0- 1000.0", SetVal = 9.2, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 23, ParaName = "지령 FIR필터", range = "0.0- 1000.0", SetVal = 1.0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 24, ParaName = "제5 노치 주파수", range = "50- 5000", SetVal = 5000, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 25, ParaName = "제5 노치 폭", range = "0- 20", SetVal = 2, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 26, ParaName = "제5 노치 깊이", range = "0- 99", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 27, ParaName = "제1 제진 폭 설정", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 28, ParaName = "제2 제진 폭 설정", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 29, ParaName = "제3 제진 폭 설정", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 30, ParaName = "제4 제진 폭 설정", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 31, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 32, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 33, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 34, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 35, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 36, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "2", SubIndex = 37, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
            };
            para3 = new ObservableCollection<ServoParaModel>()
            {
                 new ServoParaModel() { MainIndex = "3", SubIndex = 0, ParaName = "속도 설정 내외 전환", range = "0- 3", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 1, ParaName = "속도 지령 방향 지정 선택", range = "0- 1", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 2, ParaName = "속도 지령 입력 게인", range = "10- 2000", SetVal = 500, unitVal = "(r/min)/V" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 3, ParaName = "속도 지령 입력 반전", range = "0- 1", SetVal = 1, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 4, ParaName = "속도 설정 제 1속", range = "-20000- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 5, ParaName = "속도 설정 제 2속", range = "-20000- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 6, ParaName = "속도 설정 제 3속", range = "-20000- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 7, ParaName = "속도 설정 제 4속", range = "-20000- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 8, ParaName = "속도 설정 제 5속", range = "-20000- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 9, ParaName = "속도 설정 제 6속", range = "-20000- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 10, ParaName = "속도 설정 제 7속", range = "-20000- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 11, ParaName = "속도 설정 제 8속", range = "-20000- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 12, ParaName = "가속 시간 설정", range = "0- 10000", SetVal = 0, unitVal = "ms/(1000r/min)" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 13, ParaName = "감속 시간 설정", range = "0- 10000", SetVal = 0, unitVal = "ms/(1000r/min)" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 14, ParaName = "S자 가감속 설정", range = "0- 1000", SetVal = 0, unitVal = "ms" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 15, ParaName = "속도 제로 클램프 기능 선택", range = "0- 3", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 16, ParaName = "속도 제로 클램프 레벨", range = "10- 20000", SetVal = 30, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 17, ParaName = "토크 지령 선택", range = "0- 2", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 18, ParaName = "토크 지령 방향 지정 선택", range = "0- 1", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 19, ParaName = "토크 지령 입력 게인", range = "1.0- 10.0", SetVal = 3.0, unitVal = "V/10" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 20, ParaName = "토크 지령 입력 반전", range = "0- 1", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 21, ParaName = "속도 제한값 1", range = "0- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 22, ParaName = "속도 제한값 2", range = "0- 20000", SetVal = 0, unitVal = "r/min" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 23, ParaName = "외부 스케일 타입 선택", range = "0- 6", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 24, ParaName = "외부 스케일 분주 분자", range = "0- 8388608", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 25, ParaName = "외부 스케일 분주 분모", range = "1- 8388608", SetVal = 10000, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 26, ParaName = "외부 스케일 방향 반전", range = "0- 3", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 27, ParaName = "외부 스케일 Z상 단선 검출", range = "0- 1", SetVal = 0, unitVal = "---" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 28, ParaName = "하이브리드 편차 과대 설정", range = "1- 134217728", SetVal = 16000, unitVal = "지령" }
                ,new ServoParaModel() { MainIndex = "3", SubIndex = 29, ParaName = "하이브리드 편차 클리어 설정", range = "0- 100", SetVal = 0, unitVal = "회전" }
            };
            para4 = new ObservableCollection<ServoParaModel>()
            {
                new ServoParaModel() { MainIndex = "4", SubIndex = 0, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 1, ParaName = "SI2 입력 선택", range = "0- 16777215", SetVal = 8487297, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 2, ParaName = "SI3 입력 선택", range = "0- 16777215", SetVal = 9539850, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 3, ParaName = "SI4 입력 선택", range = "0- 16777215", SetVal = 394758, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 4, ParaName = "SI5 입력 선택", range = "0- 16777215", SetVal = 4108, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 5, ParaName = "SI6 입력 선택", range = "0- 16777215", SetVal = 197379, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 6, ParaName = "SI7 입력 선택", range = "0- 16777215", SetVal = 3847, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 7, ParaName = "SI8 입력 선택", range = "0- 16777215", SetVal = 263172, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 8, ParaName = "SI9 입력 선택", range = "0- 16777215", SetVal = 328965, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 9, ParaName = "SI10 입력 선택", range = "0- 16777215", SetVal = 3720, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 10, ParaName = "SO1 출력 선택", range = "0- 16777215", SetVal = 197379, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 11, ParaName = "SO2 출력 선택", range = "0- 16777215", SetVal = 131586, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 12, ParaName = "SO3 출력 선택", range = "0- 16777215", SetVal = 65793, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 13, ParaName = "SO4 출력 선택", range = "0- 16777215", SetVal = 328964, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 14, ParaName = "SO5 출력 선택", range = "0- 16777215", SetVal = 460551, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 15, ParaName = "SO6 출력 선택", range = "0- 16777215", SetVal = 394758, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 16, ParaName = "아날로그 모니터1 종류", range = "0- 28", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 17, ParaName = "아날로그 모니터1 출력 게인", range = "0- 214748364", SetVal = 0, unitVal = "모니터 단위/V" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 18, ParaName = "아날로그 모니터2 종류", range = "0- 28", SetVal = 4, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 19, ParaName = "아날로그 모니터2 출력 게인", range = "0- 214748364", SetVal = 0, unitVal = "모니터 단위/V" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 20, ParaName = "제조사 사용", range = "0- 3", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 21, ParaName = "아날로그 모니터 출력 설정", range = "0- 2", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 22, ParaName = "아날로그 입력1(AI1)오프셋 설정", range = "-27888- 27888", SetVal = 0, unitVal = "LSB" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 23, ParaName = "아날로그 입력1(AI1)필터 설정", range = "0.00- 64.00", SetVal = 0.00, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 24, ParaName = "아날로그 입력1(AI1)과전압 설정", range = "0.0- 10.0", SetVal = 0.0, unitVal = "V" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 25, ParaName = "아날로그 입력2(AI2)오프셋 설정", range = "-1707- 1707", SetVal = 0, unitVal = "LSB" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 26, ParaName = "아날로그 입력2(AI2)필터 설정", range = "-0.00- 64.00", SetVal = 0.00, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 27, ParaName = "아날로그 입력2(AI2)과전압 설정", range = "0.0- 10.0", SetVal = 0.0, unitVal = "V" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 28, ParaName = "아날로그 입력3(AI3)오프셋 설정", range = "-1707- 1707", SetVal = 0, unitVal = "LSB" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 29, ParaName = "아날로그 입력3(AI3)필터 설정", range = "0.00- 64.00", SetVal = 0.00, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 30, ParaName = "아날로그 입력3(AI3)과전압 설정", range = "-0.0- 10.0", SetVal = 0.0, unitVal = "V" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 31, ParaName = "위치 결정 완료 범위", range = "0- 2097152", SetVal = 10, unitVal = "단위 설정에 의존" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 32, ParaName = "위치 결정 완료 출력 설정", range = "0- 10", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 33, ParaName = "위치 결정 완료 홀딩 시간", range = "0- 30000", SetVal = 0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 34, ParaName = "제로 속도", range = "10- 20000", SetVal = 50, unitVal = "r/min" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 35, ParaName = "속도 일치 폭", range = "10- 20000", SetVal = 50, unitVal = "r/min" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 36, ParaName = "도달 속도", range = "10- 20000", SetVal = 1000, unitVal = "r/min" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 37, ParaName = "정지 시 메카 브레이크 동작 설정", range = "0- 10000", SetVal = 0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 38, ParaName = "동작 시 브레이크 동작 설정", range = "0- 32000", SetVal = 0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 39, ParaName = "브레이크 해제 속도 설정", range = "30- 3000", SetVal = 30, unitVal = "r/min" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 40, ParaName = "경고 출력 선택1", range = "0- 40", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 41, ParaName = "경고 출력 선택2", range = "0- 40", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 42, ParaName = "위치 결정 완료 범위2", range = "0- 2097152", SetVal = 10, unitVal = "단위 설정에 의존" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 44, ParaName = "위치 컴페어 출력 펄스 폭 설정", range = "0.0- 3276.7", SetVal = 0.0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 45, ParaName = "위치 컴페어 출력 극성 선택", range = "0- 63", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 47, ParaName = "펄스 출력 선택", range = "0- 7", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 48, ParaName = "위치 컴페어값1", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "지령 단위" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 49, ParaName = "위치 컴페어값2", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "지령 단위" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 50, ParaName = "위치 컴페어값3", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "지령 단위" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 51, ParaName = "위치 컴페어값4", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "지령 단위" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 52, ParaName = "위치 컴페어값5", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "지령 단위" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 53, ParaName = "위치 컴페어값6", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "지령 단위" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 54, ParaName = "위치 컴페어값7", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "지령 단위" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 55, ParaName = "위치 컴페어값8", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "지령 단위" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 56, ParaName = "위치 컴페어 출력 지연 보상량", range = "-3276.8- 3276.7", SetVal = 0.0, unitVal = "us" }
               ,new ServoParaModel() { MainIndex = "4", SubIndex = 57, ParaName = "위치 컴페어 출력 할당 설정", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "---" }
             };
            para5 = new ObservableCollection<ServoParaModel>()
            {
                new ServoParaModel() { MainIndex = "5", SubIndex = 0, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 1, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 2, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 3, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 4, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 5, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 6, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 7, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 8, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 9, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 10, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 11, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 12, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 13, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 14, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 15, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 16, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 17, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 18, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 19, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 20, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 21, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 22, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 23, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 24, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 25, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 26, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 27, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 28, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 29, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 30, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 31, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 32, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 33, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 34, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 35, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 36, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 37, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 38, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 39, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 40, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 41, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 42, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 43, ParaName = "SI1 입력 선택", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 44, ParaName = "", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 45, ParaName = "상한 돌기 정방향 보상치", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 46, ParaName = "상한 돌기 부방향 보상치", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 47, ParaName = "상한 돌기 보상 지연 시간", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 48, ParaName = "상한 돌기 보상 필터 설정 L", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 49, ParaName = "상한 돌기 보상 필터 설정 H", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 50, ParaName = "제조사 사용", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 51, ParaName = "제조사 사용", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 52, ParaName = "제조사 사용", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 53, ParaName = "제조사 사용", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 54, ParaName = "제조사 사용", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 55, ParaName = "제조사 사용", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 56, ParaName = "Slow Stop 시 감속시간 설정", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 57, ParaName = "Slow Stop 시 S자 가감속 설정", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 58, ParaName = "Modbus 미러 레지스터 설정 1", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 59, ParaName = "Modbus 미러 레지스터 설정 2", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 60, ParaName = "Modbus 미러 레지스터 설정 3", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 61, ParaName = "Modbus 미러 레지스터 설정 4", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 62, ParaName = "Modbus 미러 레지스터 설정 5", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 63, ParaName = "Modbus 미러 레지스터 설정 6", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 64, ParaName = "Modbus 미러 레지스터 설정 7", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 65, ParaName = "Modbus 미러 레지스터 설정 8", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 66, ParaName = "열화 진단 수속 판정시간", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 67, ParaName = "열화 진단 관성비 상한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 68, ParaName = "열화 진단 관성비 하한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 69, ParaName = "열화 진단 편하중 상한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 70, ParaName = "열화 진단 편하중 하한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 71, ParaName = "열화 진단 동마찰 상한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 72, ParaName = "열화 진단 동마찰 하한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 73, ParaName = "열화 진단 점성마찰 상한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 74, ParaName = "열화 진단 점성마찰 하한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 75, ParaName = "열화 진단 속도 설정", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 76, ParaName = "열화 진단 토크 평균 시간", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 77, ParaName = "열화 진단 토크 상한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 78, ParaName = "열화 진단 토크 하한값", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 79, ParaName = "Modbus 미러 레지스터 설정 9", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 80, ParaName = "Modbus 미러 레지스터 설정 10", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 81, ParaName = "Modbus 미러 레지스터 설정 11", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 82, ParaName = "Modbus 미러 레지스터 설정 12", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 83, ParaName = "Modbus 미러 레지스터 설정 13", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 84, ParaName = "Modbus 미러 레지스터 설정 14", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 85, ParaName = "Modbus 미러 레지스터 설정 15", range = "0 - 16777215", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "5", SubIndex = 86, ParaName = "Modbus 미러 레지스터 설정 16", range = "0- 16777215", SetVal = 8553090, unitVal = "---" }
            };
            para6 = new ObservableCollection<ServoParaModel>()
            {
                new ServoParaModel() { MainIndex = "6", SubIndex = 0, ParaName = "아날로그 토크 피드포워드 변환 게인", range = "0.0- 10.0", SetVal = 0.0, unitVal = "V/100%" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 2, ParaName = "속도 편차 과대 설정", range = "0- 20000", SetVal = 0, unitVal = "r/min" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 4, ParaName = "JOG 시운전 지령 속도", range = "0- 500", SetVal = 300, unitVal = "r/min" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 5, ParaName = "위치 제3게인 유효 시간", range = "0.0- 1000.0", SetVal = 0.0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 6, ParaName = "위치 제3게인 배율", range = "50- 1000", SetVal = 100, unitVal = "%" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 7, ParaName = "토크 지령 가산값", range = "-100- 100", SetVal = 0, unitVal = "%" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 8, ParaName = "정방향 토크 보상값", range = "-100- 100", SetVal = 0, unitVal = "%" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 9, ParaName = "부방향 토크 보상값", range = "-100- 100", SetVal = 0, unitVal = "%" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 10, ParaName = "기능 확장 설정", range = "-32768- 32767", SetVal = 16, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 11, ParaName = "전류 응답 설정", range = "10- 300", SetVal = 16, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 13, ParaName = "제2 관성비", range = "0- 10000", SetVal = 250, unitVal = "%" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 14, ParaName = "알람 시 즉시 정지 시간", range = "0- 1000", SetVal = 200, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 15, ParaName = "제2 과속도 레벨 설정", range = "0- 20000", SetVal = 0, unitVal = "r/min" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 16, ParaName = "제조사 사용", range = "0- 1", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 17, ParaName = "전면 패널 매개변수 입력 선택", range = "0- 1", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 18, ParaName = "전원 투입 유휴 시간", range = "0.0- 10.0", SetVal = 0.0, unitVal = "s" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 19, ParaName = "인코더 Z상 설정", range = "0- 32767", SetVal = 0, unitVal = "pulse" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 20, ParaName = "외부 스케일 Z상 설정", range = "0- 400", SetVal = 0, unitVal = "us" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 21, ParaName = "시리얼 앱솔루트 외부 스케일 Z상 설정", range = "0- 268435456", SetVal = 0, unitVal = "외부 스케일 A상 펄스 수" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 22, ParaName = "AB상 외부 스케일 펄스 출력 방법 선택", range = "0- 1", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 23, ParaName = "부하 변동 보상 게인", range = "-100- 100", SetVal = 0, unitVal = "%" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 24, ParaName = "부하 변동 보상 필터", range = "0.10- 25.00", SetVal = 0.53, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 27, ParaName = "경고 래치 시간 선택", range = "0- 10", SetVal = 5, unitVal = "s" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 28, ParaName = "특수 기능 선택", range = "0- 2", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 30, ParaName = "제조사 사용", range = "0- 1", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 31, ParaName = "실시간 오토튜닝 추정 속도", range = "0- 3", SetVal = 1, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 32, ParaName = "실시간 오토튜닝 사용자 정의 설정", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 33, ParaName = "제조사 사용", range = "1000- 3000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 34, ParaName = "하이브리드 진동 억제 게인", range = "0.0- 3000.0", SetVal = 0.0, unitVal = "1/s" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 35, ParaName = "하이브리드 진동 억제 필터", range = "0.00- 320.00", SetVal = 0.10, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 36, ParaName = "다이나믹 브레이크 조작 입력", range = "0- 1", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 37, ParaName = "발진 검출 타이밍", range = "0.0- 100.0", SetVal = 0.0, unitVal = "%" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 38, ParaName = "경고 마스크 설정", range = "-32768- 32767", SetVal = 4, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 39, ParaName = "경고 마스크 설정 2", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 41, ParaName = "제1 제진 깊이", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 42, ParaName = "2단 토크 필터 시정수", range = "0.00- 25.00", SetVal = 0.00, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 43, ParaName = "2단 토크 필터 감쇠 항", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 47, ParaName = "기능 확장 설정 2", range = "-32768- 32767", SetVal = 1, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 48, ParaName = "조정 필터", range = "0.0- 200.0", SetVal = 1.1, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 49, ParaName = "지령 응답 / 조정 필터 감쇠", range = "0- 99", SetVal = 15, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 50, ParaName = "점성 마찰 보상 게인", range = "0.0- 1000.0", SetVal = 0.0, unitVal = "%/(10000r/min)" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 51, ParaName = "즉시 정지 완료 기다리는 시간", range = "0- 10000", SetVal = 0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 52, ParaName = "제조사 사용", range = "0- 95", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 53, ParaName = "제조사 사용", range = "0- 95", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 54, ParaName = "제조사 사용", range = "0- 1", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 57, ParaName = "토크 포화 이상 보호 검출 시간", range = "0- 5000", SetVal = 0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 58, ParaName = "시리얼 앱솔루트 외부 스케일 Z상 이동량", range = "0- 5000", SetVal = 0, unitVal = "외부 스케일 A상 펄스 수" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 60, ParaName = "제2 제진 깊이", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 61, ParaName = "제1 공진 주파소", range = "0.0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 62, ParaName = "제1 공진 감쇠비", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 63, ParaName = "제1 반공진 주파수", range = "0.0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 64, ParaName = "제1 반공진 감쇠비", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 65, ParaName = "제1 응답 주파수", range = "0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 66, ParaName = "제2 공진 주파수", range = "0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 67, ParaName = "제2 공진 감쇠비", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 68, ParaName = "제2 반공진 주파수", range = "0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 69, ParaName = "제2 반공진 감쇠비", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 70, ParaName = "제2 응답 주파수", range = "0- 300.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 71, ParaName = "제3 제진 깊이", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 72, ParaName = "제4 제진 깊이", range = "0- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 73, ParaName = "부하 추정 필터", range = "0.00- 25.00", SetVal = 0.00, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 74, ParaName = "토크 보상 주파수 1", range = "0.0- 500.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 75, ParaName = "토크 보상 주파수 2", range = "0.0- 500.0", SetVal = 0.0, unitVal = "Hz" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 76, ParaName = "부하 추정 횟수", range = "0- 8", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 87, ParaName = "제조사 사용", range = "0- 65534", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 88, ParaName = "앱솔루트 다회전 데이터 상한치", range = "0- 65534", SetVal = 8553090, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 97, ParaName = "기능 확장 설정 3", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "6", SubIndex = 98, ParaName = "기능 확장 설정 4", range = "-2147483648- 2147483647", SetVal = 0, unitVal = "---" }
            };
            para7 = new ObservableCollection<ServoParaModel>()
            {
                new ServoParaModel() { MainIndex = "7", SubIndex = 0, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 1, ParaName = "제조사 사용", range = "-1- 1000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 3, ParaName = "제조사 사용", range = "0- 1", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 4, ParaName = "제조사 사용", range = "0- 2", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 5, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 6, ParaName = "제조사 사용", range = "0- 6400", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 7, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 8, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 9, ParaName = "제조사 사용", range = "-2000- 2000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 10, ParaName = "제조사 사용", range = "0- 3", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 11, ParaName = "제조사 사용", range = "-1073741823- 1073741823", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 12, ParaName = "제조사 사용", range = "-1073741823- 1073741823", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 13, ParaName = "제조사 사용", range = "-1073741823- 1073741823", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 14, ParaName = "주전원 오프 경고 검출 시간", range = "0- 2000", SetVal = 0, unitVal = "ms" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 15, ParaName = "제조사 사용", range = "0- 1073741823", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 16, ParaName = "제조사 사용", range = "0- 30000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 20, ParaName = "제조사 사용", range = "-1- 12", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 21, ParaName = "제조사 사용", range = "-0- 2", SetVal = 1, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 22, ParaName = "특수 기능 확장 설정1", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 23, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 24, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 25, ParaName = "제조사 사용", range = "0- 1", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 26, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 27, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 28, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 29, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 30, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 31, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 32, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 33, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 34, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 35, ParaName = "제조사 사용", range = "0- 2", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 36, ParaName = "제조사 사용", range = "0- 2", SetVal = 0, unitVal = "--- " }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 37, ParaName = "제조사 사용", range = "0- 2", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 38, ParaName = "제조사 사용", range = "0- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 39, ParaName = "제조사 사용", range = "0- 31", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 41, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 87, ParaName = "제조사 사용", range = "-32768- 32767", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 91, ParaName = "제조사 사용", range = "0- 2147483647", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 92, ParaName = "제조사 사용", range = "-2000- 2000", SetVal = 0, unitVal = "---" }
               ,new ServoParaModel() { MainIndex = "7", SubIndex = 93, ParaName = "제조사 사용", range = "0- 2000", SetVal = 0, unitVal = "---" }
            };

        }
        #endregion

        //#region MainPanel1제어 커맨드 함수
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
      
        #region 블럭 셋팅 창 커맨드 함수
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
            blockSettingDialog.Hide();

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
        #endregion

        #region 블럭 파라미터 수신, 송신, EEP버튼 커맨드 함수
        //블럭 파라미터 수신,송신,EEP 커맨드
        private void ExecuteRecCommand(object parameter)
        {
            Debug.WriteLine("수신버튼 테스트");
            BlockParaModel2s[0].SettingValue = 33;                  //첫번째 객체에 값을 기록.
        }

        private bool CanexecuteRecCommand(object parameter)
        {
            return true;
        }

        private void ExecuteTransCommand(object parameter)
        {
            Debug.WriteLine("송신버튼 테스트");
            Debug.WriteLine(BlockParaModel2s[0].SettingValue.ToString());       //첫번째 객체의 값을 가져옴.
        }

        private bool CanexecuteTransCommand(object parameter)
        {
            return true;
        }

        private void ExecuteEepCommand(object parameter)
        {
            Debug.WriteLine("EEP버튼 테스트");
        }

        private bool CanexecuteEepCommand(object parameter)
        {
            return true;
        }
        #endregion
     
        #region ControlPanel 제어 버튼 커맨드 함수
        //ControlPanel 제어버튼
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
        #endregion
       
        //블럭 셋팅 창 오픈
        public void showWindow(object BlocksettingDialog)
        {
            blockSettingDialog = new BlockSettingDialogs();
            blockSettingDialog.DataContext = this;
            blockSettingDialog.FunctionSelect1.ItemsSource = blockFunctions;
            blockSettingDialog.ShowDialog();
        }
       
    }
}
