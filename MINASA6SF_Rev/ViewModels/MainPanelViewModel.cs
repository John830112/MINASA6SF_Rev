using System;
using System.Collections.Generic;
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
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Net;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Interactivity;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Collections;

namespace MINASA6SF_Rev.ViewModels
{
    public class MainPanelViewModel : ViewModelBase, IWindowService
    {
        BackgroundWorker worker = new BackgroundWorker();  //MirrorTimer 동작
        BackgroundWorker worker2 = new BackgroundWorker(); //블럭 파라미터 송신
        bool mirrorONOFF;
        bool servoON;
        int mirrTime;
        byte[] _servoONStatus;
        int lampstatus = 0;
        public int LampStatus
        {
            get { return lampstatus; }
            set { SetProperty(ref lampstatus, value); }
        }


        byte[] _alarmStatus = new byte[2];
        int alarmStatus = 0;
        public int AlarmStatus
        {
            get { return alarmStatus; }
            set { SetProperty(ref alarmStatus, value); }
        }

        bool _modbusOnStatus = false;
        public bool ModbusOnStatus
        {
            get { return _modbusOnStatus; }
            set { SetProperty(ref _modbusOnStatus, value); }
        }


        byte[] _errorCode = new byte[2];
        ushort _maincode = 0;
        ushort _subcode = 0;

        string errorcode = "00.0";
        public string ErrorCode
        {
            get { return errorcode; }
            set { SetProperty(ref errorcode, value); }
        }


        ushort _eeprom = 0x6173;
        byte[] _eepromwrite = new byte[4];



        byte[] blockNumSelect = new byte[2];
        byte[] selectedBlock = new byte[2];
        byte[] selectedBlock2 = new byte[2];
        int axisNum1 = 1;


        float overload1;
        float torquecmd;
        Int32 powerontimetemp;

        private Master modbusTCP = new Master();
        Settings settings;
        BlockPara blockpara;

        public ObservableCollection<int> axisNum { set; get; }
        ObservableCollection<int> axisNums = new ObservableCollection<int>();
        public ObservableCollection<int> cycTime { set; get; }
        ObservableCollection<int> cycTimes = new ObservableCollection<int>();

        //Block동작 편집 파라미터 VM Instance
        public ObservableCollection<BlockParaModel1> blockParaModel1s { get; set; }
        ObservableCollection<BlockParaModel1> BlockParaModel1s = new ObservableCollection<BlockParaModel1>();
        BlockSettingDialogs blockSettingDialog;
        public ObservableCollection<BlockFunction> blockFunctions { set; get; }

        //BlockSettingDialog Frame 페이지 생성
        IncPosition_Page1 incPosition_Page1 = new IncPosition_Page1();
        Abs_Position_Page2 abs_Position_Page2 = new Abs_Position_Page2();
        JOG_Operation_Page3 jOG_Operation_Page3 = new JOG_Operation_Page3();
        HomeReturn_Page4 homeReturn_Page4 = new HomeReturn_Page4();
        DecStop_Page5 decStop_Page5 = new DecStop_Page5();
        SpeedUpdate_Page6 speedUpdate_Page6 = new SpeedUpdate_Page6();
        DecrementCount_Page7 cecrementCount_Page7 = new DecrementCount_Page7();
        OutPutSignal_Page8 outPutSignal_Page8 = new OutPutSignal_Page8();
        Jump_Page9 jump_Page9 = new Jump_Page9();
        ConditionDiv_Page10 conditionDiv_Page10 = new ConditionDiv_Page10();
        ConditionDiv_Page11 conditionDiv_Page11 = new ConditionDiv_Page11();
        ConditionDiv_Page12 conditionDiv_Page12 = new ConditionDiv_Page12();

        //Block매개변수 편집 VM Instance
        public ObservableCollection<BlockParaModel2> blockParaModel2s { set; get; }
        ObservableCollection<BlockParaModel2> BlockParaModel2s = new ObservableCollection<BlockParaModel2>();

        //ServoPara para0~para1의 객체생성
        public ObservableCollection<ServoParaModel> para0 { set; get; }
        public ObservableCollection<ServoParaModel> para1 { set; get; }
        public ObservableCollection<ServoParaModel> para2 { set; get; }
        public ObservableCollection<ServoParaModel> para3 { set; get; }
        public ObservableCollection<ServoParaModel> para4 { set; get; }
        public ObservableCollection<ServoParaModel> para5 { set; get; }
        public ObservableCollection<ServoParaModel> para6 { set; get; }
        public ObservableCollection<ServoParaModel> para7 { set; get; }
        //ControlPanel 콤보박스 변수
        public ObservableCollection<int> SelectBlockNum { get; set; }
        public ObservableCollection<int> BlockAccSpeed { get; set; }
        public ObservableCollection<int> BlockDecSpeed { get; set; }
        public ObservableCollection<int> BlockSpeed { get; set; }
        ObservableCollection<int> selectBlockNum = new ObservableCollection<int>();
        ObservableCollection<int> blockAccSpeed = new ObservableCollection<int>();
        ObservableCollection<int> blockDecSpeed = new ObservableCollection<int>();
        ObservableCollection<int> blockSpeed = new ObservableCollection<int>();



        /*------------------------------------------------------------------------------------------------------
         *BlockSettingDialog 각 기능별 셋팅 변수
         *  01h 상대이동
         *  02h 절대이동
         *  03h JOG 무한장 운전
         *  04h 원점복귀
         *  05h 감속정지
         *  06h 속도갱신
         *  07h 디크리멘트 카운터 기동
         *  08h 출력신호조작
         *  09h 점프
         *  0Ah 조건분기(=)
         *  0Bh 조건분기(>)
         *  0Ch 조건분기(<)
          ------------------------------------------------------------------------------------------------------*/

        //BlockSetting 변수        
        ushort cmdcode01h = 0x01;
        ushort cmdcode02h = 0x02;
        ushort cmdcode03h = 0x03;
        ushort cmdcode04h = 0x04;
        ushort cmdcode05h = 0x05;
        ushort cmdcode06h = 0x06;
        ushort cmdcode07h = 0x07;
        ushort cmdcode08h = 0x08;
        ushort cmdcode09h = 0x09;
        ushort cmdcode0Ah = 0x0A;
        ushort cmdcode0Bh = 0x0B;
        ushort cmdcode0Ch = 0x0C;
        byte[] value1 = new byte[4];
        byte[] value11 = new byte[4];

        byte[] recValue1;
        byte[] recValue2;
        byte[] recValue3;
        byte[] recValue4;
        byte[] recValue5;
        byte[] recValue6;
        byte[] recValue7;
        byte[] recValue8;
        byte[] recValue9;
        byte[] recValue10;
        byte[] recValue11;
        byte[] recValue12;
        byte[] recValue13;
        byte[] recValue14;
        byte[] recValue15;
        byte[] recValue16;
        byte[] recValue17;
        byte[] recValue18;
        byte[] recValue19;
        byte[] recValue20; 
        byte[] recValue21;
        byte[] recValue22;
        byte[] recValue23;
        byte[] recValue24;
        byte[] recValue25;
        byte[] recValue26;
        byte[] recValue27;
        byte[] recValue28;
        byte[] recValue29;
        byte[] recValue30;
        byte[] recValue31;
        byte[] recValue32;
        byte[] recValue33;
        byte[] recValue34;
        byte[] recValue35;
        byte[] recValue36;
        byte[] recValue37;
        byte[] recValue38;
        byte[] recValue39;
        byte[] recValue40;
        byte[] recValue41;
        byte[] recValue42;
        byte[] recValue43;
        byte[] recValue44;
        byte[] recValue45;
        byte[] recValue46;
        byte[] recValue47;
        byte[] recValue48;
        byte[] recValue49;
        byte[] recValue50;
        byte[] recValue51;
        byte[] recValue52;
        byte[] recValue53;
        byte[] recValue54;
        byte[] recValue55;
        byte[] recValue56;
        byte[] recValue57;
        byte[] recValue58;
        byte[] recValue59;
        byte[] recValue60;
        byte[] recValue61;
        byte[] recValue62;
        byte[] recValue63;
        byte[] recValue64;
        byte[] recValue65;
        byte[] recValue66;
        byte[] recValue67;
        byte[] recValue68;
        byte[] recValue69;
        byte[] recValue70;
        byte[] recValue71;
        byte[] recValue72;
        byte[] recValue73;
        byte[] recValue74;
        byte[] recValue75;
        byte[] recValue76;
        byte[] recValue77;
        byte[] recValue78;
        byte[] recValue79;
        byte[] recValue80;
        byte[] recValue81;
        byte[] recValue82;
        byte[] recValue83;
        byte[] recValue84;
        byte[] recValue85;
        byte[] recValue86;
        byte[] recValue87;
        byte[] recValue88;
        byte[] recValue89;
        byte[] recValue90;
        byte[] recValue91;
        byte[] recValue92;
        byte[] recValue93;
        byte[] recValue94;
        byte[] recValue95;
        byte[] recValue96;
        byte[] recValue97;
        byte[] recValue98;
        byte[] recValue99;
        byte[] recValue100;
        byte[] recValue101;
        byte[] recValue102;
        byte[] recValue103;
        byte[] recValue104;
        byte[] recValue105;
        byte[] recValue106;
        byte[] recValue107;
        byte[] recValue108;
        byte[] recValue109;
        byte[] recValue110;
        byte[] recValue111;
        byte[] recValue112;
        byte[] recValue113;
        byte[] recValue114;
        byte[] recValue115;
        byte[] recValue116;
        byte[] recValue117;
        byte[] recValue118;
        byte[] recValue119;
        byte[] recValue120;
        byte[] recValue121;
        byte[] recValue122;
        byte[] recValue123;
        byte[] recValue124;
        byte[] recValue125;
        byte[] recValue126;
        byte[] recValue127;
        byte[] recValue128;
        byte[] recValue129;
        byte[] recValue130;
        byte[] recValue131;
        byte[] recValue132;
        byte[] recValue133;
        byte[] recValue134;
        byte[] recValue135;
        byte[] recValue136;
        byte[] recValue137;
        byte[] recValue138;
        byte[] recValue139;
        byte[] recValue140;
        byte[] recValue141;
        byte[] recValue142;
        byte[] recValue143;
        byte[] recValue144;
        byte[] recValue145;
        byte[] recValue146;
        byte[] recValue147;
        byte[] recValue148;
        byte[] recValue149;
        byte[] recValue150;
        byte[] recValue151;
        byte[] recValue152;
        byte[] recValue153;
        byte[] recValue154;
        byte[] recValue155;
        byte[] recValue156;
        byte[] recValue157;
        byte[] recValue158;
        byte[] recValue159;
        byte[] recValue160;
        byte[] recValue161;
        byte[] recValue162;
        byte[] recValue163;
        byte[] recValue164;
        byte[] recValue165;
        byte[] recValue166;
        byte[] recValue167;
        byte[] recValue168;
        byte[] recValue169;
        byte[] recValue170;
        byte[] recValue171;
        byte[] recValue172;
        byte[] recValue173;
        byte[] recValue174;
        byte[] recValue175;
        byte[] recValue176;
        byte[] recValue177;
        byte[] recValue178;
        byte[] recValue179;
        byte[] recValue180;
        byte[] recValue181;
        byte[] recValue182;
        byte[] recValue183;
        byte[] recValue184;
        byte[] recValue185;
        byte[] recValue186;
        byte[] recValue187;
        byte[] recValue188;
        byte[] recValue189;
        byte[] recValue190;
        byte[] recValue191;
        byte[] recValue192;
        byte[] recValue193;
        byte[] recValue194;
        byte[] recValue195;
        byte[] recValue196;
        byte[] recValue197;
        byte[] recValue198;
        byte[] recValue199;
        byte[] recValue200;
        byte[] recValue201;
        byte[] recValue202;
        byte[] recValue203;
        byte[] recValue204;
        byte[] recValue205;
        byte[] recValue206;
        byte[] recValue207;
        byte[] recValue208;
        byte[] recValue209;
        byte[] recValue210;
        byte[] recValue211;
        byte[] recValue212;
        byte[] recValue213;
        byte[] recValue214;
        byte[] recValue215;
        byte[] recValue216;
        byte[] recValue217;
        byte[] recValue218;
        byte[] recValue219;
        byte[] recValue220;
        byte[] recValue221;
        byte[] recValue222;
        byte[] recValue223;
        byte[] recValue224;
        byte[] recValue225;
        byte[] recValue226;
        byte[] recValue227;
        byte[] recValue228;
        byte[] recValue229;
        byte[] recValue230;
        byte[] recValue231;
        byte[] recValue232;
        byte[] recValue233;
        byte[] recValue234;
        byte[] recValue235;
        byte[] recValue236;
        byte[] recValue237;
        byte[] recValue238;
        byte[] recValue239;
        byte[] recValue240; 
        byte[] recValue241;
        byte[] recValue242;
        byte[] recValue243;
        byte[] recValue244;
        byte[] recValue245;
        byte[] recValue246;
        byte[] recValue247;
        byte[] recValue248;
        byte[] recValue249;
        byte[] recValue250; 
        byte[] recValue251;
        byte[] recValue252;
        byte[] recValue253;
        byte[] recValue254;
        byte[] recValue255;
        


        //상대이동 및 절대이동
        ushort spdnum = 0;
        ushort accnum = 0;
        ushort decnum = 0;
        ushort movdir = 0;
        ushort blockchif = 0;
        Int32 targetposition = 0;
        public Int32 TargetPosition
        {
            get { return targetposition; }
            set { SetProperty(ref targetposition, value); }
        }
     
        ushort hiki3;
        ushort hiki4;
        ushort hiki5;
        ushort hiki1;
        ushort hiki2;


        //StatusBar 변수
        string statusBar = "";
        public string StatusBar
        {
            get { return statusBar; }
            set { SetProperty(ref statusBar, value); }
        }

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
                blockNumSelect[0] = (byte)(selectedBlockNum >> 8);
                blockNumSelect[1] = (byte)(selectedBlockNum);
                modbusTCP.WriteSingleRegister(0, 0x01, 0x4414, blockNumSelect);
            }
        }

        ushort selectBlockNumMon1;
        public ushort SelectBlockNumMon1
        {
            get
            {
                return selectBlockNumMon1;
            }
            set
            {
                SetProperty(ref selectBlockNumMon1, value);

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
        string framesource = "ControlPanel1.xaml";
        public string FrameSource
        {
            get { return framesource; }
            set
            {
                if (framesource.Equals(value))
                { return; }
                SetProperty(ref framesource, value);
            }
        }

        #region ModbusTCP MirrReg Value
        /*------------------------------------------------------------------------------------------------------
         * ModbusTCP MirrReg value
         ------------------------------------------------------------------------------------------------------*/
        byte[] _mirrReg1;
        byte[] _mirrReg2;

        /*------------------------------------------------------------------------------------------------------
        * ModbusTCP WriteRegister value
        ------------------------------------------------------------------------------------------------------*/
        byte[] jogBlockSelect = new byte[2];    //jogBlockSelect
        byte[] alarmClear = new byte[4];       //Warning, Alarm 클리어
        ushort Alarmclear = 29300;
        /*------------------------------------------------------------------------------------------------------
         * MirrorReg 0 ~ 8
          ------------------------------------------------------------------------------------------------------*/
        //모터 실위치  0x600F
        Int32 _positionActualValue = 0;
        public Int32 PositionActualValue
        {
            get { return _positionActualValue; }
            set { SetProperty(ref _positionActualValue, value); }
        }
        byte[] positionactualvalue = new byte[4];

        //모터 속도   0x4D06
        Int32 _velotcityActualValue = 0;
        public Int32 VelocityActualValue
        {
            get { return _velotcityActualValue; }
            set { SetProperty(ref _velotcityActualValue, value); }
        }
        byte[] velocityactualvalue = new byte[4];

        //Torque demand   0x4D08
        float _torqueDemand = 0;
        public float TorqueDemand
        {
            get { return _torqueDemand; }
            set { SetProperty(ref _torqueDemand, value); }
        }
        byte[] torquedemand = new byte[4];

        //부하율   0x4D12
        float _overLoad = 0.0f;
        public float OverLoad
        {
            get { return _overLoad; }
            set { SetProperty(ref _overLoad, value); }
        }
        byte[] overload = new byte[4];

        /*------------------------------------------------------------------------------------------------------
         * MirrorReg 9 ~ 16
          ------------------------------------------------------------------------------------------------------*/
        //현재 유효한 블록 NO    0x4416
        Int16 _blockNumMon = 0;
        public Int16 BlockNumMon
        {
            get { return _blockNumMon; }
            set { SetProperty(ref _blockNumMon, value); }
        }
        byte[] blocknummon = new byte[2];

        //주전원 PN간 전압   0x602C
        Int32 _dcLinkCircuitVolt = 0;
        public Int32 DCLinkCircuitvolt
        {
            get { return _dcLinkCircuitVolt; }
            set { SetProperty(ref _dcLinkCircuitVolt, value); }
        }
        byte[] dclinkcircuitvolt = new byte[4];

        //앰프 온도    0x4D30
        Int16 _ampTemp = 0;
        public Int16 AmpTemp
        {
            get { return _ampTemp; }
            set { SetProperty(ref _ampTemp, value); }
        }
        byte[] amptemp = new byte[2];


        //엔코더 온도   0x4D32
        Int32 _encoderTemp = 0;
        public Int32 EncoderTemp
        {
            get { return _encoderTemp; }
            set { SetProperty(ref _encoderTemp, value); }
        }
        byte[] encodertemp = new byte[4];


        //전원 ON 적산 시간   0x4D2C
        Int32 _powerONTime = 0;
        public Int32 PowerONTime
        {
            get { return _powerONTime; }
            set { SetProperty(ref _powerONTime, value); }
        }
        byte[] powerontime = new byte[4];


        //현재 유효한 블록 번호
        int blockFunction;
        public int BlockFunctionID
        {
            get { return blockFunction; }
            set
            {
                SetProperty(ref blockFunction, value);
                Debug.WriteLine(blockFunction.ToString());
            }
        }
        #endregion

        #region 각종 ICommand객체 생성
        //ControlPanel1 제어 버튼
        public ICommand servoOn { set; get; }
        public ICommand stB { set; get; }
        public ICommand a_Clear { set; get; }
        public ICommand s_Stop { set; get; }
        public ICommand h_Stop { get; set; }
        
        //BlockSettingDialogCloseCommand
        public ICommand BlockSettingDialogCloseCommand { get; set; }

        //블럭 동작 편집 커맨드
        public ICommand BlockActDouClick { set; get; }
        public ICommand Setting_Reset { set; get; }
        public ICommand Confirm { set; get; }
        public ICommand Cancel { set; get; }
        public ICommand funSelection { set; get; }

        //블럭 파라미터 수신, 송신, EEP 커맨드
        public ICommand RecCommand { set; get; }
        public ICommand TranCommand { set; get; }
        public ICommand EepCommand { set; get; }
        //ServoParameter 수신, 송신,
        public ICommand SPReCommand { set; get; }
        public ICommand SPTranCommand { set; get; }
        //Settings 커맨드
        public ICommand SettingConfirm { set; get; }
        public ICommand Disconnect { set; get; }
        //Mouse 커맨드
        public ICommand jogrewind1 { set; get; }
        public ICommand jogrewind2 { set; get; }
        public ICommand jogplaybtn1 { set; get; }
        public ICommand jogplaybtn2 { set; get; }
        public ICommand jogpause1 { set; get; }
        public ICommand jogpause2 { set; get; }
        public ICommand jogplaybtn3 { set; get; }
        public ICommand jogplaybtn4 { set; get; }
        public ICommand jogfastford1 { set; get; }
        public ICommand jogfastford2 { set; get; }
        #endregion

        #region viewmodel 생성자
        public MainPanelViewModel() { }

        public MainPanelViewModel(Settings _settings, BlockPara _blockpara)
        {
            mirrorONOFF = false;
            settings = _settings;
            blockpara = _blockpara;

            //BlockParaDialog Page Datacontext 설정
            incPosition_Page1.DataContext = this;
            abs_Position_Page2.DataContext = this;

            //ControlPanel 버튼 커맨드
            this.servoOn = new commandModel(ExecuteServoOn, CanexecuteServoOn);
            this.stB = new commandModel(ExecutestB, CanexecutestB);
            this.a_Clear = new commandModel(Executea_Clear, Canexecutea_Clear);
            this.s_Stop = new commandModel(Executes_Stop, Canexecutes_Stop);
            this.h_Stop = new commandModel(Executeh_Stop, Canexecuteh_Stop);

            //BlockSettingDialogClose 커맨드
            this.BlockSettingDialogCloseCommand = new commandModel(ExecuteBlockSettingDialogCloseCommand, CanexecuteBlockSettingDialogCloseCommand);

            //블럭 동작 편집 커맨드
            this.BlockActDouClick = new commandModel(ExecuteBlockActDouClick, CanexecuteBlockActDuoClick);

            //블럭 동작 파라미터 설정 창 커맨드
            this.Setting_Reset = new commandModel(ExecuteSetting_reset, CanexecuteSetting_Rset);
            this.Confirm = new commandModel(ExecuteConfirm, CanexecuteConfirm);
            this.Cancel = new commandModel(ExecuteCancel, CanexecuteCancel);
            this.funSelection = new commandModel(ExecuteFunSelection, CanexecutefunSelection);

            //블럭 파라미터 수신,송신,EEP 커맨드
            this.RecCommand = new commandModel(ExecuteRecCommand, CanexecuteRecCommand);
            this.TranCommand = new commandModel(ExecuteTransCommand, CanexecuteTransCommand);
            this.EepCommand = new commandModel(ExecuteEepCommand, CanexecuteEepCommand);

            //ServoParameter 수신, 송신,
            this.SPReCommand = new commandModel(ExecuteSPReCommand, CanexecuteSPRCommand);
            this.SPTranCommand = new commandModel(ExecuteSPTranCommand, CanexecuteSPTranCommand);

            //Setting 커맨드
            this.SettingConfirm = new commandModel(ExecuteSettingsConfirm, CanexecuteSettingsConfirm);
            this.Disconnect = new commandModel(ExecuteDisconnect, CanexecuteDisconnect);

            //JOG Mouse  커맨드
            this.jogrewind1 = new commandModel(Executejogrewind1, Canexecutejogrewind1);        //빠른 부방향 시작
            this.jogrewind2 = new commandModel(Executejogrewind2, Canexecutejogrewind2);        //빠른 부방향 정지

            this.jogplaybtn1 = new commandModel(Executejogplaybtn1, Canexecutejogplaybtn1);     //느린 부방향 시작
            this.jogplaybtn2 = new commandModel(Executejogplaybtn2, Canexecutejogplaybtn2);     //느린 부방향 정지

            this.jogpause1 = new commandModel(Executejogpause1, Canexecutejogpause1);           //Pause  보류
            this.jogpause2 = new commandModel(Executejogpause2, Canexecutejogpause2);           //Pause  보류

            this.jogplaybtn3 = new commandModel(Executejogplaybtn3, Canexecutejogplaybtn3);     //느린 정방향 시작
            this.jogplaybtn4 = new commandModel(Executejogplaybtn4, Canexecutejogplaybtn4);     //느린 정방향 정지

            this.jogfastford1 = new commandModel(Executejogfastford1, Canexecutejogfastford1);  //빠른 부방향 시작
            this.jogfastford2 = new commandModel(Executejogfastford2, Canexecutejogfastford2);  //빠른 부방향 정지

            //축수 등록
            for (int i = 1; i <= 30; i++)
            {
                axisNums.Add(i);
            }
            axisNum = axisNums;

            //CycleTime설정
            cycTimes.Add(3);
            cycTime = cycTimes;

            //Block동작 편집 파라미터, Block매개변수 편집 VM Instance
            LoadObjectViewModel();
            worker.WorkerReportsProgress = false;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += MirrTimer_Tick;

            worker2.WorkerReportsProgress = true;
            worker2.WorkerSupportsCancellation = false;
            worker2.DoWork += BlockParameterRec;

            //BlockSettingDialog 객체 할당
            blockSettingDialog = new BlockSettingDialogs();
            blockSettingDialog.DataContext = this;
            blockSettingDialog.FunctionSelect1.ItemsSource = blockFunctions;
            blockSettingDialog.BlockActionParaWindow.Navigate(incPosition_Page1);


        }    
        #endregion

        #region BlockSettingDialog Frame Command함수
        private void ExecuteFunSelection(object parameter)
        {
            switch (blockSettingDialog.FunctionSelect1.SelectedIndex)
            {
                case 0:
                    blockSettingDialog.BlockActionParaWindow.Navigate(incPosition_Page1);
                    break;
                case 1:
                    blockSettingDialog.BlockActionParaWindow.Navigate(abs_Position_Page2);
                    break;
                case 2:
                    blockSettingDialog.BlockActionParaWindow.Navigate(jOG_Operation_Page3);
                    break;
                case 3:
                    blockSettingDialog.BlockActionParaWindow.Navigate(homeReturn_Page4);
                    break;
                case 4:
                    blockSettingDialog.BlockActionParaWindow.Navigate(decStop_Page5);
                    break;
                case 5:
                    blockSettingDialog.BlockActionParaWindow.Navigate(speedUpdate_Page6);
                    break;
                case 6:
                    blockSettingDialog.BlockActionParaWindow.Navigate(cecrementCount_Page7);
                    break;
                case 7:
                    blockSettingDialog.BlockActionParaWindow.Navigate(outPutSignal_Page8);
                    break;
                case 8:
                    blockSettingDialog.BlockActionParaWindow.Navigate(jump_Page9);
                    break;
                case 9:
                    blockSettingDialog.BlockActionParaWindow.Navigate(conditionDiv_Page10);
                    break;
                case 10:
                    blockSettingDialog.BlockActionParaWindow.Navigate(conditionDiv_Page11);
                    break;
                case 11:
                    blockSettingDialog.BlockActionParaWindow.Navigate(conditionDiv_Page12);
                    break;
            }
        }
        private bool CanexecutefunSelection(object parameter)
        {
            return true;
        }

        private void ExecuteBlockSettingDialogCloseCommand(object parameter)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    blockSettingDialog.Opacity -= 0.1;
            //}

            blockSettingDialog.Hide();
        }

        private bool CanexecuteBlockSettingDialogCloseCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region JOG 버튼 동작 
        /*------------------------------------------------------------------------------------------------------
          JOG버튼 동작
         *------------------------------------------------------------------------------------------------------*/

        //빠른 부방향 시작
        private void Executejogrewind1(object parameter)
        {
            jogBlockSelect[0] = (byte)(252 >> 8);
            jogBlockSelect[1] = (byte)(252);
            modbusTCP.WriteSingleRegister(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 17428, jogBlockSelect);
            modbusTCP.ReadCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0060, 1, ref _servoONStatus);
            if (_servoONStatus != null)
            {

                if (_servoONStatus[0] == 0)
                {
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0060, true);
                    servoON = false;
                }
            }
            else
            {
                StatusBar = "서보 ON 상태 읽기 실패_JOG운전 버튼";
                return;
            }
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0120, true);
            Debug.WriteLine("Down");
        }

        private bool Canexecutejogrewind1(object parameter)
        {
            return true;
        }

        //빠른 부방향 정지
        private void Executejogrewind2(object parameter)
        {
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0123, true);
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0123, false);
            Debug.WriteLine("Up");
        }

        private bool Canexecutejogrewind2(object parameter)
        {
            return true;
        }

        //느린 부방향 시작
        private void Executejogplaybtn1(object parameter)
        {
            jogBlockSelect[0] = (byte)(253 >> 8);
            jogBlockSelect[1] = (byte)(253);
            modbusTCP.WriteSingleRegister(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 17428, jogBlockSelect);
            modbusTCP.ReadCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0060, 1, ref _servoONStatus);
            if (_servoONStatus != null)
            {
                if (_servoONStatus[0] == 0)
                {
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0060, true);
                    servoON = false;
                }
            }
            else
            {
                StatusBar = "서보 ON 상태 읽기 실패_JOG운전 버튼";
                return;
            }
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0120, true);
            Debug.WriteLine("Down");
        }

        private bool Canexecutejogplaybtn1(object parameter)
        {
            return true;
        }

        //느린 부방향 정지
        private void Executejogplaybtn2(object parameter)
        {
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0123, true);
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0123, false);
            Debug.WriteLine("Up");
        }

        private bool Canexecutejogplaybtn2(object parameter)
        {
            return true;
        }

        //Pause  보류
        private void Executejogpause1(object parameter)
        {
            return;
        }

        private bool Canexecutejogpause1(object parameter)
        {
            return false;
        }

        //Pause  보류
        private void Executejogpause2(object parameter)
        {
            return;
        }

        private bool Canexecutejogpause2(object parameter)
        {
            return false;
        }

        //느린 정방향 시작
        private void Executejogplaybtn3(object parameter)
        {
            jogBlockSelect[0] = (byte)(254 >> 8);
            jogBlockSelect[1] = (byte)(254);
            modbusTCP.WriteSingleRegister(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 17428, jogBlockSelect);
            modbusTCP.ReadCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 96, 1, ref _servoONStatus);
            if (_servoONStatus != null)
            {

                if (_servoONStatus[0] == 0)
                {
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0060, true);
                    servoON = false;
                }
            }
            else
            {
                StatusBar = "서보 ON 상태 읽기 실패_JOG운전 버튼";
                return;
            }
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0120, true);
            Debug.WriteLine("Down");
        }

        private bool Canexecutejogplaybtn3(object parameter)
        {
            return true;
        }

        //느린 정방향 정지
        private void Executejogplaybtn4(object parameter)
        {
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 291, true);
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 291, false);
            Debug.WriteLine("Up");
        }

        private bool Canexecutejogplaybtn4(object parameter)
        {
            return true;
        }

        //빠른 부방향 시작
        private void Executejogfastford1(object parameter)
        {
            jogBlockSelect[0] = (byte)(255 >> 8);
            jogBlockSelect[1] = (byte)(255);
            modbusTCP.WriteSingleRegister(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 17428, jogBlockSelect);
            modbusTCP.ReadCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 96, 1, ref _servoONStatus);
            if (_servoONStatus != null)
            {

                if (_servoONStatus[0] == 0)
                {
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0060, true);
                    servoON = false;
                }
            }
            else
            {
                StatusBar = "서보 ON 상태 읽기 실패_JOG운전 버튼";
                return;
            }
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0120, true);
            Debug.WriteLine("Down");
        }

        private bool Canexecutejogfastford1(object parameter)
        {
            return true;
        }

        //빠른 부방향 정지
        private void Executejogfastford2(object parameter)
        {
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0123, true);
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0123, false);
            Debug.WriteLine("Up");
        }

        private bool Canexecutejogfastford2(object parameter)
        {
            return true;
        }
        #endregion

        #region MirrTimer
        //MirrTimer 실행 함수
        private void MirrTimer_Tick(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (mirrorONOFF)
                {
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 17432, 8, ref _mirrReg1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 17440, 8, ref _mirrReg2);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4001, 1, ref _errorCode);
                    modbusTCP.ReadCoils(0, (byte)axisNum1, 96, 1, ref _servoONStatus);
                    modbusTCP.ReadCoils(0, (byte)axisNum1, 161, 1, ref _alarmStatus);

                    if (_mirrReg1 != null && _mirrReg2 != null && _servoONStatus != null && _alarmStatus != null && _errorCode != null)
                    {
                        LampStatus = _servoONStatus[0];
                        AlarmStatus = _alarmStatus[0];
                        ModbusOnStatus = modbusTCP.connected;

                        Array.Reverse(_errorCode);
                        Array.Reverse(_mirrReg1);
                        Array.Reverse(_mirrReg2);

                        Array.Copy(_mirrReg1, 12, positionactualvalue, 0, 4);
                        Array.Copy(_mirrReg1, 8, velocityactualvalue, 0, 4);
                        Array.Copy(_mirrReg1, 4, torquedemand, 0, 4);
                        Array.Copy(_mirrReg1, 0, overload, 0, 4);

                        Array.Copy(_mirrReg2, 14, blocknummon, 0, 2);
                        Array.Copy(_mirrReg2, 10, dclinkcircuitvolt, 0, 4);
                        Array.Copy(_mirrReg2, 8, amptemp, 0, 2);
                        Array.Copy(_mirrReg2, 4, encodertemp, 0, 4);
                        Array.Copy(_mirrReg2, 0, powerontime, 0, 4);

                        PositionActualValue = BitConverter.ToInt32(positionactualvalue, 0);
                        VelocityActualValue = BitConverter.ToInt32(velocityactualvalue, 0);
                        torquecmd = BitConverter.ToInt32(torquedemand, 0);
                        TorqueDemand = torquecmd / 20;
                        overload1 = BitConverter.ToInt32(overload, 0);
                        OverLoad = overload1 / 5;

                        BlockNumMon = BitConverter.ToInt16(blocknummon, 0);
                        DCLinkCircuitvolt = BitConverter.ToInt32(dclinkcircuitvolt, 0);
                        AmpTemp = BitConverter.ToInt16(amptemp, 0);
                        EncoderTemp = BitConverter.ToInt32(encodertemp, 0);
                        powerontimetemp = BitConverter.ToInt32(powerontime, 0);
                        PowerONTime = powerontimetemp / 2;

                        _maincode = _errorCode[1];
                        _subcode = _errorCode[0];
                        ErrorCode = _maincode.ToString() + "." + _subcode.ToString();


                        modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4415, 1, ref selectedBlock);
                        if (selectedBlock != null)
                        {
                            Array.Reverse(selectedBlock);
                            Array.Copy(selectedBlock, 0, selectedBlock2, 0, 2);
                            SelectBlockNumMon1 = BitConverter.ToUInt16(selectedBlock2, 0);
                        }
                    }
                    else if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        StatusBar = "MirrReg_Timer값 수신되지 않음";
                        return;
                    }
                }
            }
            catch (Exception es)
            {
                StatusBar = es.Message + "  MirrReg_timer";
            }
        }
        #endregion

        #region Settings화면
        //Settings 화면 Confirm 커맨드 
        private void ExecuteSettingsConfirm(object parameter)
        {
            try
            {
                mirrTime = int.Parse(settings.cycleTime.SelectedValue.ToString());
                modbusTCP.connect(settings.xxxx.Address, Convert.ToUInt16(settings.portxxxx.Text), false);
                axisNum1 = int.Parse(settings.axisNumselect.SelectedValue.ToString());

                worker.RunWorkerAsync();

                //Register값 리딩 확인.
                //modbusTCP.ReadCoils(0, 0x01, 4096, 8, ref num1);
                //modbusTCP.ReadHoldingRegister(0, 0x01, 19740, 2, ref num1);
                //Thread.Sleep(50);
                //Debug.WriteLine(num1.Length);

                if (!mirrorONOFF)
                {
                    mirrorONOFF = true;
                }
                else
                {
                    mirrorONOFF = false;
                }

                StatusBar = "접속";
            }
            catch (Exception e)
            {
                mirrorONOFF = false;
                MessageBox.Show(e.Message, "예외발생_ConfirmBtn", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private bool CanexecuteSettingsConfirm(object parameter)
        {
            if (!mirrorONOFF)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Settings 화면 Disconnect 커맨드
        private void ExecuteDisconnect(object parameter)
        {
            try
            {
                if (modbusTCP.connected)
                {
                    if (LampStatus == 1)
                    {
                        modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 96, false);
                        Thread.Sleep(150);
                        mirrorONOFF = false;
                        servoON = true;
                        Debug.WriteLine(LampStatus.ToString());
                    }
                }

                worker.CancelAsync();
                mirrorONOFF = false;
                modbusTCP.disconnect();
                StatusBar = "통신 끊음";
                AlarmStatus = 0;
                LampStatus = 0;
                ModbusOnStatus = false;
                ErrorCode = "00.0";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CanexecuteDisconnect(object parameter)
        {
            if (mirrorONOFF)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region ControlPanel 제어 버튼 커맨드 함수
        /*------------------------------------------------------------------------------------------------------
          ControlPanel 제어버튼
         *------------------------------------------------------------------------------------------------------*/

        //ServoON
        private void ExecuteServoOn(object parameter)
        {
            //ControlPanel combobox 바인딩 테스트
            //Debug.WriteLine(Selected_BlockNum.ToString());
            //Debug.WriteLine(Selected_BlockSpeed.ToString());
            //Debug.WriteLine(Selected_BlockAccSpeed.ToString());
            //Debug.WriteLine(Selected_BlockDecSpeed.ToString());

            modbusTCP.ReadCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 96, 1, ref _servoONStatus);

            if (_servoONStatus != null)
            {
                if (_servoONStatus[0] == 0)
                {
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 96, true);
                    servoON = false;
                }
                else
                {
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 96, false);
                    servoON = true;
                }
            }
            else
            {
                StatusBar = "서보온 상태를 확인 할 수 없습니다." + "ServoON_OFF버튼";
                return;
            }
        }

        private bool CanexecuteServoOn(object parameter)
        {
            return true;
        }

        //STB버튼
        private void ExecutestB(object parameter)
        {
            modbusTCP.ReadCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 96, 1, ref _servoONStatus);

            if (_servoONStatus != null)
            {
                if (_servoONStatus[0] == 0)
                {
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 96, true);
                    servoON = false;
                }
                modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0120, true);
            }
            else
            {
                StatusBar = "서보온 상태를 확인 할 수 없습니다." + " STB버튼";
                return;
            }
        }

        private bool CanexecutestB(object parameter)
        {
            return true;
        }

        //Alarm클리어 버튼
        private void Executea_Clear(object parameter)
        {
            modbusTCP.ReadCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x00A1, 1, ref _alarmStatus);
            if (_alarmStatus != null)
            {
                if (_alarmStatus[0] != 0)
                {
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0061, true);
                    System.Threading.Thread.Sleep(200);
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0061, false);
                    System.Threading.Thread.Sleep(100);
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0061, true);
                    System.Threading.Thread.Sleep(200);
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0061, false);
                    System.Threading.Thread.Sleep(100);
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0061, true);
                    System.Threading.Thread.Sleep(200);
                    modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x0061, false);
                    System.Threading.Thread.Sleep(100);
                    Debug.WriteLine("알람 클리어");
                }
            }
            else
            {
                StatusBar = "알람 상태를 확인 할 수 없습니다." + " AlarmClear버튼";
                return;
            }
        }

        private bool Canexecutea_Clear(object parameter)
        {
            return true;
        }

        //S-Stop
        private void Executes_Stop(object parameter)
        {
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 292, true);
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 292, false);
        }

        private bool Canexecutes_Stop(object parameter)
        {
            return true;
        }

        //H-Stop
        private void Executeh_Stop(object parameter)
        {
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 291, true);
            modbusTCP.WriteSingleCoils(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 291, false);
        }

        private bool Canexecuteh_Stop(object parameter)
        {
            return true;
        }
        #endregion

        #region 블럭 동작 편집, 블럭 매개변수, 서보파라미터 모델 객체 생성 함수
        private void LoadObjectViewModel()
        {
            //Block동작 편집 Instance생성
            for (int i = 0; i < 256; i++)
            {
                BlockParaModel1s.Add(new BlockParaModel1() { BlockNum = i, BlockData = "설정 안됨" });
                blockParaModel1s = BlockParaModel1s;
            }

            //Block매개변수 Instance생성
            for (int i = 0; i < 16; i++)
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
            for (int i = 32; i < 48; i++)
            {
                BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = i, ParameterName = " 블록 동작 감속도 " + $"D{b}", Range = "0 - 10000", SettingValue = 0, Unit = " ms/(3000r/min)" });
                // blockParaModel2s = BlockParaModel2s;
                ++b;
            }
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 48, ParameterName = " 블록 동작 방법 설정 ", Range = "0 - 3", SettingValue = 0, Unit = "" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 49, ParameterName = " 블록 동작 원점 오프셋 ", Range = "-2147483648 - 2147483647", SettingValue = 0, Unit = " 지령 단위" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 50, ParameterName = " 블록 동작 정방향 소프트웨어 한계값 ", Range = "-2147483648 - 2147483647", SettingValue = 0, Unit = " 지령단위" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 51, ParameterName = " 블록 동작 부방향 소프트웨어 한계값 ", Range = "-2147483648 - 2147483647", SettingValue = 0, Unit = " 지령단위" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 52, ParameterName = " 블록 동작 시 원점 복귀 속도(고속) ", Range = "0 - 20000", SettingValue = 0, Unit = " r/min" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 53, ParameterName = " 블록 동작 시 원점 복귀 속도(저속) ", Range = "0 - 20000", SettingValue = 0, Unit = " r/min" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 54, ParameterName = " 블록 동작 원점 복귀 가속도 ", Range = "0 - 10000", SettingValue = 0, Unit = " ms/(3000r/min)" });
            BlockParaModel2s.Add(new BlockParaModel2() { MainIndex = 60, SubIndex = 55, ParameterName = " 원점 복귀 무효화 설정 ", Range = "0 - 1", SettingValue = 0, Unit = "" });
            blockParaModel2s = BlockParaModel2s;

            //ControlPanel1 컴보박스 인스턴스 생성
            for (int i = 0; i < 256; i++)
            {
                selectBlockNum.Add(i);
                SelectBlockNum = selectBlockNum;
            }
            for (int i = 0; i < 16; i++)
            {
                blockAccSpeed.Add(i);
                blockDecSpeed.Add(i);
                blockSpeed.Add(i);
                BlockAccSpeed = blockAccSpeed;
                BlockDecSpeed = blockDecSpeed;
                BlockSpeed = blockSpeed;
            }

            //BlockSettingDialogs Function 설정 콤보박스 아이템 생성
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
            if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.IncPosition_Page1"))
            {
                spdnum = (ushort)incPosition_Page1.SpeedNumCombo1.SelectedIndex;
                accnum = (ushort)incPosition_Page1.AccNumCombo1.SelectedIndex;
                decnum = (ushort)incPosition_Page1.DecNumCombo1.SelectedIndex;
                movdir = 0;
                blockchif = (ushort)incPosition_Page1.SuccNumCombo1.SelectedIndex;

                hiki3 = (byte)(decnum << 4);
                hiki4 = (byte)(movdir << 2);
                hiki5 = (byte)(blockchif);
                hiki1 = (byte)(spdnum << 4);
                hiki2 = accnum;

                value1[0] = (byte)(hiki3 + hiki4 + hiki5);
                value1[1] = 0;
                value1[2] = (byte)cmdcode01h;
                value1[3] = (byte)(hiki1 + hiki2);

                value11[0] = (byte)(TargetPosition >> 8);
                value11[1] = (byte)(TargetPosition);
                value11[2] = (byte)(TargetPosition >> 24);
                value11[3] = (byte)(TargetPosition >> 16);

                switch (((BlockParaModel1)blockpara.blockParaModel1.SelectedItem).BlockNum)
                {
                    case 0:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4800, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4802, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 1:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4804, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4806, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 2:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4808, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x480A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 3:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x480C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x480E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 4:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4810, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4812, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 5:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4814, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4816, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 6:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4818, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x481A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 7:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x481C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x481E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 8:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4820, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4822, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 9:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4824, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4826, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 10:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4828, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x482A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 11:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x482C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x482E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 12:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4830, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4832, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 13:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4834, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4836, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 14:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4838, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x483A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 15:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x483C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x483E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 16:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4840, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4842, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 17:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4844, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4846, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 18:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4848, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x484A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 19:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x484C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x484E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 20:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4850, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4852, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 21:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4854, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4856, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 22:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4858, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x485A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 23:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x485C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x485E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 24:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4860, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4862, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 25:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4864, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4866, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 26:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4868, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x486A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 27:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x486C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x486E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 28:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4870, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4872, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 29:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4874, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4876, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 30:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4878, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x487A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 31:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x487C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x487E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 32:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4880, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4882, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 33:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4884, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4886, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 34:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4888, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x488A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 35:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x488C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x488E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 36:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4890, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4892, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 37:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4894, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4896, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 38:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4898, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x489A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 39:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x489C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x489E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 40:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 41:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 42:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48AA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 43:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48AC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48AE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 44:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 45:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 46:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48BA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 47:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48BC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48BE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 48:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 49:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 50:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48CA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 51:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48CC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48CE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 52:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 53:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 54:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48DA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 55:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48DC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48DE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 56:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 57:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 58:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48EA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 59:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48EC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48EE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 60:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 61:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 62:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48FA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 63:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48FC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48FE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 64:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4900, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4902, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 65:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4904, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4906, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 66:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4908, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x490A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 67:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x490C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x490E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 68:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4910, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4912, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 69:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4914, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4916, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 70:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4918, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x491A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 71:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x491C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x491E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 72:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4920, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4922, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 73:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4924, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4926, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 74:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4928, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x492A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 75:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x492C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x492E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 76:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4930, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4932, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 77:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4934, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4936, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 78:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4938, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x493A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 79:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x493C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x493E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 80:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4940, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4942, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 81:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4944, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4946, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 82:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4948, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x494A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 83:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x494C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x494E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 84:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4950, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4952, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 85:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4954, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4956, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 86:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4958, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x495A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 87:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x495C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x495E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 88:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4960, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4962, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 89:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4964, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4966, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 90:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4968, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x496A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 91:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x496C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x496E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 92:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4970, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4972, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 93:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4974, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4976, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 94:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4978, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x497A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 95:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x497C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x497E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 96:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4980, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4982, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 97:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4984, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4986, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 98:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4988, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x498A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 99:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x498C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x498E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 100:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4990, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4992, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 101:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4994, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4996, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 102:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4998, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x499A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 103:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x499C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x499E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 104:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 105:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 106:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49AA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 107:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49AC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49AE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 108:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 109:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 110:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49BA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 111:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49BC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49BE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 112:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 113:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 114:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49CA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 115:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49CC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49CE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 116:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 117:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 118:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49DA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 119:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49DC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49DE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 120:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 121:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 122:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49EA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 123:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49EC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49EE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 124:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 125:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 126:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49FA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 127:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49FC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49FE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 128:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A00, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A02, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 129:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A04, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A06, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 130:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A08, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A0A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 131:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A0C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A0E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 132:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A10, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A12, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 133:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A14, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A16, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 134:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A18, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A1A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 135:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A1C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A1E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 136:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A20, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A22, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 137:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A24, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A26, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 138:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A28, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A2A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 139:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A2C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A2E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 140://
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A30, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A32, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 141:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A34, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A36, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 142:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A38, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A3A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 143:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A3C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A3E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 144:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A40, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A42, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 145:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A44, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A46, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 146:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A48, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A4A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 147:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A4C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A4E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 148:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A50, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A52, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 149:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A54, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A56, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 150:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A58, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A5A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 151:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A5C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A5E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 152:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A60, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A62, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 153:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A64, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A66, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 154:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A68, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A6A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 155:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A6C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A6E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 156:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A70, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A72, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 157:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A74, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A76, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 158:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A78, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A7A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 159:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A7C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A7E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 160:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A80, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A82, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 161:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A84, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A86, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 162:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A88, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A8A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 163:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A8C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A8E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 164:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A90, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A92, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 165:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A94, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A96, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 166:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A98, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A9A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 167:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A9C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A9E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 168:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 169:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 170:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AAA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 171:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AAC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AAE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 172://
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 173:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 174:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ABA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 175:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ABC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ABE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 176:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 177:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 178:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ACA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 179:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ACC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ACE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 180:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 181:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 182:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ADA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 183:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ADC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ADE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 184:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 185:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 186:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AEA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 187:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AEC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AEE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 188:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 189:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 190:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AFA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 191:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AFC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AFE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 192:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B00, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B02, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 193:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B04, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B06, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 194:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B08, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B0A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 195:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B0C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B0E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 196:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B10, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B12, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 197:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B14, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B16, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 198:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B18, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B1A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 199:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B1C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B1E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 200:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B20, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B22, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 201:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B24, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B26, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 202:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B28, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B2A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 203:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B2C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B2E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 204:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B30, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B32, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 205:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B34, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B36, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 206:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B38, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B3A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 207:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B3C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B3E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 208:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B40, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B42, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 209:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B44, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B46, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 210:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B48, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B4A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 211:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B4C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B4E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 212:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B50, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B52, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 213:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B54, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B56, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 214:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B58, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B5A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 215:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B5C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B5E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 216:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B60, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B62, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 217:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B64, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B66, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 218:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B68, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B6A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 219:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B6C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B6E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 220:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B70, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B72, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 221:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B74, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B76, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 222:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B78, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B7A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 223:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B7C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B7E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 224:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B80, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B82, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 225:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B84, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B86, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 226:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B88, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B8A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 227:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B8C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B8E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 228:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B90, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B92, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 229:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B94, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B96, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 230:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B98, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B9A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 231:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B9C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B9E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 232:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 233:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 234:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BAA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 235:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BAC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BAE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 236:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 237:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 238:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BBA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 239:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BBC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BBE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 240:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 241:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 242:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BCA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 243:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BCC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BCE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 244:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 245:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 246:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BDA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 247:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BDC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BDE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 248:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 249:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 250:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BEA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 251:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BEC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BEE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 252:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 253:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 254:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BFA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 255:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BFC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BFE, value11);
                        blockSettingDialog.Hide();
                        return;
                }

                Debug.WriteLine(((BlockParaModel1)blockpara.blockParaModel1.SelectedItem).BlockNum.ToString());  //BlockParaModel  BlockNumber
                Debug.WriteLine(((BlockParaModel1)blockpara.blockParaModel1.SelectedItem).BlockData.ToString()); //BlockParaModel  BlockData
                ((BlockParaModel1)blockpara.blockParaModel1.SelectedItem).BlockData = "hi";  //값 반영됨 확인
                Debug.WriteLine(blockpara.blockParaModel1.SelectedIndex.ToString());        //BlockParameter Number

                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.Abs_Position_Page2"))
            {
                spdnum = (ushort)abs_Position_Page2.SpeedNumCombo2.SelectedIndex;
                accnum = (ushort)abs_Position_Page2.AccNumCombo2.SelectedIndex;
                decnum = (ushort)abs_Position_Page2.DecNumCombo2.SelectedIndex;
                movdir = 0;
                blockchif = (ushort)abs_Position_Page2.SuccNumCombo2.SelectedIndex;

                hiki3 = (byte)(decnum << 4);
                hiki4 = (byte)(movdir << 2);
                hiki5 = (byte)(blockchif);
                hiki1 = (byte)(spdnum << 4);
                hiki2 = accnum;

                value1[0] = (byte)(hiki3 + hiki4 + hiki5);
                value1[1] = 0;
                value1[2] = (byte)cmdcode02h;
                value1[3] = (byte)(hiki1 + hiki2);

                value11[0] = (byte)(TargetPosition >> 8);
                value11[1] = (byte)(TargetPosition);
                value11[2] = (byte)(TargetPosition >> 24);
                value11[3] = (byte)(TargetPosition >> 16);

                switch (((BlockParaModel1)blockpara.blockParaModel1.SelectedItem).BlockNum)
                {
                    case 0:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4800, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4802, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 1:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4804, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4806, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 2:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4808, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x480A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 3:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x480C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x480E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 4:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4810, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4812, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 5:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4814, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4816, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 6:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4818, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x481A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 7:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x481C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x481E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 8:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4820, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4822, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 9:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4824, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4826, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 10:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4828, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x482A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 11:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x482C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x482E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 12:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4830, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4832, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 13:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4834, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4836, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 14:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4838, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x483A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 15:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x483C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x483E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 16:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4840, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4842, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 17:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4844, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4846, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 18:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4848, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x484A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 19:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x484C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x484E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 20:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4850, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4852, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 21:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4854, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4856, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 22:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4858, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x485A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 23:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x485C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x485E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 24:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4860, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4862, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 25:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4864, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4866, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 26:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4868, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x486A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 27:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x486C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x486E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 28:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4870, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4872, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 29:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4874, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4876, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 30:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4878, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x487A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 31:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x487C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x487E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 32:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4880, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4882, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 33:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4884, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4886, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 34:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4888, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x488A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 35:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x488C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x488E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 36:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4890, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4892, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 37:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4894, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4896, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 38:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4898, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x489A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 39:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x489C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x489E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 40:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 41:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 42:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48A8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48AA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 43:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48AC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48AE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 44:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 45:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 46:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48B8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48BA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 47:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48BC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48BE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 48:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 49:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 50:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48C8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48CA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 51:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48CC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48CE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 52:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 53:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 54:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48D8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48DA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 55:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48DC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48DE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 56:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 57:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 58:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48E8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48EA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 59:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48EC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48EE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 60:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 61:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 62:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48F8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48FA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 63:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48FC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x48FE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 64:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4900, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4902, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 65:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4904, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4906, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 66:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4908, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x490A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 67:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x490C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x490E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 68:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4910, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4912, value11);
                         blockSettingDialog.Hide();
                        return;
                    case 69:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4914, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4916, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 70:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4918, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x491A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 71:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x491C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x491E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 72:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4920, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4922, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 73:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4924, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4926, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 74:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4928, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x492A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 75:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x492C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x492E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 76:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4930, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4932, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 77:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4934, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4936, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 78:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4938, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x493A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 79:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x493C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x493E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 80:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4940, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4942, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 81:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4944, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4946, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 82:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4948, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x494A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 83:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x494C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x494E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 84:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4950, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4952, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 85:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4954, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4956, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 86:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4958, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x495A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 87:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x495C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x495E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 88:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4960, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4962, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 89:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4964, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4966, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 90:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4968, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x496A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 91:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x496C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x496E, value11); 
                        blockSettingDialog.Hide();
                        return;
                    case 92:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4970, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4972, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 93:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4974, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4976, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 94:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4978, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x497A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 95:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x497C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x497E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 96:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4980, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4982, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 97:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4984, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4986, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 98:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4988, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x498A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 99:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x498C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x498E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 100:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4990, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4992, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 101:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4994, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4996, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 102:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4998, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x499A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 103:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x499C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x499E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 104:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 105:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 106:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49A8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49AA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 107:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49AC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49AE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 108:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 109:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 110:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49B8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49BA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 111:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49BC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49BE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 112:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 113:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 114:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49C8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49CA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 115:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49CC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49CE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 116:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 117:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 118:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49D8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49DA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 119:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49DC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49DE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 120:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 121:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 122:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49E8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49EA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 123:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49EC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49EE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 124:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 125:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 126:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49F8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49FA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 127:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49FC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x49FE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 128:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A00, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A02, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 129:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A04, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A06, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 130:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A08, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A0A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 131:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A0C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A0E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 132:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A10, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A12, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 133:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A14, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A16, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 134:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A18, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A1A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 135:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A1C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A1E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 136:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A20, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A22, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 137:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A24, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A26, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 138:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A28, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A2A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 139:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A2C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A2E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 140://
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A30, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A32, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 141:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A34, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A36, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 142:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A38, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A3A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 143:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A3C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A3E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 144:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A40, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A42, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 145:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A44, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A46, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 146:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A48, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A4A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 147:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A4C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A4E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 148:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A50, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A52, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 149:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A54, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A56, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 150:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A58, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A5A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 151:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A5C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A5E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 152:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A60, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A62, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 153:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A64, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A66, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 154:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A68, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A6A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 155:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A6C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A6E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 156:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A70, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A72, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 157:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A74, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A76, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 158:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A78, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A7A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 159:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A7C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A7E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 160:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A80, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A82, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 161:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A84, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A86, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 162:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A88, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A8A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 163:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A8C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A8E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 164:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A90, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A92, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 165:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A94, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A96, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 166:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A98, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A9A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 167:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A9C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4A9E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 168:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 169:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 170:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AA8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AAA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 171:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AAC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AAE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 172://
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 173:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 174:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AB8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ABA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 175:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ABC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ABE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 176:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 177:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 178:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AC8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ACA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 179:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ACC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ACE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 180:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 181:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 182:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AD8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ADA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 183:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ADC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4ADE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 184:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 185:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 186:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AE8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AEA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 187:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AEC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AEE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 188:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 189:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 190:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AF8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AFA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 191:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AFC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4AFE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 192:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B00, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B02, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 193:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B04, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B06, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 194:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B08, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B0A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 195:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B0C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B0E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 196:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B10, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B12, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 197:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B14, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B16, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 198:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B18, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B1A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 199:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B1C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B1E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 200:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B20, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B22, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 201:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B24, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B26, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 202:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B28, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B2A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 203:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B2C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B2E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 204:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B30, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B32, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 205:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B34, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B36, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 206:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B38, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B3A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 207:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B3C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B3E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 208:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B40, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B42, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 209:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B44, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B46, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 210:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B48, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B4A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 211:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B4C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B4E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 212:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B50, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B52, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 213:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B54, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B56, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 214:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B58, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B5A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 215:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B5C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B5E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 216:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B60, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B62, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 217:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B64, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B66, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 218:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B68, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B6A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 219:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B6C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B6E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 220:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B70, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B72, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 221:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B74, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B76, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 222:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B78, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B7A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 223:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B7C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B7E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 224:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B80, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B82, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 225:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B84, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B86, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 226:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B88, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B8A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 227:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B8C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B8E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 228:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B90, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B92, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 229:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B94, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B96, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 230:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B98, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B9A, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 231:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B9C, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4B9E, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 232:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 233:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 234:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BA8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BAA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 235:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BAC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BAE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 236:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 237:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 238:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BB8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BBA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 239:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BBC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BBE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 240:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 241:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 242:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BC8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BCA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 243:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BCC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BCE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 244:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 245:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 246:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BD8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BDA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 247:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BDC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BDE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 248:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 249:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 250:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BE8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BEA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 251:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BEC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BEE, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 252:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF0, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF2, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 253:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF4, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF6, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 254:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BF8, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BFA, value11);
                        blockSettingDialog.Hide();
                        return;
                    case 255:
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BFC, value1);
                        modbusTCP.WriteMultipleRegister(0, (byte)axisNum1, 0x4BFE, value11);
                        blockSettingDialog.Hide();
                        return;
                }


                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.JOG_Operation_Page3"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.HomeReturn_Page4"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.DecStop_Page5"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.SpeedUpdate_Page6"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.DecrementCount_Page7"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.OutPutSignal_Page8"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.Jump_Page9"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.ConditionDiv_Page10"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.ConditionDiv_Page11"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }
            else if (blockSettingDialog.BlockActionParaWindow.Content.ToString().Equals("MINASA6SF_Rev.Views.ConditionDiv_Page12"))
            {
                Debug.WriteLine(blockSettingDialog.BlockActionParaWindow.Content.ToString());
                Debug.WriteLine(((BlockFunction)blockSettingDialog.FunctionSelect1.SelectedValue).Id.ToString());
            }

            Debug.WriteLine(incPosition_Page1.SpeedNumCombo1.SelectedIndex.ToString());
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
            worker2.RunWorkerAsync();


            //BlockParaModel1s[0].BlockData
            //BlockParaModel2s[0].SettingValue = 33;                  //Block속도 파라미터 값
            Debug.WriteLine("수신버튼 테스트");
        }
        private bool CanexecuteRecCommand(object parameter)
        {
            return true;
        }

        //블럭 파라미터 수신 백그라운드 워커
        private void BlockParameterRec(object sender, DoWorkEventArgs e)
        {

            switch (((BlockParaModel1)blockpara.blockParaModel1.SelectedItem).BlockNum)
            {
                case 0:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4800, 2, ref recValue1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4802, 2, ref recValue2);
                    return;
                case 1:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4804, 2, ref recValue3);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4806, 2, ref recValue4);
                    return;
                case 2:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4808, 2, ref recValue5);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x480A, 2, ref recValue6);
                    return;
                case 3:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x480C, 2, ref recValue7);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x480E, 2, ref recValue8);
                    return;
                case 4:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4810, 2, ref recValue9);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4812, 2, ref recValue10);
                    return;
                case 5:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4814, 2, ref recValue11);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4816, 2, ref recValue22);
                    return;
                case 6:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4818, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x481A, 2, ref value11);
                    return;
                case 7:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x481C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x481E, 2, ref value11);
                    return;
                case 8:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4820, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4822, 2, ref value11);
                    return;
                case 9:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4824, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4826, 2, ref value11);
                    return;
                case 10:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4828, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x482A, 2, ref value11);
                    return;
                case 11:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x482C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x482E, 2, ref value11);
                    return;
                case 12:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4830, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4832, 2, ref value11);
                    return;
                case 13:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4834, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4836, 2, ref value11);
                    return;
                case 14:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4838, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x483A, 2, ref value11);
                    return;
                case 15:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x483C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x483E, 2, ref value11);
                    return;
                case 16:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4840, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4842, 2, ref value11);
                    return;
                case 17:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4844, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4846, 2, ref value11);
                    return;
                case 18:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4848, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x484A, 2, ref value11);
                    return;
                case 19:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x484C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x484E, 2, ref value11);
                    return;
                case 20:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4850, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4852, 2, ref value11);
                    return;
                case 21:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4854, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4856, 2, ref value11);
                    return;
                case 22:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4858, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x485A, 2, ref value11);
                    return;
                case 23:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x485C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x485E, 2, ref value11);
                    return;
                case 24:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4860, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4862, 2, ref value11);
                    return;
                case 25:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4864, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4866, 2, ref value11);
                    return;
                case 26:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4868, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x486A, 2, ref value11);
                    return;
                case 27:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x486C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x486E, 2, ref value11);
                    return;
                case 28:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4870, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4872, 2, ref value11);
                    return;
                case 29:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4874, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4876, 2, ref value11);
                    return;
                case 30:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4878, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x487A, 2, ref value11);
                    return;
                case 31:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x487C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x487E, 2, ref value11);
                    return;
                case 32:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4880, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4882, 2, ref value11);
                    return;
                case 33:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4884, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4886, 2, ref value11);
                    return;
                case 34:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4888, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x488A, 2, ref value11);
                    return;
                case 35:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x488C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x488E, 2, ref value11);
                    return;
                case 36:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4890, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4892, 2, ref value11);
                    return;
                case 37:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4894, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4896, 2, ref value11);
                    return;
                case 38:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4898, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x489A, 2, ref value11);
                    return;
                case 39:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x489C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x489E, 2, ref value11);
                    return;
                case 40:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A2, 2, ref value11);
                    return;
                case 41:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A6, 2, ref value11);
                    return;
                case 42:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48AA, 2, ref value11);
                    return;
                case 43:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48AC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48AE, 2, ref value11);
                    return;
                case 44:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B2, 2, ref value11);
                    return;
                case 45:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B6, 2, ref value11);
                    return;
                case 46:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48BA, 2, ref value11);
                    return;
                case 47:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48BC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48BE, 2, ref value11);
                    return;
                case 48:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C2, 2, ref value11);
                    return;
                case 49:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C6, 2, ref value11);
                    return;
                case 50:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48CA, 2, ref value11);
                    return;
                case 51:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48CC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48CE, 2, ref value11);
                    return;
                case 52:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D2, 2, ref value11);
                    return;
                case 53:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D6, 2, ref value11);
                    return;
                case 54:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48DA, 2, ref value11);
                    return;
                case 55:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48DC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48DE, 2, ref value11);
                    return;
                case 56:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E2, 2, ref value11);
                    return;
                case 57:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E6, 2, ref value11);
                    return;
                case 58:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48EA, 2, ref value11);
                    return;
                case 59:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48EC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48EE, 2, ref value11);
                    return;
                case 60:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F2, 2, ref value11);
                    return;
                case 61:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F6, 2, ref value11);
                    return;
                case 62:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48FA, 2, ref value11);
                    return;
                case 63:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48FC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48FE, 2, ref value11);
                    return;
                case 64:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4900, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4902, 2, ref value11);
                    return;
                case 65:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4904, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4906, 2, ref value11);
                    return;
                case 66:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4908, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x490A, 2, ref value11);
                    return;
                case 67:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x490C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x490E, 2, ref value11);
                    return;
                case 68:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4910, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4912, 2, ref value11);
                    return;
                case 69:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4914, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4916, 2, ref value11);
                    return;
                case 70:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4918, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x491A, 2, ref value11);
                    return;
                case 71:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x491C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x491E, 2, ref value11);
                    return;
                case 72:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4920, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4922, 2, ref value11);
                    return;
                case 73:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4924, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4926, 2, ref value11);
                    return;
                case 74:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4928, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x492A, 2, ref value11);
                    return;
                case 75:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x492C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x492E, 2, ref value11);
                    return;
                case 76:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4930, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4932, 2, ref value11);
                    return;
                case 77:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4934, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4936, 2, ref value11);
                    return;
                case 78:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4938, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x493A, 2, ref value11);
                    return;
                case 79:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x493C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x493E, 2, ref value11);
                    return;
                case 80:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4940, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4942, 2, ref value11);
                    return;
                case 81:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4944, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4946, 2, ref value11);
                    return;
                case 82:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4948, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x494A, 2, ref value11);
                    return;
                case 83:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x494C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x494E, 2, ref value11);
                    return;
                case 84:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4950, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4952, 2, ref value11);
                    return;
                case 85:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4954, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4956, 2, ref value11);
                    return;
                case 86:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4958, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x495A, 2, ref value11);
                    return;
                case 87:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x495C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x495E, 2, ref value11);
                    return;
                case 88:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4960, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4962, 2, ref value11);
                    return;
                case 89:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4964, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4966, 2, ref value11);
                    return;
                case 90:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4968, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x496A, 2, ref value11);
                    return;
                case 91:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x496C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x496E, 2, ref value11);
                    return;
                case 92:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4970, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4972, 2, ref value11);
                    return;
                case 93:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4974, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4976, 2, ref value11);
                    return;
                case 94:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4978, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x497A, 2, ref value11);
                    return;
                case 95:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x497C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x497E, 2, ref value11);
                    return;
                case 96:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4980, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4982, 2, ref value11);
                    return;
                case 97:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4984, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4986, 2, ref value11);
                    return;
                case 98:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4988, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x498A, 2, ref value11);
                    return;
                case 99:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x498C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x498E, 2, ref value11);
                    return;
                case 100:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4990, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4992, 2, ref value11);
                    return;
                case 101:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4994, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4996, 2, ref value11);
                    return;
                case 102:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4998, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x499A, 2, ref value11);
                    return;
                case 103:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x499C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x499E, 2, ref value11);
                    return;
                case 104:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A2, 2, ref value11);
                    return;
                case 105:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A6, 2, ref value11);
                    return;
                case 106:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49AA, 2, ref value11);
                    return;
                case 107:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49AC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49AE, 2, ref value11);
                    return;
                case 108:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B2, 2, ref value11);
                    return;
                case 109:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B6, 2, ref value11);
                    return;
                case 110:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49BA, 2, ref value11);
                    return;
                case 111:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49BC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49BE, 2, ref value11);
                    return;
                case 112:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C2, 2, ref value11);
                    return;
                case 113:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C6, 2, ref value11);
                    return;
                case 114:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49CA, 2, ref value11);
                    return;
                case 115:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49CC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49CE, 2, ref value11);
                    return;
                case 116:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D2, 2, ref value11);
                    return;
                case 117:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D6, 2, ref value11);
                    return;
                case 118:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49DA, 2, ref value11);
                    return;
                case 119:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49DC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49DE, 2, ref value11);
                    return;
                case 120:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E2, 2, ref value11);
                    return;
                case 121:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E6, 2, ref value11);
                    return;
                case 122:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49EA, 2, ref value11);
                    return;
                case 123:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49EC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49EE, 2, ref value11);
                    return;
                case 124:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F2, 2, ref value11);
                    return;
                case 125:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F6, 2, ref value11);
                    return;
                case 126:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49FA, 2, ref value11);
                    return;
                case 127:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49FC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49FE, 2, ref value11);
                    return;
                case 128:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A00, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A02, 2, ref value11);
                    return;
                case 129:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A04, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A06, 2, ref value11);
                    return;
                case 130:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A08, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A0A, 2, ref value11);
                    return;
                case 131:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A0C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A0E, 2, ref value11);
                    return;
                case 132:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A10, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A12, 2, ref value11);
                    return;
                case 133:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A14, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A16, 2, ref value11);
                    return;
                case 134:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A18, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A1A, 2, ref value11);
                    return;
                case 135:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A1C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A1E, 2, ref value11);
                    return;
                case 136:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A20, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A22, 2, ref value11);
                    return;
                case 137:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A24, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A26, 2, ref value11);
                    return;
                case 138:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A28, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A2A, 2, ref value11);
                    return;
                case 139:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A2C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A2E, 2, ref value11);
                    return;
                case 140://
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A30, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A32, 2, ref value11);
                    return;
                case 141:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A34, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A36, 2, ref value11);
                    return;
                case 142:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A38, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A3A, 2, ref value11);
                    return;
                case 143:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A3C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A3E, 2, ref value11);
                    return;
                case 144:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A40, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A42, 2, ref value11);
                    return;
                case 145:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A44, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A46, 2, ref value11);
                    return;
                case 146:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A48, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A4A, 2, ref value11);
                    return;
                case 147:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A4C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A4E, 2, ref value11);
                    return;
                case 148:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A50, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A52, 2, ref value11);
                    return;
                case 149:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A54, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A56, 2, ref value11);
                    return;
                case 150:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A58, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A5A, 2, ref value11);
                    return;
                case 151:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A5C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A5E, 2, ref value11);
                    return;
                case 152:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A60, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A62, 2, ref value11);
                    return;
                case 153:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A64, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A66, 2, ref value11);
                    return;
                case 154:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A68, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A6A, 2, ref value11);
                    return;
                case 155:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A6C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A6E, 2, ref value11);
                    return;
                case 156:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A70, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A72, 2, ref value11);
                    return;
                case 157:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A74, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A76, 2, ref value11);
                    return;
                case 158:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A78, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A7A, 2, ref value11);
                    return;
                case 159:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A7C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A7E, 2, ref value11);
                    return;
                case 160:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A80, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A82, 2, ref value11);
                    return;
                case 161:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A84, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A86, 2, ref value11);
                    return;
                case 162:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A88, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A8A, 2, ref value11);
                    return;
                case 163:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A8C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A8E, 2, ref value11);
                    return;
                case 164:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A90, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A92, 2, ref value11);
                    return;
                case 165:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A94, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A96, 2, ref value11);
                    return;
                case 166:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A98, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A9A, 2, ref value11);
                    return;
                case 167:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A9C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A9E, 2, ref value11);
                    return;
                case 168:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA2, 2, ref value11);
                    return;
                case 169:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA6, 2, ref value11);
                    return;
                case 170:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AAA, 2, ref value11);
                    return;
                case 171:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AAC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AAE, 2, ref value11);
                    return;
                case 172://
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB2, 2, ref value11);
                    return;
                case 173:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB6, 2, ref value11);
                    return;
                case 174:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ABA, 2, ref value11);
                    return;
                case 175:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ABC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ABE, 2, ref value11);
                    return;
                case 176:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC2, 2, ref value11);
                    return;
                case 177:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC6, 2, ref value11);
                    return;
                case 178:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ACA, 2, ref value11);
                    return;
                case 179:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ACC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ACE, 2, ref value11);
                    return;
                case 180:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD2, 2, ref value11);
                    return;
                case 181:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD6, 2, ref value11);
                    return;
                case 182:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ADA, 2, ref value11);
                    return;
                case 183:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ADC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ADE, 2, ref value11);
                    return;
                case 184:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE2, 2, ref value11);
                    return;
                case 185:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE6, 2, ref value11);
                    return;
                case 186:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AEA, 2, ref value11);
                    return;
                case 187:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AEC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AEE, 2, ref value11);
                    return;
                case 188:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF2, 2, ref value11);
                    return;
                case 189:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF6, 2, ref value11);
                    return;
                case 190:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AFA, 2, ref value11);
                    return;
                case 191:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AFC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AFE, 2, ref value11);
                    return;
                case 192:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B00, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B02, 2, ref value11);
                    return;
                case 193:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B04, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B06, 2, ref value11);
                    return;
                case 194:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B08, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B0A, 2, ref value11);
                    return;
                case 195:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B0C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B0E, 2, ref value11);
                    return;
                case 196:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B10, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B12, 2, ref value11);
                    return;
                case 197:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B14, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B16, 2, ref value11);
                    return;
                case 198:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B18, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B1A, 2, ref value11);
                    return;
                case 199:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B1C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B1E, 2, ref value11);
                    return;
                case 200:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B20, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B22, 2, ref value11);
                    return;
                case 201:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B24, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B26, 2, ref value11);
                    return;
                case 202:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B28, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B2A, 2, ref value11);
                    return;
                case 203:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B2C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B2E, 2, ref value11);
                    return;
                case 204:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B30, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B32, 2, ref value11);
                    return;
                case 205:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B34, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B36, 2, ref value11);
                    return;
                case 206:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B38, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B3A, 2, ref value11);
                    return;
                case 207:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B3C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B3E, 2, ref value11);
                    return;
                case 208:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B40, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B42, 2, ref value11);
                    return;
                case 209:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B44, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B46, 2, ref value11);
                    return;
                case 210:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B48, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B4A, 2, ref value11);
                    return;
                case 211:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B4C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B4E, 2, ref value11);
                    return;
                case 212:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B50, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B52, 2, ref value11);
                    return;
                case 213:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B54, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B56, 2, ref value11);
                    return;
                case 214:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B58, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B5A, 2, ref value11);
                    return;
                case 215:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B5C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B5E, 2, ref value11);
                    return;
                case 216:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B60, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B62, 2, ref value11);
                    return;
                case 217:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B64, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B66, 2, ref value11);
                    return;
                case 218:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B68, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B6A, 2, ref value11);
                    return;
                case 219:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B6C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B6E, 2, ref value11);
                    return;
                case 220:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B70, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B72, 2, ref value11);
                    return;
                case 221:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B74, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B76, 2, ref value11);
                    return;
                case 222:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B78, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B7A, 2, ref value11);
                    return;
                case 223:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B7C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B7E, 2, ref value11);
                    return;
                case 224:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B80, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B82, 2, ref value11);
                    return;
                case 225:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B84, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B86, 2, ref value11);
                    return;
                case 226:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B88, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B8A, 2, ref value11);
                    return;
                case 227:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B8C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B8E, 2, ref value11);
                    return;
                case 228:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B90, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B92, 2, ref value11);
                    return;
                case 229:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B94, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B96, 2, ref value11);
                    return;
                case 230:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B98, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B9A, 2, ref value11);
                    return;
                case 231:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B9C, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B9E, 2, ref value11);
                    return;
                case 232:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA2, 2, ref value11);
                    return;
                case 233:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA6, 2, ref value11);
                    return;
                case 234:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BAA, 2, ref value11);
                    return;
                case 235:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BAC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BAE, 2, ref value11);
                    return;
                case 236:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB2, 2, ref value11);
                    return;
                case 237:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB6, 2, ref value11);
                    return;
                case 238:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BBA, 2, ref value11);
                    return;
                case 239:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BBC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BBE, 2, ref value11);
                    return;
                case 240:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC2, 2, ref value11);
                    return;
                case 241:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC6, 2, ref value11);
                    return;
                case 242:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BCA, 2, ref value11);
                    return;
                case 243:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BCC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BCE, 2, ref value11);
                    return;
                case 244:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD2, 2, ref value11);
                    return;
                case 245:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD6, 2, ref value11);
                    return;
                case 246:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BDA, 2, ref value11);
                    return;
                case 247:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BDC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BDE, 2, ref value11);
                    return;
                case 248:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE2, 2, ref value11);
                    return;
                case 249:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE6, 2, ref value11);
                    return;
                case 250:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BEA, 2, ref value11);
                    return;
                case 251:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BEC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BEE, 2, ref value11);
                    return;
                case 252:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF0, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF2, 2, ref value11);
                    return;
                case 253:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF4, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF6, 2, ref value11);
                    return;
                case 254:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF8, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BFA, 2, ref value11);
                    return;
                case 255:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BFC, 2, ref value1);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BFE, 2, ref value11);
                    return;
            }


            modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4800, 2, ref recValue1);
            Thread.Sleep(5);
            Debug.WriteLine(recValue1.Length.ToString());
            modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4802, 2, ref recValue2);
            Thread.Sleep(5);
            Debug.WriteLine(recValue1.Length.ToString());

            //modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4832, 50, ref recValue3);
            //Thread.Sleep(20);
            //Debug.WriteLine(recValue1.Length.ToString());
            //modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x484B, 50, ref recValue4);
            //Thread.Sleep(20);
            //Debug.WriteLine(recValue1.Length.ToString());
            //modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4800, 50, ref recValue5);
            //Thread.Sleep(20);
            //Debug.WriteLine(recValue1.Length.ToString());
            //modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4800, 50, ref recValue6);
            //Thread.Sleep(20);
            //Debug.WriteLine(recValue1.Length.ToString());
            //modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4800, 50, ref recValue7);
            //Thread.Sleep(20);
            //Debug.WriteLine(recValue1.Length.ToString());
            //modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4800, 50, ref recValue8);
            //Thread.Sleep(20);
            //Debug.WriteLine(recValue1.Length.ToString());
            //modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4800, 50, ref recValue9);
            //Thread.Sleep(20);
            //Debug.WriteLine(recValue1.Length.ToString());
            //modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4800, 50, ref recValue10);
            //Thread.Sleep(20);
            //Debug.WriteLine(recValue1.Length.ToString());


            Array.Reverse(recValue1);
            Array.Reverse(recValue2);
            //Array.Reverse(recValue3);
            //Array.Reverse(recValue4);
            //Array.Reverse(recValue5);
            //Array.Reverse(recValue6);
            //Array.Reverse(recValue7);
            //Array.Reverse(recValue8);
            //Array.Reverse(recValue9);
            //Array.Reverse(recValue10);

            byte[] parameter7_4byte1 = new byte[4];
            byte[] parameter7_4byte11 = new byte[4];
            byte[] parameter7_4byte2 = new byte[4];
            byte[] parameter7_4byte22 = new byte[4];

            Array.Copy(recValue1, 0, parameter7_4byte1, 0, 4);
            Array.Copy(recValue2, 0, parameter7_4byte2, 0, 4);

            //0x4800 command
            parameter7_4byte11[0] = parameter7_4byte1[0];    //속도와 가속
            parameter7_4byte11[1] = parameter7_4byte1[1];    //커맨드 Code
            parameter7_4byte11[2] = parameter7_4byte1[2];    //예약                   
            parameter7_4byte11[3] = parameter7_4byte1[3];    //감속, 방향, 천이 조건
            
            //0x4802 data
            parameter7_4byte22[0] = parameter7_4byte2[2];
            parameter7_4byte22[1] = parameter7_4byte2[3];
            parameter7_4byte22[2] = parameter7_4byte2[0];
            parameter7_4byte22[3] = parameter7_4byte2[1];

            int cmdCode = Convert.ToInt32(parameter7_4byte11[1]);  //커맨드 Code 
            int spdNum = (Convert.ToInt32(parameter7_4byte11[0]) >>4 );   // 속도 번호  hiki1
            int accNum = (Convert.ToInt32(parameter7_4byte11[0]) & 0b_0000_1111);  //가속 번호  hiki2
            int dummy = Convert.ToInt32(parameter7_4byte11[2]);   //예약
            int decNum = (Convert.ToInt32(parameter7_4byte11[3]) >>4 );   //감속 번호  hiki3
            int movdir = ((Convert.ToInt32(parameter7_4byte11[3]) & 0b_0000_1111) >>2 );  //  방향  hiki4
            int blockchif = (Convert.ToInt32(parameter7_4byte11[3]) & 0b_0000_0011);  //천이 조건  hiki5
            int dataConfig = BitConverter.ToInt32(parameter7_4byte22, 0);   //블록 데이터 구성

            Debug.WriteLine(cmdCode.ToString());
            Debug.WriteLine(spdNum.ToString());
            Debug.WriteLine(accNum.ToString());
            Debug.WriteLine(decNum.ToString());
            Debug.WriteLine(movdir.ToString());
            Debug.WriteLine(blockchif.ToString());
            Debug.WriteLine(dataConfig.ToString());


        }



        private void ExecuteTransCommand(object parameter)
        {
            Debug.WriteLine(BlockParaModel2s[0].SettingValue.ToString());       //첫번째 객체의 값을 가져옴.
            Debug.WriteLine("송신버튼 테스트");
        }
        private bool CanexecuteTransCommand(object parameter)
        {
            return true;
        }

        private void ExecuteEepCommand(object parameter)
        {
            _eepromwrite[0] = (byte)(_eeprom >> 8);
            _eepromwrite[1] = (byte)_eeprom;

            Thread.Sleep(5);
            modbusTCP.WriteSingleRegister(0, byte.Parse(settings.axisNumselect.SelectedValue.ToString()), 0x1020, _eepromwrite);
            Debug.WriteLine("EEP버튼 테스트");
        }
        private bool CanexecuteEepCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region ServoParameter 수신, 송신 버튼
        // 서보파라미터 수신, 송신 버튼
        private void ExecuteSPReCommand(object parameter)
        {
            Debug.WriteLine("서보 파라미터 수신버튼 테스트");
        }

        private bool CanexecuteSPRCommand(object parameter)
        {
            return true;
        }

        private void ExecuteSPTranCommand(object parameter)
        {
            Debug.WriteLine("서보 파라미터 송신버튼 테스트");
        }

        private bool CanexecuteSPTranCommand(object parameter)
        {
            return true;
        }
        #endregion

        //블럭 셋팅 창 오픈
        public void showWindow(object BlocksettingDialog)
        {
            blockSettingDialog.ShowDialog();
        }
    }
}
