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

        #region BlockSetting 수신 변수  recValue1~recValue512
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
        byte[] recValue256;
        byte[] recValue257;
        byte[] recValue258;
        byte[] recValue259;
        byte[] recValue260;
        byte[] recValue261;
        byte[] recValue262;
        byte[] recValue263;
        byte[] recValue264;
        byte[] recValue265;
        byte[] recValue266;
        byte[] recValue267;
        byte[] recValue268;
        byte[] recValue269;
        byte[] recValue270;
        byte[] recValue271;
        byte[] recValue272;
        byte[] recValue273;
        byte[] recValue274;
        byte[] recValue275;
        byte[] recValue276;
        byte[] recValue277;
        byte[] recValue278;
        byte[] recValue279;
        byte[] recValue280;
        byte[] recValue281;
        byte[] recValue282;
        byte[] recValue283;
        byte[] recValue284;
        byte[] recValue285;
        byte[] recValue286;
        byte[] recValue287;
        byte[] recValue288;
        byte[] recValue289;
        byte[] recValue290;
        byte[] recValue291;
        byte[] recValue292;
        byte[] recValue293;
        byte[] recValue294;
        byte[] recValue295;
        byte[] recValue296;
        byte[] recValue297;
        byte[] recValue298;
        byte[] recValue299;
        byte[] recValue300;
        byte[] recValue301;
        byte[] recValue302;
        byte[] recValue303;
        byte[] recValue304;
        byte[] recValue305;
        byte[] recValue306;
        byte[] recValue307;
        byte[] recValue308;
        byte[] recValue309;
        byte[] recValue310;
        byte[] recValue311;
        byte[] recValue312;
        byte[] recValue313;
        byte[] recValue314;
        byte[] recValue315;
        byte[] recValue316;
        byte[] recValue317;
        byte[] recValue318;
        byte[] recValue319;
        byte[] recValue320;
        byte[] recValue321;
        byte[] recValue322;
        byte[] recValue323;
        byte[] recValue324;
        byte[] recValue325;
        byte[] recValue326;
        byte[] recValue327;
        byte[] recValue328;
        byte[] recValue329;
        byte[] recValue330;
        byte[] recValue331;
        byte[] recValue332;
        byte[] recValue333;
        byte[] recValue334;
        byte[] recValue335;
        byte[] recValue336;
        byte[] recValue337;
        byte[] recValue338;
        byte[] recValue339;
        byte[] recValue340;
        byte[] recValue341;
        byte[] recValue342;
        byte[] recValue343;
        byte[] recValue344;
        byte[] recValue345;
        byte[] recValue346;
        byte[] recValue347;
        byte[] recValue348;
        byte[] recValue349;
        byte[] recValue350;
        byte[] recValue351;
        byte[] recValue352;
        byte[] recValue353;
        byte[] recValue354;
        byte[] recValue355;
        byte[] recValue356;
        byte[] recValue357;
        byte[] recValue358;
        byte[] recValue359;
        byte[] recValue360;
        byte[] recValue361;
        byte[] recValue362;
        byte[] recValue363;
        byte[] recValue364;
        byte[] recValue365;
        byte[] recValue366;
        byte[] recValue367;
        byte[] recValue368;
        byte[] recValue369;
        byte[] recValue370;
        byte[] recValue371;
        byte[] recValue372;
        byte[] recValue373;
        byte[] recValue374;
        byte[] recValue375;
        byte[] recValue376;
        byte[] recValue377;
        byte[] recValue378;
        byte[] recValue379;
        byte[] recValue380;
        byte[] recValue381;
        byte[] recValue382;
        byte[] recValue383;
        byte[] recValue384;
        byte[] recValue385;
        byte[] recValue386;
        byte[] recValue387;
        byte[] recValue388;
        byte[] recValue389;
        byte[] recValue390;
        byte[] recValue391;
        byte[] recValue392;
        byte[] recValue393;
        byte[] recValue394;
        byte[] recValue395;
        byte[] recValue396;
        byte[] recValue397;
        byte[] recValue398;
        byte[] recValue399;
        byte[] recValue400;
        byte[] recValue401;
        byte[] recValue402;
        byte[] recValue403;
        byte[] recValue404;
        byte[] recValue405;
        byte[] recValue406;
        byte[] recValue407;
        byte[] recValue408;
        byte[] recValue409;
        byte[] recValue410;
        byte[] recValue411;
        byte[] recValue412;
        byte[] recValue413;
        byte[] recValue414;
        byte[] recValue415;
        byte[] recValue416;
        byte[] recValue417;
        byte[] recValue418;
        byte[] recValue419;
        byte[] recValue420;
        byte[] recValue421;
        byte[] recValue422;
        byte[] recValue423;
        byte[] recValue424;
        byte[] recValue425;
        byte[] recValue426;
        byte[] recValue427;
        byte[] recValue428;
        byte[] recValue429;
        byte[] recValue430;
        byte[] recValue431;
        byte[] recValue432;
        byte[] recValue433;
        byte[] recValue434;
        byte[] recValue435;
        byte[] recValue436;
        byte[] recValue437;
        byte[] recValue438;
        byte[] recValue439;
        byte[] recValue440;
        byte[] recValue441;
        byte[] recValue442;
        byte[] recValue443;
        byte[] recValue444;
        byte[] recValue445;
        byte[] recValue446;
        byte[] recValue447;
        byte[] recValue448;
        byte[] recValue449;
        byte[] recValue450;
        byte[] recValue451;
        byte[] recValue452;
        byte[] recValue453;
        byte[] recValue454;
        byte[] recValue455;
        byte[] recValue456;
        byte[] recValue457;
        byte[] recValue458;
        byte[] recValue459;
        byte[] recValue460;
        byte[] recValue461;
        byte[] recValue462;
        byte[] recValue463;
        byte[] recValue464;
        byte[] recValue465;
        byte[] recValue466;
        byte[] recValue467;
        byte[] recValue468;
        byte[] recValue469;
        byte[] recValue470;
        byte[] recValue471;
        byte[] recValue472;
        byte[] recValue473;
        byte[] recValue474;
        byte[] recValue475;
        byte[] recValue476;
        byte[] recValue477;
        byte[] recValue478;
        byte[] recValue479;
        byte[] recValue480;
        byte[] recValue481;
        byte[] recValue482;
        byte[] recValue483;
        byte[] recValue484;
        byte[] recValue485;
        byte[] recValue486;
        byte[] recValue487;
        byte[] recValue488;
        byte[] recValue489;
        byte[] recValue490;
        byte[] recValue491;
        byte[] recValue492;
        byte[] recValue493;
        byte[] recValue494;
        byte[] recValue495;
        byte[] recValue496;
        byte[] recValue497;
        byte[] recValue498;
        byte[] recValue499;
        byte[] recValue500;
        byte[] recValue501;
        byte[] recValue502;
        byte[] recValue503;
        byte[] recValue504;
        byte[] recValue505;
        byte[] recValue506;
        byte[] recValue507;
        byte[] recValue508;
        byte[] recValue509;
        byte[] recValue510;
        byte[] recValue511;
        byte[] recValue512;
        #endregion

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

        #region 파라미터 인수 분리1 parameter7_4byte1~parameter7_4byte512
        byte[] parameter7_4byte1 = new byte[4];
        byte[] parameter7_4byte2 = new byte[4];
        byte[] parameter7_4byte3 = new byte[4];
        byte[] parameter7_4byte4 = new byte[4];
        byte[] parameter7_4byte5 = new byte[4];
        byte[] parameter7_4byte6 = new byte[4];
        byte[] parameter7_4byte7 = new byte[4];
        byte[] parameter7_4byte8 = new byte[4];
        byte[] parameter7_4byte9 = new byte[4];
        byte[] parameter7_4byte10 = new byte[4];
        byte[] parameter7_4byte11 = new byte[4];
        byte[] parameter7_4byte12 = new byte[4];
        byte[] parameter7_4byte13 = new byte[4];
        byte[] parameter7_4byte14 = new byte[4];
        byte[] parameter7_4byte15 = new byte[4];
        byte[] parameter7_4byte16 = new byte[4];
        byte[] parameter7_4byte17 = new byte[4];
        byte[] parameter7_4byte18 = new byte[4];
        byte[] parameter7_4byte19 = new byte[4];
        byte[] parameter7_4byte20 = new byte[4];
        byte[] parameter7_4byte21 = new byte[4];
        byte[] parameter7_4byte22 = new byte[4];
        byte[] parameter7_4byte23 = new byte[4];
        byte[] parameter7_4byte24 = new byte[4];
        byte[] parameter7_4byte25 = new byte[4];
        byte[] parameter7_4byte26 = new byte[4];
        byte[] parameter7_4byte27 = new byte[4];
        byte[] parameter7_4byte28 = new byte[4];
        byte[] parameter7_4byte29 = new byte[4];
        byte[] parameter7_4byte30 = new byte[4];
        byte[] parameter7_4byte31 = new byte[4];
        byte[] parameter7_4byte32 = new byte[4];
        byte[] parameter7_4byte33 = new byte[4];
        byte[] parameter7_4byte34 = new byte[4];
        byte[] parameter7_4byte35 = new byte[4];
        byte[] parameter7_4byte36 = new byte[4];
        byte[] parameter7_4byte37 = new byte[4];
        byte[] parameter7_4byte38 = new byte[4];
        byte[] parameter7_4byte39 = new byte[4];
        byte[] parameter7_4byte40 = new byte[4];
        byte[] parameter7_4byte41 = new byte[4];
        byte[] parameter7_4byte42 = new byte[4];
        byte[] parameter7_4byte43 = new byte[4];
        byte[] parameter7_4byte44 = new byte[4];
        byte[] parameter7_4byte45 = new byte[4];
        byte[] parameter7_4byte46 = new byte[4];
        byte[] parameter7_4byte47 = new byte[4];
        byte[] parameter7_4byte48 = new byte[4];
        byte[] parameter7_4byte49 = new byte[4];
        byte[] parameter7_4byte50 = new byte[4];
        byte[] parameter7_4byte51 = new byte[4];
        byte[] parameter7_4byte52 = new byte[4];
        byte[] parameter7_4byte53 = new byte[4];
        byte[] parameter7_4byte54 = new byte[4];
        byte[] parameter7_4byte55 = new byte[4];
        byte[] parameter7_4byte56 = new byte[4];
        byte[] parameter7_4byte57 = new byte[4];
        byte[] parameter7_4byte58 = new byte[4];
        byte[] parameter7_4byte59 = new byte[4];
        byte[] parameter7_4byte60 = new byte[4];
        byte[] parameter7_4byte61 = new byte[4];
        byte[] parameter7_4byte62 = new byte[4];
        byte[] parameter7_4byte63 = new byte[4];
        byte[] parameter7_4byte64 = new byte[4];
        byte[] parameter7_4byte65 = new byte[4];
        byte[] parameter7_4byte66 = new byte[4];
        byte[] parameter7_4byte67 = new byte[4];
        byte[] parameter7_4byte68 = new byte[4];
        byte[] parameter7_4byte69 = new byte[4];
        byte[] parameter7_4byte70 = new byte[4];
        byte[] parameter7_4byte71 = new byte[4];
        byte[] parameter7_4byte72 = new byte[4];
        byte[] parameter7_4byte73 = new byte[4];
        byte[] parameter7_4byte74 = new byte[4];
        byte[] parameter7_4byte75 = new byte[4];
        byte[] parameter7_4byte76 = new byte[4];
        byte[] parameter7_4byte77 = new byte[4];
        byte[] parameter7_4byte78 = new byte[4];
        byte[] parameter7_4byte79 = new byte[4];
        byte[] parameter7_4byte80 = new byte[4];
        byte[] parameter7_4byte81 = new byte[4];
        byte[] parameter7_4byte82 = new byte[4];
        byte[] parameter7_4byte83 = new byte[4];
        byte[] parameter7_4byte84 = new byte[4];
        byte[] parameter7_4byte85 = new byte[4];
        byte[] parameter7_4byte86 = new byte[4];
        byte[] parameter7_4byte87 = new byte[4];
        byte[] parameter7_4byte88 = new byte[4];
        byte[] parameter7_4byte89 = new byte[4];
        byte[] parameter7_4byte90 = new byte[4];
        byte[] parameter7_4byte91 = new byte[4];
        byte[] parameter7_4byte92 = new byte[4];
        byte[] parameter7_4byte93 = new byte[4];
        byte[] parameter7_4byte94 = new byte[4];
        byte[] parameter7_4byte95 = new byte[4];
        byte[] parameter7_4byte96 = new byte[4];
        byte[] parameter7_4byte97 = new byte[4];
        byte[] parameter7_4byte98 = new byte[4];
        byte[] parameter7_4byte99 = new byte[4];
        byte[] parameter7_4byte100 = new byte[4];
        byte[] parameter7_4byte101 = new byte[4];
        byte[] parameter7_4byte102 = new byte[4];
        byte[] parameter7_4byte103 = new byte[4];
        byte[] parameter7_4byte104 = new byte[4];
        byte[] parameter7_4byte105 = new byte[4];
        byte[] parameter7_4byte106 = new byte[4];
        byte[] parameter7_4byte107 = new byte[4];
        byte[] parameter7_4byte108 = new byte[4];
        byte[] parameter7_4byte109 = new byte[4];
        byte[] parameter7_4byte110 = new byte[4];
        byte[] parameter7_4byte111 = new byte[4];
        byte[] parameter7_4byte112 = new byte[4];
        byte[] parameter7_4byte113 = new byte[4];
        byte[] parameter7_4byte114 = new byte[4];
        byte[] parameter7_4byte115 = new byte[4];
        byte[] parameter7_4byte116 = new byte[4];
        byte[] parameter7_4byte117 = new byte[4];
        byte[] parameter7_4byte118 = new byte[4];
        byte[] parameter7_4byte119 = new byte[4];
        byte[] parameter7_4byte120 = new byte[4];
        byte[] parameter7_4byte121 = new byte[4];
        byte[] parameter7_4byte122 = new byte[4];
        byte[] parameter7_4byte123 = new byte[4];
        byte[] parameter7_4byte124 = new byte[4];
        byte[] parameter7_4byte125 = new byte[4];
        byte[] parameter7_4byte126 = new byte[4];
        byte[] parameter7_4byte127 = new byte[4];
        byte[] parameter7_4byte128 = new byte[4];
        byte[] parameter7_4byte129 = new byte[4];
        byte[] parameter7_4byte130 = new byte[4];
        byte[] parameter7_4byte131 = new byte[4];
        byte[] parameter7_4byte132 = new byte[4];
        byte[] parameter7_4byte133 = new byte[4];
        byte[] parameter7_4byte134 = new byte[4];
        byte[] parameter7_4byte135 = new byte[4];
        byte[] parameter7_4byte136 = new byte[4];
        byte[] parameter7_4byte137 = new byte[4];
        byte[] parameter7_4byte138 = new byte[4];
        byte[] parameter7_4byte139 = new byte[4];
        byte[] parameter7_4byte140 = new byte[4];
        byte[] parameter7_4byte141 = new byte[4];
        byte[] parameter7_4byte142 = new byte[4];
        byte[] parameter7_4byte143 = new byte[4];
        byte[] parameter7_4byte144 = new byte[4];
        byte[] parameter7_4byte145 = new byte[4];
        byte[] parameter7_4byte146 = new byte[4];
        byte[] parameter7_4byte147 = new byte[4];
        byte[] parameter7_4byte148 = new byte[4];
        byte[] parameter7_4byte149 = new byte[4];
        byte[] parameter7_4byte150 = new byte[4];
        byte[] parameter7_4byte151 = new byte[4];
        byte[] parameter7_4byte152 = new byte[4];
        byte[] parameter7_4byte153 = new byte[4];
        byte[] parameter7_4byte154 = new byte[4];
        byte[] parameter7_4byte155 = new byte[4];
        byte[] parameter7_4byte156 = new byte[4];
        byte[] parameter7_4byte157 = new byte[4];
        byte[] parameter7_4byte158 = new byte[4];
        byte[] parameter7_4byte159 = new byte[4];
        byte[] parameter7_4byte160 = new byte[4];
        byte[] parameter7_4byte161 = new byte[4];
        byte[] parameter7_4byte162 = new byte[4];
        byte[] parameter7_4byte163 = new byte[4];
        byte[] parameter7_4byte164 = new byte[4];
        byte[] parameter7_4byte165 = new byte[4];
        byte[] parameter7_4byte166 = new byte[4];
        byte[] parameter7_4byte167 = new byte[4];
        byte[] parameter7_4byte168 = new byte[4];
        byte[] parameter7_4byte169 = new byte[4];
        byte[] parameter7_4byte170 = new byte[4];
        byte[] parameter7_4byte171 = new byte[4];
        byte[] parameter7_4byte172 = new byte[4];
        byte[] parameter7_4byte173 = new byte[4];
        byte[] parameter7_4byte174 = new byte[4];
        byte[] parameter7_4byte175 = new byte[4];
        byte[] parameter7_4byte176 = new byte[4];
        byte[] parameter7_4byte177 = new byte[4];
        byte[] parameter7_4byte178 = new byte[4];
        byte[] parameter7_4byte179 = new byte[4];
        byte[] parameter7_4byte180 = new byte[4];
        byte[] parameter7_4byte181 = new byte[4];
        byte[] parameter7_4byte182 = new byte[4];
        byte[] parameter7_4byte183 = new byte[4];
        byte[] parameter7_4byte184 = new byte[4];
        byte[] parameter7_4byte185 = new byte[4];
        byte[] parameter7_4byte186 = new byte[4];
        byte[] parameter7_4byte187 = new byte[4];
        byte[] parameter7_4byte188 = new byte[4];
        byte[] parameter7_4byte189 = new byte[4];
        byte[] parameter7_4byte190 = new byte[4];
        byte[] parameter7_4byte191 = new byte[4];
        byte[] parameter7_4byte192 = new byte[4];
        byte[] parameter7_4byte193 = new byte[4];
        byte[] parameter7_4byte194 = new byte[4];
        byte[] parameter7_4byte195 = new byte[4];
        byte[] parameter7_4byte196 = new byte[4];
        byte[] parameter7_4byte197 = new byte[4];
        byte[] parameter7_4byte198 = new byte[4];
        byte[] parameter7_4byte199 = new byte[4];
        byte[] parameter7_4byte200 = new byte[4];
        byte[] parameter7_4byte201 = new byte[4];
        byte[] parameter7_4byte202 = new byte[4];
        byte[] parameter7_4byte203 = new byte[4];
        byte[] parameter7_4byte204 = new byte[4];
        byte[] parameter7_4byte205 = new byte[4];
        byte[] parameter7_4byte206 = new byte[4];
        byte[] parameter7_4byte207 = new byte[4];
        byte[] parameter7_4byte208 = new byte[4];
        byte[] parameter7_4byte209 = new byte[4];
        byte[] parameter7_4byte210 = new byte[4];
        byte[] parameter7_4byte211 = new byte[4];
        byte[] parameter7_4byte212 = new byte[4];
        byte[] parameter7_4byte213 = new byte[4];
        byte[] parameter7_4byte214 = new byte[4];
        byte[] parameter7_4byte215 = new byte[4];
        byte[] parameter7_4byte216 = new byte[4];
        byte[] parameter7_4byte217 = new byte[4];
        byte[] parameter7_4byte218 = new byte[4];
        byte[] parameter7_4byte219 = new byte[4];
        byte[] parameter7_4byte220 = new byte[4];
        byte[] parameter7_4byte221 = new byte[4];
        byte[] parameter7_4byte222 = new byte[4];
        byte[] parameter7_4byte223 = new byte[4];
        byte[] parameter7_4byte224 = new byte[4];
        byte[] parameter7_4byte225 = new byte[4];
        byte[] parameter7_4byte226 = new byte[4];
        byte[] parameter7_4byte227 = new byte[4];
        byte[] parameter7_4byte228 = new byte[4];
        byte[] parameter7_4byte229 = new byte[4];
        byte[] parameter7_4byte230 = new byte[4];
        byte[] parameter7_4byte231 = new byte[4];
        byte[] parameter7_4byte232 = new byte[4];
        byte[] parameter7_4byte233 = new byte[4];
        byte[] parameter7_4byte234 = new byte[4];
        byte[] parameter7_4byte235 = new byte[4];
        byte[] parameter7_4byte236 = new byte[4];
        byte[] parameter7_4byte237 = new byte[4];
        byte[] parameter7_4byte238 = new byte[4];
        byte[] parameter7_4byte239 = new byte[4];
        byte[] parameter7_4byte240 = new byte[4];
        byte[] parameter7_4byte241 = new byte[4];
        byte[] parameter7_4byte242 = new byte[4];
        byte[] parameter7_4byte243 = new byte[4];
        byte[] parameter7_4byte244 = new byte[4];
        byte[] parameter7_4byte245 = new byte[4];
        byte[] parameter7_4byte246 = new byte[4];
        byte[] parameter7_4byte247 = new byte[4];
        byte[] parameter7_4byte248 = new byte[4];
        byte[] parameter7_4byte249 = new byte[4];
        byte[] parameter7_4byte250 = new byte[4];
        byte[] parameter7_4byte251 = new byte[4];
        byte[] parameter7_4byte252 = new byte[4];
        byte[] parameter7_4byte253 = new byte[4];
        byte[] parameter7_4byte254 = new byte[4];
        byte[] parameter7_4byte255 = new byte[4];
        byte[] parameter7_4byte256 = new byte[4];
        byte[] parameter7_4byte257 = new byte[4];
        byte[] parameter7_4byte258 = new byte[4];
        byte[] parameter7_4byte259 = new byte[4];
        byte[] parameter7_4byte260 = new byte[4];
        byte[] parameter7_4byte261 = new byte[4];
        byte[] parameter7_4byte262 = new byte[4];
        byte[] parameter7_4byte263 = new byte[4];
        byte[] parameter7_4byte264 = new byte[4];
        byte[] parameter7_4byte265 = new byte[4];
        byte[] parameter7_4byte266 = new byte[4];
        byte[] parameter7_4byte267 = new byte[4];
        byte[] parameter7_4byte268 = new byte[4];
        byte[] parameter7_4byte269 = new byte[4];
        byte[] parameter7_4byte270 = new byte[4];
        byte[] parameter7_4byte271 = new byte[4];
        byte[] parameter7_4byte272 = new byte[4];
        byte[] parameter7_4byte273 = new byte[4];
        byte[] parameter7_4byte274 = new byte[4];
        byte[] parameter7_4byte275 = new byte[4];
        byte[] parameter7_4byte276 = new byte[4];
        byte[] parameter7_4byte277 = new byte[4];
        byte[] parameter7_4byte278 = new byte[4];
        byte[] parameter7_4byte279 = new byte[4];
        byte[] parameter7_4byte280 = new byte[4];
        byte[] parameter7_4byte281 = new byte[4];
        byte[] parameter7_4byte282 = new byte[4];
        byte[] parameter7_4byte283 = new byte[4];
        byte[] parameter7_4byte284 = new byte[4];
        byte[] parameter7_4byte285 = new byte[4];
        byte[] parameter7_4byte286 = new byte[4];
        byte[] parameter7_4byte287 = new byte[4];
        byte[] parameter7_4byte288 = new byte[4];
        byte[] parameter7_4byte289 = new byte[4];
        byte[] parameter7_4byte290 = new byte[4];
        byte[] parameter7_4byte291 = new byte[4];
        byte[] parameter7_4byte292 = new byte[4];
        byte[] parameter7_4byte293 = new byte[4];
        byte[] parameter7_4byte294 = new byte[4];
        byte[] parameter7_4byte295 = new byte[4];
        byte[] parameter7_4byte296 = new byte[4];
        byte[] parameter7_4byte297 = new byte[4];
        byte[] parameter7_4byte298 = new byte[4];
        byte[] parameter7_4byte299 = new byte[4];
        byte[] parameter7_4byte300 = new byte[4];
        byte[] parameter7_4byte301 = new byte[4];
        byte[] parameter7_4byte302 = new byte[4];
        byte[] parameter7_4byte303 = new byte[4];
        byte[] parameter7_4byte304 = new byte[4];
        byte[] parameter7_4byte305 = new byte[4];
        byte[] parameter7_4byte306 = new byte[4];
        byte[] parameter7_4byte307 = new byte[4];
        byte[] parameter7_4byte308 = new byte[4];
        byte[] parameter7_4byte309 = new byte[4];
        byte[] parameter7_4byte310 = new byte[4];
        byte[] parameter7_4byte311 = new byte[4];
        byte[] parameter7_4byte312 = new byte[4];
        byte[] parameter7_4byte313 = new byte[4];
        byte[] parameter7_4byte314 = new byte[4];
        byte[] parameter7_4byte315 = new byte[4];
        byte[] parameter7_4byte316 = new byte[4];
        byte[] parameter7_4byte317 = new byte[4];
        byte[] parameter7_4byte318 = new byte[4];
        byte[] parameter7_4byte319 = new byte[4];
        byte[] parameter7_4byte320 = new byte[4];
        byte[] parameter7_4byte321 = new byte[4];
        byte[] parameter7_4byte322 = new byte[4];
        byte[] parameter7_4byte323 = new byte[4];
        byte[] parameter7_4byte324 = new byte[4];
        byte[] parameter7_4byte325 = new byte[4];
        byte[] parameter7_4byte326 = new byte[4];
        byte[] parameter7_4byte327 = new byte[4];
        byte[] parameter7_4byte328 = new byte[4];
        byte[] parameter7_4byte329 = new byte[4];
        byte[] parameter7_4byte330 = new byte[4];
        byte[] parameter7_4byte331 = new byte[4];
        byte[] parameter7_4byte332 = new byte[4];
        byte[] parameter7_4byte333 = new byte[4];
        byte[] parameter7_4byte334 = new byte[4];
        byte[] parameter7_4byte335 = new byte[4];
        byte[] parameter7_4byte336 = new byte[4];
        byte[] parameter7_4byte337 = new byte[4];
        byte[] parameter7_4byte338 = new byte[4];
        byte[] parameter7_4byte339 = new byte[4];
        byte[] parameter7_4byte340 = new byte[4];
        byte[] parameter7_4byte341 = new byte[4];
        byte[] parameter7_4byte342 = new byte[4];
        byte[] parameter7_4byte343 = new byte[4];
        byte[] parameter7_4byte344 = new byte[4];
        byte[] parameter7_4byte345 = new byte[4];
        byte[] parameter7_4byte346 = new byte[4];
        byte[] parameter7_4byte347 = new byte[4];
        byte[] parameter7_4byte348 = new byte[4];
        byte[] parameter7_4byte349 = new byte[4];
        byte[] parameter7_4byte350 = new byte[4];
        byte[] parameter7_4byte351 = new byte[4];
        byte[] parameter7_4byte352 = new byte[4];
        byte[] parameter7_4byte353 = new byte[4];
        byte[] parameter7_4byte354 = new byte[4];
        byte[] parameter7_4byte355 = new byte[4];
        byte[] parameter7_4byte356 = new byte[4];
        byte[] parameter7_4byte357 = new byte[4];
        byte[] parameter7_4byte358 = new byte[4];
        byte[] parameter7_4byte359 = new byte[4];
        byte[] parameter7_4byte360 = new byte[4];
        byte[] parameter7_4byte361 = new byte[4];
        byte[] parameter7_4byte362 = new byte[4];
        byte[] parameter7_4byte363 = new byte[4];
        byte[] parameter7_4byte364 = new byte[4];
        byte[] parameter7_4byte365 = new byte[4];
        byte[] parameter7_4byte366 = new byte[4];
        byte[] parameter7_4byte367 = new byte[4];
        byte[] parameter7_4byte368 = new byte[4];
        byte[] parameter7_4byte369 = new byte[4];
        byte[] parameter7_4byte370 = new byte[4];
        byte[] parameter7_4byte371 = new byte[4];
        byte[] parameter7_4byte372 = new byte[4];
        byte[] parameter7_4byte373 = new byte[4];
        byte[] parameter7_4byte374 = new byte[4];
        byte[] parameter7_4byte375 = new byte[4];
        byte[] parameter7_4byte376 = new byte[4];
        byte[] parameter7_4byte377 = new byte[4];
        byte[] parameter7_4byte378 = new byte[4];
        byte[] parameter7_4byte379 = new byte[4];
        byte[] parameter7_4byte380 = new byte[4];
        byte[] parameter7_4byte381 = new byte[4];
        byte[] parameter7_4byte382 = new byte[4];
        byte[] parameter7_4byte383 = new byte[4];
        byte[] parameter7_4byte384 = new byte[4];
        byte[] parameter7_4byte385 = new byte[4];
        byte[] parameter7_4byte386 = new byte[4];
        byte[] parameter7_4byte387 = new byte[4];
        byte[] parameter7_4byte388 = new byte[4];
        byte[] parameter7_4byte389 = new byte[4];
        byte[] parameter7_4byte390 = new byte[4];
        byte[] parameter7_4byte391 = new byte[4];
        byte[] parameter7_4byte392 = new byte[4];
        byte[] parameter7_4byte393 = new byte[4];
        byte[] parameter7_4byte394 = new byte[4];
        byte[] parameter7_4byte395 = new byte[4];
        byte[] parameter7_4byte396 = new byte[4];
        byte[] parameter7_4byte397 = new byte[4];
        byte[] parameter7_4byte398 = new byte[4];
        byte[] parameter7_4byte399 = new byte[4];
        byte[] parameter7_4byte400 = new byte[4];
        byte[] parameter7_4byte401 = new byte[4];
        byte[] parameter7_4byte402 = new byte[4];
        byte[] parameter7_4byte403 = new byte[4];
        byte[] parameter7_4byte404 = new byte[4];
        byte[] parameter7_4byte405 = new byte[4];
        byte[] parameter7_4byte406 = new byte[4];
        byte[] parameter7_4byte407 = new byte[4];
        byte[] parameter7_4byte408 = new byte[4];
        byte[] parameter7_4byte409 = new byte[4];
        byte[] parameter7_4byte410 = new byte[4];
        byte[] parameter7_4byte411 = new byte[4];
        byte[] parameter7_4byte412 = new byte[4];
        byte[] parameter7_4byte413 = new byte[4];
        byte[] parameter7_4byte414 = new byte[4];
        byte[] parameter7_4byte415 = new byte[4];
        byte[] parameter7_4byte416 = new byte[4];
        byte[] parameter7_4byte417 = new byte[4];
        byte[] parameter7_4byte418 = new byte[4];
        byte[] parameter7_4byte419 = new byte[4];
        byte[] parameter7_4byte420 = new byte[4];
        byte[] parameter7_4byte421 = new byte[4];
        byte[] parameter7_4byte422 = new byte[4];
        byte[] parameter7_4byte423 = new byte[4];
        byte[] parameter7_4byte424 = new byte[4];
        byte[] parameter7_4byte425 = new byte[4];
        byte[] parameter7_4byte426 = new byte[4];
        byte[] parameter7_4byte427 = new byte[4];
        byte[] parameter7_4byte428 = new byte[4];
        byte[] parameter7_4byte429 = new byte[4];
        byte[] parameter7_4byte430 = new byte[4];
        byte[] parameter7_4byte431 = new byte[4];
        byte[] parameter7_4byte432 = new byte[4];
        byte[] parameter7_4byte433 = new byte[4];
        byte[] parameter7_4byte434 = new byte[4];
        byte[] parameter7_4byte435 = new byte[4];
        byte[] parameter7_4byte436 = new byte[4];
        byte[] parameter7_4byte437 = new byte[4];
        byte[] parameter7_4byte438 = new byte[4];
        byte[] parameter7_4byte439 = new byte[4];
        byte[] parameter7_4byte440 = new byte[4];
        byte[] parameter7_4byte441 = new byte[4];
        byte[] parameter7_4byte442 = new byte[4];
        byte[] parameter7_4byte443 = new byte[4];
        byte[] parameter7_4byte444 = new byte[4];
        byte[] parameter7_4byte445 = new byte[4];
        byte[] parameter7_4byte446 = new byte[4];
        byte[] parameter7_4byte447 = new byte[4];
        byte[] parameter7_4byte448 = new byte[4];
        byte[] parameter7_4byte449 = new byte[4];
        byte[] parameter7_4byte450 = new byte[4];
        byte[] parameter7_4byte451 = new byte[4];
        byte[] parameter7_4byte452 = new byte[4];
        byte[] parameter7_4byte453 = new byte[4];
        byte[] parameter7_4byte454 = new byte[4];
        byte[] parameter7_4byte455 = new byte[4];
        byte[] parameter7_4byte456 = new byte[4];
        byte[] parameter7_4byte457 = new byte[4];
        byte[] parameter7_4byte458 = new byte[4];
        byte[] parameter7_4byte459 = new byte[4];
        byte[] parameter7_4byte460 = new byte[4];
        byte[] parameter7_4byte461 = new byte[4];
        byte[] parameter7_4byte462 = new byte[4];
        byte[] parameter7_4byte463 = new byte[4];
        byte[] parameter7_4byte464 = new byte[4];
        byte[] parameter7_4byte465 = new byte[4];
        byte[] parameter7_4byte466 = new byte[4];
        byte[] parameter7_4byte467 = new byte[4];
        byte[] parameter7_4byte468 = new byte[4];
        byte[] parameter7_4byte469 = new byte[4];
        byte[] parameter7_4byte470 = new byte[4];
        byte[] parameter7_4byte471 = new byte[4];
        byte[] parameter7_4byte472 = new byte[4];
        byte[] parameter7_4byte473 = new byte[4];
        byte[] parameter7_4byte474 = new byte[4];
        byte[] parameter7_4byte475 = new byte[4];
        byte[] parameter7_4byte476 = new byte[4];
        byte[] parameter7_4byte477 = new byte[4];
        byte[] parameter7_4byte478 = new byte[4];
        byte[] parameter7_4byte479 = new byte[4];
        byte[] parameter7_4byte480 = new byte[4];
        byte[] parameter7_4byte481 = new byte[4];
        byte[] parameter7_4byte482 = new byte[4];
        byte[] parameter7_4byte483 = new byte[4];
        byte[] parameter7_4byte484 = new byte[4];
        byte[] parameter7_4byte485 = new byte[4];
        byte[] parameter7_4byte486 = new byte[4];
        byte[] parameter7_4byte487 = new byte[4];
        byte[] parameter7_4byte488 = new byte[4];
        byte[] parameter7_4byte489 = new byte[4];
        byte[] parameter7_4byte490 = new byte[4];
        byte[] parameter7_4byte491 = new byte[4];
        byte[] parameter7_4byte492 = new byte[4];
        byte[] parameter7_4byte493 = new byte[4];
        byte[] parameter7_4byte494 = new byte[4];
        byte[] parameter7_4byte495 = new byte[4];
        byte[] parameter7_4byte496 = new byte[4];
        byte[] parameter7_4byte497 = new byte[4];
        byte[] parameter7_4byte498 = new byte[4];
        byte[] parameter7_4byte499 = new byte[4];
        byte[] parameter7_4byte500 = new byte[4];
        byte[] parameter7_4byte501 = new byte[4];
        byte[] parameter7_4byte502 = new byte[4];
        byte[] parameter7_4byte503 = new byte[4];
        byte[] parameter7_4byte504 = new byte[4];
        byte[] parameter7_4byte505 = new byte[4];
        byte[] parameter7_4byte506 = new byte[4];
        byte[] parameter7_4byte507 = new byte[4];
        byte[] parameter7_4byte508 = new byte[4];
        byte[] parameter7_4byte509 = new byte[4];
        byte[] parameter7_4byte510 = new byte[4];
        byte[] parameter7_4byte511 = new byte[4];
        byte[] parameter7_4byte512 = new byte[4];
        #endregion

        #region 파라미터 인수 분리11  parameter7_4byte1_1~ parameter7_4byte1_512
        byte[] parameter7_4byte1_1 = new byte[4];
        byte[] parameter7_4byte1_2 = new byte[4];
        byte[] parameter7_4byte1_3 = new byte[4];
        byte[] parameter7_4byte1_4 = new byte[4];
        byte[] parameter7_4byte1_5 = new byte[4];
        byte[] parameter7_4byte1_6 = new byte[4];
        byte[] parameter7_4byte1_7 = new byte[4];
        byte[] parameter7_4byte1_8 = new byte[4];
        byte[] parameter7_4byte1_9 = new byte[4];
        byte[] parameter7_4byte1_10 = new byte[4];
        byte[] parameter7_4byte1_11 = new byte[4];
        byte[] parameter7_4byte1_12 = new byte[4];
        byte[] parameter7_4byte1_13 = new byte[4];
        byte[] parameter7_4byte1_14 = new byte[4];
        byte[] parameter7_4byte1_15 = new byte[4];
        byte[] parameter7_4byte1_16 = new byte[4];
        byte[] parameter7_4byte1_17 = new byte[4];
        byte[] parameter7_4byte1_18 = new byte[4];
        byte[] parameter7_4byte1_19 = new byte[4];
        byte[] parameter7_4byte1_20 = new byte[4];
        byte[] parameter7_4byte1_21 = new byte[4];
        byte[] parameter7_4byte1_22 = new byte[4];
        byte[] parameter7_4byte1_23 = new byte[4];
        byte[] parameter7_4byte1_24 = new byte[4];
        byte[] parameter7_4byte1_25 = new byte[4];
        byte[] parameter7_4byte1_26 = new byte[4];
        byte[] parameter7_4byte1_27 = new byte[4];
        byte[] parameter7_4byte1_28 = new byte[4];
        byte[] parameter7_4byte1_29 = new byte[4];
        byte[] parameter7_4byte1_30 = new byte[4];
        byte[] parameter7_4byte1_31 = new byte[4];
        byte[] parameter7_4byte1_32 = new byte[4];
        byte[] parameter7_4byte1_33 = new byte[4];
        byte[] parameter7_4byte1_34 = new byte[4];
        byte[] parameter7_4byte1_35 = new byte[4];
        byte[] parameter7_4byte1_36 = new byte[4];
        byte[] parameter7_4byte1_37 = new byte[4];
        byte[] parameter7_4byte1_38 = new byte[4];
        byte[] parameter7_4byte1_39 = new byte[4];
        byte[] parameter7_4byte1_40 = new byte[4];
        byte[] parameter7_4byte1_41 = new byte[4];
        byte[] parameter7_4byte1_42 = new byte[4];
        byte[] parameter7_4byte1_43 = new byte[4];
        byte[] parameter7_4byte1_44 = new byte[4];
        byte[] parameter7_4byte1_45 = new byte[4];
        byte[] parameter7_4byte1_46 = new byte[4];
        byte[] parameter7_4byte1_47 = new byte[4];
        byte[] parameter7_4byte1_48 = new byte[4];
        byte[] parameter7_4byte1_49 = new byte[4];
        byte[] parameter7_4byte1_50 = new byte[4];
        byte[] parameter7_4byte1_51 = new byte[4];
        byte[] parameter7_4byte1_52 = new byte[4];
        byte[] parameter7_4byte1_53 = new byte[4];
        byte[] parameter7_4byte1_54 = new byte[4];
        byte[] parameter7_4byte1_55 = new byte[4];
        byte[] parameter7_4byte1_56 = new byte[4];
        byte[] parameter7_4byte1_57 = new byte[4];
        byte[] parameter7_4byte1_58 = new byte[4];
        byte[] parameter7_4byte1_59 = new byte[4];
        byte[] parameter7_4byte1_60 = new byte[4];
        byte[] parameter7_4byte1_61 = new byte[4];
        byte[] parameter7_4byte1_62 = new byte[4];
        byte[] parameter7_4byte1_63 = new byte[4];
        byte[] parameter7_4byte1_64 = new byte[4];
        byte[] parameter7_4byte1_65 = new byte[4];
        byte[] parameter7_4byte1_66 = new byte[4];
        byte[] parameter7_4byte1_67 = new byte[4];
        byte[] parameter7_4byte1_68 = new byte[4];
        byte[] parameter7_4byte1_69 = new byte[4];
        byte[] parameter7_4byte1_70 = new byte[4];
        byte[] parameter7_4byte1_71 = new byte[4];
        byte[] parameter7_4byte1_72 = new byte[4];
        byte[] parameter7_4byte1_73 = new byte[4];
        byte[] parameter7_4byte1_74 = new byte[4];
        byte[] parameter7_4byte1_75 = new byte[4];
        byte[] parameter7_4byte1_76 = new byte[4];
        byte[] parameter7_4byte1_77 = new byte[4];
        byte[] parameter7_4byte1_78 = new byte[4];
        byte[] parameter7_4byte1_79 = new byte[4];
        byte[] parameter7_4byte1_80 = new byte[4];
        byte[] parameter7_4byte1_81 = new byte[4];
        byte[] parameter7_4byte1_82 = new byte[4];
        byte[] parameter7_4byte1_83 = new byte[4];
        byte[] parameter7_4byte1_84 = new byte[4];
        byte[] parameter7_4byte1_85 = new byte[4];
        byte[] parameter7_4byte1_86 = new byte[4];
        byte[] parameter7_4byte1_87 = new byte[4];
        byte[] parameter7_4byte1_88 = new byte[4];
        byte[] parameter7_4byte1_89 = new byte[4];
        byte[] parameter7_4byte1_90 = new byte[4];
        byte[] parameter7_4byte1_91 = new byte[4];
        byte[] parameter7_4byte1_92 = new byte[4];
        byte[] parameter7_4byte1_93 = new byte[4];
        byte[] parameter7_4byte1_94 = new byte[4];
        byte[] parameter7_4byte1_95 = new byte[4];
        byte[] parameter7_4byte1_96 = new byte[4];
        byte[] parameter7_4byte1_97 = new byte[4];
        byte[] parameter7_4byte1_98 = new byte[4];
        byte[] parameter7_4byte1_99 = new byte[4];
        byte[] parameter7_4byte1_100 = new byte[4];
        byte[] parameter7_4byte1_101 = new byte[4];
        byte[] parameter7_4byte1_102 = new byte[4];
        byte[] parameter7_4byte1_103 = new byte[4];
        byte[] parameter7_4byte1_104 = new byte[4];
        byte[] parameter7_4byte1_105 = new byte[4];
        byte[] parameter7_4byte1_106 = new byte[4];
        byte[] parameter7_4byte1_107 = new byte[4];
        byte[] parameter7_4byte1_108 = new byte[4];
        byte[] parameter7_4byte1_109 = new byte[4];
        byte[] parameter7_4byte1_110 = new byte[4];
        byte[] parameter7_4byte1_111 = new byte[4];
        byte[] parameter7_4byte1_112 = new byte[4];
        byte[] parameter7_4byte1_113 = new byte[4];
        byte[] parameter7_4byte1_114 = new byte[4];
        byte[] parameter7_4byte1_115 = new byte[4];
        byte[] parameter7_4byte1_116 = new byte[4];
        byte[] parameter7_4byte1_117 = new byte[4];
        byte[] parameter7_4byte1_118 = new byte[4];
        byte[] parameter7_4byte1_119 = new byte[4];
        byte[] parameter7_4byte1_120 = new byte[4];
        byte[] parameter7_4byte1_121 = new byte[4];
        byte[] parameter7_4byte1_122 = new byte[4];
        byte[] parameter7_4byte1_123 = new byte[4];
        byte[] parameter7_4byte1_124 = new byte[4];
        byte[] parameter7_4byte1_125 = new byte[4];
        byte[] parameter7_4byte1_126 = new byte[4];
        byte[] parameter7_4byte1_127 = new byte[4];
        byte[] parameter7_4byte1_128 = new byte[4];
        byte[] parameter7_4byte1_129 = new byte[4];
        byte[] parameter7_4byte1_130 = new byte[4];
        byte[] parameter7_4byte1_131 = new byte[4];
        byte[] parameter7_4byte1_132 = new byte[4];
        byte[] parameter7_4byte1_133 = new byte[4];
        byte[] parameter7_4byte1_134 = new byte[4];
        byte[] parameter7_4byte1_135 = new byte[4];
        byte[] parameter7_4byte1_136 = new byte[4];
        byte[] parameter7_4byte1_137 = new byte[4];
        byte[] parameter7_4byte1_138 = new byte[4];
        byte[] parameter7_4byte1_139 = new byte[4];
        byte[] parameter7_4byte1_140 = new byte[4];
        byte[] parameter7_4byte1_141 = new byte[4];
        byte[] parameter7_4byte1_142 = new byte[4];
        byte[] parameter7_4byte1_143 = new byte[4];
        byte[] parameter7_4byte1_144 = new byte[4];
        byte[] parameter7_4byte1_145 = new byte[4];
        byte[] parameter7_4byte1_146 = new byte[4];
        byte[] parameter7_4byte1_147 = new byte[4];
        byte[] parameter7_4byte1_148 = new byte[4];
        byte[] parameter7_4byte1_149 = new byte[4];
        byte[] parameter7_4byte1_150 = new byte[4];
        byte[] parameter7_4byte1_151 = new byte[4];
        byte[] parameter7_4byte1_152 = new byte[4];
        byte[] parameter7_4byte1_153 = new byte[4];
        byte[] parameter7_4byte1_154 = new byte[4];
        byte[] parameter7_4byte1_155 = new byte[4];
        byte[] parameter7_4byte1_156 = new byte[4];
        byte[] parameter7_4byte1_157 = new byte[4];
        byte[] parameter7_4byte1_158 = new byte[4];
        byte[] parameter7_4byte1_159 = new byte[4];
        byte[] parameter7_4byte1_160 = new byte[4];
        byte[] parameter7_4byte1_161 = new byte[4];
        byte[] parameter7_4byte1_162 = new byte[4];
        byte[] parameter7_4byte1_163 = new byte[4];
        byte[] parameter7_4byte1_164 = new byte[4];
        byte[] parameter7_4byte1_165 = new byte[4];
        byte[] parameter7_4byte1_166 = new byte[4];
        byte[] parameter7_4byte1_167 = new byte[4];
        byte[] parameter7_4byte1_168 = new byte[4];
        byte[] parameter7_4byte1_169 = new byte[4];
        byte[] parameter7_4byte1_170 = new byte[4];
        byte[] parameter7_4byte1_171 = new byte[4];
        byte[] parameter7_4byte1_172 = new byte[4];
        byte[] parameter7_4byte1_173 = new byte[4];
        byte[] parameter7_4byte1_174 = new byte[4];
        byte[] parameter7_4byte1_175 = new byte[4];
        byte[] parameter7_4byte1_176 = new byte[4];
        byte[] parameter7_4byte1_177 = new byte[4];
        byte[] parameter7_4byte1_178 = new byte[4];
        byte[] parameter7_4byte1_179 = new byte[4];
        byte[] parameter7_4byte1_180 = new byte[4];
        byte[] parameter7_4byte1_181 = new byte[4];
        byte[] parameter7_4byte1_182 = new byte[4];
        byte[] parameter7_4byte1_183 = new byte[4];
        byte[] parameter7_4byte1_184 = new byte[4];
        byte[] parameter7_4byte1_185 = new byte[4];
        byte[] parameter7_4byte1_186 = new byte[4];
        byte[] parameter7_4byte1_187 = new byte[4];
        byte[] parameter7_4byte1_188 = new byte[4];
        byte[] parameter7_4byte1_189 = new byte[4];
        byte[] parameter7_4byte1_190 = new byte[4];
        byte[] parameter7_4byte1_191 = new byte[4];
        byte[] parameter7_4byte1_192 = new byte[4];
        byte[] parameter7_4byte1_193 = new byte[4];
        byte[] parameter7_4byte1_194 = new byte[4];
        byte[] parameter7_4byte1_195 = new byte[4];
        byte[] parameter7_4byte1_196 = new byte[4];
        byte[] parameter7_4byte1_197 = new byte[4];
        byte[] parameter7_4byte1_198 = new byte[4];
        byte[] parameter7_4byte1_199 = new byte[4];
        byte[] parameter7_4byte1_200 = new byte[4];
        byte[] parameter7_4byte1_201 = new byte[4];
        byte[] parameter7_4byte1_202 = new byte[4];
        byte[] parameter7_4byte1_203 = new byte[4];
        byte[] parameter7_4byte1_204 = new byte[4];
        byte[] parameter7_4byte1_205 = new byte[4];
        byte[] parameter7_4byte1_206 = new byte[4];
        byte[] parameter7_4byte1_207 = new byte[4];
        byte[] parameter7_4byte1_208 = new byte[4];
        byte[] parameter7_4byte1_209 = new byte[4];
        byte[] parameter7_4byte1_210 = new byte[4];
        byte[] parameter7_4byte1_211 = new byte[4];
        byte[] parameter7_4byte1_212 = new byte[4];
        byte[] parameter7_4byte1_213 = new byte[4];
        byte[] parameter7_4byte1_214 = new byte[4];
        byte[] parameter7_4byte1_215 = new byte[4];
        byte[] parameter7_4byte1_216 = new byte[4];
        byte[] parameter7_4byte1_217 = new byte[4];
        byte[] parameter7_4byte1_218 = new byte[4];
        byte[] parameter7_4byte1_219 = new byte[4];
        byte[] parameter7_4byte1_220 = new byte[4];
        byte[] parameter7_4byte1_221 = new byte[4];
        byte[] parameter7_4byte1_222 = new byte[4];
        byte[] parameter7_4byte1_223 = new byte[4];
        byte[] parameter7_4byte1_224 = new byte[4];
        byte[] parameter7_4byte1_225 = new byte[4];
        byte[] parameter7_4byte1_226 = new byte[4];
        byte[] parameter7_4byte1_227 = new byte[4];
        byte[] parameter7_4byte1_228 = new byte[4];
        byte[] parameter7_4byte1_229 = new byte[4];
        byte[] parameter7_4byte1_230 = new byte[4];
        byte[] parameter7_4byte1_231 = new byte[4];
        byte[] parameter7_4byte1_232 = new byte[4];
        byte[] parameter7_4byte1_233 = new byte[4];
        byte[] parameter7_4byte1_234 = new byte[4];
        byte[] parameter7_4byte1_235 = new byte[4];
        byte[] parameter7_4byte1_236 = new byte[4];
        byte[] parameter7_4byte1_237 = new byte[4];
        byte[] parameter7_4byte1_238 = new byte[4];
        byte[] parameter7_4byte1_239 = new byte[4];
        byte[] parameter7_4byte1_240 = new byte[4];
        byte[] parameter7_4byte1_241 = new byte[4];
        byte[] parameter7_4byte1_242 = new byte[4];
        byte[] parameter7_4byte1_243 = new byte[4];
        byte[] parameter7_4byte1_244 = new byte[4];
        byte[] parameter7_4byte1_245 = new byte[4];
        byte[] parameter7_4byte1_246 = new byte[4];
        byte[] parameter7_4byte1_247 = new byte[4];
        byte[] parameter7_4byte1_248 = new byte[4];
        byte[] parameter7_4byte1_249 = new byte[4];
        byte[] parameter7_4byte1_250 = new byte[4];
        byte[] parameter7_4byte1_251 = new byte[4];
        byte[] parameter7_4byte1_252 = new byte[4];
        byte[] parameter7_4byte1_253 = new byte[4];
        byte[] parameter7_4byte1_254 = new byte[4];
        byte[] parameter7_4byte1_255 = new byte[4];
        byte[] parameter7_4byte1_256 = new byte[4];
        byte[] parameter7_4byte1_257 = new byte[4];
        byte[] parameter7_4byte1_258 = new byte[4];
        byte[] parameter7_4byte1_259 = new byte[4];
        byte[] parameter7_4byte1_260 = new byte[4];
        byte[] parameter7_4byte1_261 = new byte[4];
        byte[] parameter7_4byte1_262 = new byte[4];
        byte[] parameter7_4byte1_263 = new byte[4];
        byte[] parameter7_4byte1_264 = new byte[4];
        byte[] parameter7_4byte1_265 = new byte[4];
        byte[] parameter7_4byte1_266 = new byte[4];
        byte[] parameter7_4byte1_267 = new byte[4];
        byte[] parameter7_4byte1_268 = new byte[4];
        byte[] parameter7_4byte1_269 = new byte[4];
        byte[] parameter7_4byte1_270 = new byte[4];
        byte[] parameter7_4byte1_271 = new byte[4];
        byte[] parameter7_4byte1_272 = new byte[4];
        byte[] parameter7_4byte1_273 = new byte[4];
        byte[] parameter7_4byte1_274 = new byte[4];
        byte[] parameter7_4byte1_275 = new byte[4];
        byte[] parameter7_4byte1_276 = new byte[4];
        byte[] parameter7_4byte1_277 = new byte[4];
        byte[] parameter7_4byte1_278 = new byte[4];
        byte[] parameter7_4byte1_279 = new byte[4];
        byte[] parameter7_4byte1_280 = new byte[4];
        byte[] parameter7_4byte1_281 = new byte[4];
        byte[] parameter7_4byte1_282 = new byte[4];
        byte[] parameter7_4byte1_283 = new byte[4];
        byte[] parameter7_4byte1_284 = new byte[4];
        byte[] parameter7_4byte1_285 = new byte[4];
        byte[] parameter7_4byte1_286 = new byte[4];
        byte[] parameter7_4byte1_287 = new byte[4];
        byte[] parameter7_4byte1_288 = new byte[4];
        byte[] parameter7_4byte1_289 = new byte[4];
        byte[] parameter7_4byte1_290 = new byte[4];
        byte[] parameter7_4byte1_291 = new byte[4];
        byte[] parameter7_4byte1_292 = new byte[4];
        byte[] parameter7_4byte1_293 = new byte[4];
        byte[] parameter7_4byte1_294 = new byte[4];
        byte[] parameter7_4byte1_295 = new byte[4];
        byte[] parameter7_4byte1_296 = new byte[4];
        byte[] parameter7_4byte1_297 = new byte[4];
        byte[] parameter7_4byte1_298 = new byte[4];
        byte[] parameter7_4byte1_299 = new byte[4];
        byte[] parameter7_4byte1_300 = new byte[4];
        byte[] parameter7_4byte1_301 = new byte[4];
        byte[] parameter7_4byte1_302 = new byte[4];
        byte[] parameter7_4byte1_303 = new byte[4];
        byte[] parameter7_4byte1_304 = new byte[4];
        byte[] parameter7_4byte1_305 = new byte[4];
        byte[] parameter7_4byte1_306 = new byte[4];
        byte[] parameter7_4byte1_307 = new byte[4];
        byte[] parameter7_4byte1_308 = new byte[4];
        byte[] parameter7_4byte1_309 = new byte[4];
        byte[] parameter7_4byte1_310 = new byte[4];
        byte[] parameter7_4byte1_311 = new byte[4];
        byte[] parameter7_4byte1_312 = new byte[4];
        byte[] parameter7_4byte1_313 = new byte[4];
        byte[] parameter7_4byte1_314 = new byte[4];
        byte[] parameter7_4byte1_315 = new byte[4];
        byte[] parameter7_4byte1_316 = new byte[4];
        byte[] parameter7_4byte1_317 = new byte[4];
        byte[] parameter7_4byte1_318 = new byte[4];
        byte[] parameter7_4byte1_319 = new byte[4];
        byte[] parameter7_4byte1_320 = new byte[4];
        byte[] parameter7_4byte1_321 = new byte[4];
        byte[] parameter7_4byte1_322 = new byte[4];
        byte[] parameter7_4byte1_323 = new byte[4];
        byte[] parameter7_4byte1_324 = new byte[4];
        byte[] parameter7_4byte1_325 = new byte[4];
        byte[] parameter7_4byte1_326 = new byte[4];
        byte[] parameter7_4byte1_327 = new byte[4];
        byte[] parameter7_4byte1_328 = new byte[4];
        byte[] parameter7_4byte1_329 = new byte[4];
        byte[] parameter7_4byte1_330 = new byte[4];
        byte[] parameter7_4byte1_331 = new byte[4];
        byte[] parameter7_4byte1_332 = new byte[4];
        byte[] parameter7_4byte1_333 = new byte[4];
        byte[] parameter7_4byte1_334 = new byte[4];
        byte[] parameter7_4byte1_335 = new byte[4];
        byte[] parameter7_4byte1_336 = new byte[4];
        byte[] parameter7_4byte1_337 = new byte[4];
        byte[] parameter7_4byte1_338 = new byte[4];
        byte[] parameter7_4byte1_339 = new byte[4];
        byte[] parameter7_4byte1_340 = new byte[4];
        byte[] parameter7_4byte1_341 = new byte[4];
        byte[] parameter7_4byte1_342 = new byte[4];
        byte[] parameter7_4byte1_343 = new byte[4];
        byte[] parameter7_4byte1_344 = new byte[4];
        byte[] parameter7_4byte1_345 = new byte[4];
        byte[] parameter7_4byte1_346 = new byte[4];
        byte[] parameter7_4byte1_347 = new byte[4];
        byte[] parameter7_4byte1_348 = new byte[4];
        byte[] parameter7_4byte1_349 = new byte[4];
        byte[] parameter7_4byte1_350 = new byte[4];
        byte[] parameter7_4byte1_351 = new byte[4];
        byte[] parameter7_4byte1_352 = new byte[4];
        byte[] parameter7_4byte1_353 = new byte[4];
        byte[] parameter7_4byte1_354 = new byte[4];
        byte[] parameter7_4byte1_355 = new byte[4];
        byte[] parameter7_4byte1_356 = new byte[4];
        byte[] parameter7_4byte1_357 = new byte[4];
        byte[] parameter7_4byte1_358 = new byte[4];
        byte[] parameter7_4byte1_359 = new byte[4];
        byte[] parameter7_4byte1_360 = new byte[4];
        byte[] parameter7_4byte1_361 = new byte[4];
        byte[] parameter7_4byte1_362 = new byte[4];
        byte[] parameter7_4byte1_363 = new byte[4];
        byte[] parameter7_4byte1_364 = new byte[4];
        byte[] parameter7_4byte1_365 = new byte[4];
        byte[] parameter7_4byte1_366 = new byte[4];
        byte[] parameter7_4byte1_367 = new byte[4];
        byte[] parameter7_4byte1_368 = new byte[4];
        byte[] parameter7_4byte1_369 = new byte[4];
        byte[] parameter7_4byte1_370 = new byte[4];
        byte[] parameter7_4byte1_371 = new byte[4];
        byte[] parameter7_4byte1_372 = new byte[4];
        byte[] parameter7_4byte1_373 = new byte[4];
        byte[] parameter7_4byte1_374 = new byte[4];
        byte[] parameter7_4byte1_375 = new byte[4];
        byte[] parameter7_4byte1_376 = new byte[4];
        byte[] parameter7_4byte1_377 = new byte[4];
        byte[] parameter7_4byte1_378 = new byte[4];
        byte[] parameter7_4byte1_379 = new byte[4];
        byte[] parameter7_4byte1_380 = new byte[4];
        byte[] parameter7_4byte1_381 = new byte[4];
        byte[] parameter7_4byte1_382 = new byte[4];
        byte[] parameter7_4byte1_383 = new byte[4];
        byte[] parameter7_4byte1_384 = new byte[4];
        byte[] parameter7_4byte1_385 = new byte[4];
        byte[] parameter7_4byte1_386 = new byte[4];
        byte[] parameter7_4byte1_387 = new byte[4];
        byte[] parameter7_4byte1_388 = new byte[4];
        byte[] parameter7_4byte1_389 = new byte[4];
        byte[] parameter7_4byte1_390 = new byte[4];
        byte[] parameter7_4byte1_391 = new byte[4];
        byte[] parameter7_4byte1_392 = new byte[4];
        byte[] parameter7_4byte1_393 = new byte[4];
        byte[] parameter7_4byte1_394 = new byte[4];
        byte[] parameter7_4byte1_395 = new byte[4];
        byte[] parameter7_4byte1_396 = new byte[4];
        byte[] parameter7_4byte1_397 = new byte[4];
        byte[] parameter7_4byte1_398 = new byte[4];
        byte[] parameter7_4byte1_399 = new byte[4];
        byte[] parameter7_4byte1_400 = new byte[4];
        byte[] parameter7_4byte1_401 = new byte[4];
        byte[] parameter7_4byte1_402 = new byte[4];
        byte[] parameter7_4byte1_403 = new byte[4];
        byte[] parameter7_4byte1_404 = new byte[4];
        byte[] parameter7_4byte1_405 = new byte[4];
        byte[] parameter7_4byte1_406 = new byte[4];
        byte[] parameter7_4byte1_407 = new byte[4];
        byte[] parameter7_4byte1_408 = new byte[4];
        byte[] parameter7_4byte1_409 = new byte[4];
        byte[] parameter7_4byte1_410 = new byte[4];
        byte[] parameter7_4byte1_411 = new byte[4];
        byte[] parameter7_4byte1_412 = new byte[4];
        byte[] parameter7_4byte1_413 = new byte[4];
        byte[] parameter7_4byte1_414 = new byte[4];
        byte[] parameter7_4byte1_415 = new byte[4];
        byte[] parameter7_4byte1_416 = new byte[4];
        byte[] parameter7_4byte1_417 = new byte[4];
        byte[] parameter7_4byte1_418 = new byte[4];
        byte[] parameter7_4byte1_419 = new byte[4];
        byte[] parameter7_4byte1_420 = new byte[4];
        byte[] parameter7_4byte1_421 = new byte[4];
        byte[] parameter7_4byte1_422 = new byte[4];
        byte[] parameter7_4byte1_423 = new byte[4];
        byte[] parameter7_4byte1_424 = new byte[4];
        byte[] parameter7_4byte1_425 = new byte[4];
        byte[] parameter7_4byte1_426 = new byte[4];
        byte[] parameter7_4byte1_427 = new byte[4];
        byte[] parameter7_4byte1_428 = new byte[4];
        byte[] parameter7_4byte1_429 = new byte[4];
        byte[] parameter7_4byte1_430 = new byte[4];
        byte[] parameter7_4byte1_431 = new byte[4];
        byte[] parameter7_4byte1_432 = new byte[4];
        byte[] parameter7_4byte1_433 = new byte[4];
        byte[] parameter7_4byte1_434 = new byte[4];
        byte[] parameter7_4byte1_435 = new byte[4];
        byte[] parameter7_4byte1_436 = new byte[4];
        byte[] parameter7_4byte1_437 = new byte[4];
        byte[] parameter7_4byte1_438 = new byte[4];
        byte[] parameter7_4byte1_439 = new byte[4];
        byte[] parameter7_4byte1_440 = new byte[4];
        byte[] parameter7_4byte1_441 = new byte[4];
        byte[] parameter7_4byte1_442 = new byte[4];
        byte[] parameter7_4byte1_443 = new byte[4];
        byte[] parameter7_4byte1_444 = new byte[4];
        byte[] parameter7_4byte1_445 = new byte[4];
        byte[] parameter7_4byte1_446 = new byte[4];
        byte[] parameter7_4byte1_447 = new byte[4];
        byte[] parameter7_4byte1_448 = new byte[4];
        byte[] parameter7_4byte1_449 = new byte[4];
        byte[] parameter7_4byte1_450 = new byte[4];
        byte[] parameter7_4byte1_451 = new byte[4];
        byte[] parameter7_4byte1_452 = new byte[4];
        byte[] parameter7_4byte1_453 = new byte[4];
        byte[] parameter7_4byte1_454 = new byte[4];
        byte[] parameter7_4byte1_455 = new byte[4];
        byte[] parameter7_4byte1_456 = new byte[4];
        byte[] parameter7_4byte1_457 = new byte[4];
        byte[] parameter7_4byte1_458 = new byte[4];
        byte[] parameter7_4byte1_459 = new byte[4];
        byte[] parameter7_4byte1_460 = new byte[4];
        byte[] parameter7_4byte1_461 = new byte[4];
        byte[] parameter7_4byte1_462 = new byte[4];
        byte[] parameter7_4byte1_463 = new byte[4];
        byte[] parameter7_4byte1_464 = new byte[4];
        byte[] parameter7_4byte1_465 = new byte[4];
        byte[] parameter7_4byte1_466 = new byte[4];
        byte[] parameter7_4byte1_467 = new byte[4];
        byte[] parameter7_4byte1_468 = new byte[4];
        byte[] parameter7_4byte1_469 = new byte[4];
        byte[] parameter7_4byte1_470 = new byte[4];
        byte[] parameter7_4byte1_471 = new byte[4];
        byte[] parameter7_4byte1_472 = new byte[4];
        byte[] parameter7_4byte1_473 = new byte[4];
        byte[] parameter7_4byte1_474 = new byte[4];
        byte[] parameter7_4byte1_475 = new byte[4];
        byte[] parameter7_4byte1_476 = new byte[4];
        byte[] parameter7_4byte1_477 = new byte[4];
        byte[] parameter7_4byte1_478 = new byte[4];
        byte[] parameter7_4byte1_479 = new byte[4];
        byte[] parameter7_4byte1_480 = new byte[4];
        byte[] parameter7_4byte1_481 = new byte[4];
        byte[] parameter7_4byte1_482 = new byte[4];
        byte[] parameter7_4byte1_483 = new byte[4];
        byte[] parameter7_4byte1_484 = new byte[4];
        byte[] parameter7_4byte1_485 = new byte[4];
        byte[] parameter7_4byte1_486 = new byte[4];
        byte[] parameter7_4byte1_487 = new byte[4];
        byte[] parameter7_4byte1_488 = new byte[4];
        byte[] parameter7_4byte1_489 = new byte[4];
        byte[] parameter7_4byte1_490 = new byte[4];
        byte[] parameter7_4byte1_491 = new byte[4];
        byte[] parameter7_4byte1_492 = new byte[4];
        byte[] parameter7_4byte1_493 = new byte[4];
        byte[] parameter7_4byte1_494 = new byte[4];
        byte[] parameter7_4byte1_495 = new byte[4];
        byte[] parameter7_4byte1_496 = new byte[4];
        byte[] parameter7_4byte1_497 = new byte[4];
        byte[] parameter7_4byte1_498 = new byte[4];
        byte[] parameter7_4byte1_499 = new byte[4];
        byte[] parameter7_4byte1_500 = new byte[4];
        byte[] parameter7_4byte1_501 = new byte[4];
        byte[] parameter7_4byte1_502 = new byte[4];
        byte[] parameter7_4byte1_503 = new byte[4];
        byte[] parameter7_4byte1_504 = new byte[4];
        byte[] parameter7_4byte1_505 = new byte[4];
        byte[] parameter7_4byte1_506 = new byte[4];
        byte[] parameter7_4byte1_507 = new byte[4];
        byte[] parameter7_4byte1_508 = new byte[4];
        byte[] parameter7_4byte1_509 = new byte[4];
        byte[] parameter7_4byte1_510 = new byte[4];
        byte[] parameter7_4byte1_511 = new byte[4];
        byte[] parameter7_4byte1_512 = new byte[4];
        #endregion
        

        //byte[] parameter7_4byte11 = new byte[4];
        //byte[] parameter7_4byte22 = new byte[4];


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
            //BlockParaModel2s[0].SettingValue = 33;            //Block속도 파라미터 값
            Debug.WriteLine("수신버튼 테스트");
        }
        private bool CanexecuteRecCommand(object parameter)
        {
            return true;
        }

        //블럭 파라미터 수신 백그라운드 워커
        private void BlockParameterRec(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 256; i++)
            {
                BlockParameterRec(i);
                Thread.Sleep(10);
            }

            #region 블럭 파라미터 수신 변수 Reverse처리
            Array.Reverse(recValue1);
            Array.Reverse(recValue2);
            Array.Reverse(recValue3);
            Array.Reverse(recValue4);
            Array.Reverse(recValue5);
            Array.Reverse(recValue6);
            Array.Reverse(recValue7);
            Array.Reverse(recValue8);
            Array.Reverse(recValue9);
            Array.Reverse(recValue10);
            Array.Reverse(recValue11);
            Array.Reverse(recValue12);
            Array.Reverse(recValue13);
            Array.Reverse(recValue14);
            Array.Reverse(recValue15);
            Array.Reverse(recValue16);
            Array.Reverse(recValue17);
            Array.Reverse(recValue18);
            Array.Reverse(recValue19);
            Array.Reverse(recValue20);
            Array.Reverse(recValue21);
            Array.Reverse(recValue22);
            Array.Reverse(recValue23);
            Array.Reverse(recValue24);
            Array.Reverse(recValue25);
            Array.Reverse(recValue26);
            Array.Reverse(recValue27);
            Array.Reverse(recValue28);
            Array.Reverse(recValue29);
            Array.Reverse(recValue30);
            Array.Reverse(recValue31);
            Array.Reverse(recValue32);
            Array.Reverse(recValue33);
            Array.Reverse(recValue34);
            Array.Reverse(recValue35);
            Array.Reverse(recValue36);
            Array.Reverse(recValue37);
            Array.Reverse(recValue38);
            Array.Reverse(recValue39);
            Array.Reverse(recValue40);
            Array.Reverse(recValue41);
            Array.Reverse(recValue42);
            Array.Reverse(recValue43);
            Array.Reverse(recValue44);
            Array.Reverse(recValue45);
            Array.Reverse(recValue46);
            Array.Reverse(recValue47);
            Array.Reverse(recValue48);
            Array.Reverse(recValue49);
            Array.Reverse(recValue50);
            Array.Reverse(recValue51);
            Array.Reverse(recValue52);
            Array.Reverse(recValue53);
            Array.Reverse(recValue54);
            Array.Reverse(recValue55);
            Array.Reverse(recValue56);
            Array.Reverse(recValue57);
            Array.Reverse(recValue58);
            Array.Reverse(recValue59);
            Array.Reverse(recValue60);
            Array.Reverse(recValue61);
            Array.Reverse(recValue62);
            Array.Reverse(recValue63);
            Array.Reverse(recValue64);
            Array.Reverse(recValue65);
            Array.Reverse(recValue66);
            Array.Reverse(recValue67);
            Array.Reverse(recValue68);
            Array.Reverse(recValue69);
            Array.Reverse(recValue70);
            Array.Reverse(recValue71);
            Array.Reverse(recValue72);
            Array.Reverse(recValue73);
            Array.Reverse(recValue74);
            Array.Reverse(recValue75);
            Array.Reverse(recValue76);
            Array.Reverse(recValue77);
            Array.Reverse(recValue78);
            Array.Reverse(recValue79);
            Array.Reverse(recValue80);
            Array.Reverse(recValue81);
            Array.Reverse(recValue82);
            Array.Reverse(recValue83);
            Array.Reverse(recValue84);
            Array.Reverse(recValue85);
            Array.Reverse(recValue86);
            Array.Reverse(recValue87);
            Array.Reverse(recValue88);
            Array.Reverse(recValue89);
            Array.Reverse(recValue90);
            Array.Reverse(recValue91);
            Array.Reverse(recValue92);
            Array.Reverse(recValue93);
            Array.Reverse(recValue94);
            Array.Reverse(recValue95);
            Array.Reverse(recValue96);
            Array.Reverse(recValue97);
            Array.Reverse(recValue98);
            Array.Reverse(recValue99);
            Array.Reverse(recValue100);
            Array.Reverse(recValue101);
            Array.Reverse(recValue102);
            Array.Reverse(recValue103);
            Array.Reverse(recValue104);
            Array.Reverse(recValue105);
            Array.Reverse(recValue106);
            Array.Reverse(recValue107);
            Array.Reverse(recValue108);
            Array.Reverse(recValue109);
            Array.Reverse(recValue110);
            Array.Reverse(recValue111);
            Array.Reverse(recValue112);
            Array.Reverse(recValue113);
            Array.Reverse(recValue114);
            Array.Reverse(recValue115);
            Array.Reverse(recValue116);
            Array.Reverse(recValue117);
            Array.Reverse(recValue118);
            Array.Reverse(recValue119);
            Array.Reverse(recValue120);
            Array.Reverse(recValue121);
            Array.Reverse(recValue122);
            Array.Reverse(recValue123);
            Array.Reverse(recValue124);
            Array.Reverse(recValue125);
            Array.Reverse(recValue126);
            Array.Reverse(recValue127);
            Array.Reverse(recValue128);
            Array.Reverse(recValue129);
            Array.Reverse(recValue130);
            Array.Reverse(recValue131);
            Array.Reverse(recValue132);
            Array.Reverse(recValue133);
            Array.Reverse(recValue134);
            Array.Reverse(recValue135);
            Array.Reverse(recValue136);
            Array.Reverse(recValue137);
            Array.Reverse(recValue138);
            Array.Reverse(recValue139);
            Array.Reverse(recValue140);
            Array.Reverse(recValue141);
            Array.Reverse(recValue142);
            Array.Reverse(recValue143);
            Array.Reverse(recValue144);
            Array.Reverse(recValue145);
            Array.Reverse(recValue146);
            Array.Reverse(recValue147);
            Array.Reverse(recValue148);
            Array.Reverse(recValue149);
            Array.Reverse(recValue150);
            Array.Reverse(recValue151);
            Array.Reverse(recValue152);
            Array.Reverse(recValue153);
            Array.Reverse(recValue154);
            Array.Reverse(recValue155);
            Array.Reverse(recValue156);
            Array.Reverse(recValue157);
            Array.Reverse(recValue158);
            Array.Reverse(recValue159);
            Array.Reverse(recValue160);
            Array.Reverse(recValue161);
            Array.Reverse(recValue162);
            Array.Reverse(recValue163);
            Array.Reverse(recValue164);
            Array.Reverse(recValue165);
            Array.Reverse(recValue166);
            Array.Reverse(recValue167);
            Array.Reverse(recValue168);
            Array.Reverse(recValue169);
            Array.Reverse(recValue170);
            Array.Reverse(recValue171);
            Array.Reverse(recValue172);
            Array.Reverse(recValue173);
            Array.Reverse(recValue174);
            Array.Reverse(recValue175);
            Array.Reverse(recValue176);
            Array.Reverse(recValue177);
            Array.Reverse(recValue178);
            Array.Reverse(recValue179);
            Array.Reverse(recValue180);
            Array.Reverse(recValue181);
            Array.Reverse(recValue182);
            Array.Reverse(recValue183);
            Array.Reverse(recValue184);
            Array.Reverse(recValue185);
            Array.Reverse(recValue186);
            Array.Reverse(recValue187);
            Array.Reverse(recValue188);
            Array.Reverse(recValue189);
            Array.Reverse(recValue190);
            Array.Reverse(recValue191);
            Array.Reverse(recValue192);
            Array.Reverse(recValue193);
            Array.Reverse(recValue194);
            Array.Reverse(recValue195);
            Array.Reverse(recValue196);
            Array.Reverse(recValue197);
            Array.Reverse(recValue198);
            Array.Reverse(recValue199);
            Array.Reverse(recValue200);
            Array.Reverse(recValue201);
            Array.Reverse(recValue202);
            Array.Reverse(recValue203);
            Array.Reverse(recValue204);
            Array.Reverse(recValue205);
            Array.Reverse(recValue206);
            Array.Reverse(recValue207);
            Array.Reverse(recValue208);
            Array.Reverse(recValue209);
            Array.Reverse(recValue210);
            Array.Reverse(recValue211);
            Array.Reverse(recValue212);
            Array.Reverse(recValue213);
            Array.Reverse(recValue214);
            Array.Reverse(recValue215);
            Array.Reverse(recValue216);
            Array.Reverse(recValue217);
            Array.Reverse(recValue218);
            Array.Reverse(recValue219);
            Array.Reverse(recValue220);
            Array.Reverse(recValue221);
            Array.Reverse(recValue222);
            Array.Reverse(recValue223);
            Array.Reverse(recValue224);
            Array.Reverse(recValue225);
            Array.Reverse(recValue226);
            Array.Reverse(recValue227);
            Array.Reverse(recValue228);
            Array.Reverse(recValue229);
            Array.Reverse(recValue230);
            Array.Reverse(recValue231);
            Array.Reverse(recValue232);
            Array.Reverse(recValue233);
            Array.Reverse(recValue234);
            Array.Reverse(recValue235);
            Array.Reverse(recValue236);
            Array.Reverse(recValue237);
            Array.Reverse(recValue238);
            Array.Reverse(recValue239);
            Array.Reverse(recValue240);
            Array.Reverse(recValue241);
            Array.Reverse(recValue242);
            Array.Reverse(recValue243);
            Array.Reverse(recValue244);
            Array.Reverse(recValue245);
            Array.Reverse(recValue246);
            Array.Reverse(recValue247);
            Array.Reverse(recValue248);
            Array.Reverse(recValue249);
            Array.Reverse(recValue250);
            Array.Reverse(recValue251);
            Array.Reverse(recValue252);
            Array.Reverse(recValue253);
            Array.Reverse(recValue254);
            Array.Reverse(recValue255);
            Array.Reverse(recValue256);
            Array.Reverse(recValue257);
            Array.Reverse(recValue258);
            Array.Reverse(recValue259);
            Array.Reverse(recValue260);
            Array.Reverse(recValue261);
            Array.Reverse(recValue262);
            Array.Reverse(recValue263);
            Array.Reverse(recValue264);
            Array.Reverse(recValue265);
            Array.Reverse(recValue266);
            Array.Reverse(recValue267);
            Array.Reverse(recValue268);
            Array.Reverse(recValue269);
            Array.Reverse(recValue270);
            Array.Reverse(recValue271);
            Array.Reverse(recValue272);
            Array.Reverse(recValue273);
            Array.Reverse(recValue274);
            Array.Reverse(recValue275);
            Array.Reverse(recValue276);
            Array.Reverse(recValue277);
            Array.Reverse(recValue278);
            Array.Reverse(recValue279);
            Array.Reverse(recValue280);
            Array.Reverse(recValue281);
            Array.Reverse(recValue282);
            Array.Reverse(recValue283);
            Array.Reverse(recValue284);
            Array.Reverse(recValue285);
            Array.Reverse(recValue286);
            Array.Reverse(recValue287);
            Array.Reverse(recValue288);
            Array.Reverse(recValue289);
            Array.Reverse(recValue290);
            Array.Reverse(recValue291);
            Array.Reverse(recValue292);
            Array.Reverse(recValue293);
            Array.Reverse(recValue294);
            Array.Reverse(recValue295);
            Array.Reverse(recValue296);
            Array.Reverse(recValue297);
            Array.Reverse(recValue298);
            Array.Reverse(recValue299);
            Array.Reverse(recValue300);
            Array.Reverse(recValue301);
            Array.Reverse(recValue302);
            Array.Reverse(recValue303);
            Array.Reverse(recValue304);
            Array.Reverse(recValue305);
            Array.Reverse(recValue306);
            Array.Reverse(recValue307);
            Array.Reverse(recValue308);
            Array.Reverse(recValue309);
            Array.Reverse(recValue310);
            Array.Reverse(recValue311);
            Array.Reverse(recValue312);
            Array.Reverse(recValue313);
            Array.Reverse(recValue314);
            Array.Reverse(recValue315);
            Array.Reverse(recValue316);
            Array.Reverse(recValue317);
            Array.Reverse(recValue318);
            Array.Reverse(recValue319);
            Array.Reverse(recValue320);
            Array.Reverse(recValue321);
            Array.Reverse(recValue322);
            Array.Reverse(recValue323);
            Array.Reverse(recValue324);
            Array.Reverse(recValue325);
            Array.Reverse(recValue326);
            Array.Reverse(recValue327);
            Array.Reverse(recValue328);
            Array.Reverse(recValue329);
            Array.Reverse(recValue330);
            Array.Reverse(recValue331);
            Array.Reverse(recValue332);
            Array.Reverse(recValue333);
            Array.Reverse(recValue334);
            Array.Reverse(recValue335);
            Array.Reverse(recValue336);
            Array.Reverse(recValue337);
            Array.Reverse(recValue338);
            Array.Reverse(recValue339);
            Array.Reverse(recValue340);
            Array.Reverse(recValue341);
            Array.Reverse(recValue342);
            Array.Reverse(recValue343);
            Array.Reverse(recValue344);
            Array.Reverse(recValue345);
            Array.Reverse(recValue346);
            Array.Reverse(recValue347);
            Array.Reverse(recValue348);
            Array.Reverse(recValue349);
            Array.Reverse(recValue350);
            Array.Reverse(recValue351);
            Array.Reverse(recValue352);
            Array.Reverse(recValue353);
            Array.Reverse(recValue354);
            Array.Reverse(recValue355);
            Array.Reverse(recValue356);
            Array.Reverse(recValue357);
            Array.Reverse(recValue358);
            Array.Reverse(recValue359);
            Array.Reverse(recValue360);
            Array.Reverse(recValue361);
            Array.Reverse(recValue362);
            Array.Reverse(recValue363);
            Array.Reverse(recValue364);
            Array.Reverse(recValue365);
            Array.Reverse(recValue366);
            Array.Reverse(recValue367);
            Array.Reverse(recValue368);
            Array.Reverse(recValue369);
            Array.Reverse(recValue370);
            Array.Reverse(recValue371);
            Array.Reverse(recValue372);
            Array.Reverse(recValue373);
            Array.Reverse(recValue374);
            Array.Reverse(recValue375);
            Array.Reverse(recValue376);
            Array.Reverse(recValue377);
            Array.Reverse(recValue378);
            Array.Reverse(recValue379);
            Array.Reverse(recValue380);
            Array.Reverse(recValue381);
            Array.Reverse(recValue382);
            Array.Reverse(recValue383);
            Array.Reverse(recValue384);
            Array.Reverse(recValue385);
            Array.Reverse(recValue386);
            Array.Reverse(recValue387);
            Array.Reverse(recValue388);
            Array.Reverse(recValue389);
            Array.Reverse(recValue390);
            Array.Reverse(recValue391);
            Array.Reverse(recValue392);
            Array.Reverse(recValue393);
            Array.Reverse(recValue394);
            Array.Reverse(recValue395);
            Array.Reverse(recValue396);
            Array.Reverse(recValue397);
            Array.Reverse(recValue398);
            Array.Reverse(recValue399);
            Array.Reverse(recValue400);
            Array.Reverse(recValue391);
            Array.Reverse(recValue392);
            Array.Reverse(recValue393);
            Array.Reverse(recValue394);
            Array.Reverse(recValue395);
            Array.Reverse(recValue396);
            Array.Reverse(recValue397);
            Array.Reverse(recValue398);
            Array.Reverse(recValue399);
            Array.Reverse(recValue400);
            Array.Reverse(recValue401);
            Array.Reverse(recValue402);
            Array.Reverse(recValue403);
            Array.Reverse(recValue404);
            Array.Reverse(recValue405);
            Array.Reverse(recValue406);
            Array.Reverse(recValue407);
            Array.Reverse(recValue408);
            Array.Reverse(recValue409);
            Array.Reverse(recValue400);
            Array.Reverse(recValue411);
            Array.Reverse(recValue412);
            Array.Reverse(recValue413);
            Array.Reverse(recValue414);
            Array.Reverse(recValue415);
            Array.Reverse(recValue416);
            Array.Reverse(recValue417);
            Array.Reverse(recValue418);
            Array.Reverse(recValue419);
            Array.Reverse(recValue420);
            Array.Reverse(recValue421);
            Array.Reverse(recValue422);
            Array.Reverse(recValue423);
            Array.Reverse(recValue424);
            Array.Reverse(recValue425);
            Array.Reverse(recValue426);
            Array.Reverse(recValue427);
            Array.Reverse(recValue428);
            Array.Reverse(recValue429);
            Array.Reverse(recValue430);
            Array.Reverse(recValue431);
            Array.Reverse(recValue432);
            Array.Reverse(recValue433);
            Array.Reverse(recValue434);
            Array.Reverse(recValue435);
            Array.Reverse(recValue436);
            Array.Reverse(recValue437);
            Array.Reverse(recValue438);
            Array.Reverse(recValue439);
            Array.Reverse(recValue440);
            Array.Reverse(recValue441);
            Array.Reverse(recValue442);
            Array.Reverse(recValue443);
            Array.Reverse(recValue444);
            Array.Reverse(recValue445);
            Array.Reverse(recValue446);
            Array.Reverse(recValue447);
            Array.Reverse(recValue448);
            Array.Reverse(recValue449);
            Array.Reverse(recValue450);
            Array.Reverse(recValue451);
            Array.Reverse(recValue452);
            Array.Reverse(recValue453);
            Array.Reverse(recValue454);
            Array.Reverse(recValue455);
            Array.Reverse(recValue456);
            Array.Reverse(recValue457);
            Array.Reverse(recValue458);
            Array.Reverse(recValue459);
            Array.Reverse(recValue460);
            Array.Reverse(recValue461);
            Array.Reverse(recValue462);
            Array.Reverse(recValue463);
            Array.Reverse(recValue464);
            Array.Reverse(recValue465);
            Array.Reverse(recValue466);
            Array.Reverse(recValue467);
            Array.Reverse(recValue468);
            Array.Reverse(recValue469);
            Array.Reverse(recValue470);
            Array.Reverse(recValue471);
            Array.Reverse(recValue472);
            Array.Reverse(recValue473);
            Array.Reverse(recValue474);
            Array.Reverse(recValue475);
            Array.Reverse(recValue476);
            Array.Reverse(recValue477);
            Array.Reverse(recValue478);
            Array.Reverse(recValue479);
            Array.Reverse(recValue480);
            Array.Reverse(recValue481);
            Array.Reverse(recValue482);
            Array.Reverse(recValue483);
            Array.Reverse(recValue484);
            Array.Reverse(recValue485);
            Array.Reverse(recValue486);
            Array.Reverse(recValue487);
            Array.Reverse(recValue488);
            Array.Reverse(recValue489);
            Array.Reverse(recValue490);
            Array.Reverse(recValue491);
            Array.Reverse(recValue492);
            Array.Reverse(recValue493);
            Array.Reverse(recValue494);
            Array.Reverse(recValue495);
            Array.Reverse(recValue496);
            Array.Reverse(recValue497);
            Array.Reverse(recValue498);
            Array.Reverse(recValue499);
            Array.Reverse(recValue500);
            Array.Reverse(recValue501);
            Array.Reverse(recValue502);
            Array.Reverse(recValue503);
            Array.Reverse(recValue504);
            Array.Reverse(recValue505);
            Array.Reverse(recValue506);
            Array.Reverse(recValue507);
            Array.Reverse(recValue508);
            Array.Reverse(recValue509);
            Array.Reverse(recValue510);
            Array.Reverse(recValue511);
            Array.Reverse(recValue512);
            #endregion

            #region 블럭 수신 데이터 변수에 할당
            Array.Copy(recValue1, 0, parameter7_4byte1, 0, 4);
            Array.Copy(recValue2, 0, parameter7_4byte2, 0, 4);
            Array.Copy(recValue3, 0, parameter7_4byte3, 0, 4);
            Array.Copy(recValue4, 0, parameter7_4byte4, 0, 4);
            Array.Copy(recValue5, 0, parameter7_4byte5, 0, 4);
            Array.Copy(recValue6, 0, parameter7_4byte6, 0, 4);
            Array.Copy(recValue7, 0, parameter7_4byte7, 0, 4);
            Array.Copy(recValue8, 0, parameter7_4byte8, 0, 4);
            Array.Copy(recValue9, 0, parameter7_4byte9, 0, 4);
            Array.Copy(recValue10, 0, parameter7_4byte10, 0, 4);
            Array.Copy(recValue11, 0, parameter7_4byte11, 0, 4);
            Array.Copy(recValue12, 0, parameter7_4byte12, 0, 4);
            Array.Copy(recValue13, 0, parameter7_4byte13, 0, 4);
            Array.Copy(recValue14, 0, parameter7_4byte14, 0, 4);
            Array.Copy(recValue15, 0, parameter7_4byte15, 0, 4);
            Array.Copy(recValue16, 0, parameter7_4byte16, 0, 4);
            Array.Copy(recValue17, 0, parameter7_4byte17, 0, 4);
            Array.Copy(recValue18, 0, parameter7_4byte18, 0, 4);
            Array.Copy(recValue19, 0, parameter7_4byte19, 0, 4);
            Array.Copy(recValue20, 0, parameter7_4byte20, 0, 4);
            Array.Copy(recValue21, 0, parameter7_4byte21, 0, 4);
            Array.Copy(recValue22, 0, parameter7_4byte22, 0, 4);
            Array.Copy(recValue23, 0, parameter7_4byte23, 0, 4);
            Array.Copy(recValue24, 0, parameter7_4byte24, 0, 4);
            Array.Copy(recValue25, 0, parameter7_4byte25, 0, 4);
            Array.Copy(recValue26, 0, parameter7_4byte26, 0, 4);
            Array.Copy(recValue27, 0, parameter7_4byte27, 0, 4);
            Array.Copy(recValue28, 0, parameter7_4byte28, 0, 4);
            Array.Copy(recValue29, 0, parameter7_4byte29, 0, 4);
            Array.Copy(recValue30, 0, parameter7_4byte30, 0, 4);
            Array.Copy(recValue31, 0, parameter7_4byte31, 0, 4);
            Array.Copy(recValue32, 0, parameter7_4byte32, 0, 4);
            Array.Copy(recValue33, 0, parameter7_4byte33, 0, 4);
            Array.Copy(recValue34, 0, parameter7_4byte34, 0, 4);
            Array.Copy(recValue35, 0, parameter7_4byte35, 0, 4);
            Array.Copy(recValue36, 0, parameter7_4byte36, 0, 4);
            Array.Copy(recValue37, 0, parameter7_4byte37, 0, 4);
            Array.Copy(recValue38, 0, parameter7_4byte38, 0, 4);
            Array.Copy(recValue39, 0, parameter7_4byte39, 0, 4);
            Array.Copy(recValue40, 0, parameter7_4byte40, 0, 4);
            Array.Copy(recValue41, 0, parameter7_4byte41, 0, 4);
            Array.Copy(recValue42, 0, parameter7_4byte42, 0, 4);
            Array.Copy(recValue43, 0, parameter7_4byte43, 0, 4);
            Array.Copy(recValue44, 0, parameter7_4byte44, 0, 4);
            Array.Copy(recValue45, 0, parameter7_4byte45, 0, 4);
            Array.Copy(recValue46, 0, parameter7_4byte46, 0, 4);
            Array.Copy(recValue47, 0, parameter7_4byte47, 0, 4);
            Array.Copy(recValue48, 0, parameter7_4byte48, 0, 4);
            Array.Copy(recValue49, 0, parameter7_4byte49, 0, 4);
            Array.Copy(recValue50, 0, parameter7_4byte50, 0, 4);
            Array.Copy(recValue51, 0, parameter7_4byte51, 0, 4);
            Array.Copy(recValue52, 0, parameter7_4byte52, 0, 4);
            Array.Copy(recValue53, 0, parameter7_4byte53, 0, 4);
            Array.Copy(recValue54, 0, parameter7_4byte54, 0, 4);
            Array.Copy(recValue55, 0, parameter7_4byte55, 0, 4);
            Array.Copy(recValue56, 0, parameter7_4byte56, 0, 4);
            Array.Copy(recValue57, 0, parameter7_4byte57, 0, 4);
            Array.Copy(recValue58, 0, parameter7_4byte58, 0, 4);
            Array.Copy(recValue59, 0, parameter7_4byte59, 0, 4);
            Array.Copy(recValue60, 0, parameter7_4byte60, 0, 4);
            Array.Copy(recValue61, 0, parameter7_4byte61, 0, 4);
            Array.Copy(recValue62, 0, parameter7_4byte62, 0, 4);
            Array.Copy(recValue63, 0, parameter7_4byte63, 0, 4);
            Array.Copy(recValue64, 0, parameter7_4byte64, 0, 4);
            Array.Copy(recValue65, 0, parameter7_4byte65, 0, 4);
            Array.Copy(recValue66, 0, parameter7_4byte66, 0, 4);
            Array.Copy(recValue67, 0, parameter7_4byte67, 0, 4);
            Array.Copy(recValue68, 0, parameter7_4byte68, 0, 4);
            Array.Copy(recValue69, 0, parameter7_4byte69, 0, 4);
            Array.Copy(recValue70, 0, parameter7_4byte70, 0, 4);
            Array.Copy(recValue71, 0, parameter7_4byte71, 0, 4);
            Array.Copy(recValue72, 0, parameter7_4byte72, 0, 4);
            Array.Copy(recValue73, 0, parameter7_4byte73, 0, 4);
            Array.Copy(recValue74, 0, parameter7_4byte74, 0, 4);
            Array.Copy(recValue75, 0, parameter7_4byte75, 0, 4);
            Array.Copy(recValue76, 0, parameter7_4byte76, 0, 4);
            Array.Copy(recValue77, 0, parameter7_4byte77, 0, 4);
            Array.Copy(recValue78, 0, parameter7_4byte78, 0, 4);
            Array.Copy(recValue79, 0, parameter7_4byte79, 0, 4);
            Array.Copy(recValue80, 0, parameter7_4byte80, 0, 4);
            Array.Copy(recValue81, 0, parameter7_4byte81, 0, 4);
            Array.Copy(recValue82, 0, parameter7_4byte82, 0, 4);
            Array.Copy(recValue83, 0, parameter7_4byte83, 0, 4);
            Array.Copy(recValue84, 0, parameter7_4byte84, 0, 4);
            Array.Copy(recValue85, 0, parameter7_4byte85, 0, 4);
            Array.Copy(recValue86, 0, parameter7_4byte86, 0, 4);
            Array.Copy(recValue87, 0, parameter7_4byte87, 0, 4);
            Array.Copy(recValue88, 0, parameter7_4byte88, 0, 4);
            Array.Copy(recValue89, 0, parameter7_4byte89, 0, 4);
            Array.Copy(recValue90, 0, parameter7_4byte90, 0, 4);
            Array.Copy(recValue91, 0, parameter7_4byte91, 0, 4);
            Array.Copy(recValue92, 0, parameter7_4byte92, 0, 4);
            Array.Copy(recValue93, 0, parameter7_4byte93, 0, 4);
            Array.Copy(recValue94, 0, parameter7_4byte94, 0, 4);
            Array.Copy(recValue95, 0, parameter7_4byte95, 0, 4);
            Array.Copy(recValue96, 0, parameter7_4byte96, 0, 4);
            Array.Copy(recValue97, 0, parameter7_4byte97, 0, 4);
            Array.Copy(recValue98, 0, parameter7_4byte98, 0, 4);
            Array.Copy(recValue99, 0, parameter7_4byte99, 0, 4);
            Array.Copy(recValue100, 0, parameter7_4byte100, 0, 4);
            Array.Copy(recValue101, 0, parameter7_4byte101, 0, 4);
            Array.Copy(recValue102, 0, parameter7_4byte102, 0, 4);
            Array.Copy(recValue103, 0, parameter7_4byte103, 0, 4);
            Array.Copy(recValue104, 0, parameter7_4byte104, 0, 4);
            Array.Copy(recValue105, 0, parameter7_4byte105, 0, 4);
            Array.Copy(recValue106, 0, parameter7_4byte106, 0, 4);
            Array.Copy(recValue107, 0, parameter7_4byte107, 0, 4);
            Array.Copy(recValue108, 0, parameter7_4byte108, 0, 4);
            Array.Copy(recValue109, 0, parameter7_4byte109, 0, 4);
            Array.Copy(recValue110, 0, parameter7_4byte110, 0, 4);
            Array.Copy(recValue111, 0, parameter7_4byte111, 0, 4);
            Array.Copy(recValue112, 0, parameter7_4byte112, 0, 4);
            Array.Copy(recValue113, 0, parameter7_4byte113, 0, 4);
            Array.Copy(recValue114, 0, parameter7_4byte114, 0, 4);
            Array.Copy(recValue115, 0, parameter7_4byte115, 0, 4);
            Array.Copy(recValue116, 0, parameter7_4byte116, 0, 4);
            Array.Copy(recValue117, 0, parameter7_4byte117, 0, 4);
            Array.Copy(recValue118, 0, parameter7_4byte118, 0, 4);
            Array.Copy(recValue119, 0, parameter7_4byte119, 0, 4);
            Array.Copy(recValue120, 0, parameter7_4byte120, 0, 4);
            Array.Copy(recValue121, 0, parameter7_4byte121, 0, 4);
            Array.Copy(recValue122, 0, parameter7_4byte122, 0, 4);
            Array.Copy(recValue123, 0, parameter7_4byte123, 0, 4);
            Array.Copy(recValue124, 0, parameter7_4byte124, 0, 4);
            Array.Copy(recValue125, 0, parameter7_4byte125, 0, 4);
            Array.Copy(recValue126, 0, parameter7_4byte126, 0, 4);
            Array.Copy(recValue127, 0, parameter7_4byte127, 0, 4);
            Array.Copy(recValue128, 0, parameter7_4byte128, 0, 4);
            Array.Copy(recValue129, 0, parameter7_4byte129, 0, 4);
            Array.Copy(recValue130, 0, parameter7_4byte130, 0, 4);
            Array.Copy(recValue131, 0, parameter7_4byte131, 0, 4);
            Array.Copy(recValue132, 0, parameter7_4byte132, 0, 4);
            Array.Copy(recValue133, 0, parameter7_4byte133, 0, 4);
            Array.Copy(recValue134, 0, parameter7_4byte134, 0, 4);
            Array.Copy(recValue135, 0, parameter7_4byte135, 0, 4);
            Array.Copy(recValue136, 0, parameter7_4byte136, 0, 4);
            Array.Copy(recValue137, 0, parameter7_4byte137, 0, 4);
            Array.Copy(recValue138, 0, parameter7_4byte138, 0, 4);
            Array.Copy(recValue139, 0, parameter7_4byte139, 0, 4);
            Array.Copy(recValue140, 0, parameter7_4byte140, 0, 4);
            Array.Copy(recValue141, 0, parameter7_4byte141, 0, 4);
            Array.Copy(recValue142, 0, parameter7_4byte142, 0, 4);
            Array.Copy(recValue143, 0, parameter7_4byte143, 0, 4);
            Array.Copy(recValue144, 0, parameter7_4byte144, 0, 4);
            Array.Copy(recValue145, 0, parameter7_4byte145, 0, 4);
            Array.Copy(recValue146, 0, parameter7_4byte146, 0, 4);
            Array.Copy(recValue147, 0, parameter7_4byte147, 0, 4);
            Array.Copy(recValue148, 0, parameter7_4byte148, 0, 4);
            Array.Copy(recValue149, 0, parameter7_4byte149, 0, 4);
            Array.Copy(recValue150, 0, parameter7_4byte150, 0, 4);
            Array.Copy(recValue151, 0, parameter7_4byte151, 0, 4);
            Array.Copy(recValue152, 0, parameter7_4byte152, 0, 4);
            Array.Copy(recValue153, 0, parameter7_4byte153, 0, 4);
            Array.Copy(recValue154, 0, parameter7_4byte154, 0, 4);
            Array.Copy(recValue155, 0, parameter7_4byte155, 0, 4);
            Array.Copy(recValue156, 0, parameter7_4byte156, 0, 4);
            Array.Copy(recValue157, 0, parameter7_4byte157, 0, 4);
            Array.Copy(recValue158, 0, parameter7_4byte158, 0, 4);
            Array.Copy(recValue159, 0, parameter7_4byte159, 0, 4);
            Array.Copy(recValue160, 0, parameter7_4byte160, 0, 4);
            Array.Copy(recValue161, 0, parameter7_4byte161, 0, 4);
            Array.Copy(recValue162, 0, parameter7_4byte162, 0, 4);
            Array.Copy(recValue163, 0, parameter7_4byte163, 0, 4);
            Array.Copy(recValue164, 0, parameter7_4byte164, 0, 4);
            Array.Copy(recValue165, 0, parameter7_4byte165, 0, 4);
            Array.Copy(recValue166, 0, parameter7_4byte166, 0, 4);
            Array.Copy(recValue167, 0, parameter7_4byte167, 0, 4);
            Array.Copy(recValue168, 0, parameter7_4byte168, 0, 4);
            Array.Copy(recValue169, 0, parameter7_4byte169, 0, 4);
            Array.Copy(recValue170, 0, parameter7_4byte170, 0, 4);
            Array.Copy(recValue171, 0, parameter7_4byte171, 0, 4);
            Array.Copy(recValue172, 0, parameter7_4byte172, 0, 4);
            Array.Copy(recValue173, 0, parameter7_4byte173, 0, 4);
            Array.Copy(recValue174, 0, parameter7_4byte174, 0, 4);
            Array.Copy(recValue175, 0, parameter7_4byte175, 0, 4);
            Array.Copy(recValue176, 0, parameter7_4byte176, 0, 4);
            Array.Copy(recValue177, 0, parameter7_4byte177, 0, 4);
            Array.Copy(recValue178, 0, parameter7_4byte178, 0, 4);
            Array.Copy(recValue179, 0, parameter7_4byte179, 0, 4);
            Array.Copy(recValue180, 0, parameter7_4byte180, 0, 4);
            Array.Copy(recValue181, 0, parameter7_4byte181, 0, 4);
            Array.Copy(recValue182, 0, parameter7_4byte182, 0, 4);
            Array.Copy(recValue183, 0, parameter7_4byte183, 0, 4);
            Array.Copy(recValue184, 0, parameter7_4byte184, 0, 4);
            Array.Copy(recValue185, 0, parameter7_4byte185, 0, 4);
            Array.Copy(recValue186, 0, parameter7_4byte186, 0, 4);
            Array.Copy(recValue187, 0, parameter7_4byte187, 0, 4);
            Array.Copy(recValue188, 0, parameter7_4byte188, 0, 4);
            Array.Copy(recValue189, 0, parameter7_4byte189, 0, 4);
            Array.Copy(recValue190, 0, parameter7_4byte190, 0, 4);
            Array.Copy(recValue191, 0, parameter7_4byte191, 0, 4);
            Array.Copy(recValue192, 0, parameter7_4byte192, 0, 4);
            Array.Copy(recValue193, 0, parameter7_4byte193, 0, 4);
            Array.Copy(recValue194, 0, parameter7_4byte194, 0, 4);
            Array.Copy(recValue195, 0, parameter7_4byte195, 0, 4);
            Array.Copy(recValue196, 0, parameter7_4byte196, 0, 4);
            Array.Copy(recValue197, 0, parameter7_4byte197, 0, 4);
            Array.Copy(recValue198, 0, parameter7_4byte198, 0, 4);
            Array.Copy(recValue199, 0, parameter7_4byte199, 0, 4);
            Array.Copy(recValue200, 0, parameter7_4byte200, 0, 4);
            Array.Copy(recValue201, 0, parameter7_4byte201, 0, 4);
            Array.Copy(recValue202, 0, parameter7_4byte202, 0, 4);
            Array.Copy(recValue203, 0, parameter7_4byte203, 0, 4);
            Array.Copy(recValue204, 0, parameter7_4byte204, 0, 4);
            Array.Copy(recValue205, 0, parameter7_4byte205, 0, 4);
            Array.Copy(recValue206, 0, parameter7_4byte206, 0, 4);
            Array.Copy(recValue207, 0, parameter7_4byte207, 0, 4);
            Array.Copy(recValue208, 0, parameter7_4byte208, 0, 4);
            Array.Copy(recValue209, 0, parameter7_4byte209, 0, 4);
            Array.Copy(recValue210, 0, parameter7_4byte210, 0, 4);
            Array.Copy(recValue211, 0, parameter7_4byte211, 0, 4);
            Array.Copy(recValue212, 0, parameter7_4byte212, 0, 4);
            Array.Copy(recValue213, 0, parameter7_4byte213, 0, 4);
            Array.Copy(recValue214, 0, parameter7_4byte214, 0, 4);
            Array.Copy(recValue215, 0, parameter7_4byte215, 0, 4);
            Array.Copy(recValue216, 0, parameter7_4byte216, 0, 4);
            Array.Copy(recValue217, 0, parameter7_4byte217, 0, 4);
            Array.Copy(recValue218, 0, parameter7_4byte218, 0, 4);
            Array.Copy(recValue219, 0, parameter7_4byte219, 0, 4);
            Array.Copy(recValue220, 0, parameter7_4byte220, 0, 4);
            Array.Copy(recValue221, 0, parameter7_4byte221, 0, 4);
            Array.Copy(recValue222, 0, parameter7_4byte222, 0, 4);
            Array.Copy(recValue223, 0, parameter7_4byte223, 0, 4);
            Array.Copy(recValue224, 0, parameter7_4byte224, 0, 4);
            Array.Copy(recValue225, 0, parameter7_4byte225, 0, 4);
            Array.Copy(recValue226, 0, parameter7_4byte226, 0, 4);
            Array.Copy(recValue227, 0, parameter7_4byte227, 0, 4);
            Array.Copy(recValue228, 0, parameter7_4byte228, 0, 4);
            Array.Copy(recValue229, 0, parameter7_4byte229, 0, 4);
            Array.Copy(recValue230, 0, parameter7_4byte230, 0, 4);
            Array.Copy(recValue231, 0, parameter7_4byte231, 0, 4);
            Array.Copy(recValue232, 0, parameter7_4byte232, 0, 4);
            Array.Copy(recValue233, 0, parameter7_4byte233, 0, 4);
            Array.Copy(recValue234, 0, parameter7_4byte234, 0, 4);
            Array.Copy(recValue235, 0, parameter7_4byte235, 0, 4);
            Array.Copy(recValue236, 0, parameter7_4byte236, 0, 4);
            Array.Copy(recValue237, 0, parameter7_4byte237, 0, 4);
            Array.Copy(recValue238, 0, parameter7_4byte238, 0, 4);
            Array.Copy(recValue239, 0, parameter7_4byte239, 0, 4);
            Array.Copy(recValue240, 0, parameter7_4byte240, 0, 4);
            Array.Copy(recValue241, 0, parameter7_4byte241, 0, 4);
            Array.Copy(recValue242, 0, parameter7_4byte242, 0, 4);
            Array.Copy(recValue243, 0, parameter7_4byte243, 0, 4);
            Array.Copy(recValue244, 0, parameter7_4byte244, 0, 4);
            Array.Copy(recValue245, 0, parameter7_4byte245, 0, 4);
            Array.Copy(recValue246, 0, parameter7_4byte246, 0, 4);
            Array.Copy(recValue247, 0, parameter7_4byte247, 0, 4);
            Array.Copy(recValue248, 0, parameter7_4byte248, 0, 4);
            Array.Copy(recValue249, 0, parameter7_4byte249, 0, 4);
            Array.Copy(recValue250, 0, parameter7_4byte250, 0, 4);
            Array.Copy(recValue251, 0, parameter7_4byte251, 0, 4);
            Array.Copy(recValue252, 0, parameter7_4byte252, 0, 4);
            Array.Copy(recValue253, 0, parameter7_4byte253, 0, 4);
            Array.Copy(recValue254, 0, parameter7_4byte254, 0, 4);
            Array.Copy(recValue255, 0, parameter7_4byte255, 0, 4);
            Array.Copy(recValue256, 0, parameter7_4byte256, 0, 4);
            Array.Copy(recValue257, 0, parameter7_4byte257, 0, 4);
            Array.Copy(recValue258, 0, parameter7_4byte258, 0, 4);
            Array.Copy(recValue259, 0, parameter7_4byte259, 0, 4);
            Array.Copy(recValue260, 0, parameter7_4byte260, 0, 4);
            Array.Copy(recValue261, 0, parameter7_4byte261, 0, 4);
            Array.Copy(recValue262, 0, parameter7_4byte262, 0, 4);
            Array.Copy(recValue263, 0, parameter7_4byte263, 0, 4);
            Array.Copy(recValue264, 0, parameter7_4byte264, 0, 4);
            Array.Copy(recValue265, 0, parameter7_4byte265, 0, 4);
            Array.Copy(recValue266, 0, parameter7_4byte266, 0, 4);
            Array.Copy(recValue267, 0, parameter7_4byte267, 0, 4);
            Array.Copy(recValue268, 0, parameter7_4byte268, 0, 4);
            Array.Copy(recValue269, 0, parameter7_4byte269, 0, 4);
            Array.Copy(recValue270, 0, parameter7_4byte270, 0, 4);
            Array.Copy(recValue271, 0, parameter7_4byte271, 0, 4);
            Array.Copy(recValue272, 0, parameter7_4byte272, 0, 4);
            Array.Copy(recValue273, 0, parameter7_4byte273, 0, 4);
            Array.Copy(recValue274, 0, parameter7_4byte274, 0, 4);
            Array.Copy(recValue275, 0, parameter7_4byte275, 0, 4);
            Array.Copy(recValue276, 0, parameter7_4byte276, 0, 4);
            Array.Copy(recValue277, 0, parameter7_4byte277, 0, 4);
            Array.Copy(recValue278, 0, parameter7_4byte278, 0, 4);
            Array.Copy(recValue279, 0, parameter7_4byte279, 0, 4);
            Array.Copy(recValue280, 0, parameter7_4byte280, 0, 4);
            Array.Copy(recValue281, 0, parameter7_4byte281, 0, 4);
            Array.Copy(recValue282, 0, parameter7_4byte282, 0, 4);
            Array.Copy(recValue283, 0, parameter7_4byte283, 0, 4);
            Array.Copy(recValue284, 0, parameter7_4byte284, 0, 4);
            Array.Copy(recValue285, 0, parameter7_4byte285, 0, 4);
            Array.Copy(recValue286, 0, parameter7_4byte286, 0, 4);
            Array.Copy(recValue287, 0, parameter7_4byte287, 0, 4);
            Array.Copy(recValue288, 0, parameter7_4byte288, 0, 4);
            Array.Copy(recValue289, 0, parameter7_4byte289, 0, 4);
            Array.Copy(recValue290, 0, parameter7_4byte290, 0, 4);
            Array.Copy(recValue291, 0, parameter7_4byte291, 0, 4);
            Array.Copy(recValue292, 0, parameter7_4byte292, 0, 4);
            Array.Copy(recValue293, 0, parameter7_4byte293, 0, 4);
            Array.Copy(recValue294, 0, parameter7_4byte294, 0, 4);
            Array.Copy(recValue295, 0, parameter7_4byte295, 0, 4);
            Array.Copy(recValue296, 0, parameter7_4byte296, 0, 4);
            Array.Copy(recValue297, 0, parameter7_4byte297, 0, 4);
            Array.Copy(recValue298, 0, parameter7_4byte298, 0, 4);
            Array.Copy(recValue299, 0, parameter7_4byte299, 0, 4);
            Array.Copy(recValue300, 0, parameter7_4byte300, 0, 4);
            Array.Copy(recValue301, 0, parameter7_4byte301, 0, 4);
            Array.Copy(recValue302, 0, parameter7_4byte302, 0, 4);
            Array.Copy(recValue303, 0, parameter7_4byte303, 0, 4);
            Array.Copy(recValue304, 0, parameter7_4byte304, 0, 4);
            Array.Copy(recValue305, 0, parameter7_4byte305, 0, 4);
            Array.Copy(recValue306, 0, parameter7_4byte306, 0, 4);
            Array.Copy(recValue307, 0, parameter7_4byte307, 0, 4);
            Array.Copy(recValue308, 0, parameter7_4byte308, 0, 4);
            Array.Copy(recValue309, 0, parameter7_4byte309, 0, 4);
            Array.Copy(recValue310, 0, parameter7_4byte310, 0, 4);
            Array.Copy(recValue311, 0, parameter7_4byte311, 0, 4);
            Array.Copy(recValue312, 0, parameter7_4byte312, 0, 4);
            Array.Copy(recValue313, 0, parameter7_4byte313, 0, 4);
            Array.Copy(recValue314, 0, parameter7_4byte314, 0, 4);
            Array.Copy(recValue315, 0, parameter7_4byte315, 0, 4);
            Array.Copy(recValue316, 0, parameter7_4byte316, 0, 4);
            Array.Copy(recValue317, 0, parameter7_4byte317, 0, 4);
            Array.Copy(recValue318, 0, parameter7_4byte318, 0, 4);
            Array.Copy(recValue319, 0, parameter7_4byte319, 0, 4);
            Array.Copy(recValue320, 0, parameter7_4byte320, 0, 4);
            Array.Copy(recValue321, 0, parameter7_4byte321, 0, 4);
            Array.Copy(recValue322, 0, parameter7_4byte322, 0, 4);
            Array.Copy(recValue323, 0, parameter7_4byte323, 0, 4);
            Array.Copy(recValue324, 0, parameter7_4byte324, 0, 4);
            Array.Copy(recValue325, 0, parameter7_4byte325, 0, 4);
            Array.Copy(recValue326, 0, parameter7_4byte326, 0, 4);
            Array.Copy(recValue327, 0, parameter7_4byte327, 0, 4);
            Array.Copy(recValue328, 0, parameter7_4byte328, 0, 4);
            Array.Copy(recValue329, 0, parameter7_4byte329, 0, 4);
            Array.Copy(recValue330, 0, parameter7_4byte330, 0, 4);
            Array.Copy(recValue331, 0, parameter7_4byte331, 0, 4);
            Array.Copy(recValue332, 0, parameter7_4byte332, 0, 4);
            Array.Copy(recValue333, 0, parameter7_4byte333, 0, 4);
            Array.Copy(recValue334, 0, parameter7_4byte334, 0, 4);
            Array.Copy(recValue335, 0, parameter7_4byte335, 0, 4);
            Array.Copy(recValue336, 0, parameter7_4byte336, 0, 4);
            Array.Copy(recValue337, 0, parameter7_4byte337, 0, 4);
            Array.Copy(recValue338, 0, parameter7_4byte338, 0, 4);
            Array.Copy(recValue339, 0, parameter7_4byte339, 0, 4);
            Array.Copy(recValue340, 0, parameter7_4byte340, 0, 4);
            Array.Copy(recValue341, 0, parameter7_4byte341, 0, 4);
            Array.Copy(recValue342, 0, parameter7_4byte342, 0, 4);
            Array.Copy(recValue343, 0, parameter7_4byte343, 0, 4);
            Array.Copy(recValue344, 0, parameter7_4byte344, 0, 4);
            Array.Copy(recValue345, 0, parameter7_4byte345, 0, 4);
            Array.Copy(recValue346, 0, parameter7_4byte346, 0, 4);
            Array.Copy(recValue347, 0, parameter7_4byte347, 0, 4);
            Array.Copy(recValue348, 0, parameter7_4byte348, 0, 4);
            Array.Copy(recValue349, 0, parameter7_4byte349, 0, 4);
            Array.Copy(recValue350, 0, parameter7_4byte350, 0, 4);
            Array.Copy(recValue351, 0, parameter7_4byte351, 0, 4);
            Array.Copy(recValue352, 0, parameter7_4byte352, 0, 4);
            Array.Copy(recValue353, 0, parameter7_4byte353, 0, 4);
            Array.Copy(recValue354, 0, parameter7_4byte354, 0, 4);
            Array.Copy(recValue355, 0, parameter7_4byte355, 0, 4);
            Array.Copy(recValue356, 0, parameter7_4byte356, 0, 4);
            Array.Copy(recValue357, 0, parameter7_4byte357, 0, 4);
            Array.Copy(recValue358, 0, parameter7_4byte358, 0, 4);
            Array.Copy(recValue359, 0, parameter7_4byte359, 0, 4);
            Array.Copy(recValue360, 0, parameter7_4byte360, 0, 4);
            Array.Copy(recValue361, 0, parameter7_4byte361, 0, 4);
            Array.Copy(recValue362, 0, parameter7_4byte362, 0, 4);
            Array.Copy(recValue363, 0, parameter7_4byte363, 0, 4);
            Array.Copy(recValue364, 0, parameter7_4byte364, 0, 4);
            Array.Copy(recValue365, 0, parameter7_4byte365, 0, 4);
            Array.Copy(recValue366, 0, parameter7_4byte366, 0, 4);
            Array.Copy(recValue367, 0, parameter7_4byte367, 0, 4);
            Array.Copy(recValue368, 0, parameter7_4byte368, 0, 4);
            Array.Copy(recValue369, 0, parameter7_4byte369, 0, 4);
            Array.Copy(recValue370, 0, parameter7_4byte370, 0, 4);
            Array.Copy(recValue371, 0, parameter7_4byte371, 0, 4);
            Array.Copy(recValue372, 0, parameter7_4byte372, 0, 4);
            Array.Copy(recValue373, 0, parameter7_4byte373, 0, 4);
            Array.Copy(recValue374, 0, parameter7_4byte374, 0, 4);
            Array.Copy(recValue375, 0, parameter7_4byte375, 0, 4);
            Array.Copy(recValue376, 0, parameter7_4byte376, 0, 4);
            Array.Copy(recValue377, 0, parameter7_4byte377, 0, 4);
            Array.Copy(recValue378, 0, parameter7_4byte378, 0, 4);
            Array.Copy(recValue379, 0, parameter7_4byte389, 0, 4);
            Array.Copy(recValue380, 0, parameter7_4byte380, 0, 4);
            Array.Copy(recValue381, 0, parameter7_4byte381, 0, 4);
            Array.Copy(recValue382, 0, parameter7_4byte382, 0, 4);
            Array.Copy(recValue383, 0, parameter7_4byte383, 0, 4);
            Array.Copy(recValue384, 0, parameter7_4byte384, 0, 4);
            Array.Copy(recValue385, 0, parameter7_4byte385, 0, 4);
            Array.Copy(recValue386, 0, parameter7_4byte386, 0, 4);
            Array.Copy(recValue387, 0, parameter7_4byte387, 0, 4);
            Array.Copy(recValue388, 0, parameter7_4byte388, 0, 4);
            Array.Copy(recValue389, 0, parameter7_4byte389, 0, 4);
            Array.Copy(recValue390, 0, parameter7_4byte390, 0, 4);
            Array.Copy(recValue391, 0, parameter7_4byte391, 0, 4);
            Array.Copy(recValue392, 0, parameter7_4byte392, 0, 4);
            Array.Copy(recValue393, 0, parameter7_4byte393, 0, 4);
            Array.Copy(recValue394, 0, parameter7_4byte394, 0, 4);
            Array.Copy(recValue395, 0, parameter7_4byte395, 0, 4);
            Array.Copy(recValue396, 0, parameter7_4byte396, 0, 4);
            Array.Copy(recValue397, 0, parameter7_4byte397, 0, 4);
            Array.Copy(recValue398, 0, parameter7_4byte398, 0, 4);
            Array.Copy(recValue399, 0, parameter7_4byte399, 0, 4);
            Array.Copy(recValue400, 0, parameter7_4byte400, 0, 4);
            Array.Copy(recValue401, 0, parameter7_4byte401, 0, 4);
            Array.Copy(recValue402, 0, parameter7_4byte402, 0, 4);
            Array.Copy(recValue403, 0, parameter7_4byte403, 0, 4);
            Array.Copy(recValue404, 0, parameter7_4byte404, 0, 4);
            Array.Copy(recValue405, 0, parameter7_4byte405, 0, 4);
            Array.Copy(recValue406, 0, parameter7_4byte406, 0, 4);
            Array.Copy(recValue407, 0, parameter7_4byte407, 0, 4);
            Array.Copy(recValue408, 0, parameter7_4byte408, 0, 4);
            Array.Copy(recValue409, 0, parameter7_4byte409, 0, 4);
            Array.Copy(recValue410, 0, parameter7_4byte410, 0, 4);
            Array.Copy(recValue411, 0, parameter7_4byte411, 0, 4);
            Array.Copy(recValue412, 0, parameter7_4byte412, 0, 4);
            Array.Copy(recValue413, 0, parameter7_4byte413, 0, 4);
            Array.Copy(recValue414, 0, parameter7_4byte414, 0, 4);
            Array.Copy(recValue415, 0, parameter7_4byte415, 0, 4);
            Array.Copy(recValue416, 0, parameter7_4byte416, 0, 4);
            Array.Copy(recValue417, 0, parameter7_4byte417, 0, 4);
            Array.Copy(recValue418, 0, parameter7_4byte418, 0, 4);
            Array.Copy(recValue419, 0, parameter7_4byte419, 0, 4);
            Array.Copy(recValue420, 0, parameter7_4byte420, 0, 4);
            Array.Copy(recValue421, 0, parameter7_4byte421, 0, 4);
            Array.Copy(recValue422, 0, parameter7_4byte422, 0, 4);
            Array.Copy(recValue423, 0, parameter7_4byte423, 0, 4);
            Array.Copy(recValue424, 0, parameter7_4byte424, 0, 4);
            Array.Copy(recValue425, 0, parameter7_4byte425, 0, 4);
            Array.Copy(recValue426, 0, parameter7_4byte426, 0, 4);
            Array.Copy(recValue427, 0, parameter7_4byte427, 0, 4);
            Array.Copy(recValue428, 0, parameter7_4byte428, 0, 4);
            Array.Copy(recValue429, 0, parameter7_4byte429, 0, 4);
            Array.Copy(recValue430, 0, parameter7_4byte430, 0, 4);
            Array.Copy(recValue431, 0, parameter7_4byte431, 0, 4);
            Array.Copy(recValue432, 0, parameter7_4byte432, 0, 4);
            Array.Copy(recValue433, 0, parameter7_4byte433, 0, 4);
            Array.Copy(recValue434, 0, parameter7_4byte434, 0, 4);
            Array.Copy(recValue435, 0, parameter7_4byte435, 0, 4);
            Array.Copy(recValue436, 0, parameter7_4byte436, 0, 4);
            Array.Copy(recValue437, 0, parameter7_4byte437, 0, 4);
            Array.Copy(recValue438, 0, parameter7_4byte438, 0, 4);
            Array.Copy(recValue439, 0, parameter7_4byte439, 0, 4);
            Array.Copy(recValue440, 0, parameter7_4byte440, 0, 4);
            Array.Copy(recValue441, 0, parameter7_4byte441, 0, 4);
            Array.Copy(recValue442, 0, parameter7_4byte442, 0, 4);
            Array.Copy(recValue443, 0, parameter7_4byte443, 0, 4);
            Array.Copy(recValue444, 0, parameter7_4byte444, 0, 4);
            Array.Copy(recValue445, 0, parameter7_4byte445, 0, 4);
            Array.Copy(recValue446, 0, parameter7_4byte446, 0, 4);
            Array.Copy(recValue447, 0, parameter7_4byte447, 0, 4);
            Array.Copy(recValue448, 0, parameter7_4byte448, 0, 4);
            Array.Copy(recValue449, 0, parameter7_4byte449, 0, 4);
            Array.Copy(recValue450, 0, parameter7_4byte450, 0, 4);
            Array.Copy(recValue451, 0, parameter7_4byte451, 0, 4);
            Array.Copy(recValue452, 0, parameter7_4byte452, 0, 4);
            Array.Copy(recValue453, 0, parameter7_4byte453, 0, 4);
            Array.Copy(recValue454, 0, parameter7_4byte454, 0, 4);
            Array.Copy(recValue455, 0, parameter7_4byte455, 0, 4);
            Array.Copy(recValue456, 0, parameter7_4byte456, 0, 4);
            Array.Copy(recValue457, 0, parameter7_4byte457, 0, 4);
            Array.Copy(recValue458, 0, parameter7_4byte458, 0, 4);
            Array.Copy(recValue459, 0, parameter7_4byte459, 0, 4);
            Array.Copy(recValue460, 0, parameter7_4byte460, 0, 4);
            Array.Copy(recValue461, 0, parameter7_4byte461, 0, 4);
            Array.Copy(recValue462, 0, parameter7_4byte462, 0, 4);
            Array.Copy(recValue463, 0, parameter7_4byte463, 0, 4);
            Array.Copy(recValue464, 0, parameter7_4byte464, 0, 4);
            Array.Copy(recValue465, 0, parameter7_4byte465, 0, 4);
            Array.Copy(recValue466, 0, parameter7_4byte466, 0, 4);
            Array.Copy(recValue467, 0, parameter7_4byte467, 0, 4);
            Array.Copy(recValue468, 0, parameter7_4byte468, 0, 4);
            Array.Copy(recValue469, 0, parameter7_4byte469, 0, 4);
            Array.Copy(recValue470, 0, parameter7_4byte470, 0, 4);
            Array.Copy(recValue471, 0, parameter7_4byte471, 0, 4);
            Array.Copy(recValue472, 0, parameter7_4byte472, 0, 4);
            Array.Copy(recValue473, 0, parameter7_4byte473, 0, 4);
            Array.Copy(recValue474, 0, parameter7_4byte474, 0, 4);
            Array.Copy(recValue475, 0, parameter7_4byte475, 0, 4);
            Array.Copy(recValue476, 0, parameter7_4byte476, 0, 4);
            Array.Copy(recValue477, 0, parameter7_4byte477, 0, 4);
            Array.Copy(recValue478, 0, parameter7_4byte478, 0, 4);
            Array.Copy(recValue479, 0, parameter7_4byte479, 0, 4);
            Array.Copy(recValue480, 0, parameter7_4byte480, 0, 4);
            Array.Copy(recValue481, 0, parameter7_4byte481, 0, 4);
            Array.Copy(recValue482, 0, parameter7_4byte482, 0, 4);
            Array.Copy(recValue483, 0, parameter7_4byte483, 0, 4);
            Array.Copy(recValue484, 0, parameter7_4byte484, 0, 4);
            Array.Copy(recValue485, 0, parameter7_4byte485, 0, 4);
            Array.Copy(recValue486, 0, parameter7_4byte486, 0, 4);
            Array.Copy(recValue487, 0, parameter7_4byte487, 0, 4);
            Array.Copy(recValue488, 0, parameter7_4byte488, 0, 4);
            Array.Copy(recValue489, 0, parameter7_4byte489, 0, 4);
            Array.Copy(recValue490, 0, parameter7_4byte490, 0, 4);
            Array.Copy(recValue491, 0, parameter7_4byte491, 0, 4);
            Array.Copy(recValue492, 0, parameter7_4byte492, 0, 4);
            Array.Copy(recValue493, 0, parameter7_4byte493, 0, 4);
            Array.Copy(recValue494, 0, parameter7_4byte494, 0, 4);
            Array.Copy(recValue495, 0, parameter7_4byte495, 0, 4);
            Array.Copy(recValue496, 0, parameter7_4byte496, 0, 4);
            Array.Copy(recValue497, 0, parameter7_4byte497, 0, 4);
            Array.Copy(recValue498, 0, parameter7_4byte498, 0, 4);
            Array.Copy(recValue499, 0, parameter7_4byte499, 0, 4);
            Array.Copy(recValue500, 0, parameter7_4byte500, 0, 4);
            Array.Copy(recValue501, 0, parameter7_4byte501, 0, 4);
            Array.Copy(recValue502, 0, parameter7_4byte502, 0, 4);
            Array.Copy(recValue503, 0, parameter7_4byte503, 0, 4);
            Array.Copy(recValue504, 0, parameter7_4byte504, 0, 4);
            Array.Copy(recValue505, 0, parameter7_4byte505, 0, 4);
            Array.Copy(recValue506, 0, parameter7_4byte506, 0, 4);
            Array.Copy(recValue507, 0, parameter7_4byte507, 0, 4);
            Array.Copy(recValue508, 0, parameter7_4byte508, 0, 4);
            Array.Copy(recValue509, 0, parameter7_4byte509, 0, 4);
            Array.Copy(recValue510, 0, parameter7_4byte510, 0, 4);
            Array.Copy(recValue511, 0, parameter7_4byte511, 0, 4);
            Array.Copy(recValue512, 0, parameter7_4byte512, 0, 4);
            #endregion



            //0x4800 command
            parameter7_4byte1_1[0] = parameter7_4byte1[0];    //속도와 가속
            parameter7_4byte1_1[1] = parameter7_4byte1[1];    //커맨드 Code
            parameter7_4byte1_1[2] = parameter7_4byte1[2];    //예약                   
            parameter7_4byte1_1[3] = parameter7_4byte1[3];    //감속, 방향, 천이 조건
            
            //0x4802 data
            parameter7_4byte1_2[0] = parameter7_4byte2[2];
            parameter7_4byte1_2[1] = parameter7_4byte2[3];
            parameter7_4byte1_2[2] = parameter7_4byte2[0];
            parameter7_4byte1_2[3] = parameter7_4byte2[1];



            //0x4800 command
            parameter7_4byte1_3[0] = parameter7_4byte3[0];    //속도와 가속
            parameter7_4byte1_3[1] = parameter7_4byte3[1];    //커맨드 Code
            parameter7_4byte1_3[2] = parameter7_4byte3[2];    //예약                   
            parameter7_4byte1_3[3] = parameter7_4byte3[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_4[0] = parameter7_4byte4[2];
            parameter7_4byte1_4[1] = parameter7_4byte4[3];
            parameter7_4byte1_4[2] = parameter7_4byte4[0];
            parameter7_4byte1_4[3] = parameter7_4byte4[1];

            //0x4800 command
            parameter7_4byte1_5[0] = parameter7_4byte5[0];    //속도와 가속
            parameter7_4byte1_5[1] = parameter7_4byte5[1];    //커맨드 Code
            parameter7_4byte1_5[2] = parameter7_4byte5[2];    //예약                   
            parameter7_4byte1_5[3] = parameter7_4byte5[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_6[0] = parameter7_4byte6[2];
            parameter7_4byte1_6[1] = parameter7_4byte6[3];
            parameter7_4byte1_6[2] = parameter7_4byte6[0];
            parameter7_4byte1_6[3] = parameter7_4byte6[1];
                            
            //0x4800 command1
            parameter7_4byte1_7[0] = parameter7_4byte7[0];    //속도와 가속
            parameter7_4byte1_7[1] = parameter7_4byte7[1];    //커맨드 Code
            parameter7_4byte1_7[2] = parameter7_4byte7[2];    //예약                   
            parameter7_4byte1_7[3] = parameter7_4byte7[3];    //감속, 방향, 천이 조건
                            
            //0x4802 data   
            parameter7_4byte1_8[0] = parameter7_4byte8[2];
            parameter7_4byte1_8[1] = parameter7_4byte8[3];
            parameter7_4byte1_8[2] = parameter7_4byte8[0];
            parameter7_4byte1_8[3] = parameter7_4byte8[1];

            //0x4800 command
            parameter7_4byte1_9[0] = parameter7_4byte9[0];    //속도와 가속
            parameter7_4byte1_9[1] = parameter7_4byte9[1];    //커맨드 Code
            parameter7_4byte1_9[2] = parameter7_4byte9[2];    //예약                   
            parameter7_4byte1_9[3] = parameter7_4byte9[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_10[0] = parameter7_4byte10[2];
            parameter7_4byte1_10[1] = parameter7_4byte10[3];
            parameter7_4byte1_10[2] = parameter7_4byte10[0];
            parameter7_4byte1_10[3] = parameter7_4byte10[1];
                            
            //0x4800 command1
            parameter7_4byte1_11[0] = parameter7_4byte11[0];    //속도와 가속
            parameter7_4byte1_11[1] = parameter7_4byte11[1];    //커맨드 Code
            parameter7_4byte1_11[2] = parameter7_4byte11[2];    //예약                   
            parameter7_4byte1_11[3] = parameter7_4byte11[3];    //감속, 방향, 천이 조건
                            
            //0x4802 data   
            parameter7_4byte1_12[0] = parameter7_4byte12[2];
            parameter7_4byte1_12[1] = parameter7_4byte12[3];
            parameter7_4byte1_12[2] = parameter7_4byte12[0];
            parameter7_4byte1_12[3] = parameter7_4byte12[1];

            //0x4800 command
            parameter7_4byte1_13[0] = parameter7_4byte13[0];    //속도와 가속
            parameter7_4byte1_13[1] = parameter7_4byte13[1];    //커맨드 Code
            parameter7_4byte1_13[2] = parameter7_4byte13[2];    //예약                   
            parameter7_4byte1_13[3] = parameter7_4byte13[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_14[0] = parameter7_4byte14[2];
            parameter7_4byte1_14[1] = parameter7_4byte14[3];
            parameter7_4byte1_14[2] = parameter7_4byte14[0];
            parameter7_4byte1_14[3] = parameter7_4byte14[1];

            //0x4800 command
            parameter7_4byte1_15[0] = parameter7_4byte15[0];    //속도와 가속
            parameter7_4byte1_15[1] = parameter7_4byte15[1];    //커맨드 Code
            parameter7_4byte1_15[2] = parameter7_4byte15[2];    //예약                   
            parameter7_4byte1_15[3] = parameter7_4byte15[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_16[0] = parameter7_4byte16[2];
            parameter7_4byte1_16[1] = parameter7_4byte16[3];
            parameter7_4byte1_16[2] = parameter7_4byte16[0];
            parameter7_4byte1_16[3] = parameter7_4byte16[1];

            //0x4800 command
            parameter7_4byte1_17[0] = parameter7_4byte17[0];    //속도와 가속
            parameter7_4byte1_17[1] = parameter7_4byte17[1];    //커맨드 Code
            parameter7_4byte1_17[2] = parameter7_4byte17[2];    //예약                   
            parameter7_4byte1_17[3] = parameter7_4byte17[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_18[0] = parameter7_4byte18[2];
            parameter7_4byte1_18[1] = parameter7_4byte18[3];
            parameter7_4byte1_18[2] = parameter7_4byte18[0];
            parameter7_4byte1_18[3] = parameter7_4byte18[1];

            //0x4800 command
            parameter7_4byte1_19[0] = parameter7_4byte19[0];    //속도와 가속
            parameter7_4byte1_19[1] = parameter7_4byte19[1];    //커맨드 Code
            parameter7_4byte1_19[2] = parameter7_4byte19[2];    //예약                   
            parameter7_4byte1_19[3] = parameter7_4byte19[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_20[0] = parameter7_4byte20[2];
            parameter7_4byte1_20[1] = parameter7_4byte20[3];
            parameter7_4byte1_20[2] = parameter7_4byte20[0];
            parameter7_4byte1_20[3] = parameter7_4byte20[1];

            //0x4800 command
            parameter7_4byte1_21[0] = parameter7_4byte21[0];    //속도와 가속
            parameter7_4byte1_21[1] = parameter7_4byte21[1];    //커맨드 Code
            parameter7_4byte1_21[2] = parameter7_4byte21[2];    //예약                   
            parameter7_4byte1_21[3] = parameter7_4byte21[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_22[0] = parameter7_4byte22[2];
            parameter7_4byte1_22[1] = parameter7_4byte22[3];
            parameter7_4byte1_22[2] = parameter7_4byte22[0];
            parameter7_4byte1_22[3] = parameter7_4byte22[1];

            //0x4800 command
            parameter7_4byte1_23[0] = parameter7_4byte23[0];    //속도와 가속
            parameter7_4byte1_23[1] = parameter7_4byte23[1];    //커맨드 Code
            parameter7_4byte1_23[2] = parameter7_4byte23[2];    //예약                   
            parameter7_4byte1_23[3] = parameter7_4byte23[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_24[0] = parameter7_4byte24[2];
            parameter7_4byte1_24[1] = parameter7_4byte24[3];
            parameter7_4byte1_24[2] = parameter7_4byte24[0];
            parameter7_4byte1_24[3] = parameter7_4byte24[1];

            //0x4800 command
            parameter7_4byte1_25[0] = parameter7_4byte25[0];    //속도와 가속
            parameter7_4byte1_25[1] = parameter7_4byte25[1];    //커맨드 Code
            parameter7_4byte1_25[2] = parameter7_4byte25[2];    //예약                   
            parameter7_4byte1_25[3] = parameter7_4byte25[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_26[0] = parameter7_4byte26[2];
            parameter7_4byte1_26[1] = parameter7_4byte26[3];
            parameter7_4byte1_26[2] = parameter7_4byte26[0];
            parameter7_4byte1_26[3] = parameter7_4byte26[1];

            //0x4800 command
            parameter7_4byte1_27[0] = parameter7_4byte27[0];    //속도와 가속
            parameter7_4byte1_27[1] = parameter7_4byte27[1];    //커맨드 Code
            parameter7_4byte1_27[2] = parameter7_4byte27[2];    //예약                   
            parameter7_4byte1_27[3] = parameter7_4byte27[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_28[0] = parameter7_4byte28[2];
            parameter7_4byte1_28[1] = parameter7_4byte28[3];
            parameter7_4byte1_28[2] = parameter7_4byte28[0];
            parameter7_4byte1_28[3] = parameter7_4byte28[1];

            //0x4800 command
            parameter7_4byte1_29[0] = parameter7_4byte29[0];    //속도와 가속
            parameter7_4byte1_29[1] = parameter7_4byte29[1];    //커맨드 Code
            parameter7_4byte1_29[2] = parameter7_4byte29[2];    //예약                   
            parameter7_4byte1_29[3] = parameter7_4byte29[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_30[0] = parameter7_4byte30[2];
            parameter7_4byte1_30[1] = parameter7_4byte30[3];
            parameter7_4byte1_30[2] = parameter7_4byte30[0];
            parameter7_4byte1_30[3] = parameter7_4byte30[1];

            //0x4800 command
            parameter7_4byte1_31[0] = parameter7_4byte31[0];    //속도와 가속
            parameter7_4byte1_31[1] = parameter7_4byte31[1];    //커맨드 Code
            parameter7_4byte1_31[2] = parameter7_4byte31[2];    //예약                   
            parameter7_4byte1_31[3] = parameter7_4byte31[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_32[0] = parameter7_4byte32[2];
            parameter7_4byte1_32[1] = parameter7_4byte32[3];
            parameter7_4byte1_32[2] = parameter7_4byte32[0];
            parameter7_4byte1_32[3] = parameter7_4byte32[1];

            //0x4800 command
            parameter7_4byte1_33[0] = parameter7_4byte33[0];    //속도와 가속
            parameter7_4byte1_33[1] = parameter7_4byte33[1];    //커맨드 Code
            parameter7_4byte1_33[2] = parameter7_4byte33[2];    //예약                   
            parameter7_4byte1_33[3] = parameter7_4byte33[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_34[0] = parameter7_4byte34[2];
            parameter7_4byte1_34[1] = parameter7_4byte34[3];
            parameter7_4byte1_34[2] = parameter7_4byte34[0];
            parameter7_4byte1_34[3] = parameter7_4byte34[1];

            //0x4800 command
            parameter7_4byte1_35[0] = parameter7_4byte35[0];    //속도와 가속
            parameter7_4byte1_35[1] = parameter7_4byte35[1];    //커맨드 Code
            parameter7_4byte1_35[2] = parameter7_4byte35[2];    //예약                   
            parameter7_4byte1_35[3] = parameter7_4byte35[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_36[0] = parameter7_4byte36[2];
            parameter7_4byte1_36[1] = parameter7_4byte36[3];
            parameter7_4byte1_36[2] = parameter7_4byte36[0];
            parameter7_4byte1_36[3] = parameter7_4byte36[1];

            //0x4800 command
            parameter7_4byte1_37[0] = parameter7_4byte37[0];    //속도와 가속
            parameter7_4byte1_37[1] = parameter7_4byte37[1];    //커맨드 Code
            parameter7_4byte1_37[2] = parameter7_4byte37[2];    //예약                   
            parameter7_4byte1_37[3] = parameter7_4byte37[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_38[0] = parameter7_4byte38[2];
            parameter7_4byte1_38[1] = parameter7_4byte38[3];
            parameter7_4byte1_38[2] = parameter7_4byte38[0];
            parameter7_4byte1_38[3] = parameter7_4byte38[1];

            //0x4800 command
            parameter7_4byte1_39[0] = parameter7_4byte39[0];    //속도와 가속
            parameter7_4byte1_39[1] = parameter7_4byte39[1];    //커맨드 Code
            parameter7_4byte1_39[2] = parameter7_4byte39[2];    //예약                   
            parameter7_4byte1_39[3] = parameter7_4byte39[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_40[0] = parameter7_4byte40[2];
            parameter7_4byte1_40[1] = parameter7_4byte40[3];
            parameter7_4byte1_40[2] = parameter7_4byte40[0];
            parameter7_4byte1_40[3] = parameter7_4byte40[1];

            //0x4800 command
            parameter7_4byte1_41[0] = parameter7_4byte41[0];    //속도와 가속
            parameter7_4byte1_41[1] = parameter7_4byte41[1];    //커맨드 Code
            parameter7_4byte1_41[2] = parameter7_4byte41[2];    //예약                   
            parameter7_4byte1_41[3] = parameter7_4byte41[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_42[0] = parameter7_4byte42[2];
            parameter7_4byte1_42[1] = parameter7_4byte42[3];
            parameter7_4byte1_42[2] = parameter7_4byte42[0];
            parameter7_4byte1_42[3] = parameter7_4byte42[1];

            //0x4800 command
            parameter7_4byte1_43[0] = parameter7_4byte43[0];    //속도와 가속
            parameter7_4byte1_43[1] = parameter7_4byte43[1];    //커맨드 Code
            parameter7_4byte1_43[2] = parameter7_4byte43[2];    //예약                   
            parameter7_4byte1_43[3] = parameter7_4byte43[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_44[0] = parameter7_4byte44[2];
            parameter7_4byte1_44[1] = parameter7_4byte44[3];
            parameter7_4byte1_44[2] = parameter7_4byte44[0];
            parameter7_4byte1_44[3] = parameter7_4byte44[1];

            //0x4800 command
            parameter7_4byte1_45[0] = parameter7_4byte45[0];    //속도와 가속
            parameter7_4byte1_45[1] = parameter7_4byte45[1];    //커맨드 Code
            parameter7_4byte1_45[2] = parameter7_4byte45[2];    //예약                   
            parameter7_4byte1_45[3] = parameter7_4byte45[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_46[0] = parameter7_4byte46[2];
            parameter7_4byte1_46[1] = parameter7_4byte46[3];
            parameter7_4byte1_46[2] = parameter7_4byte46[0];
            parameter7_4byte1_46[3] = parameter7_4byte46[1];

            //0x4800 command
            parameter7_4byte1_47[0] = parameter7_4byte47[0];    //속도와 가속
            parameter7_4byte1_47[1] = parameter7_4byte47[1];    //커맨드 Code
            parameter7_4byte1_47[2] = parameter7_4byte47[2];    //예약                   
            parameter7_4byte1_47[3] = parameter7_4byte47[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_48[0] = parameter7_4byte48[2];
            parameter7_4byte1_48[1] = parameter7_4byte48[3];
            parameter7_4byte1_48[2] = parameter7_4byte48[0];
            parameter7_4byte1_48[3] = parameter7_4byte48[1];

            //0x4800 command
            parameter7_4byte1_49[0] = parameter7_4byte49[0];    //속도와 가속
            parameter7_4byte1_49[1] = parameter7_4byte49[1];    //커맨드 Code
            parameter7_4byte1_49[2] = parameter7_4byte49[2];    //예약                   
            parameter7_4byte1_49[3] = parameter7_4byte49[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_50[0] = parameter7_4byte50[2];
            parameter7_4byte1_50[1] = parameter7_4byte50[3];
            parameter7_4byte1_50[2] = parameter7_4byte50[0];
            parameter7_4byte1_50[3] = parameter7_4byte50[1];

            //0x4800 command
            parameter7_4byte1_51[0] = parameter7_4byte51[0];    //속도와 가속
            parameter7_4byte1_51[1] = parameter7_4byte51[1];    //커맨드 Code
            parameter7_4byte1_51[2] = parameter7_4byte51[2];    //예약                   
            parameter7_4byte1_51[3] = parameter7_4byte51[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_52[0] = parameter7_4byte52[2];
            parameter7_4byte1_52[1] = parameter7_4byte52[3];
            parameter7_4byte1_52[2] = parameter7_4byte52[0];
            parameter7_4byte1_52[3] = parameter7_4byte52[1];

            //0x4800 command
            parameter7_4byte1_53[0] = parameter7_4byte53[0];    //속도와 가속
            parameter7_4byte1_53[1] = parameter7_4byte53[1];    //커맨드 Code
            parameter7_4byte1_53[2] = parameter7_4byte53[2];    //예약                   
            parameter7_4byte1_53[3] = parameter7_4byte53[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_54[0] = parameter7_4byte54[2];
            parameter7_4byte1_54[1] = parameter7_4byte54[3];
            parameter7_4byte1_54[2] = parameter7_4byte54[0];
            parameter7_4byte1_54[3] = parameter7_4byte54[1];

            //0x4800 command
            parameter7_4byte1_55[0] = parameter7_4byte55[0];    //속도와 가속
            parameter7_4byte1_55[1] = parameter7_4byte55[1];    //커맨드 Code
            parameter7_4byte1_55[2] = parameter7_4byte55[2];    //예약                   
            parameter7_4byte1_55[3] = parameter7_4byte55[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_56[0] = parameter7_4byte56[2];
            parameter7_4byte1_56[1] = parameter7_4byte56[3];
            parameter7_4byte1_56[2] = parameter7_4byte56[0];
            parameter7_4byte1_56[3] = parameter7_4byte56[1];

            //0x4800 command
            parameter7_4byte1_57[0] = parameter7_4byte57[0];    //속도와 가속
            parameter7_4byte1_57[1] = parameter7_4byte57[1];    //커맨드 Code
            parameter7_4byte1_57[2] = parameter7_4byte57[2];    //예약                   
            parameter7_4byte1_57[3] = parameter7_4byte57[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_58[0] = parameter7_4byte58[2];
            parameter7_4byte1_58[1] = parameter7_4byte58[3];
            parameter7_4byte1_58[2] = parameter7_4byte58[0];
            parameter7_4byte1_58[3] = parameter7_4byte58[1];

            //0x4800 command
            parameter7_4byte1_59[0] = parameter7_4byte59[0];    //속도와 가속
            parameter7_4byte1_59[1] = parameter7_4byte59[1];    //커맨드 Code
            parameter7_4byte1_59[2] = parameter7_4byte59[2];    //예약                   
            parameter7_4byte1_59[3] = parameter7_4byte59[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_60[0] = parameter7_4byte60[2];
            parameter7_4byte1_60[1] = parameter7_4byte60[3];
            parameter7_4byte1_60[2] = parameter7_4byte60[0];
            parameter7_4byte1_60[3] = parameter7_4byte60[1];

            //0x4800 command
            parameter7_4byte1_61[0] = parameter7_4byte61[0];    //속도와 가속
            parameter7_4byte1_61[1] = parameter7_4byte61[1];    //커맨드 Code
            parameter7_4byte1_61[2] = parameter7_4byte61[2];    //예약                   
            parameter7_4byte1_61[3] = parameter7_4byte61[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_62[0] = parameter7_4byte62[2];
            parameter7_4byte1_62[1] = parameter7_4byte62[3];
            parameter7_4byte1_62[2] = parameter7_4byte62[0];
            parameter7_4byte1_62[3] = parameter7_4byte62[1];

            //0x4800 command
            parameter7_4byte1_63[0] = parameter7_4byte63[0];    //속도와 가속
            parameter7_4byte1_63[1] = parameter7_4byte63[1];    //커맨드 Code
            parameter7_4byte1_63[2] = parameter7_4byte63[2];    //예약                   
            parameter7_4byte1_63[3] = parameter7_4byte63[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_64[0] = parameter7_4byte64[2];
            parameter7_4byte1_64[1] = parameter7_4byte64[3];
            parameter7_4byte1_64[2] = parameter7_4byte64[0];
            parameter7_4byte1_64[3] = parameter7_4byte64[1];

            //0x4800 command
            parameter7_4byte1_65[0] = parameter7_4byte65[0];    //속도와 가속
            parameter7_4byte1_65[1] = parameter7_4byte65[1];    //커맨드 Code
            parameter7_4byte1_65[2] = parameter7_4byte65[2];    //예약                   
            parameter7_4byte1_65[3] = parameter7_4byte65[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_66[0] = parameter7_4byte66[2];
            parameter7_4byte1_66[1] = parameter7_4byte66[3];
            parameter7_4byte1_66[2] = parameter7_4byte66[0];
            parameter7_4byte1_66[3] = parameter7_4byte66[1];

            //0x4800 command
            parameter7_4byte1_67[0] = parameter7_4byte67[0];    //속도와 가속
            parameter7_4byte1_67[1] = parameter7_4byte67[1];    //커맨드 Code
            parameter7_4byte1_67[2] = parameter7_4byte67[2];    //예약                   
            parameter7_4byte1_67[3] = parameter7_4byte67[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_68[0] = parameter7_4byte68[2];
            parameter7_4byte1_68[1] = parameter7_4byte68[3];
            parameter7_4byte1_68[2] = parameter7_4byte68[0];
            parameter7_4byte1_68[3] = parameter7_4byte68[1];

            //0x4800 command
            parameter7_4byte1_69[0] = parameter7_4byte69[0];    //속도와 가속
            parameter7_4byte1_69[1] = parameter7_4byte69[1];    //커맨드 Code
            parameter7_4byte1_69[2] = parameter7_4byte69[2];    //예약                   
            parameter7_4byte1_69[3] = parameter7_4byte69[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_70[0] = parameter7_4byte70[2];
            parameter7_4byte1_70[1] = parameter7_4byte70[3];
            parameter7_4byte1_70[2] = parameter7_4byte70[0];
            parameter7_4byte1_70[3] = parameter7_4byte70[1];

            //0x4800 command
            parameter7_4byte1_71[0] = parameter7_4byte71[0];    //속도와 가속
            parameter7_4byte1_71[1] = parameter7_4byte71[1];    //커맨드 Code
            parameter7_4byte1_71[2] = parameter7_4byte71[2];    //예약                   
            parameter7_4byte1_71[3] = parameter7_4byte71[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_72[0] = parameter7_4byte72[2];
            parameter7_4byte1_72[1] = parameter7_4byte72[3];
            parameter7_4byte1_72[2] = parameter7_4byte72[0];
            parameter7_4byte1_72[3] = parameter7_4byte72[1];

            //0x4800 command
            parameter7_4byte1_73[0] = parameter7_4byte73[0];    //속도와 가속
            parameter7_4byte1_73[1] = parameter7_4byte73[1];    //커맨드 Code
            parameter7_4byte1_73[2] = parameter7_4byte73[2];    //예약                   
            parameter7_4byte1_73[3] = parameter7_4byte73[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_74[0] = parameter7_4byte74[2];
            parameter7_4byte1_74[1] = parameter7_4byte74[3];
            parameter7_4byte1_74[2] = parameter7_4byte74[0];
            parameter7_4byte1_74[3] = parameter7_4byte74[1];

            //0x4800 command
            parameter7_4byte1_75[0] = parameter7_4byte75[0];    //속도와 가속
            parameter7_4byte1_75[1] = parameter7_4byte75[1];    //커맨드 Code
            parameter7_4byte1_75[2] = parameter7_4byte75[2];    //예약                   
            parameter7_4byte1_75[3] = parameter7_4byte75[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_76[0] = parameter7_4byte76[2];
            parameter7_4byte1_76[1] = parameter7_4byte76[3];
            parameter7_4byte1_76[2] = parameter7_4byte76[0];
            parameter7_4byte1_76[3] = parameter7_4byte76[1];

            //0x4800 command
            parameter7_4byte1_77[0] = parameter7_4byte77[0];    //속도와 가속
            parameter7_4byte1_77[1] = parameter7_4byte77[1];    //커맨드 Code
            parameter7_4byte1_77[2] = parameter7_4byte77[2];    //예약                   
            parameter7_4byte1_77[3] = parameter7_4byte77[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_78[0] = parameter7_4byte78[2];
            parameter7_4byte1_78[1] = parameter7_4byte78[3];
            parameter7_4byte1_78[2] = parameter7_4byte78[0];
            parameter7_4byte1_78[3] = parameter7_4byte78[1];

            //0x4800 command
            parameter7_4byte1_79[0] = parameter7_4byte79[0];    //속도와 가속
            parameter7_4byte1_79[1] = parameter7_4byte79[1];    //커맨드 Code
            parameter7_4byte1_79[2] = parameter7_4byte79[2];    //예약                   
            parameter7_4byte1_79[3] = parameter7_4byte79[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_80[0] = parameter7_4byte80[2];
            parameter7_4byte1_80[1] = parameter7_4byte80[3];
            parameter7_4byte1_80[2] = parameter7_4byte80[0];
            parameter7_4byte1_80[3] = parameter7_4byte80[1];

            //0x4800 command
            parameter7_4byte1_81[0] = parameter7_4byte81[0];    //속도와 가속
            parameter7_4byte1_81[1] = parameter7_4byte81[1];    //커맨드 Code
            parameter7_4byte1_81[2] = parameter7_4byte81[2];    //예약                   
            parameter7_4byte1_81[3] = parameter7_4byte81[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_82[0] = parameter7_4byte82[2];
            parameter7_4byte1_82[1] = parameter7_4byte82[3];
            parameter7_4byte1_82[2] = parameter7_4byte82[0];
            parameter7_4byte1_82[3] = parameter7_4byte82[1];

            //0x4800 command
            parameter7_4byte1_83[0] = parameter7_4byte83[0];    //속도와 가속
            parameter7_4byte1_83[1] = parameter7_4byte83[1];    //커맨드 Code
            parameter7_4byte1_83[2] = parameter7_4byte83[2];    //예약                   
            parameter7_4byte1_83[3] = parameter7_4byte83[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_84[0] = parameter7_4byte84[2];
            parameter7_4byte1_84[1] = parameter7_4byte84[3];
            parameter7_4byte1_84[2] = parameter7_4byte84[0];
            parameter7_4byte1_84[3] = parameter7_4byte84[1];

            //0x4800 command
            parameter7_4byte1_85[0] = parameter7_4byte85[0];    //속도와 가속
            parameter7_4byte1_85[1] = parameter7_4byte85[1];    //커맨드 Code
            parameter7_4byte1_85[2] = parameter7_4byte85[2];    //예약                   
            parameter7_4byte1_85[3] = parameter7_4byte85[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_86[0] = parameter7_4byte86[2];
            parameter7_4byte1_86[1] = parameter7_4byte86[3];
            parameter7_4byte1_86[2] = parameter7_4byte86[0];
            parameter7_4byte1_86[3] = parameter7_4byte86[1];

            //0x4800 command
            parameter7_4byte1_87[0] = parameter7_4byte87[0];    //속도와 가속
            parameter7_4byte1_87[1] = parameter7_4byte87[1];    //커맨드 Code
            parameter7_4byte1_87[2] = parameter7_4byte87[2];    //예약                   
            parameter7_4byte1_87[3] = parameter7_4byte87[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_88[0] = parameter7_4byte88[2];
            parameter7_4byte1_88[1] = parameter7_4byte88[3];
            parameter7_4byte1_88[2] = parameter7_4byte88[0];
            parameter7_4byte1_88[3] = parameter7_4byte88[1];

            //0x4800 command
            parameter7_4byte1_89[0] = parameter7_4byte89[0];    //속도와 가속
            parameter7_4byte1_89[1] = parameter7_4byte89[1];    //커맨드 Code
            parameter7_4byte1_89[2] = parameter7_4byte89[2];    //예약                   
            parameter7_4byte1_89[3] = parameter7_4byte89[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_90[0] = parameter7_4byte90[2];
            parameter7_4byte1_90[1] = parameter7_4byte90[3];
            parameter7_4byte1_90[2] = parameter7_4byte90[0];
            parameter7_4byte1_90[3] = parameter7_4byte90[1];

            //0x4800 command
            parameter7_4byte1_91[0] = parameter7_4byte91[0];    //속도와 가속
            parameter7_4byte1_91[1] = parameter7_4byte91[1];    //커맨드 Code
            parameter7_4byte1_91[2] = parameter7_4byte91[2];    //예약                   
            parameter7_4byte1_91[3] = parameter7_4byte91[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_92[0] = parameter7_4byte92[2];
            parameter7_4byte1_92[1] = parameter7_4byte92[3];
            parameter7_4byte1_92[2] = parameter7_4byte92[0];
            parameter7_4byte1_92[3] = parameter7_4byte92[1];

            //0x4800 command
            parameter7_4byte1_93[0] = parameter7_4byte93[0];    //속도와 가속
            parameter7_4byte1_93[1] = parameter7_4byte93[1];    //커맨드 Code
            parameter7_4byte1_93[2] = parameter7_4byte93[2];    //예약                   
            parameter7_4byte1_93[3] = parameter7_4byte93[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_94[0] = parameter7_4byte94[2];
            parameter7_4byte1_94[1] = parameter7_4byte94[3];
            parameter7_4byte1_94[2] = parameter7_4byte94[0];
            parameter7_4byte1_94[3] = parameter7_4byte94[1];

            //0x4800 command
            parameter7_4byte1_95[0] = parameter7_4byte95[0];    //속도와 가속
            parameter7_4byte1_95[1] = parameter7_4byte95[1];    //커맨드 Code
            parameter7_4byte1_95[2] = parameter7_4byte95[2];    //예약                   
            parameter7_4byte1_95[3] = parameter7_4byte95[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_96[0] = parameter7_4byte96[2];
            parameter7_4byte1_96[1] = parameter7_4byte96[3];
            parameter7_4byte1_96[2] = parameter7_4byte96[0];
            parameter7_4byte1_96[3] = parameter7_4byte96[1];

            //0x4800 command
            parameter7_4byte1_97[0] = parameter7_4byte97[0];    //속도와 가속
            parameter7_4byte1_97[1] = parameter7_4byte97[1];    //커맨드 Code
            parameter7_4byte1_97[2] = parameter7_4byte97[2];    //예약                   
            parameter7_4byte1_97[3] = parameter7_4byte97[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_98[0] = parameter7_4byte98[2];
            parameter7_4byte1_98[1] = parameter7_4byte98[3];
            parameter7_4byte1_98[2] = parameter7_4byte98[0];
            parameter7_4byte1_98[3] = parameter7_4byte98[1];

            //0x4800 command
            parameter7_4byte1_99[0] = parameter7_4byte99[0];    //속도와 가속
            parameter7_4byte1_99[1] = parameter7_4byte99[1];    //커맨드 Code
            parameter7_4byte1_99[2] = parameter7_4byte99[2];    //예약                   
            parameter7_4byte1_99[3] = parameter7_4byte99[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_100[0] = parameter7_4byte100[2];
            parameter7_4byte1_100[1] = parameter7_4byte100[3];
            parameter7_4byte1_100[2] = parameter7_4byte100[0];
            parameter7_4byte1_100[3] = parameter7_4byte100[1];

            //0x4800 command
            parameter7_4byte1_101[0] = parameter7_4byte101[0];    //속도와 가속
            parameter7_4byte1_101[1] = parameter7_4byte101[1];    //커맨드 Code
            parameter7_4byte1_101[2] = parameter7_4byte101[2];    //예약                   
            parameter7_4byte1_101[3] = parameter7_4byte101[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_102[0] = parameter7_4byte102[2];
            parameter7_4byte1_102[1] = parameter7_4byte102[3];
            parameter7_4byte1_102[2] = parameter7_4byte102[0];
            parameter7_4byte1_102[3] = parameter7_4byte102[1];

            //0x4800 command
            parameter7_4byte1_103[0] = parameter7_4byte103[0];    //속도와 가속
            parameter7_4byte1_103[1] = parameter7_4byte103[1];    //커맨드 Code
            parameter7_4byte1_103[2] = parameter7_4byte103[2];    //예약                   
            parameter7_4byte1_103[3] = parameter7_4byte103[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_104[0] = parameter7_4byte104[2];
            parameter7_4byte1_104[1] = parameter7_4byte104[3];
            parameter7_4byte1_104[2] = parameter7_4byte104[0];
            parameter7_4byte1_104[3] = parameter7_4byte104[1];

            //0x4800 command
            parameter7_4byte1_105[0] = parameter7_4byte105[0];    //속도와 가속
            parameter7_4byte1_105[1] = parameter7_4byte105[1];    //커맨드 Code
            parameter7_4byte1_105[2] = parameter7_4byte105[2];    //예약                   
            parameter7_4byte1_105[3] = parameter7_4byte105[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_106[0] = parameter7_4byte106[2];
            parameter7_4byte1_106[1] = parameter7_4byte106[3];
            parameter7_4byte1_106[2] = parameter7_4byte106[0];
            parameter7_4byte1_106[3] = parameter7_4byte106[1];

            //0x4800 command
            parameter7_4byte1_107[0] = parameter7_4byte107[0];    //속도와 가속
            parameter7_4byte1_107[1] = parameter7_4byte107[1];    //커맨드 Code
            parameter7_4byte1_107[2] = parameter7_4byte107[2];    //예약                   
            parameter7_4byte1_107[3] = parameter7_4byte107[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_108[0] = parameter7_4byte108[2];
            parameter7_4byte1_108[1] = parameter7_4byte108[3];
            parameter7_4byte1_108[2] = parameter7_4byte108[0];
            parameter7_4byte1_108[3] = parameter7_4byte108[1];

            //0x4800 command
            parameter7_4byte1_109[0] = parameter7_4byte109[0];    //속도와 가속
            parameter7_4byte1_109[1] = parameter7_4byte109[1];    //커맨드 Code
            parameter7_4byte1_109[2] = parameter7_4byte109[2];    //예약                   
            parameter7_4byte1_109[3] = parameter7_4byte109[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_110[0] = parameter7_4byte110[2];
            parameter7_4byte1_110[1] = parameter7_4byte110[3];
            parameter7_4byte1_110[2] = parameter7_4byte110[0];
            parameter7_4byte1_110[3] = parameter7_4byte110[1];

            //0x4800 command
            parameter7_4byte1_111[0] = parameter7_4byte111[0];    //속도와 가속
            parameter7_4byte1_111[1] = parameter7_4byte111[1];    //커맨드 Code
            parameter7_4byte1_111[2] = parameter7_4byte111[2];    //예약                   
            parameter7_4byte1_111[3] = parameter7_4byte111[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_112[0] = parameter7_4byte112[2];
            parameter7_4byte1_112[1] = parameter7_4byte112[3];
            parameter7_4byte1_112[2] = parameter7_4byte112[0];
            parameter7_4byte1_112[3] = parameter7_4byte112[1];

            //0x4800 command
            parameter7_4byte1_113[0] = parameter7_4byte113[0];    //속도와 가속
            parameter7_4byte1_113[1] = parameter7_4byte113[1];    //커맨드 Code
            parameter7_4byte1_113[2] = parameter7_4byte113[2];    //예약                   
            parameter7_4byte1_113[3] = parameter7_4byte113[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_114[0] = parameter7_4byte114[2];
            parameter7_4byte1_114[1] = parameter7_4byte114[3];
            parameter7_4byte1_114[2] = parameter7_4byte114[0];
            parameter7_4byte1_114[3] = parameter7_4byte114[1];

            //0x4800 command
            parameter7_4byte1_115[0] = parameter7_4byte115[0];    //속도와 가속
            parameter7_4byte1_115[1] = parameter7_4byte115[1];    //커맨드 Code
            parameter7_4byte1_115[2] = parameter7_4byte115[2];    //예약                   
            parameter7_4byte1_115[3] = parameter7_4byte115[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_116[0] = parameter7_4byte116[2];
            parameter7_4byte1_116[1] = parameter7_4byte116[3];
            parameter7_4byte1_116[2] = parameter7_4byte116[0];
            parameter7_4byte1_116[3] = parameter7_4byte116[1];

            //0x4800 command
            parameter7_4byte1_117[0] = parameter7_4byte117[0];    //속도와 가속
            parameter7_4byte1_117[1] = parameter7_4byte117[1];    //커맨드 Code
            parameter7_4byte1_117[2] = parameter7_4byte117[2];    //예약                   
            parameter7_4byte1_117[3] = parameter7_4byte117[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_118[0] = parameter7_4byte118[2];
            parameter7_4byte1_118[1] = parameter7_4byte118[3];
            parameter7_4byte1_118[2] = parameter7_4byte118[0];
            parameter7_4byte1_118[3] = parameter7_4byte118[1];

            //0x4800 command
            parameter7_4byte1_119[0] = parameter7_4byte119[0];    //속도와 가속
            parameter7_4byte1_119[1] = parameter7_4byte119[1];    //커맨드 Code
            parameter7_4byte1_119[2] = parameter7_4byte119[2];    //예약                   
            parameter7_4byte1_119[3] = parameter7_4byte119[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_120[0] = parameter7_4byte120[2];
            parameter7_4byte1_120[1] = parameter7_4byte120[3];
            parameter7_4byte1_120[2] = parameter7_4byte120[0];
            parameter7_4byte1_120[3] = parameter7_4byte120[1];

            //0x4800 command
            parameter7_4byte1_121[0] = parameter7_4byte121[0];    //속도와 가속
            parameter7_4byte1_121[1] = parameter7_4byte121[1];    //커맨드 Code
            parameter7_4byte1_121[2] = parameter7_4byte121[2];    //예약                   
            parameter7_4byte1_121[3] = parameter7_4byte121[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_122[0] = parameter7_4byte122[2];
            parameter7_4byte1_122[1] = parameter7_4byte122[3];
            parameter7_4byte1_122[2] = parameter7_4byte122[0];
            parameter7_4byte1_122[3] = parameter7_4byte122[1];

            //0x4800 command
            parameter7_4byte1_123[0] = parameter7_4byte123[0];    //속도와 가속
            parameter7_4byte1_123[1] = parameter7_4byte123[1];    //커맨드 Code
            parameter7_4byte1_123[2] = parameter7_4byte123[2];    //예약                   
            parameter7_4byte1_123[3] = parameter7_4byte123[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_124[0] = parameter7_4byte124[2];
            parameter7_4byte1_124[1] = parameter7_4byte124[3];
            parameter7_4byte1_124[2] = parameter7_4byte124[0];
            parameter7_4byte1_124[3] = parameter7_4byte124[1];

            //0x4800 command
            parameter7_4byte1_125[0] = parameter7_4byte125[0];    //속도와 가속
            parameter7_4byte1_125[1] = parameter7_4byte125[1];    //커맨드 Code
            parameter7_4byte1_125[2] = parameter7_4byte125[2];    //예약                   
            parameter7_4byte1_125[3] = parameter7_4byte125[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_126[0] = parameter7_4byte126[2];
            parameter7_4byte1_126[1] = parameter7_4byte126[3];
            parameter7_4byte1_126[2] = parameter7_4byte126[0];
            parameter7_4byte1_126[3] = parameter7_4byte126[1];

            //0x4800 command
            parameter7_4byte1_127[0] = parameter7_4byte127[0];    //속도와 가속
            parameter7_4byte1_127[1] = parameter7_4byte127[1];    //커맨드 Code
            parameter7_4byte1_127[2] = parameter7_4byte127[2];    //예약                   
            parameter7_4byte1_127[3] = parameter7_4byte127[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_128[0] = parameter7_4byte128[2];
            parameter7_4byte1_128[1] = parameter7_4byte128[3];
            parameter7_4byte1_128[2] = parameter7_4byte128[0];
            parameter7_4byte1_128[3] = parameter7_4byte128[1];

            //0x4800 command
            parameter7_4byte1_129[0] = parameter7_4byte129[0];    //속도와 가속
            parameter7_4byte1_129[1] = parameter7_4byte129[1];    //커맨드 Code
            parameter7_4byte1_129[2] = parameter7_4byte129[2];    //예약                   
            parameter7_4byte1_129[3] = parameter7_4byte129[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_130[0] = parameter7_4byte130[2];
            parameter7_4byte1_130[1] = parameter7_4byte130[3];
            parameter7_4byte1_130[2] = parameter7_4byte130[0];
            parameter7_4byte1_130[3] = parameter7_4byte130[1];

            //0x4800 command
            parameter7_4byte1_131[0] = parameter7_4byte131[0];    //속도와 가속
            parameter7_4byte1_131[1] = parameter7_4byte131[1];    //커맨드 Code
            parameter7_4byte1_131[2] = parameter7_4byte131[2];    //예약                   
            parameter7_4byte1_131[3] = parameter7_4byte131[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_132[0] = parameter7_4byte132[2];
            parameter7_4byte1_132[1] = parameter7_4byte132[3];
            parameter7_4byte1_132[2] = parameter7_4byte132[0];
            parameter7_4byte1_132[3] = parameter7_4byte132[1];

            //0x4800 command
            parameter7_4byte1_133[0] = parameter7_4byte133[0];    //속도와 가속
            parameter7_4byte1_133[1] = parameter7_4byte133[1];    //커맨드 Code
            parameter7_4byte1_133[2] = parameter7_4byte133[2];    //예약                   
            parameter7_4byte1_133[3] = parameter7_4byte133[3];    //감속, 방향, 천이 조건


            //0x4802 data
            parameter7_4byte1_134[0] = parameter7_4byte134[2];
            parameter7_4byte1_134[1] = parameter7_4byte134[3];
            parameter7_4byte1_134[2] = parameter7_4byte134[0];
            parameter7_4byte1_134[3] = parameter7_4byte134[1];

            //0x4800 command
            parameter7_4byte1_135[0] = parameter7_4byte135[0];    //속도와 가속
            parameter7_4byte1_135[1] = parameter7_4byte135[1];    //커맨드 Code
            parameter7_4byte1_135[2] = parameter7_4byte135[2];    //예약                   
            parameter7_4byte1_135[3] = parameter7_4byte135[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_136[0] = parameter7_4byte136[2];
            parameter7_4byte1_136[1] = parameter7_4byte136[3];
            parameter7_4byte1_136[2] = parameter7_4byte136[0];
            parameter7_4byte1_136[3] = parameter7_4byte136[1];

            //0x4800 command
            parameter7_4byte1_137[0] = parameter7_4byte137[0];    //속도와 가속
            parameter7_4byte1_137[1] = parameter7_4byte137[1];    //커맨드 Code
            parameter7_4byte1_137[2] = parameter7_4byte137[2];    //예약                   
            parameter7_4byte1_137[3] = parameter7_4byte137[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_138[0] = parameter7_4byte138[2];
            parameter7_4byte1_138[1] = parameter7_4byte138[3];
            parameter7_4byte1_138[2] = parameter7_4byte138[0];
            parameter7_4byte1_138[3] = parameter7_4byte138[1];

            //0x4800 command
            parameter7_4byte1_139[0] = parameter7_4byte139[0];    //속도와 가속
            parameter7_4byte1_139[1] = parameter7_4byte139[1];    //커맨드 Code
            parameter7_4byte1_139[2] = parameter7_4byte139[2];    //예약                   
            parameter7_4byte1_139[3] = parameter7_4byte139[3];    //감속, 방향, 천이 조건

            
            parameter7_4byte1_140[0] = parameter7_4byte140[0];    //속도와 가속
            parameter7_4byte1_140[1] = parameter7_4byte140[1];    //커맨드 Code
            parameter7_4byte1_140[2] = parameter7_4byte140[2];    //예약                   
            parameter7_4byte1_140[3] = parameter7_4byte140[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_141[0] = parameter7_4byte141[2];
            parameter7_4byte1_141[1] = parameter7_4byte141[3];
            parameter7_4byte1_141[2] = parameter7_4byte141[0];
            parameter7_4byte1_141[3] = parameter7_4byte141[1];

            //0x4800 command
            parameter7_4byte1_142[0] = parameter7_4byte142[0];    //속도와 가속
            parameter7_4byte1_142[1] = parameter7_4byte142[1];    //커맨드 Code
            parameter7_4byte1_142[2] = parameter7_4byte142[2];    //예약                   
            parameter7_4byte1_142[3] = parameter7_4byte142[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_143[0] = parameter7_4byte143[2];
            parameter7_4byte1_143[1] = parameter7_4byte143[3];
            parameter7_4byte1_143[2] = parameter7_4byte143[0];
            parameter7_4byte1_143[3] = parameter7_4byte143[1];

            //0x4800 command
            parameter7_4byte1_144[0] = parameter7_4byte144[0];    //속도와 가속
            parameter7_4byte1_144[1] = parameter7_4byte144[1];    //커맨드 Code
            parameter7_4byte1_144[2] = parameter7_4byte144[2];    //예약                   
            parameter7_4byte1_144[3] = parameter7_4byte144[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_145[0] = parameter7_4byte145[2];
            parameter7_4byte1_145[1] = parameter7_4byte145[3];
            parameter7_4byte1_145[2] = parameter7_4byte145[0];
            parameter7_4byte1_145[3] = parameter7_4byte145[1];

            //0x4800 command
            parameter7_4byte1_146[0] = parameter7_4byte146[0];    //속도와 가속
            parameter7_4byte1_146[1] = parameter7_4byte146[1];    //커맨드 Code
            parameter7_4byte1_146[2] = parameter7_4byte146[2];    //예약                   
            parameter7_4byte1_146[3] = parameter7_4byte146[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_147[0] = parameter7_4byte147[2];
            parameter7_4byte1_147[1] = parameter7_4byte147[3];
            parameter7_4byte1_147[2] = parameter7_4byte147[0];
            parameter7_4byte1_147[3] = parameter7_4byte147[1];

            //0x4800 command
            parameter7_4byte1_148[0] = parameter7_4byte148[0];    //속도와 가속
            parameter7_4byte1_148[1] = parameter7_4byte148[1];    //커맨드 Code
            parameter7_4byte1_148[2] = parameter7_4byte148[2];    //예약                   
            parameter7_4byte1_148[3] = parameter7_4byte148[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_149[0] = parameter7_4byte149[2];
            parameter7_4byte1_149[1] = parameter7_4byte149[3];
            parameter7_4byte1_149[2] = parameter7_4byte149[0];
            parameter7_4byte1_149[3] = parameter7_4byte149[1];

            //0x4800 command
            parameter7_4byte1_150[0] = parameter7_4byte150[0];    //속도와 가속
            parameter7_4byte1_150[1] = parameter7_4byte150[1];    //커맨드 Code
            parameter7_4byte1_150[2] = parameter7_4byte150[2];    //예약                   
            parameter7_4byte1_150[3] = parameter7_4byte150[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_151[0] = parameter7_4byte151[2];
            parameter7_4byte1_151[1] = parameter7_4byte151[3];
            parameter7_4byte1_151[2] = parameter7_4byte151[0];
            parameter7_4byte1_151[3] = parameter7_4byte151[1];

            //0x4800 command
            parameter7_4byte1_152[0] = parameter7_4byte152[0];    //속도와 가속
            parameter7_4byte1_152[1] = parameter7_4byte152[1];    //커맨드 Code
            parameter7_4byte1_152[2] = parameter7_4byte152[2];    //예약                   
            parameter7_4byte1_152[3] = parameter7_4byte152[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_153[0] = parameter7_4byte153[2];
            parameter7_4byte1_153[1] = parameter7_4byte153[3];
            parameter7_4byte1_153[2] = parameter7_4byte153[0];
            parameter7_4byte1_153[3] = parameter7_4byte153[1];

            //0x4800 command
            parameter7_4byte1_154[0] = parameter7_4byte154[0];    //속도와 가속
            parameter7_4byte1_154[1] = parameter7_4byte154[1];    //커맨드 Code
            parameter7_4byte1_154[2] = parameter7_4byte154[2];    //예약                   
            parameter7_4byte1_154[3] = parameter7_4byte154[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_155[0] = parameter7_4byte155[2];
            parameter7_4byte1_155[1] = parameter7_4byte155[3];
            parameter7_4byte1_155[2] = parameter7_4byte155[0];
            parameter7_4byte1_155[3] = parameter7_4byte155[1];

            //0x4800 command
            parameter7_4byte1_156[0] = parameter7_4byte156[0];    //속도와 가속
            parameter7_4byte1_156[1] = parameter7_4byte156[1];    //커맨드 Code
            parameter7_4byte1_156[2] = parameter7_4byte156[2];    //예약                   
            parameter7_4byte1_156[3] = parameter7_4byte156[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_157[0] = parameter7_4byte157[2];
            parameter7_4byte1_157[1] = parameter7_4byte157[3];
            parameter7_4byte1_157[2] = parameter7_4byte157[0];
            parameter7_4byte1_157[3] = parameter7_4byte157[1];

            //0x4800 command
            parameter7_4byte1_158[0] = parameter7_4byte158[0];    //속도와 가속
            parameter7_4byte1_158[1] = parameter7_4byte158[1];    //커맨드 Code
            parameter7_4byte1_158[2] = parameter7_4byte158[2];    //예약                   
            parameter7_4byte1_158[3] = parameter7_4byte158[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_159[0] = parameter7_4byte159[2];
            parameter7_4byte1_159[1] = parameter7_4byte159[3];
            parameter7_4byte1_159[2] = parameter7_4byte159[0];
            parameter7_4byte1_159[3] = parameter7_4byte159[1];

            //0x4800 command
            parameter7_4byte1_160[0] = parameter7_4byte160[0];    //속도와 가속
            parameter7_4byte1_160[1] = parameter7_4byte160[1];    //커맨드 Code
            parameter7_4byte1_160[2] = parameter7_4byte160[2];    //예약                   
            parameter7_4byte1_160[3] = parameter7_4byte160[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_161[0] = parameter7_4byte161[2];
            parameter7_4byte1_161[1] = parameter7_4byte161[3];
            parameter7_4byte1_161[2] = parameter7_4byte161[0];
            parameter7_4byte1_161[3] = parameter7_4byte161[1];

            //0x4800 command
            parameter7_4byte1_162[0] = parameter7_4byte162[0];    //속도와 가속
            parameter7_4byte1_162[1] = parameter7_4byte162[1];    //커맨드 Code
            parameter7_4byte1_162[2] = parameter7_4byte162[2];    //예약                   
            parameter7_4byte1_162[3] = parameter7_4byte162[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_163[0] = parameter7_4byte163[2];
            parameter7_4byte1_163[1] = parameter7_4byte163[3];
            parameter7_4byte1_163[2] = parameter7_4byte163[0];
            parameter7_4byte1_163[3] = parameter7_4byte163[1];

            //0x4800 command
            parameter7_4byte1_164[0] = parameter7_4byte164[0];    //속도와 가속
            parameter7_4byte1_164[1] = parameter7_4byte164[1];    //커맨드 Code
            parameter7_4byte1_164[2] = parameter7_4byte164[2];    //예약                   
            parameter7_4byte1_164[3] = parameter7_4byte164[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_165[0] = parameter7_4byte165[2];
            parameter7_4byte1_165[1] = parameter7_4byte165[3];
            parameter7_4byte1_165[2] = parameter7_4byte165[0];
            parameter7_4byte1_165[3] = parameter7_4byte165[1];

            //0x4800 command
            parameter7_4byte1_166[0] = parameter7_4byte166[0];    //속도와 가속
            parameter7_4byte1_166[1] = parameter7_4byte166[1];    //커맨드 Code
            parameter7_4byte1_166[2] = parameter7_4byte166[2];    //예약                   
            parameter7_4byte1_166[3] = parameter7_4byte166[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_167[0] = parameter7_4byte167[2];
            parameter7_4byte1_167[1] = parameter7_4byte167[3];
            parameter7_4byte1_167[2] = parameter7_4byte167[0];
            parameter7_4byte1_167[3] = parameter7_4byte167[1];

            //0x4800 command
            parameter7_4byte1_168[0] = parameter7_4byte168[0];    //속도와 가속
            parameter7_4byte1_168[1] = parameter7_4byte168[1];    //커맨드 Code
            parameter7_4byte1_168[2] = parameter7_4byte168[2];    //예약                   
            parameter7_4byte1_168[3] = parameter7_4byte168[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_169[0] = parameter7_4byte169[2];
            parameter7_4byte1_169[1] = parameter7_4byte169[3];
            parameter7_4byte1_169[2] = parameter7_4byte169[0];
            parameter7_4byte1_169[3] = parameter7_4byte169[1];

            //0x4800 command
            parameter7_4byte1_170[0] = parameter7_4byte170[0];    //속도와 가속
            parameter7_4byte1_170[1] = parameter7_4byte170[1];    //커맨드 Code
            parameter7_4byte1_170[2] = parameter7_4byte170[2];    //예약                   
            parameter7_4byte1_170[3] = parameter7_4byte170[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_171[0] = parameter7_4byte171[2];
            parameter7_4byte1_171[1] = parameter7_4byte171[3];
            parameter7_4byte1_171[2] = parameter7_4byte171[0];
            parameter7_4byte1_171[3] = parameter7_4byte171[1];

            //0x4800 command
            parameter7_4byte1_172[0] = parameter7_4byte172[0];    //속도와 가속
            parameter7_4byte1_172[1] = parameter7_4byte172[1];    //커맨드 Code
            parameter7_4byte1_172[2] = parameter7_4byte172[2];    //예약                   
            parameter7_4byte1_172[3] = parameter7_4byte172[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_173[0] = parameter7_4byte173[2];
            parameter7_4byte1_173[1] = parameter7_4byte173[3];
            parameter7_4byte1_173[2] = parameter7_4byte173[0];
            parameter7_4byte1_173[3] = parameter7_4byte173[1];

            //0x4800 command
            parameter7_4byte1_174[0] = parameter7_4byte174[0];    //속도와 가속
            parameter7_4byte1_174[1] = parameter7_4byte174[1];    //커맨드 Code
            parameter7_4byte1_174[2] = parameter7_4byte174[2];    //예약                   
            parameter7_4byte1_174[3] = parameter7_4byte174[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_175[0] = parameter7_4byte175[2];
            parameter7_4byte1_175[1] = parameter7_4byte175[3];
            parameter7_4byte1_175[2] = parameter7_4byte175[0];
            parameter7_4byte1_175[3] = parameter7_4byte175[1];

            //0x4800 command
            parameter7_4byte1_176[0] = parameter7_4byte176[0];    //속도와 가속
            parameter7_4byte1_176[1] = parameter7_4byte176[1];    //커맨드 Code
            parameter7_4byte1_176[2] = parameter7_4byte176[2];    //예약                   
            parameter7_4byte1_176[3] = parameter7_4byte176[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_177[0] = parameter7_4byte177[2];
            parameter7_4byte1_177[1] = parameter7_4byte177[3];
            parameter7_4byte1_177[2] = parameter7_4byte177[0];
            parameter7_4byte1_177[3] = parameter7_4byte177[1];

            //0x4800 command
            parameter7_4byte1_178[0] = parameter7_4byte178[0];    //속도와 가속
            parameter7_4byte1_178[1] = parameter7_4byte178[1];    //커맨드 Code
            parameter7_4byte1_178[2] = parameter7_4byte178[2];    //예약                   
            parameter7_4byte1_178[3] = parameter7_4byte178[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_179[0] = parameter7_4byte179[2];
            parameter7_4byte1_179[1] = parameter7_4byte179[3];
            parameter7_4byte1_179[2] = parameter7_4byte179[0];
            parameter7_4byte1_179[3] = parameter7_4byte179[1];

            //0x4800 command
            parameter7_4byte1_180[0] = parameter7_4byte180[0];    //속도와 가속
            parameter7_4byte1_180[1] = parameter7_4byte180[1];    //커맨드 Code
            parameter7_4byte1_180[2] = parameter7_4byte180[2];    //예약                   
            parameter7_4byte1_180[3] = parameter7_4byte180[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_181[0] = parameter7_4byte181[2];
            parameter7_4byte1_181[1] = parameter7_4byte181[3];
            parameter7_4byte1_181[2] = parameter7_4byte181[0];
            parameter7_4byte1_181[3] = parameter7_4byte181[1];

            //0x4800 command
            parameter7_4byte1_182[0] = parameter7_4byte182[0];    //속도와 가속
            parameter7_4byte1_182[1] = parameter7_4byte182[1];    //커맨드 Code
            parameter7_4byte1_182[2] = parameter7_4byte182[2];    //예약                   
            parameter7_4byte1_182[3] = parameter7_4byte182[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_183[0] = parameter7_4byte183[2];
            parameter7_4byte1_183[1] = parameter7_4byte183[3];
            parameter7_4byte1_183[2] = parameter7_4byte183[0];
            parameter7_4byte1_183[3] = parameter7_4byte183[1];

            //0x4800 command
            parameter7_4byte1_184[0] = parameter7_4byte184[0];    //속도와 가속
            parameter7_4byte1_184[1] = parameter7_4byte184[1];    //커맨드 Code
            parameter7_4byte1_184[2] = parameter7_4byte184[2];    //예약                   
            parameter7_4byte1_184[3] = parameter7_4byte184[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_185[0] = parameter7_4byte185[2];
            parameter7_4byte1_185[1] = parameter7_4byte185[3];
            parameter7_4byte1_185[2] = parameter7_4byte185[0];
            parameter7_4byte1_185[3] = parameter7_4byte185[1];

            //0x4800 command
            parameter7_4byte1_186[0] = parameter7_4byte186[0];    //속도와 가속
            parameter7_4byte1_186[1] = parameter7_4byte186[1];    //커맨드 Code
            parameter7_4byte1_186[2] = parameter7_4byte186[2];    //예약                   
            parameter7_4byte1_186[3] = parameter7_4byte186[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_187[0] = parameter7_4byte187[2];
            parameter7_4byte1_187[1] = parameter7_4byte187[3];
            parameter7_4byte1_187[2] = parameter7_4byte187[0];
            parameter7_4byte1_187[3] = parameter7_4byte187[1];

            //0x4800 command
            parameter7_4byte1_188[0] = parameter7_4byte188[0];    //속도와 가속
            parameter7_4byte1_188[1] = parameter7_4byte188[1];    //커맨드 Code
            parameter7_4byte1_188[2] = parameter7_4byte188[2];    //예약                   
            parameter7_4byte1_188[3] = parameter7_4byte188[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_189[0] = parameter7_4byte189[2];
            parameter7_4byte1_189[1] = parameter7_4byte189[3];
            parameter7_4byte1_189[2] = parameter7_4byte189[0];
            parameter7_4byte1_189[3] = parameter7_4byte189[1];

            //0x4800 command
            parameter7_4byte1_190[0] = parameter7_4byte190[0];    //속도와 가속
            parameter7_4byte1_190[1] = parameter7_4byte190[1];    //커맨드 Code
            parameter7_4byte1_190[2] = parameter7_4byte190[2];    //예약                   
            parameter7_4byte1_190[3] = parameter7_4byte190[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_191[0] = parameter7_4byte191[2];
            parameter7_4byte1_191[1] = parameter7_4byte191[3];
            parameter7_4byte1_191[2] = parameter7_4byte191[0];
            parameter7_4byte1_191[3] = parameter7_4byte191[1];

            //0x4800 command
            parameter7_4byte1_192[0] = parameter7_4byte192[0];    //속도와 가속
            parameter7_4byte1_192[1] = parameter7_4byte192[1];    //커맨드 Code
            parameter7_4byte1_192[2] = parameter7_4byte192[2];    //예약                   
            parameter7_4byte1_192[3] = parameter7_4byte192[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_193[0] = parameter7_4byte193[2];
            parameter7_4byte1_193[1] = parameter7_4byte193[3];
            parameter7_4byte1_193[2] = parameter7_4byte193[0];
            parameter7_4byte1_193[3] = parameter7_4byte193[1];

            //0x4800 command
            parameter7_4byte1_194[0] = parameter7_4byte194[0];    //속도와 가속
            parameter7_4byte1_194[1] = parameter7_4byte194[1];    //커맨드 Code
            parameter7_4byte1_194[2] = parameter7_4byte194[2];    //예약                   
            parameter7_4byte1_194[3] = parameter7_4byte194[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_195[0] = parameter7_4byte195[2];
            parameter7_4byte1_195[1] = parameter7_4byte195[3];
            parameter7_4byte1_195[2] = parameter7_4byte195[0];
            parameter7_4byte1_195[3] = parameter7_4byte195[1];

            //0x4800 command
            parameter7_4byte1_196[0] = parameter7_4byte196[0];    //속도와 가속
            parameter7_4byte1_196[1] = parameter7_4byte196[1];    //커맨드 Code
            parameter7_4byte1_196[2] = parameter7_4byte196[2];    //예약                   
            parameter7_4byte1_196[3] = parameter7_4byte196[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_197[0] = parameter7_4byte197[2];
            parameter7_4byte1_197[1] = parameter7_4byte197[3];
            parameter7_4byte1_197[2] = parameter7_4byte197[0];
            parameter7_4byte1_197[3] = parameter7_4byte197[1];

            //0x4800 command
            parameter7_4byte1_198[0] = parameter7_4byte198[0];    //속도와 가속
            parameter7_4byte1_198[1] = parameter7_4byte198[1];    //커맨드 Code
            parameter7_4byte1_198[2] = parameter7_4byte198[2];    //예약                   
            parameter7_4byte1_198[3] = parameter7_4byte198[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_199[0] = parameter7_4byte199[2];
            parameter7_4byte1_199[1] = parameter7_4byte199[3];
            parameter7_4byte1_199[2] = parameter7_4byte199[0];
            parameter7_4byte1_199[3] = parameter7_4byte199[1];

            //0x4800 command
            parameter7_4byte1_200[0] = parameter7_4byte200[0];    //속도와 가속
            parameter7_4byte1_200[1] = parameter7_4byte200[1];    //커맨드 Code
            parameter7_4byte1_200[2] = parameter7_4byte200[2];    //예약                   
            parameter7_4byte1_200[3] = parameter7_4byte200[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_201[0] = parameter7_4byte201[2];
            parameter7_4byte1_201[1] = parameter7_4byte201[3];
            parameter7_4byte1_201[2] = parameter7_4byte201[0];
            parameter7_4byte1_201[3] = parameter7_4byte201[1];

            //0x4800 command
            parameter7_4byte1_202[0] = parameter7_4byte202[0];    //속도와 가속
            parameter7_4byte1_202[1] = parameter7_4byte202[1];    //커맨드 Code
            parameter7_4byte1_202[2] = parameter7_4byte202[2];    //예약                   
            parameter7_4byte1_202[3] = parameter7_4byte202[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_203[0] = parameter7_4byte203[2];
            parameter7_4byte1_203[1] = parameter7_4byte203[3];
            parameter7_4byte1_203[2] = parameter7_4byte203[0];
            parameter7_4byte1_203[3] = parameter7_4byte203[1];

            //0x4800 command
            parameter7_4byte1_204[0] = parameter7_4byte204[0];    //속도와 가속
            parameter7_4byte1_204[1] = parameter7_4byte204[1];    //커맨드 Code
            parameter7_4byte1_204[2] = parameter7_4byte204[2];    //예약                   
            parameter7_4byte1_204[3] = parameter7_4byte204[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_205[0] = parameter7_4byte205[2];
            parameter7_4byte1_205[1] = parameter7_4byte205[3];
            parameter7_4byte1_205[2] = parameter7_4byte205[0];
            parameter7_4byte1_205[3] = parameter7_4byte205[1];

            //0x4800 command
            parameter7_4byte1_206[0] = parameter7_4byte206[0];    //속도와 가속
            parameter7_4byte1_206[1] = parameter7_4byte206[1];    //커맨드 Code
            parameter7_4byte1_206[2] = parameter7_4byte206[2];    //예약                   
            parameter7_4byte1_206[3] = parameter7_4byte206[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_207[0] = parameter7_4byte207[2];
            parameter7_4byte1_207[1] = parameter7_4byte207[3];
            parameter7_4byte1_207[2] = parameter7_4byte207[0];
            parameter7_4byte1_207[3] = parameter7_4byte207[1];

            //0x4800 command
            parameter7_4byte1_208[0] = parameter7_4byte208[0];    //속도와 가속
            parameter7_4byte1_208[1] = parameter7_4byte208[1];    //커맨드 Code
            parameter7_4byte1_208[2] = parameter7_4byte208[2];    //예약                   
            parameter7_4byte1_208[3] = parameter7_4byte208[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_209[0] = parameter7_4byte209[2];
            parameter7_4byte1_209[1] = parameter7_4byte209[3];
            parameter7_4byte1_209[2] = parameter7_4byte209[0];
            parameter7_4byte1_209[3] = parameter7_4byte209[1];








            //0x4800 command
            parameter7_4byte1_210[0] = parameter7_4byte210[0];    //속도와 가속
            parameter7_4byte1_210[1] = parameter7_4byte210[1];    //커맨드 Code
            parameter7_4byte1_210[2] = parameter7_4byte210[2];    //예약                   
            parameter7_4byte1_210[3] = parameter7_4byte210[3];    //감속, 방향, 천이 조건

            


            //0x4802 data
            parameter7_4byte1_211[0] = parameter7_4byte211[2];
            parameter7_4byte1_211[1] = parameter7_4byte211[3];
            parameter7_4byte1_211[2] = parameter7_4byte211[0];
            parameter7_4byte1_211[3] = parameter7_4byte211[1];

            //0x4800 command
            parameter7_4byte1_212[0] = parameter7_4byte212[0];    //속도와 가속
            parameter7_4byte1_212[1] = parameter7_4byte212[1];    //커맨드 Code
            parameter7_4byte1_212[2] = parameter7_4byte212[2];    //예약                   
            parameter7_4byte1_212[3] = parameter7_4byte212[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_213[0] = parameter7_4byte213[2];
            parameter7_4byte1_213[1] = parameter7_4byte213[3];
            parameter7_4byte1_213[2] = parameter7_4byte213[0];
            parameter7_4byte1_213[3] = parameter7_4byte213[1];

            //0x4800 command
            parameter7_4byte1_214[0] = parameter7_4byte214[0];    //속도와 가속
            parameter7_4byte1_214[1] = parameter7_4byte214[1];    //커맨드 Code
            parameter7_4byte1_214[2] = parameter7_4byte214[2];    //예약                   
            parameter7_4byte1_214[3] = parameter7_4byte214[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_215[0] = parameter7_4byte215[2];
            parameter7_4byte1_215[1] = parameter7_4byte215[3];
            parameter7_4byte1_215[2] = parameter7_4byte215[0];
            parameter7_4byte1_215[3] = parameter7_4byte215[1];

            //0x4800 command
            parameter7_4byte1_216[0] = parameter7_4byte216[0];    //속도와 가속
            parameter7_4byte1_216[1] = parameter7_4byte216[1];    //커맨드 Code
            parameter7_4byte1_216[2] = parameter7_4byte216[2];    //예약                   
            parameter7_4byte1_216[3] = parameter7_4byte216[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_217[0] = parameter7_4byte217[2];
            parameter7_4byte1_217[1] = parameter7_4byte217[3];
            parameter7_4byte1_217[2] = parameter7_4byte217[0];
            parameter7_4byte1_217[3] = parameter7_4byte217[1];

            //0x4800 command
            parameter7_4byte1_218[0] = parameter7_4byte218[0];    //속도와 가속
            parameter7_4byte1_218[1] = parameter7_4byte218[1];    //커맨드 Code
            parameter7_4byte1_218[2] = parameter7_4byte218[2];    //예약                   
            parameter7_4byte1_218[3] = parameter7_4byte218[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_219[0] = parameter7_4byte219[2];
            parameter7_4byte1_219[1] = parameter7_4byte219[3];
            parameter7_4byte1_219[2] = parameter7_4byte219[0];
            parameter7_4byte1_219[3] = parameter7_4byte219[1];

            //0x4800 command
            parameter7_4byte1_220[0] = parameter7_4byte220[0];    //속도와 가속
            parameter7_4byte1_220[1] = parameter7_4byte220[1];    //커맨드 Code
            parameter7_4byte1_220[2] = parameter7_4byte220[2];    //예약                   
            parameter7_4byte1_220[3] = parameter7_4byte220[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_221[0] = parameter7_4byte221[2];
            parameter7_4byte1_221[1] = parameter7_4byte221[3];
            parameter7_4byte1_221[2] = parameter7_4byte221[0];
            parameter7_4byte1_221[3] = parameter7_4byte221[1];

            //0x4800 command
            parameter7_4byte1_222[0] = parameter7_4byte222[0];    //속도와 가속
            parameter7_4byte1_222[1] = parameter7_4byte222[1];    //커맨드 Code
            parameter7_4byte1_222[2] = parameter7_4byte222[2];    //예약                   
            parameter7_4byte1_222[3] = parameter7_4byte222[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_223[0] = parameter7_4byte223[2];
            parameter7_4byte1_223[1] = parameter7_4byte223[3];
            parameter7_4byte1_223[2] = parameter7_4byte223[0];
            parameter7_4byte1_223[3] = parameter7_4byte223[1];

            //0x4800 command
            parameter7_4byte1_224[0] = parameter7_4byte224[0];    //속도와 가속
            parameter7_4byte1_224[1] = parameter7_4byte224[1];    //커맨드 Code
            parameter7_4byte1_224[2] = parameter7_4byte224[2];    //예약                   
            parameter7_4byte1_224[3] = parameter7_4byte224[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_225[0] = parameter7_4byte225[2];
            parameter7_4byte1_225[1] = parameter7_4byte225[3];
            parameter7_4byte1_225[2] = parameter7_4byte225[0];
            parameter7_4byte1_225[3] = parameter7_4byte225[1];

            //0x4800 command
            parameter7_4byte1_226[0] = parameter7_4byte226[0];    //속도와 가속
            parameter7_4byte1_226[1] = parameter7_4byte226[1];    //커맨드 Code
            parameter7_4byte1_226[2] = parameter7_4byte226[2];    //예약                   
            parameter7_4byte1_226[3] = parameter7_4byte226[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_227[0] = parameter7_4byte227[2];
            parameter7_4byte1_227[1] = parameter7_4byte227[3];
            parameter7_4byte1_227[2] = parameter7_4byte227[0];
            parameter7_4byte1_227[3] = parameter7_4byte227[1];


            //0x4800 command
            parameter7_4byte1_228[0] = parameter7_4byte228[0];    //속도와 가속
            parameter7_4byte1_228[1] = parameter7_4byte228[1];    //커맨드 Code
            parameter7_4byte1_228[2] = parameter7_4byte228[2];    //예약                   
            parameter7_4byte1_228[3] = parameter7_4byte228[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_229[0] = parameter7_4byte229[2];
            parameter7_4byte1_229[1] = parameter7_4byte229[3];
            parameter7_4byte1_229[2] = parameter7_4byte229[0];
            parameter7_4byte1_229[3] = parameter7_4byte229[1];

            //0x4800 command
            parameter7_4byte1_230[0] = parameter7_4byte230[0];    //속도와 가속
            parameter7_4byte1_230[1] = parameter7_4byte230[1];    //커맨드 Code
            parameter7_4byte1_230[2] = parameter7_4byte230[2];    //예약                   
            parameter7_4byte1_230[3] = parameter7_4byte230[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_231[0] = parameter7_4byte231[2];
            parameter7_4byte1_231[1] = parameter7_4byte231[3];
            parameter7_4byte1_231[2] = parameter7_4byte231[0];
            parameter7_4byte1_231[3] = parameter7_4byte231[1];

            //0x4800 command
            parameter7_4byte1_232[0] = parameter7_4byte232[0];    //속도와 가속
            parameter7_4byte1_232[1] = parameter7_4byte232[1];    //커맨드 Code
            parameter7_4byte1_232[2] = parameter7_4byte232[2];    //예약                   
            parameter7_4byte1_232[3] = parameter7_4byte232[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_233[0] = parameter7_4byte233[2];
            parameter7_4byte1_233[1] = parameter7_4byte233[3];
            parameter7_4byte1_233[2] = parameter7_4byte233[0];
            parameter7_4byte1_233[3] = parameter7_4byte233[1];

            //0x4800 command
            parameter7_4byte1_234[0] = parameter7_4byte234[0];    //속도와 가속
            parameter7_4byte1_234[1] = parameter7_4byte234[1];    //커맨드 Code
            parameter7_4byte1_234[2] = parameter7_4byte234[2];    //예약                   
            parameter7_4byte1_234[3] = parameter7_4byte234[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_235[0] = parameter7_4byte235[2];
            parameter7_4byte1_235[1] = parameter7_4byte235[3];
            parameter7_4byte1_235[2] = parameter7_4byte235[0];
            parameter7_4byte1_235[3] = parameter7_4byte235[1];

            //0x4800 command
            parameter7_4byte1_236[0] = parameter7_4byte236[0];    //속도와 가속
            parameter7_4byte1_236[1] = parameter7_4byte236[1];    //커맨드 Code
            parameter7_4byte1_236[2] = parameter7_4byte236[2];    //예약                   
            parameter7_4byte1_236[3] = parameter7_4byte236[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_237[0] = parameter7_4byte237[2];
            parameter7_4byte1_237[1] = parameter7_4byte237[3];
            parameter7_4byte1_237[2] = parameter7_4byte237[0];
            parameter7_4byte1_237[3] = parameter7_4byte237[1];

            //0x4800 command
            parameter7_4byte1_238[0] = parameter7_4byte238[0];    //속도와 가속
            parameter7_4byte1_238[1] = parameter7_4byte238[1];    //커맨드 Code
            parameter7_4byte1_238[2] = parameter7_4byte238[2];    //예약                   
            parameter7_4byte1_238[3] = parameter7_4byte238[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_239[0] = parameter7_4byte239[2];
            parameter7_4byte1_239[1] = parameter7_4byte239[3];
            parameter7_4byte1_239[2] = parameter7_4byte239[0];
            parameter7_4byte1_239[3] = parameter7_4byte239[1];

            //0x4800 command
            parameter7_4byte1_240[0] = parameter7_4byte240[0];    //속도와 가속
            parameter7_4byte1_240[1] = parameter7_4byte240[1];    //커맨드 Code
            parameter7_4byte1_240[2] = parameter7_4byte240[2];    //예약                   
            parameter7_4byte1_240[3] = parameter7_4byte240[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_241[0] = parameter7_4byte241[2];
            parameter7_4byte1_241[1] = parameter7_4byte241[3];
            parameter7_4byte1_241[2] = parameter7_4byte241[0];
            parameter7_4byte1_241[3] = parameter7_4byte241[1];

            //0x4800 command
            parameter7_4byte1_242[0] = parameter7_4byte242[0];    //속도와 가속
            parameter7_4byte1_242[1] = parameter7_4byte242[1];    //커맨드 Code
            parameter7_4byte1_242[2] = parameter7_4byte242[2];    //예약                   
            parameter7_4byte1_242[3] = parameter7_4byte242[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_243[0] = parameter7_4byte243[2];
            parameter7_4byte1_243[1] = parameter7_4byte243[3];
            parameter7_4byte1_243[2] = parameter7_4byte243[0];
            parameter7_4byte1_243[3] = parameter7_4byte243[1];

            //0x4800 command
            parameter7_4byte1_244[0] = parameter7_4byte244[0];    //속도와 가속
            parameter7_4byte1_244[1] = parameter7_4byte244[1];    //커맨드 Code
            parameter7_4byte1_244[2] = parameter7_4byte244[2];    //예약                   
            parameter7_4byte1_244[3] = parameter7_4byte244[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_245[0] = parameter7_4byte245[2];
            parameter7_4byte1_245[1] = parameter7_4byte245[3];
            parameter7_4byte1_245[2] = parameter7_4byte245[0];
            parameter7_4byte1_245[3] = parameter7_4byte245[1];

            //0x4800 command
            parameter7_4byte1_246[0] = parameter7_4byte246[0];    //속도와 가속
            parameter7_4byte1_246[1] = parameter7_4byte246[1];    //커맨드 Code
            parameter7_4byte1_246[2] = parameter7_4byte246[2];    //예약                   
            parameter7_4byte1_246[3] = parameter7_4byte246[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_247[0] = parameter7_4byte247[2];
            parameter7_4byte1_247[1] = parameter7_4byte247[3];
            parameter7_4byte1_247[2] = parameter7_4byte247[0];
            parameter7_4byte1_247[3] = parameter7_4byte247[1];

            //0x4800 command
            parameter7_4byte1_248[0] = parameter7_4byte248[0];    //속도와 가속
            parameter7_4byte1_248[1] = parameter7_4byte248[1];    //커맨드 Code
            parameter7_4byte1_248[2] = parameter7_4byte248[2];    //예약                   
            parameter7_4byte1_248[3] = parameter7_4byte248[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_249[0] = parameter7_4byte249[2];
            parameter7_4byte1_249[1] = parameter7_4byte249[3];
            parameter7_4byte1_249[2] = parameter7_4byte249[0];
            parameter7_4byte1_249[3] = parameter7_4byte249[1];

            //0x4800 command
            parameter7_4byte1_250[0] = parameter7_4byte250[0];    //속도와 가속
            parameter7_4byte1_250[1] = parameter7_4byte250[1];    //커맨드 Code
            parameter7_4byte1_250[2] = parameter7_4byte250[2];    //예약                   
            parameter7_4byte1_250[3] = parameter7_4byte250[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_251[0] = parameter7_4byte251[2];
            parameter7_4byte1_251[1] = parameter7_4byte251[3];
            parameter7_4byte1_251[2] = parameter7_4byte251[0];
            parameter7_4byte1_251[3] = parameter7_4byte251[1];

            //0x4800 command
            parameter7_4byte1_252[0] = parameter7_4byte252[0];    //속도와 가속
            parameter7_4byte1_252[1] = parameter7_4byte252[1];    //커맨드 Code
            parameter7_4byte1_252[2] = parameter7_4byte252[2];    //예약                   
            parameter7_4byte1_252[3] = parameter7_4byte252[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_253[0] = parameter7_4byte253[2];
            parameter7_4byte1_253[1] = parameter7_4byte253[3];
            parameter7_4byte1_253[2] = parameter7_4byte253[0];
            parameter7_4byte1_253[3] = parameter7_4byte253[1];

            //0x4800 command
            parameter7_4byte1_254[0] = parameter7_4byte254[0];    //속도와 가속
            parameter7_4byte1_254[1] = parameter7_4byte254[1];    //커맨드 Code
            parameter7_4byte1_254[2] = parameter7_4byte254[2];    //예약                   
            parameter7_4byte1_254[3] = parameter7_4byte254[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_255[0] = parameter7_4byte255[2];
            parameter7_4byte1_255[1] = parameter7_4byte255[3];
            parameter7_4byte1_255[2] = parameter7_4byte255[0];
            parameter7_4byte1_255[3] = parameter7_4byte255[1];

            //0x4800 command
            parameter7_4byte1_256[0] = parameter7_4byte256[0];    //속도와 가속
            parameter7_4byte1_256[1] = parameter7_4byte256[1];    //커맨드 Code
            parameter7_4byte1_256[2] = parameter7_4byte256[2];    //예약                   
            parameter7_4byte1_256[3] = parameter7_4byte256[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_257[0] = parameter7_4byte257[2];
            parameter7_4byte1_257[1] = parameter7_4byte257[3];
            parameter7_4byte1_257[2] = parameter7_4byte257[0];
            parameter7_4byte1_257[3] = parameter7_4byte257[1];

            //0x4800 command
            parameter7_4byte1_258[0] = parameter7_4byte258[0];    //속도와 가속
            parameter7_4byte1_258[1] = parameter7_4byte258[1];    //커맨드 Code
            parameter7_4byte1_258[2] = parameter7_4byte258[2];    //예약                   
            parameter7_4byte1_258[3] = parameter7_4byte258[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_259[0] = parameter7_4byte259[2];
            parameter7_4byte1_259[1] = parameter7_4byte259[3];
            parameter7_4byte1_259[2] = parameter7_4byte259[0];
            parameter7_4byte1_259[3] = parameter7_4byte259[1];

            //0x4800 command
            parameter7_4byte1_260[0] = parameter7_4byte260[0];    //속도와 가속
            parameter7_4byte1_260[1] = parameter7_4byte260[1];    //커맨드 Code
            parameter7_4byte1_260[2] = parameter7_4byte260[2];    //예약                   
            parameter7_4byte1_260[3] = parameter7_4byte260[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_261[0] = parameter7_4byte261[2];
            parameter7_4byte1_261[1] = parameter7_4byte261[3];
            parameter7_4byte1_261[2] = parameter7_4byte261[0];
            parameter7_4byte1_261[3] = parameter7_4byte261[1];

            //0x4800 command
            parameter7_4byte1_262[0] = parameter7_4byte262[0];    //속도와 가속
            parameter7_4byte1_262[1] = parameter7_4byte262[1];    //커맨드 Code
            parameter7_4byte1_262[2] = parameter7_4byte262[2];    //예약                   
            parameter7_4byte1_262[3] = parameter7_4byte262[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_263[0] = parameter7_4byte263[2];
            parameter7_4byte1_263[1] = parameter7_4byte263[3];
            parameter7_4byte1_263[2] = parameter7_4byte263[0];
            parameter7_4byte1_263[3] = parameter7_4byte263[1];

            //0x4800 command
            parameter7_4byte1_264[0] = parameter7_4byte264[0];    //속도와 가속
            parameter7_4byte1_264[1] = parameter7_4byte264[1];    //커맨드 Code
            parameter7_4byte1_264[2] = parameter7_4byte264[2];    //예약                   
            parameter7_4byte1_264[3] = parameter7_4byte264[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_265[0] = parameter7_4byte265[2];
            parameter7_4byte1_265[1] = parameter7_4byte265[3];
            parameter7_4byte1_265[2] = parameter7_4byte265[0];
            parameter7_4byte1_265[3] = parameter7_4byte265[1];

            //0x4800 command
            parameter7_4byte1_266[0] = parameter7_4byte266[0];    //속도와 가속
            parameter7_4byte1_266[1] = parameter7_4byte266[1];    //커맨드 Code
            parameter7_4byte1_266[2] = parameter7_4byte266[2];    //예약                   
            parameter7_4byte1_266[3] = parameter7_4byte266[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_267[0] = parameter7_4byte267[2];
            parameter7_4byte1_267[1] = parameter7_4byte267[3];
            parameter7_4byte1_267[2] = parameter7_4byte267[0];
            parameter7_4byte1_267[3] = parameter7_4byte267[1];

            //0x4800 command
            parameter7_4byte1_268[0] = parameter7_4byte268[0];    //속도와 가속
            parameter7_4byte1_268[1] = parameter7_4byte268[1];    //커맨드 Code
            parameter7_4byte1_268[2] = parameter7_4byte268[2];    //예약                   
            parameter7_4byte1_268[3] = parameter7_4byte268[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_268[0] = parameter7_4byte269[2];
            parameter7_4byte1_268[1] = parameter7_4byte269[3];
            parameter7_4byte1_268[2] = parameter7_4byte269[0];
            parameter7_4byte1_268[3] = parameter7_4byte269[1];

            //0x4800 command
            parameter7_4byte1_270[0] = parameter7_4byte270[0];    //속도와 가속
            parameter7_4byte1_270[1] = parameter7_4byte270[1];    //커맨드 Code
            parameter7_4byte1_270[2] = parameter7_4byte270[2];    //예약                   
            parameter7_4byte1_270[3] = parameter7_4byte270[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_271[0] = parameter7_4byte271[2];
            parameter7_4byte1_271[1] = parameter7_4byte271[3];
            parameter7_4byte1_271[2] = parameter7_4byte271[0];
            parameter7_4byte1_271[3] = parameter7_4byte271[1];

            //0x4800 command
            parameter7_4byte1_272[0] = parameter7_4byte272[0];    //속도와 가속
            parameter7_4byte1_272[1] = parameter7_4byte272[1];    //커맨드 Code
            parameter7_4byte1_272[2] = parameter7_4byte272[2];    //예약                   
            parameter7_4byte1_272[3] = parameter7_4byte272[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_273[0] = parameter7_4byte273[2];
            parameter7_4byte1_273[1] = parameter7_4byte273[3];
            parameter7_4byte1_273[2] = parameter7_4byte273[0];
            parameter7_4byte1_273[3] = parameter7_4byte273[1];

            //0x4800 command
            parameter7_4byte1_274[0] = parameter7_4byte274[0];    //속도와 가속
            parameter7_4byte1_274[1] = parameter7_4byte274[1];    //커맨드 Code
            parameter7_4byte1_274[2] = parameter7_4byte274[2];    //예약                   
            parameter7_4byte1_274[3] = parameter7_4byte274[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_275[0] = parameter7_4byte275[2];
            parameter7_4byte1_275[1] = parameter7_4byte275[3];
            parameter7_4byte1_275[2] = parameter7_4byte275[0];
            parameter7_4byte1_275[3] = parameter7_4byte275[1];

            //0x4800 command
            parameter7_4byte1_276[0] = parameter7_4byte276[0];    //속도와 가속
            parameter7_4byte1_276[1] = parameter7_4byte276[1];    //커맨드 Code
            parameter7_4byte1_276[2] = parameter7_4byte276[2];    //예약                   
            parameter7_4byte1_276[3] = parameter7_4byte276[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_277[0] = parameter7_4byte277[2];
            parameter7_4byte1_277[1] = parameter7_4byte277[3];
            parameter7_4byte1_277[2] = parameter7_4byte277[0];
            parameter7_4byte1_277[3] = parameter7_4byte277[1];

            //0x4800 command
            parameter7_4byte1_278[0] = parameter7_4byte278[0];    //속도와 가속
            parameter7_4byte1_278[1] = parameter7_4byte278[1];    //커맨드 Code
            parameter7_4byte1_278[2] = parameter7_4byte278[2];    //예약                   
            parameter7_4byte1_278[3] = parameter7_4byte278[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_279[0] = parameter7_4byte279[2];
            parameter7_4byte1_279[1] = parameter7_4byte279[3];
            parameter7_4byte1_279[2] = parameter7_4byte279[0];
            parameter7_4byte1_279[3] = parameter7_4byte279[1];

            //0x4800 command
            parameter7_4byte1_280[0] = parameter7_4byte280[0];    //속도와 가속
            parameter7_4byte1_280[1] = parameter7_4byte280[1];    //커맨드 Code
            parameter7_4byte1_280[2] = parameter7_4byte280[2];    //예약                   
            parameter7_4byte1_280[3] = parameter7_4byte280[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_281[0] = parameter7_4byte281[2];
            parameter7_4byte1_281[1] = parameter7_4byte281[3];
            parameter7_4byte1_281[2] = parameter7_4byte281[0];
            parameter7_4byte1_281[3] = parameter7_4byte281[1];

            //0x4800 command
            parameter7_4byte1_282[0] = parameter7_4byte282[0];    //속도와 가속
            parameter7_4byte1_282[1] = parameter7_4byte282[1];    //커맨드 Code
            parameter7_4byte1_282[2] = parameter7_4byte282[2];    //예약                   
            parameter7_4byte1_282[3] = parameter7_4byte282[3];    //감속, 방향, 천이 조건

            //0x4802 
            parameter7_4byte1_283[0] = parameter7_4byte283[2];
            parameter7_4byte1_283[1] = parameter7_4byte283[3];
            parameter7_4byte1_283[2] = parameter7_4byte283[0];
            parameter7_4byte1_283[3] = parameter7_4byte283[1];
       

            //0x4802 data
            parameter7_4byte1_284[0] = parameter7_4byte284[2];
            parameter7_4byte1_284[1] = parameter7_4byte284[3];
            parameter7_4byte1_284[2] = parameter7_4byte284[0];
            parameter7_4byte1_284[3] = parameter7_4byte284[1];

            //0x4800 command
            parameter7_4byte1_285[0] = parameter7_4byte285[0];    //속도와 가속
            parameter7_4byte1_285[1] = parameter7_4byte285[1];    //커맨드 Code
            parameter7_4byte1_285[2] = parameter7_4byte285[2];    //예약                   
            parameter7_4byte1_285[3] = parameter7_4byte285[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_286[0] = parameter7_4byte286[2];
            parameter7_4byte1_286[1] = parameter7_4byte286[3];
            parameter7_4byte1_286[2] = parameter7_4byte286[0];
            parameter7_4byte1_286[3] = parameter7_4byte286[1];

            //0x4800 command
            parameter7_4byte1_287[0] = parameter7_4byte287[0];    //속도와 가속
            parameter7_4byte1_287[1] = parameter7_4byte287[1];    //커맨드 Code
            parameter7_4byte1_287[2] = parameter7_4byte287[2];    //예약                   
            parameter7_4byte1_287[3] = parameter7_4byte287[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_288[0] = parameter7_4byte288[2];
            parameter7_4byte1_288[1] = parameter7_4byte288[3];
            parameter7_4byte1_288[2] = parameter7_4byte288[0];
            parameter7_4byte1_288[3] = parameter7_4byte288[1];

            //0x4800 command
            parameter7_4byte1_289[0] = parameter7_4byte289[0];    //속도와 가속
            parameter7_4byte1_289[1] = parameter7_4byte289[1];    //커맨드 Code
            parameter7_4byte1_289[2] = parameter7_4byte289[2];    //예약                   
            parameter7_4byte1_289[3] = parameter7_4byte289[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_290[0] = parameter7_4byte290[2];
            parameter7_4byte1_290[1] = parameter7_4byte290[3];
            parameter7_4byte1_290[2] = parameter7_4byte290[0];
            parameter7_4byte1_290[3] = parameter7_4byte290[1];

            //0x4800 command
            parameter7_4byte1_291[0] = parameter7_4byte291[0];    //속도와 가속
            parameter7_4byte1_291[1] = parameter7_4byte291[1];    //커맨드 Code
            parameter7_4byte1_291[2] = parameter7_4byte291[2];    //예약                   
            parameter7_4byte1_291[3] = parameter7_4byte291[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_292[0] = parameter7_4byte292[2];
            parameter7_4byte1_292[1] = parameter7_4byte292[3];
            parameter7_4byte1_292[2] = parameter7_4byte292[0];
            parameter7_4byte1_292[3] = parameter7_4byte292[1];

            //0x4800 command
            parameter7_4byte1_293[0] = parameter7_4byte293[0];    //속도와 가속
            parameter7_4byte1_293[1] = parameter7_4byte293[1];    //커맨드 Code
            parameter7_4byte1_293[2] = parameter7_4byte293[2];    //예약                   
            parameter7_4byte1_293[3] = parameter7_4byte293[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_294[0] = parameter7_4byte294[2];
            parameter7_4byte1_294[1] = parameter7_4byte294[3];
            parameter7_4byte1_294[2] = parameter7_4byte294[0];
            parameter7_4byte1_294[3] = parameter7_4byte294[1];

            //0x4800 command
            parameter7_4byte1_295[0] = parameter7_4byte295[0];    //속도와 가속
            parameter7_4byte1_295[1] = parameter7_4byte295[1];    //커맨드 Code
            parameter7_4byte1_295[2] = parameter7_4byte295[2];    //예약                   
            parameter7_4byte1_295[3] = parameter7_4byte295[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_296[0] = parameter7_4byte296[2];
            parameter7_4byte1_296[1] = parameter7_4byte296[3];
            parameter7_4byte1_296[2] = parameter7_4byte296[0];
            parameter7_4byte1_296[3] = parameter7_4byte296[1];

            //0x4800 command
            parameter7_4byte1_297[0] = parameter7_4byte297[0];    //속도와 가속
            parameter7_4byte1_297[1] = parameter7_4byte297[1];    //커맨드 Code
            parameter7_4byte1_297[2] = parameter7_4byte297[2];    //예약                   
            parameter7_4byte1_297[3] = parameter7_4byte297[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_298[0] = parameter7_4byte298[2];
            parameter7_4byte1_298[1] = parameter7_4byte298[3];
            parameter7_4byte1_298[2] = parameter7_4byte298[0];
            parameter7_4byte1_298[3] = parameter7_4byte298[1];

            //0x4800 command
            parameter7_4byte1_299[0] = parameter7_4byte299[0];    //속도와 가속
            parameter7_4byte1_299[1] = parameter7_4byte299[1];    //커맨드 Code
            parameter7_4byte1_299[2] = parameter7_4byte299[2];    //예약                   
            parameter7_4byte1_299[3] = parameter7_4byte299[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_300[0] = parameter7_4byte300[2];
            parameter7_4byte1_300[1] = parameter7_4byte300[3];
            parameter7_4byte1_300[2] = parameter7_4byte300[0];
            parameter7_4byte1_300[3] = parameter7_4byte300[1];

            //0x4800 command
            parameter7_4byte1_301[0] = parameter7_4byte301[0];    //속도와 가속
            parameter7_4byte1_301[1] = parameter7_4byte301[1];    //커맨드 Code
            parameter7_4byte1_301[2] = parameter7_4byte301[2];    //예약                   
            parameter7_4byte1_301[3] = parameter7_4byte301[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_302[0] = parameter7_4byte302[2];
            parameter7_4byte1_302[1] = parameter7_4byte302[3];
            parameter7_4byte1_302[2] = parameter7_4byte302[0];
            parameter7_4byte1_302[3] = parameter7_4byte302[1];

            //0x4800 command
            parameter7_4byte1_303[0] = parameter7_4byte303[0];    //속도와 가속
            parameter7_4byte1_303[1] = parameter7_4byte303[1];    //커맨드 Code
            parameter7_4byte1_303[2] = parameter7_4byte303[2];    //예약                   
            parameter7_4byte1_303[3] = parameter7_4byte303[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_304[0] = parameter7_4byte304[2];
            parameter7_4byte1_304[1] = parameter7_4byte304[3];
            parameter7_4byte1_304[2] = parameter7_4byte304[0];
            parameter7_4byte1_304[3] = parameter7_4byte304[1];

            //0x4800 command
            parameter7_4byte1_305[0] = parameter7_4byte305[0];    //속도와 가속
            parameter7_4byte1_305[1] = parameter7_4byte305[1];    //커맨드 Code
            parameter7_4byte1_305[2] = parameter7_4byte305[2];    //예약                   
            parameter7_4byte1_305[3] = parameter7_4byte305[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_306[0] = parameter7_4byte306[2];
            parameter7_4byte1_306[1] = parameter7_4byte306[3];
            parameter7_4byte1_306[2] = parameter7_4byte306[0];
            parameter7_4byte1_306[3] = parameter7_4byte306[1];

            //0x4800 command
            parameter7_4byte1_307[0] = parameter7_4byte307[0];    //속도와 가속
            parameter7_4byte1_307[1] = parameter7_4byte307[1];    //커맨드 Code
            parameter7_4byte1_307[2] = parameter7_4byte307[2];    //예약                   
            parameter7_4byte1_307[3] = parameter7_4byte307[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_308[0] = parameter7_4byte308[2];
            parameter7_4byte1_308[1] = parameter7_4byte308[3];
            parameter7_4byte1_308[2] = parameter7_4byte308[0];
            parameter7_4byte1_308[3] = parameter7_4byte308[1];

            //0x4800 command
            parameter7_4byte1_309[0] = parameter7_4byte309[0];    //속도와 가속
            parameter7_4byte1_309[1] = parameter7_4byte309[1];    //커맨드 Code
            parameter7_4byte1_309[2] = parameter7_4byte309[2];    //예약                   
            parameter7_4byte1_309[3] = parameter7_4byte309[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_310[0] = parameter7_4byte310[2];
            parameter7_4byte1_310[1] = parameter7_4byte310[3];
            parameter7_4byte1_310[2] = parameter7_4byte310[0];
            parameter7_4byte1_310[3] = parameter7_4byte310[1];

            //0x4800 command
            parameter7_4byte1_311[0] = parameter7_4byte311[0];    //속도와 가속
            parameter7_4byte1_311[1] = parameter7_4byte311[1];    //커맨드 Code
            parameter7_4byte1_311[2] = parameter7_4byte311[2];    //예약                   
            parameter7_4byte1_311[3] = parameter7_4byte311[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_312[0] = parameter7_4byte312[2];
            parameter7_4byte1_312[1] = parameter7_4byte312[3];
            parameter7_4byte1_312[2] = parameter7_4byte312[0];
            parameter7_4byte1_312[3] = parameter7_4byte312[1];
            //0x4800 command
            parameter7_4byte1_313[0] = parameter7_4byte313[0];    //속도와 가속
            parameter7_4byte1_313[1] = parameter7_4byte313[1];    //커맨드 Code
            parameter7_4byte1_313[2] = parameter7_4byte313[2];    //예약                   
            parameter7_4byte1_313[3] = parameter7_4byte313[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_314[0] = parameter7_4byte314[2];
            parameter7_4byte1_314[1] = parameter7_4byte314[3];
            parameter7_4byte1_314[2] = parameter7_4byte314[0];
            parameter7_4byte1_314[3] = parameter7_4byte314[1];

            //0x4800 command
            parameter7_4byte1_315[0] = parameter7_4byte315[0];    //속도와 가속
            parameter7_4byte1_315[1] = parameter7_4byte315[1];    //커맨드 Code
            parameter7_4byte1_315[2] = parameter7_4byte315[2];    //예약                   
            parameter7_4byte1_315[3] = parameter7_4byte315[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_316[0] = parameter7_4byte316[2];
            parameter7_4byte1_316[1] = parameter7_4byte316[3];
            parameter7_4byte1_316[2] = parameter7_4byte316[0];
            parameter7_4byte1_316[3] = parameter7_4byte316[1];

            //0x4800 command
            parameter7_4byte1_317[0] = parameter7_4byte317[0];    //속도와 가속
            parameter7_4byte1_317[1] = parameter7_4byte317[1];    //커맨드 Code
            parameter7_4byte1_317[2] = parameter7_4byte317[2];    //예약                   
            parameter7_4byte1_317[3] = parameter7_4byte317[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_318[0] = parameter7_4byte318[2];
            parameter7_4byte1_318[1] = parameter7_4byte318[3];
            parameter7_4byte1_318[2] = parameter7_4byte318[0];
            parameter7_4byte1_318[3] = parameter7_4byte318[1];

            //0x4800 command
            parameter7_4byte1_319[0] = parameter7_4byte319[0];    //속도와 가속
            parameter7_4byte1_319[1] = parameter7_4byte319[1];    //커맨드 Code
            parameter7_4byte1_319[2] = parameter7_4byte319[2];    //예약                   
            parameter7_4byte1_319[3] = parameter7_4byte319[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_320[0] = parameter7_4byte320[2];
            parameter7_4byte1_320[1] = parameter7_4byte320[3];
            parameter7_4byte1_320[2] = parameter7_4byte320[0];
            parameter7_4byte1_320[3] = parameter7_4byte320[1];

            //0x4800 command
            parameter7_4byte1_321[0] = parameter7_4byte321[0];    //속도와 가속
            parameter7_4byte1_321[1] = parameter7_4byte321[1];    //커맨드 Code
            parameter7_4byte1_321[2] = parameter7_4byte321[2];    //예약                   
            parameter7_4byte1_321[3] = parameter7_4byte321[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_322[0] = parameter7_4byte322[2];
            parameter7_4byte1_322[1] = parameter7_4byte322[3];
            parameter7_4byte1_322[2] = parameter7_4byte322[0];
            parameter7_4byte1_322[3] = parameter7_4byte322[1];

            //0x4800 command
            parameter7_4byte1_323[0] = parameter7_4byte323[0];    //속도와 가속
            parameter7_4byte1_323[1] = parameter7_4byte323[1];    //커맨드 Code
            parameter7_4byte1_323[2] = parameter7_4byte323[2];    //예약                   
            parameter7_4byte1_323[3] = parameter7_4byte323[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_324[0] = parameter7_4byte324[2];
            parameter7_4byte1_324[1] = parameter7_4byte324[3];
            parameter7_4byte1_324[2] = parameter7_4byte324[0];
            parameter7_4byte1_324[3] = parameter7_4byte324[1];

            //0x4800 command
            parameter7_4byte1_325[0] = parameter7_4byte325[0];    //속도와 가속
            parameter7_4byte1_325[1] = parameter7_4byte325[1];    //커맨드 Code
            parameter7_4byte1_325[2] = parameter7_4byte325[2];    //예약                   
            parameter7_4byte1_325[3] = parameter7_4byte325[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_326[0] = parameter7_4byte326[2];
            parameter7_4byte1_326[1] = parameter7_4byte326[3];
            parameter7_4byte1_326[2] = parameter7_4byte326[0];
            parameter7_4byte1_326[3] = parameter7_4byte326[1];

            //0x4800 command
            parameter7_4byte1_327[0] = parameter7_4byte327[0];    //속도와 가속
            parameter7_4byte1_327[1] = parameter7_4byte327[1];    //커맨드 Code
            parameter7_4byte1_327[2] = parameter7_4byte327[2];    //예약                   
            parameter7_4byte1_327[3] = parameter7_4byte327[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_328[0] = parameter7_4byte328[2];
            parameter7_4byte1_328[1] = parameter7_4byte328[3];
            parameter7_4byte1_328[2] = parameter7_4byte328[0];
            parameter7_4byte1_328[3] = parameter7_4byte328[1];

            //0x4800 command
            parameter7_4byte1_329[0] = parameter7_4byte329[0];    //속도와 가속
            parameter7_4byte1_329[1] = parameter7_4byte329[1];    //커맨드 Code
            parameter7_4byte1_329[2] = parameter7_4byte329[2];    //예약                   
            parameter7_4byte1_329[3] = parameter7_4byte329[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_330[0] = parameter7_4byte330[2];
            parameter7_4byte1_330[1] = parameter7_4byte330[3];
            parameter7_4byte1_330[2] = parameter7_4byte330[0];
            parameter7_4byte1_330[3] = parameter7_4byte330[1];

            //0x4800 command
            parameter7_4byte1_331[0] = parameter7_4byte331[0];    //속도와 가속
            parameter7_4byte1_331[1] = parameter7_4byte331[1];    //커맨드 Code
            parameter7_4byte1_331[2] = parameter7_4byte331[2];    //예약                   
            parameter7_4byte1_331[3] = parameter7_4byte331[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_332[0] = parameter7_4byte332[2];
            parameter7_4byte1_332[1] = parameter7_4byte332[3];
            parameter7_4byte1_332[2] = parameter7_4byte332[0];
            parameter7_4byte1_332[3] = parameter7_4byte332[1];

            //0x4800 command
            parameter7_4byte1_333[0] = parameter7_4byte333[0];    //속도와 가속
            parameter7_4byte1_333[1] = parameter7_4byte333[1];    //커맨드 Code
            parameter7_4byte1_333[2] = parameter7_4byte333[2];    //예약                   
            parameter7_4byte1_333[3] = parameter7_4byte333[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_334[0] = parameter7_4byte334[2];
            parameter7_4byte1_334[1] = parameter7_4byte334[3];
            parameter7_4byte1_334[2] = parameter7_4byte334[0];
            parameter7_4byte1_334[3] = parameter7_4byte334[1];

            //0x4800 command
            parameter7_4byte1_335[0] = parameter7_4byte335[0];    //속도와 가속
            parameter7_4byte1_335[1] = parameter7_4byte335[1];    //커맨드 Code
            parameter7_4byte1_335[2] = parameter7_4byte335[2];    //예약                   
            parameter7_4byte1_335[3] = parameter7_4byte335[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_336[0] = parameter7_4byte336[2];
            parameter7_4byte1_336[1] = parameter7_4byte336[3];
            parameter7_4byte1_336[2] = parameter7_4byte336[0];
            parameter7_4byte1_336[3] = parameter7_4byte336[1];

            //0x4800 command
            parameter7_4byte1_337[0] = parameter7_4byte337[0];    //속도와 가속
            parameter7_4byte1_337[1] = parameter7_4byte337[1];    //커맨드 Code
            parameter7_4byte1_337[2] = parameter7_4byte337[2];    //예약                   
            parameter7_4byte1_337[3] = parameter7_4byte337[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_338[0] = parameter7_4byte338[2];
            parameter7_4byte1_338[1] = parameter7_4byte338[3];
            parameter7_4byte1_338[2] = parameter7_4byte338[0];
            parameter7_4byte1_338[3] = parameter7_4byte338[1];

            //0x4800 command
            parameter7_4byte1_339[0] = parameter7_4byte339[0];    //속도와 가속
            parameter7_4byte1_339[1] = parameter7_4byte339[1];    //커맨드 Code
            parameter7_4byte1_339[2] = parameter7_4byte339[2];    //예약                   
            parameter7_4byte1_339[3] = parameter7_4byte339[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_340[0] = parameter7_4byte340[2];
            parameter7_4byte1_340[1] = parameter7_4byte340[3];
            parameter7_4byte1_340[2] = parameter7_4byte340[0];
            parameter7_4byte1_340[3] = parameter7_4byte340[1];

            //0x4800 command
            parameter7_4byte1_341[0] = parameter7_4byte341[0];    //속도와 가속
            parameter7_4byte1_341[1] = parameter7_4byte341[1];    //커맨드 Code
            parameter7_4byte1_341[2] = parameter7_4byte341[2];    //예약                   
            parameter7_4byte1_341[3] = parameter7_4byte341[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_342[0] = parameter7_4byte342[2];
            parameter7_4byte1_342[1] = parameter7_4byte342[3];
            parameter7_4byte1_342[2] = parameter7_4byte342[0];
            parameter7_4byte1_342[3] = parameter7_4byte342[1];

            //0x4800 command
            parameter7_4byte1_343[0] = parameter7_4byte343[0];    //속도와 가속
            parameter7_4byte1_343[1] = parameter7_4byte343[1];    //커맨드 Code
            parameter7_4byte1_343[2] = parameter7_4byte343[2];    //예약                   
            parameter7_4byte1_343[3] = parameter7_4byte343[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_344[0] = parameter7_4byte344[2];
            parameter7_4byte1_344[1] = parameter7_4byte344[3];
            parameter7_4byte1_344[2] = parameter7_4byte344[0];
            parameter7_4byte1_344[3] = parameter7_4byte344[1];

            //0x4800 command
            parameter7_4byte1_345[0] = parameter7_4byte345[0];    //속도와 가속
            parameter7_4byte1_345[1] = parameter7_4byte345[1];    //커맨드 Code
            parameter7_4byte1_345[2] = parameter7_4byte345[2];    //예약                   
            parameter7_4byte1_345[3] = parameter7_4byte345[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_346[0] = parameter7_4byte346[2];
            parameter7_4byte1_346[1] = parameter7_4byte346[3];
            parameter7_4byte1_346[2] = parameter7_4byte346[0];
            parameter7_4byte1_346[3] = parameter7_4byte346[1];

            //0x4800 command
            parameter7_4byte1_347[0] = parameter7_4byte347[0];    //속도와 가속
            parameter7_4byte1_347[1] = parameter7_4byte347[1];    //커맨드 Code
            parameter7_4byte1_347[2] = parameter7_4byte347[2];    //예약                   
            parameter7_4byte1_347[3] = parameter7_4byte347[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_348[0] = parameter7_4byte348[2];
            parameter7_4byte1_348[1] = parameter7_4byte348[3];
            parameter7_4byte1_348[2] = parameter7_4byte348[0];
            parameter7_4byte1_348[3] = parameter7_4byte348[1];

            //0x4800 command
            parameter7_4byte1_349[0] = parameter7_4byte349[0];    //속도와 가속
            parameter7_4byte1_349[1] = parameter7_4byte349[1];    //커맨드 Code
            parameter7_4byte1_349[2] = parameter7_4byte349[2];    //예약                   
            parameter7_4byte1_349[3] = parameter7_4byte349[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_350[0] = parameter7_4byte350[2];
            parameter7_4byte1_350[1] = parameter7_4byte350[3];
            parameter7_4byte1_350[2] = parameter7_4byte350[0];
            parameter7_4byte1_350[3] = parameter7_4byte350[1];

            //0x4800 command
            parameter7_4byte1_351[0] = parameter7_4byte351[0];    //속도와 가속
            parameter7_4byte1_351[1] = parameter7_4byte351[1];    //커맨드 Code
            parameter7_4byte1_351[2] = parameter7_4byte351[2];    //예약                   
            parameter7_4byte1_351[3] = parameter7_4byte351[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_352[0] = parameter7_4byte352[2];
            parameter7_4byte1_352[1] = parameter7_4byte352[3];
            parameter7_4byte1_352[2] = parameter7_4byte352[0];
            parameter7_4byte1_352[3] = parameter7_4byte352[1];

            //0x4800 command
            parameter7_4byte1_353[0] = parameter7_4byte353[0];    //속도와 가속
            parameter7_4byte1_353[1] = parameter7_4byte353[1];    //커맨드 Code
            parameter7_4byte1_353[2] = parameter7_4byte353[2];    //예약                   
            parameter7_4byte1_353[3] = parameter7_4byte353[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_354[0] = parameter7_4byte354[2];
            parameter7_4byte1_354[1] = parameter7_4byte354[3];
            parameter7_4byte1_354[2] = parameter7_4byte354[0];
            parameter7_4byte1_354[3] = parameter7_4byte354[1];

            //0x4800 command
            parameter7_4byte1_355[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_355[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_355[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_355[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_356[0] = parameter7_4byte356[2];
            parameter7_4byte1_356[1] = parameter7_4byte356[3];
            parameter7_4byte1_356[2] = parameter7_4byte356[0];
            parameter7_4byte1_356[3] = parameter7_4byte356[1];

















            //0x4800 command
            parameter7_4byte1_357[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_357[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_357[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_357[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_358[0] = parameter7_4byte356[2];
            parameter7_4byte1_358[1] = parameter7_4byte356[3];
            parameter7_4byte1_358[2] = parameter7_4byte356[0];
            parameter7_4byte1_358[3] = parameter7_4byte356[1];




            //0x4800 command
            parameter7_4byte1_359[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_359[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_359[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_359[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_360[0] = parameter7_4byte356[2];
            parameter7_4byte1_360[1] = parameter7_4byte356[3];
            parameter7_4byte1_360[2] = parameter7_4byte356[0];
            parameter7_4byte1_360[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_361[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_361[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_361[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_361[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_362[0] = parameter7_4byte356[2];
            parameter7_4byte1_362[1] = parameter7_4byte356[3];
            parameter7_4byte1_362[2] = parameter7_4byte356[0];
            parameter7_4byte1_362[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_363[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_363[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_363[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_363[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_364[0] = parameter7_4byte356[2];
            parameter7_4byte1_364[1] = parameter7_4byte356[3];
            parameter7_4byte1_364[2] = parameter7_4byte356[0];
            parameter7_4byte1_364[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_365[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_365[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_365[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_365[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_366[0] = parameter7_4byte356[2];
            parameter7_4byte1_366[1] = parameter7_4byte356[3];
            parameter7_4byte1_366[2] = parameter7_4byte356[0];
            parameter7_4byte1_366[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_367[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_367[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_367[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_367[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_368[0] = parameter7_4byte356[2];
            parameter7_4byte1_368[1] = parameter7_4byte356[3];
            parameter7_4byte1_368[2] = parameter7_4byte356[0];
            parameter7_4byte1_368[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_369[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_369[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_369[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_369[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_370[0] = parameter7_4byte356[2];
            parameter7_4byte1_370[1] = parameter7_4byte356[3];
            parameter7_4byte1_370[2] = parameter7_4byte356[0];
            parameter7_4byte1_370[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_371[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_371[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_371[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_371[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_372[0] = parameter7_4byte356[2];
            parameter7_4byte1_372[1] = parameter7_4byte356[3];
            parameter7_4byte1_372[2] = parameter7_4byte356[0];
            parameter7_4byte1_372[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_373[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_373[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_373[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_373[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_374[0] = parameter7_4byte356[2];
            parameter7_4byte1_374[1] = parameter7_4byte356[3];
            parameter7_4byte1_374[2] = parameter7_4byte356[0];
            parameter7_4byte1_374[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_375[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_375[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_375[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_375[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_376[0] = parameter7_4byte356[2];
            parameter7_4byte1_376[1] = parameter7_4byte356[3];
            parameter7_4byte1_376[2] = parameter7_4byte356[0];
            parameter7_4byte1_376[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_377[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_377[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_377[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_377[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_378[0] = parameter7_4byte356[2];
            parameter7_4byte1_378[1] = parameter7_4byte356[3];
            parameter7_4byte1_378[2] = parameter7_4byte356[0];
            parameter7_4byte1_378[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_379[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_379[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_379[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_379[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_380[0] = parameter7_4byte356[2];
            parameter7_4byte1_380[1] = parameter7_4byte356[3];
            parameter7_4byte1_380[2] = parameter7_4byte356[0];
            parameter7_4byte1_380[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_381[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_381[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_381[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_381[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_382[0] = parameter7_4byte356[2];
            parameter7_4byte1_382[1] = parameter7_4byte356[3];
            parameter7_4byte1_382[2] = parameter7_4byte356[0];
            parameter7_4byte1_382[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_383[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_383[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_383[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_383[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_384[0] = parameter7_4byte356[2];
            parameter7_4byte1_384[1] = parameter7_4byte356[3];
            parameter7_4byte1_384[2] = parameter7_4byte356[0];
            parameter7_4byte1_384[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_385[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_385[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_385[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_385[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_386[0] = parameter7_4byte356[2];
            parameter7_4byte1_386[1] = parameter7_4byte356[3];
            parameter7_4byte1_386[2] = parameter7_4byte356[0];
            parameter7_4byte1_386[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_387[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_387[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_387[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_387[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_388[0] = parameter7_4byte356[2];
            parameter7_4byte1_388[1] = parameter7_4byte356[3];
            parameter7_4byte1_388[2] = parameter7_4byte356[0];
            parameter7_4byte1_388[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_389[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_389[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_389[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_389[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_390[0] = parameter7_4byte356[2];
            parameter7_4byte1_390[1] = parameter7_4byte356[3];
            parameter7_4byte1_390[2] = parameter7_4byte356[0];
            parameter7_4byte1_390[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_391[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_391[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_391[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_391[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_392[0] = parameter7_4byte356[2];
            parameter7_4byte1_392[1] = parameter7_4byte356[3];
            parameter7_4byte1_392[2] = parameter7_4byte356[0];
            parameter7_4byte1_392[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_393[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_393[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_393[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_393[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_394[0] = parameter7_4byte356[2];
            parameter7_4byte1_394[1] = parameter7_4byte356[3];
            parameter7_4byte1_394[2] = parameter7_4byte356[0];
            parameter7_4byte1_394[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_395[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_395[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_395[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_395[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_396[0] = parameter7_4byte356[2];
            parameter7_4byte1_396[1] = parameter7_4byte356[3];
            parameter7_4byte1_396[2] = parameter7_4byte356[0];
            parameter7_4byte1_396[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_397[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_397[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_397[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_397[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_398[0] = parameter7_4byte356[2];
            parameter7_4byte1_398[1] = parameter7_4byte356[3];
            parameter7_4byte1_398[2] = parameter7_4byte356[0];
            parameter7_4byte1_398[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_399[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_399[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_399[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_399[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_400[0] = parameter7_4byte356[2];
            parameter7_4byte1_400[1] = parameter7_4byte356[3];
            parameter7_4byte1_400[2] = parameter7_4byte356[0];
            parameter7_4byte1_400[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_401[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_401[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_401[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_401[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_402[0] = parameter7_4byte356[2];
            parameter7_4byte1_402[1] = parameter7_4byte356[3];
            parameter7_4byte1_402[2] = parameter7_4byte356[0];
            parameter7_4byte1_402[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_403[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_403[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_403[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_403[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_404[0] = parameter7_4byte356[2];
            parameter7_4byte1_404[1] = parameter7_4byte356[3];
            parameter7_4byte1_404[2] = parameter7_4byte356[0];
            parameter7_4byte1_404[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_405[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_405[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_405[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_405[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_406[0] = parameter7_4byte356[2];
            parameter7_4byte1_406[1] = parameter7_4byte356[3];
            parameter7_4byte1_406[2] = parameter7_4byte356[0];
            parameter7_4byte1_406[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_407[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_407[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_407[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_407[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건


            //0x4802 data
            parameter7_4byte1_408[0] = parameter7_4byte356[2];
            parameter7_4byte1_408[1] = parameter7_4byte356[3];
            parameter7_4byte1_408[2] = parameter7_4byte356[0];
            parameter7_4byte1_408[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_409[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_409[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_409[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_409[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_410[0] = parameter7_4byte356[2];
            parameter7_4byte1_410[1] = parameter7_4byte356[3];
            parameter7_4byte1_410[2] = parameter7_4byte356[0];
            parameter7_4byte1_410[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_411[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_411[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_411[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_411[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_412[0] = parameter7_4byte356[2];
            parameter7_4byte1_412[1] = parameter7_4byte356[3];
            parameter7_4byte1_412[2] = parameter7_4byte356[0];
            parameter7_4byte1_412[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_413[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_413[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_413[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_413[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_414[0] = parameter7_4byte356[2];
            parameter7_4byte1_414[1] = parameter7_4byte356[3];
            parameter7_4byte1_414[2] = parameter7_4byte356[0];
            parameter7_4byte1_414[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_415[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_415[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_415[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_415[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_416[0] = parameter7_4byte356[2];
            parameter7_4byte1_416[1] = parameter7_4byte356[3];
            parameter7_4byte1_416[2] = parameter7_4byte356[0];
            parameter7_4byte1_416[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_417[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_417[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_417[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_417[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_418[0] = parameter7_4byte356[2];
            parameter7_4byte1_418[1] = parameter7_4byte356[3];
            parameter7_4byte1_418[2] = parameter7_4byte356[0];
            parameter7_4byte1_418[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_419[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_419[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_419[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_419[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_420[0] = parameter7_4byte356[2];
            parameter7_4byte1_420[1] = parameter7_4byte356[3];
            parameter7_4byte1_420[2] = parameter7_4byte356[0];
            parameter7_4byte1_420[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_421[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_421[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_421[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_421[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_422[0] = parameter7_4byte356[2];
            parameter7_4byte1_422[1] = parameter7_4byte356[3];
            parameter7_4byte1_422[2] = parameter7_4byte356[0];
            parameter7_4byte1_422[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_423[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_423[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_423[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_423[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_424[0] = parameter7_4byte356[2];
            parameter7_4byte1_424[1] = parameter7_4byte356[3];
            parameter7_4byte1_424[2] = parameter7_4byte356[0];
            parameter7_4byte1_424[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_425[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_425[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_425[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_425[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_426[0] = parameter7_4byte356[2];
            parameter7_4byte1_426[1] = parameter7_4byte356[3];
            parameter7_4byte1_426[2] = parameter7_4byte356[0];
            parameter7_4byte1_426[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_427[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_427[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_427[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_427[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_428[0] = parameter7_4byte356[2];
            parameter7_4byte1_428[1] = parameter7_4byte356[3];
            parameter7_4byte1_428[2] = parameter7_4byte356[0];
            parameter7_4byte1_428[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_429[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_429[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_429[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_429[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건


            //0x4802 data
            parameter7_4byte1_430[0] = parameter7_4byte356[2];
            parameter7_4byte1_430[1] = parameter7_4byte356[3];
            parameter7_4byte1_430[2] = parameter7_4byte356[0];
            parameter7_4byte1_430[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_431[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_431[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_431[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_431[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_432[0] = parameter7_4byte356[2];
            parameter7_4byte1_432[1] = parameter7_4byte356[3];
            parameter7_4byte1_432[2] = parameter7_4byte356[0];
            parameter7_4byte1_432[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_433[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_433[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_433[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_433[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건


            //0x4802 data
            parameter7_4byte1_434[0] = parameter7_4byte356[2];
            parameter7_4byte1_434[1] = parameter7_4byte356[3];
            parameter7_4byte1_434[2] = parameter7_4byte356[0];
            parameter7_4byte1_434[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_435[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_435[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_435[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_435[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건


            //0x4802 data
            parameter7_4byte1_436[0] = parameter7_4byte356[2];
            parameter7_4byte1_436[1] = parameter7_4byte356[3];
            parameter7_4byte1_436[2] = parameter7_4byte356[0];
            parameter7_4byte1_436[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_437[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_437[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_437[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_437[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_438[0] = parameter7_4byte356[2];
            parameter7_4byte1_438[1] = parameter7_4byte356[3];
            parameter7_4byte1_438[2] = parameter7_4byte356[0];
            parameter7_4byte1_438[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_439[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_439[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_439[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_439[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_440[0] = parameter7_4byte356[2];
            parameter7_4byte1_440[1] = parameter7_4byte356[3];
            parameter7_4byte1_440[2] = parameter7_4byte356[0];
            parameter7_4byte1_440[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_441[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_441[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_441[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_441[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_442[0] = parameter7_4byte356[2];
            parameter7_4byte1_442[1] = parameter7_4byte356[3];
            parameter7_4byte1_442[2] = parameter7_4byte356[0];
            parameter7_4byte1_442[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_443[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_443[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_443[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_443[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_444[0] = parameter7_4byte356[2];
            parameter7_4byte1_444[1] = parameter7_4byte356[3];
            parameter7_4byte1_444[2] = parameter7_4byte356[0];
            parameter7_4byte1_444[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_445[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_445[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_445[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_445[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_446[0] = parameter7_4byte356[2];
            parameter7_4byte1_446[1] = parameter7_4byte356[3];
            parameter7_4byte1_446[2] = parameter7_4byte356[0];
            parameter7_4byte1_446[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_447[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_447[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_447[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_447[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_448[0] = parameter7_4byte356[2];
            parameter7_4byte1_448[1] = parameter7_4byte356[3];
            parameter7_4byte1_448[2] = parameter7_4byte356[0];
            parameter7_4byte1_448[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_449[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_449[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_449[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_449[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_450[0] = parameter7_4byte356[2];
            parameter7_4byte1_450[1] = parameter7_4byte356[3];
            parameter7_4byte1_450[2] = parameter7_4byte356[0];
            parameter7_4byte1_450[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_451[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_451[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_451[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_451[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_452[0] = parameter7_4byte356[2];
            parameter7_4byte1_452[1] = parameter7_4byte356[3];
            parameter7_4byte1_452[2] = parameter7_4byte356[0];
            parameter7_4byte1_452[3] = parameter7_4byte356[1];


            //0x4800 command
            parameter7_4byte1_453[0] = parameter7_4byte355[0];    //속도와 가속
            parameter7_4byte1_453[1] = parameter7_4byte355[1];    //커맨드 Code
            parameter7_4byte1_453[2] = parameter7_4byte355[2];    //예약                   
            parameter7_4byte1_453[3] = parameter7_4byte355[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_454[0] = parameter7_4byte356[2];
            parameter7_4byte1_454[1] = parameter7_4byte356[3];
            parameter7_4byte1_454[2] = parameter7_4byte356[0];
            parameter7_4byte1_454[3] = parameter7_4byte356[1];











            //0x4800 command
            parameter7_4byte1_455[0] = parameter7_4byte455[0];    //속도와 가속
            parameter7_4byte1_455[1] = parameter7_4byte455[1];    //커맨드 Code
            parameter7_4byte1_455[2] = parameter7_4byte455[2];    //예약                   
            parameter7_4byte1_455[3] = parameter7_4byte455[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_456[0] = parameter7_4byte456[2];
            parameter7_4byte1_456[1] = parameter7_4byte456[3];
            parameter7_4byte1_456[2] = parameter7_4byte456[0];
            parameter7_4byte1_456[3] = parameter7_4byte456[1];


            //0x4800 command
            parameter7_4byte1_457[0] = parameter7_4byte457[0];    //속도와 가속
            parameter7_4byte1_457[1] = parameter7_4byte457[1];    //커맨드 Code
            parameter7_4byte1_457[2] = parameter7_4byte457[2];    //예약                   
            parameter7_4byte1_457[3] = parameter7_4byte457[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_458[0] = parameter7_4byte458[2];
            parameter7_4byte1_458[1] = parameter7_4byte458[3];
            parameter7_4byte1_458[2] = parameter7_4byte458[0];
            parameter7_4byte1_458[3] = parameter7_4byte458[1];


            //0x4800 command
            parameter7_4byte1_459[0] = parameter7_4byte459[0];    //속도와 가속
            parameter7_4byte1_459[1] = parameter7_4byte459[1];    //커맨드 Code
            parameter7_4byte1_459[2] = parameter7_4byte459[2];    //예약                   
            parameter7_4byte1_459[3] = parameter7_4byte459[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_460[0] = parameter7_4byte460[2];
            parameter7_4byte1_460[1] = parameter7_4byte460[3];
            parameter7_4byte1_460[2] = parameter7_4byte460[0];
            parameter7_4byte1_460[3] = parameter7_4byte460[1];


            //0x4800 command
            parameter7_4byte1_461[0] = parameter7_4byte461[0];    //속도와 가속
            parameter7_4byte1_461[1] = parameter7_4byte461[1];    //커맨드 Code
            parameter7_4byte1_461[2] = parameter7_4byte461[2];    //예약                   
            parameter7_4byte1_461[3] = parameter7_4byte461[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_462[0] = parameter7_4byte462[2];
            parameter7_4byte1_462[1] = parameter7_4byte462[3];
            parameter7_4byte1_462[2] = parameter7_4byte462[0];
            parameter7_4byte1_462[3] = parameter7_4byte462[1];


            //0x4800 command
            parameter7_4byte1_463[0] = parameter7_4byte463[0];    //속도와 가속
            parameter7_4byte1_463[1] = parameter7_4byte463[1];    //커맨드 Code
            parameter7_4byte1_463[2] = parameter7_4byte463[2];    //예약                   
            parameter7_4byte1_463[3] = parameter7_4byte463[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_464[0] = parameter7_4byte464[2];
            parameter7_4byte1_464[1] = parameter7_4byte464[3];
            parameter7_4byte1_464[2] = parameter7_4byte464[0];
            parameter7_4byte1_464[3] = parameter7_4byte464[1];


            //0x4800 command
            parameter7_4byte1_465[0] = parameter7_4byte465[0];    //속도와 가속
            parameter7_4byte1_465[1] = parameter7_4byte465[1];    //커맨드 Code
            parameter7_4byte1_465[2] = parameter7_4byte465[2];    //예약                   
            parameter7_4byte1_465[3] = parameter7_4byte465[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_466[0] = parameter7_4byte466[2];
            parameter7_4byte1_466[1] = parameter7_4byte466[3];
            parameter7_4byte1_466[2] = parameter7_4byte466[0];
            parameter7_4byte1_466[3] = parameter7_4byte466[1];


            //0x4800 command
            parameter7_4byte1_467[0] = parameter7_4byte467[0];    //속도와 가속
            parameter7_4byte1_467[1] = parameter7_4byte467[1];    //커맨드 Code
            parameter7_4byte1_467[2] = parameter7_4byte467[2];    //예약                   
            parameter7_4byte1_467[3] = parameter7_4byte467[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_468[0] = parameter7_4byte468[2];
            parameter7_4byte1_468[1] = parameter7_4byte468[3];
            parameter7_4byte1_468[2] = parameter7_4byte468[0];
            parameter7_4byte1_468[3] = parameter7_4byte468[1];


            //0x4800 command
            parameter7_4byte1_469[0] = parameter7_4byte469[0];    //속도와 가속
            parameter7_4byte1_469[1] = parameter7_4byte469[1];    //커맨드 Code
            parameter7_4byte1_469[2] = parameter7_4byte469[2];    //예약                   
            parameter7_4byte1_469[3] = parameter7_4byte469[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_470[0] = parameter7_4byte470[2];
            parameter7_4byte1_470[1] = parameter7_4byte470[3];
            parameter7_4byte1_470[2] = parameter7_4byte470[0];
            parameter7_4byte1_470[3] = parameter7_4byte470[1];


            //0x4800 command
            parameter7_4byte1_471[0] = parameter7_4byte471[0];    //속도와 가속
            parameter7_4byte1_471[1] = parameter7_4byte471[1];    //커맨드 Code
            parameter7_4byte1_471[2] = parameter7_4byte471[2];    //예약                   
            parameter7_4byte1_471[3] = parameter7_4byte471[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_472[0] = parameter7_4byte472[2];
            parameter7_4byte1_472[1] = parameter7_4byte472[3];
            parameter7_4byte1_472[2] = parameter7_4byte472[0];
            parameter7_4byte1_472[3] = parameter7_4byte472[1];


            //0x4800 command
            parameter7_4byte1_473[0] = parameter7_4byte473[0];    //속도와 가속
            parameter7_4byte1_473[1] = parameter7_4byte473[1];    //커맨드 Code
            parameter7_4byte1_473[2] = parameter7_4byte473[2];    //예약                   
            parameter7_4byte1_473[3] = parameter7_4byte473[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_474[0] = parameter7_4byte474[2];
            parameter7_4byte1_474[1] = parameter7_4byte474[3];
            parameter7_4byte1_474[2] = parameter7_4byte474[0];
            parameter7_4byte1_474[3] = parameter7_4byte474[1];


            //0x4800 command
            parameter7_4byte1_475[0] = parameter7_4byte475[0];    //속도와 가속
            parameter7_4byte1_475[1] = parameter7_4byte475[1];    //커맨드 Code
            parameter7_4byte1_475[2] = parameter7_4byte475[2];    //예약                   
            parameter7_4byte1_475[3] = parameter7_4byte475[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_476[0] = parameter7_4byte476[2];
            parameter7_4byte1_476[1] = parameter7_4byte476[3];
            parameter7_4byte1_476[2] = parameter7_4byte476[0];
            parameter7_4byte1_476[3] = parameter7_4byte476[1];


            //0x4800 command
            parameter7_4byte1_477[0] = parameter7_4byte477[0];    //속도와 가속
            parameter7_4byte1_477[1] = parameter7_4byte477[1];    //커맨드 Code
            parameter7_4byte1_477[2] = parameter7_4byte477[2];    //예약                   
            parameter7_4byte1_477[3] = parameter7_4byte477[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_478[0] = parameter7_4byte478[2];
            parameter7_4byte1_478[1] = parameter7_4byte478[3];
            parameter7_4byte1_478[2] = parameter7_4byte478[0];
            parameter7_4byte1_478[3] = parameter7_4byte478[1];

            //0x4800 command
            parameter7_4byte1_479[0] = parameter7_4byte479[0];    //속도와 가속
            parameter7_4byte1_479[1] = parameter7_4byte479[1];    //커맨드 Code
            parameter7_4byte1_479[2] = parameter7_4byte479[2];    //예약                   
            parameter7_4byte1_479[3] = parameter7_4byte479[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_480[0] = parameter7_4byte480[2];
            parameter7_4byte1_480[1] = parameter7_4byte480[3];
            parameter7_4byte1_480[2] = parameter7_4byte480[0];
            parameter7_4byte1_480[3] = parameter7_4byte480[1];


            //0x4800 command
            parameter7_4byte1_481[0] = parameter7_4byte481[0];    //속도와 가속
            parameter7_4byte1_481[1] = parameter7_4byte481[1];    //커맨드 Code
            parameter7_4byte1_481[2] = parameter7_4byte481[2];    //예약                   
            parameter7_4byte1_481[3] = parameter7_4byte481[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_482[0] = parameter7_4byte482[2];
            parameter7_4byte1_482[1] = parameter7_4byte482[3];
            parameter7_4byte1_482[2] = parameter7_4byte482[0];
            parameter7_4byte1_482[3] = parameter7_4byte482[1];


            //0x4800 command
            parameter7_4byte1_483[0] = parameter7_4byte483[0];    //속도와 가속
            parameter7_4byte1_483[1] = parameter7_4byte483[1];    //커맨드 Code
            parameter7_4byte1_483[2] = parameter7_4byte483[2];    //예약                   
            parameter7_4byte1_483[3] = parameter7_4byte483[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_484[0] = parameter7_4byte484[2];
            parameter7_4byte1_484[1] = parameter7_4byte484[3];
            parameter7_4byte1_484[2] = parameter7_4byte484[0];
            parameter7_4byte1_484[3] = parameter7_4byte484[1];


            //0x4800 command
            parameter7_4byte1_485[0] = parameter7_4byte485[0];    //속도와 가속
            parameter7_4byte1_485[1] = parameter7_4byte485[1];    //커맨드 Code
            parameter7_4byte1_485[2] = parameter7_4byte485[2];    //예약                   
            parameter7_4byte1_485[3] = parameter7_4byte485[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_486[0] = parameter7_4byte486[2];
            parameter7_4byte1_486[1] = parameter7_4byte486[3];
            parameter7_4byte1_486[2] = parameter7_4byte486[0];
            parameter7_4byte1_486[3] = parameter7_4byte486[1];


            //0x4800 command
            parameter7_4byte1_487[0] = parameter7_4byte487[0];    //속도와 가속
            parameter7_4byte1_487[1] = parameter7_4byte487[1];    //커맨드 Code
            parameter7_4byte1_487[2] = parameter7_4byte487[2];    //예약                   
            parameter7_4byte1_487[3] = parameter7_4byte487[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_488[0] = parameter7_4byte488[2];
            parameter7_4byte1_488[1] = parameter7_4byte488[3];
            parameter7_4byte1_488[2] = parameter7_4byte488[0];
            parameter7_4byte1_488[3] = parameter7_4byte488[1];


            //0x4800 command
            parameter7_4byte1_489[0] = parameter7_4byte489[0];    //속도와 가속
            parameter7_4byte1_489[1] = parameter7_4byte489[1];    //커맨드 Code
            parameter7_4byte1_489[2] = parameter7_4byte489[2];    //예약                   
            parameter7_4byte1_489[3] = parameter7_4byte489[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_490[0] = parameter7_4byte490[2];
            parameter7_4byte1_490[1] = parameter7_4byte490[3];
            parameter7_4byte1_490[2] = parameter7_4byte490[0];
            parameter7_4byte1_490[3] = parameter7_4byte490[1];


            //0x4800 command
            parameter7_4byte1_491[0] = parameter7_4byte491[0];    //속도와 가속
            parameter7_4byte1_491[1] = parameter7_4byte491[1];    //커맨드 Code
            parameter7_4byte1_491[2] = parameter7_4byte491[2];    //예약                   
            parameter7_4byte1_491[3] = parameter7_4byte491[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_492[0] = parameter7_4byte492[2];
            parameter7_4byte1_492[1] = parameter7_4byte492[3];
            parameter7_4byte1_492[2] = parameter7_4byte492[0];
            parameter7_4byte1_492[3] = parameter7_4byte492[1];


            //0x4800 command
            parameter7_4byte1_493[0] = parameter7_4byte493[0];    //속도와 가속
            parameter7_4byte1_493[1] = parameter7_4byte493[1];    //커맨드 Code
            parameter7_4byte1_493[2] = parameter7_4byte493[2];    //예약                   
            parameter7_4byte1_493[3] = parameter7_4byte493[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_494[0] = parameter7_4byte494[2];
            parameter7_4byte1_494[1] = parameter7_4byte494[3];
            parameter7_4byte1_494[2] = parameter7_4byte494[0];
            parameter7_4byte1_494[3] = parameter7_4byte494[1];


            //0x4800 command
            parameter7_4byte1_495[0] = parameter7_4byte495[0];    //속도와 가속
            parameter7_4byte1_495[1] = parameter7_4byte495[1];    //커맨드 Code
            parameter7_4byte1_495[2] = parameter7_4byte495[2];    //예약                   
            parameter7_4byte1_495[3] = parameter7_4byte495[3];    //감속, 방향, 천이 조건

            //0x4802 data
            parameter7_4byte1_496[0] = parameter7_4byte496[2];
            parameter7_4byte1_496[1] = parameter7_4byte496[3];
            parameter7_4byte1_496[2] = parameter7_4byte496[0];
            parameter7_4byte1_496[3] = parameter7_4byte496[1];






            int cmdCode = Convert.ToInt32(parameter7_4byte1_1[1]);  //커맨드 Code 
            int spdNum = (Convert.ToInt32(parameter7_4byte1_1[0]) >>4 );   // 속도 번호  hiki1
            int accNum = (Convert.ToInt32(parameter7_4byte1_1[0]) & 0b_0000_1111);  //가속 번호  hiki2
            int dummy = Convert.ToInt32(parameter7_4byte1_1[2]);   //예약
            int decNum = (Convert.ToInt32(parameter7_4byte1_1[3]) >>4 );   //감속 번호  hiki3
            int movdir = ((Convert.ToInt32(parameter7_4byte1_1[3]) & 0b_0000_1111) >>2 );  //  방향  hiki4
            int blockchif = (Convert.ToInt32(parameter7_4byte1_1[3]) & 0b_0000_0011);  //천이 조건  hiki5
            int dataConfig = BitConverter.ToInt32(parameter7_4byte1_2, 0);   //블록 데이터 구성





            Debug.WriteLine(cmdCode.ToString());
            Debug.WriteLine(spdNum.ToString());
            Debug.WriteLine(accNum.ToString());
            Debug.WriteLine(decNum.ToString());
            Debug.WriteLine(movdir.ToString());
            Debug.WriteLine(blockchif.ToString());
            Debug.WriteLine(dataConfig.ToString());

        }

        // recValue1~recValue512
        private void BlockParameterRec(int blockNum)
        {
            switch (blockNum)
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
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4816, 2, ref recValue12);
                    return;
                case 6:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4818, 2, ref recValue13);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x481A, 2, ref recValue14);
                    return;
                case 7:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x481C, 2, ref recValue15);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x481E, 2, ref recValue16);
                    return;
                case 8:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4822, 2, ref recValue17);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4820, 2, ref recValue18);
                    return;
                case 9:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4824, 2, ref recValue19);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4826, 2, ref recValue20);
                    return;
                case 10:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4828, 2, ref recValue21);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x482A, 2, ref recValue22);
                    return;
                case 11:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x482C, 2, ref recValue23);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x482E, 2, ref recValue24);
                    return;
                case 12:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4830, 2, ref recValue25);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4832, 2, ref recValue26);
                    return;
                case 13:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4834, 2, ref recValue27);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4836, 2, ref recValue28);
                    return;
                case 14:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4838, 2, ref recValue29);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x483A, 2, ref recValue30);
                    return;
                case 15:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x483C, 2, ref recValue31);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x483E, 2, ref recValue32);
                    return;
                case 16:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4840, 2, ref recValue33);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4842, 2, ref recValue34);
                    return;
                case 17:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4844, 2, ref recValue35);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4846, 2, ref recValue36);
                    return;
                case 18:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4848, 2, ref recValue37);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x484A, 2, ref recValue38);
                    return;
                case 19:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x484C, 2, ref recValue39);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x484E, 2, ref recValue40);
                    return;
                case 20:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4850, 2, ref recValue41);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4852, 2, ref recValue42);
                    return;
                case 21:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4854, 2, ref recValue43);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4856, 2, ref recValue44);
                    return;
                case 22:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4858, 2, ref recValue45);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x485A, 2, ref recValue46);
                    return;
                case 23:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x485C, 2, ref recValue47);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x485E, 2, ref recValue48);
                    return;
                case 24:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4860, 2, ref recValue49);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4862, 2, ref recValue50);
                    return;
                case 25:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4864, 2, ref recValue51);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4866, 2, ref recValue52);
                    return;
                case 26:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4868, 2, ref recValue53);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x486A, 2, ref recValue54);
                    return;
                case 27:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x486C, 2, ref recValue55);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x486E, 2, ref recValue56);
                    return;
                case 28:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4870, 2, ref recValue57);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4872, 2, ref recValue58);
                    return;
                case 29:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4874, 2, ref recValue59);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4876, 2, ref recValue60);
                    return;
                case 30:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4878, 2, ref recValue61);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x487A, 2, ref recValue62);
                    return;
                case 31:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x487C, 2, ref recValue63);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x487E, 2, ref recValue64);
                    return;
                case 32:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4880, 2, ref recValue65);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4882, 2, ref recValue66);
                    return;
                case 33:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4884, 2, ref recValue67);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4886, 2, ref recValue68);
                    return;
                case 34:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4888, 2, ref recValue69);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x488A, 2, ref recValue70);
                    return;
                case 35:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x488C, 2, ref recValue71);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x488E, 2, ref recValue72);
                    return;
                case 36:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4890, 2, ref recValue73);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4892, 2, ref recValue74);
                    return;
                case 37:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4894, 2, ref recValue75);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4896, 2, ref recValue76);
                    return;
                case 38:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4898, 2, ref recValue77);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x489A, 2, ref recValue78);
                    return;
                case 39:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x489C, 2, ref recValue79);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x489E, 2, ref recValue80);
                    return;
                case 40:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A0, 2, ref recValue81);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A2, 2, ref recValue82);
                    return;
                case 41:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A4, 2, ref recValue83);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A6, 2, ref recValue84);
                    return;
                case 42:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48A8, 2, ref recValue85);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48AA, 2, ref recValue86);
                    return;
                case 43:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48AC, 2, ref recValue87);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48AE, 2, ref recValue88);
                    return;
                case 44:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B0, 2, ref recValue89);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B2, 2, ref recValue90);
                    return;
                case 45:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B4, 2, ref recValue91);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B6, 2, ref recValue92);
                    return;
                case 46:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48B8, 2, ref recValue93);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48BA, 2, ref recValue94);
                    return;
                case 47:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48BC, 2, ref recValue95);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48BE, 2, ref recValue96);
                    return;
                case 48:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C0, 2, ref recValue97);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C2, 2, ref recValue98);
                    return;
                case 49:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C4, 2, ref recValue99);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C6, 2, ref recValue100);
                    return;
                case 50:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48C8, 2, ref recValue101);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48CA, 2, ref recValue102);
                    return;
                case 51:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48CC, 2, ref recValue103);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48CE, 2, ref recValue104);
                    return;
                case 52:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D0, 2, ref recValue105);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D2, 2, ref recValue106);
                    return;
                case 53:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D4, 2, ref recValue107);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D6, 2, ref recValue108);
                    return;
                case 54:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48D8, 2, ref recValue109);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48DA, 2, ref recValue110);
                    return;
                case 55:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48DC, 2, ref recValue111);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48DE, 2, ref recValue112);
                    return;
                case 56:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E0, 2, ref recValue113);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E2, 2, ref recValue114);
                    return;
                case 57:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E4, 2, ref recValue115);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E6, 2, ref recValue116);
                    return;
                case 58:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48E8, 2, ref recValue117);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48EA, 2, ref recValue118);
                    return;
                case 59:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48EC, 2, ref recValue119);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48EE, 2, ref recValue120);
                    return;
                case 60:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F0, 2, ref recValue121);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F2, 2, ref recValue122);
                    return;
                case 61:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F4, 2, ref recValue123);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F6, 2, ref recValue124);
                    return;
                case 62:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48F8, 2, ref recValue125);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48FA, 2, ref recValue126);
                    return;
                case 63:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48FC, 2, ref recValue127);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x48FE, 2, ref recValue128);
                    return;
                case 64:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4900, 2, ref recValue129);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4902, 2, ref recValue130);
                    return;
                case 65:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4904, 2, ref recValue131);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4906, 2, ref recValue132);
                    return;
                case 66:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4908, 2, ref recValue133);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x490A, 2, ref recValue134);
                    return;
                case 67:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x490C, 2, ref recValue135);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x490E, 2, ref recValue136);
                    return;
                case 68:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4910, 2, ref recValue137);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4912, 2, ref recValue138);
                    return;
                case 69:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4914, 2, ref recValue139);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4916, 2, ref recValue140);
                    return;
                case 70:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4918, 2, ref recValue141);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x491A, 2, ref recValue142);
                    return;
                case 71:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x491C, 2, ref recValue143);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x491E, 2, ref recValue144);
                    return;
                case 72:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4920, 2, ref recValue145);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4922, 2, ref recValue146);
                    return;
                case 73:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4924, 2, ref recValue147);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4926, 2, ref recValue148);
                    return;
                case 74:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4928, 2, ref recValue149);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x492A, 2, ref recValue150);
                    return;
                case 75:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x492C, 2, ref recValue151);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x492E, 2, ref recValue152);
                    return;
                case 76:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4930, 2, ref recValue153);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4932, 2, ref recValue154);
                    return;
                case 77:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4934, 2, ref recValue155);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4936, 2, ref recValue156);
                    return;
                case 78:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4938, 2, ref recValue157);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x493A, 2, ref recValue158);
                    return;
                case 79:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x493C, 2, ref recValue159);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x493E, 2, ref recValue160);
                    return;
                case 80:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4940, 2, ref recValue161);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4942, 2, ref recValue162);
                    return;
                case 81:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4944, 2, ref recValue163);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4946, 2, ref recValue164);
                    return;
                case 82:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4948, 2, ref recValue165);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x494A, 2, ref recValue166);
                    return;
                case 83:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x494C, 2, ref recValue167);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x494E, 2, ref recValue168);
                    return;
                case 84:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4950, 2, ref recValue169);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4952, 2, ref recValue170);
                    return;
                case 85:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4954, 2, ref recValue171);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4956, 2, ref recValue172);
                    return;
                case 86:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4958, 2, ref recValue173);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x495A, 2, ref recValue174);
                    return;
                case 87:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x495C, 2, ref recValue175);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x495E, 2, ref recValue176);
                    return;
                case 88:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4960, 2, ref recValue177);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4962, 2, ref recValue178);
                    return;
                case 89:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4964, 2, ref recValue179);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4966, 2, ref recValue180);
                    return;
                case 90:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4968, 2, ref recValue181);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x496A, 2, ref recValue182);
                    return;
                case 91:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x496C, 2, ref recValue183);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x496E, 2, ref recValue184);
                    return;
                case 92:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4970, 2, ref recValue185);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4972, 2, ref recValue186);
                    return;
                case 93:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4974, 2, ref recValue187);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4976, 2, ref recValue188);
                    return;
                case 94:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4978, 2, ref recValue189);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x497A, 2, ref recValue190);
                    return;
                case 95:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x497C, 2, ref recValue191);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x497E, 2, ref recValue192);
                    return;
                case 96:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4980, 2, ref recValue193);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4982, 2, ref recValue194);
                    return;
                case 97:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4984, 2, ref recValue195);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4986, 2, ref recValue196);
                    return;
                case 98:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4988, 2, ref recValue197);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x498A, 2, ref recValue198);
                    return;
                case 99:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x498C, 2, ref recValue199);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x498E, 2, ref recValue200);
                    return;
                case 100:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4990, 2, ref recValue201);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4992, 2, ref recValue202);
                    return;
                case 101:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4994, 2, ref recValue203);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4996, 2, ref recValue204);
                    return;
                case 102:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4998, 2, ref recValue205);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x499A, 2, ref recValue206);
                    return;
                case 103:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x499C, 2, ref recValue207);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x499E, 2, ref recValue208);
                    return;
                case 104:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A0, 2, ref recValue209);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A2, 2, ref recValue210);
                    return;
                case 105:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A4, 2, ref recValue211);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A6, 2, ref recValue212);
                    return;
                case 106:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49A8, 2, ref recValue213);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49AA, 2, ref recValue214);
                    return;
                case 107:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49AC, 2, ref recValue215);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49AE, 2, ref recValue216);
                    return;
                case 108:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B0, 2, ref recValue217);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B2, 2, ref recValue218);
                    return;
                case 109:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B4, 2, ref recValue219);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B6, 2, ref recValue220);
                    return;
                case 110:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49B8, 2, ref recValue221);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49BA, 2, ref recValue222);
                    return;
                case 111:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49BC, 2, ref recValue223);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49BE, 2, ref recValue224);
                    return;
                case 112:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C0, 2, ref recValue225);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C2, 2, ref recValue226);
                    return;
                case 113:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C4, 2, ref recValue227);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C6, 2, ref recValue228);
                    return;
                case 114:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49C8, 2, ref recValue229);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49CA, 2, ref recValue230);
                    return;
                case 115:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49CC, 2, ref recValue231);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49CE, 2, ref recValue232);
                    return;
                case 116:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D0, 2, ref recValue233);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D2, 2, ref recValue234);
                    return;
                case 117:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D4, 2, ref recValue235);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D6, 2, ref recValue236);
                    return;
                case 118:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49D8, 2, ref recValue237);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49DA, 2, ref recValue238);
                    return;
                case 119:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49DC, 2, ref recValue239);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49DE, 2, ref recValue240);
                    return;
                case 120:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E0, 2, ref recValue241);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E2, 2, ref recValue242);
                    return;
                case 121:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E4, 2, ref recValue243);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E6, 2, ref recValue244);
                    return;
                case 122:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49E8, 2, ref recValue245);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49EA, 2, ref recValue246);
                    return;
                case 123:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49EC, 2, ref recValue247);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49EE, 2, ref recValue248);
                    return;
                case 124:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F0, 2, ref recValue249);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F2, 2, ref recValue250);
                    return;
                case 125:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F4, 2, ref recValue251);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F6, 2, ref recValue252);
                    return;
                case 126:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49F8, 2, ref recValue253);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49FA, 2, ref recValue254);
                    return;
                case 127:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49FC, 2, ref recValue255);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x49FE, 2, ref recValue256);
                    return;
                case 128:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A00, 2, ref recValue257);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A02, 2, ref recValue258);
                    return;
                case 129:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A04, 2, ref recValue259);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A06, 2, ref recValue260);
                    return;
                case 130:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A08, 2, ref recValue261);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A0A, 2, ref recValue262);
                    return;
                case 131:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A0C, 2, ref recValue263);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A0E, 2, ref recValue264);
                    return;
                case 132:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A10, 2, ref recValue265);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A12, 2, ref recValue266);
                    return;
                case 133:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A14, 2, ref recValue267);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A16, 2, ref recValue268);
                    return;
                case 134:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A18, 2, ref recValue269);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A1A, 2, ref recValue270);
                    return;
                case 135:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A1C, 2, ref recValue271);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A1E, 2, ref recValue272);
                    return;
                case 136:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A20, 2, ref recValue273);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A22, 2, ref recValue274);
                    return;
                case 137:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A24, 2, ref recValue275);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A26, 2, ref recValue276);
                    return;
                case 138:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A28, 2, ref recValue277);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A2A, 2, ref recValue278);
                    return;
                case 139:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A2C, 2, ref recValue279);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A2E, 2, ref recValue280);
                    return;
                case 140://
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A30, 2, ref recValue281);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A32, 2, ref recValue282);
                    return;
                case 141:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A34, 2, ref recValue283);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A36, 2, ref recValue284);
                    return;
                case 142:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A38, 2, ref recValue285);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A3A, 2, ref recValue286);
                    return;
                case 143:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A3C, 2, ref recValue287);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A3E, 2, ref recValue288);
                    return;
                case 144:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A40, 2, ref recValue289);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A42, 2, ref recValue290);
                    return;
                case 145:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A44, 2, ref recValue291);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A46, 2, ref recValue292);
                    return;
                case 146:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A48, 2, ref recValue293);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A4A, 2, ref recValue294);
                    return;
                case 147:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A4C, 2, ref recValue295);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A4E, 2, ref recValue296);
                    return;
                case 148:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A50, 2, ref recValue297);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A52, 2, ref recValue298);
                    return;
                case 149:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A54, 2, ref recValue299);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A56, 2, ref recValue300);
                    return;
                case 150:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A58, 2, ref recValue301);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A5A, 2, ref recValue302);
                    return;
                case 151:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A5C, 2, ref recValue303);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A5E, 2, ref recValue304);
                    return;
                case 152:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A60, 2, ref recValue305);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A62, 2, ref recValue306);
                    return;
                case 153:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A64, 2, ref recValue307);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A66, 2, ref recValue308);
                    return;
                case 154:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A68, 2, ref recValue309);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A6A, 2, ref recValue310);
                    return;
                case 155:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A6C, 2, ref recValue311);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A6E, 2, ref recValue312);
                    return;
                case 156:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A70, 2, ref recValue313);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A72, 2, ref recValue314);
                    return;
                case 157:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A74, 2, ref recValue315);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A76, 2, ref recValue316);
                    return;
                case 158:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A78, 2, ref recValue317);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A7A, 2, ref recValue318);
                    return;
                case 159:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A7C, 2, ref recValue319);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A7E, 2, ref recValue320);
                    return;
                case 160:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A80, 2, ref recValue321);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A82, 2, ref recValue322);
                    return;
                case 161:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A84, 2, ref recValue323);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A86, 2, ref recValue324);
                    return;
                case 162:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A88, 2, ref recValue325);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A8A, 2, ref recValue326);
                    return;
                case 163:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A8C, 2, ref recValue327);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A8E, 2, ref recValue328);
                    return;
                case 164:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A90, 2, ref recValue329);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A92, 2, ref recValue330);
                    return;
                case 165:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A94, 2, ref recValue331);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A96, 2, ref recValue332);
                    return;
                case 166:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A98, 2, ref recValue333);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A9A, 2, ref recValue334);
                    return;
                case 167:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A9C, 2, ref recValue335);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4A9E, 2, ref recValue336);
                    return;
                case 168:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA0, 2, ref recValue337);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA2, 2, ref recValue338);
                    return;
                case 169:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA4, 2, ref recValue339);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA6, 2, ref recValue340);
                    return;
                case 170:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AA8, 2, ref recValue341);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AAA, 2, ref recValue342);
                    return;
                case 171:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AAC, 2, ref recValue343);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AAE, 2, ref recValue344);
                    return;
                case 172://
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB0, 2, ref recValue345);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB2, 2, ref recValue346);
                    return;
                case 173:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB4, 2, ref recValue347);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB6, 2, ref recValue348);
                    return;
                case 174:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AB8, 2, ref recValue349);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ABA, 2, ref recValue350);
                    return;
                case 175:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ABC, 2, ref recValue351);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ABE, 2, ref recValue352);
                    return;
                case 176:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC0, 2, ref recValue353);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC2, 2, ref recValue354);
                    return;
                case 177:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC4, 2, ref recValue355);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC6, 2, ref recValue356);
                    return;
                case 178:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AC8, 2, ref recValue357);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ACA, 2, ref recValue358);
                    return;
                case 179:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ACC, 2, ref recValue359);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ACE, 2, ref recValue360);
                    return;
                case 180:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD0, 2, ref recValue361);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD2, 2, ref recValue362);
                    return;
                case 181:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD4, 2, ref recValue363);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD6, 2, ref recValue364);
                    return;
                case 182:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AD8, 2, ref recValue365);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ADA, 2, ref recValue366);
                    return;
                case 183:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ADC, 2, ref recValue367);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4ADE, 2, ref recValue368);
                    return;
                case 184:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE0, 2, ref recValue369);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE2, 2, ref recValue370);
                    return;
                case 185:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE4, 2, ref recValue371);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE6, 2, ref recValue372);
                    return;
                case 186:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AE8, 2, ref recValue373);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AEA, 2, ref recValue374);
                    return;
                case 187:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AEC, 2, ref recValue375);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AEE, 2, ref recValue376);
                    return;
                case 188:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF0, 2, ref recValue377);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF2, 2, ref recValue378);
                    return;
                case 189:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF4, 2, ref recValue379);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF6, 2, ref recValue380);
                    return;
                case 190:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AF8, 2, ref recValue381);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AFA, 2, ref recValue382);
                    return;
                case 191:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AFC, 2, ref recValue383);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4AFE, 2, ref recValue384);
                    return;
                case 192:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B00, 2, ref recValue385);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B02, 2, ref recValue386);
                    return;
                case 193:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B04, 2, ref recValue387);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B06, 2, ref recValue388);
                    return;
                case 194:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B08, 2, ref recValue389);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B0A, 2, ref recValue390);
                    return;
                case 195:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B0C, 2, ref recValue391);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B0E, 2, ref recValue392);
                    return;
                case 196:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B10, 2, ref recValue393);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B12, 2, ref recValue394);
                    return;
                case 197:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B14, 2, ref recValue395);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B16, 2, ref recValue396);
                    return;
                case 198:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B18, 2, ref recValue397);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B1A, 2, ref recValue398);
                    return;
                case 199:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B1C, 2, ref recValue399);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B1E, 2, ref recValue400);
                    return;
                case 200:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B20, 2, ref recValue401);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B22, 2, ref recValue402);
                    return;
                case 201:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B24, 2, ref recValue403);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B26, 2, ref recValue404);
                    return;
                case 202:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B28, 2, ref recValue405);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B2A, 2, ref recValue406);
                    return;
                case 203:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B2C, 2, ref recValue407);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B2E, 2, ref recValue408);
                    return;
                case 204:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B30, 2, ref recValue409);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B32, 2, ref recValue410);
                    return;
                case 205:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B34, 2, ref recValue411);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B36, 2, ref recValue412);
                    return;
                case 206:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B38, 2, ref recValue413);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B3A, 2, ref recValue414);
                    return;
                case 207:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B3C, 2, ref recValue415);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B3E, 2, ref recValue416);
                    return;
                case 208:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B40, 2, ref recValue417);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B42, 2, ref recValue418);
                    return;
                case 209:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B44, 2, ref recValue419);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B46, 2, ref recValue420);
                    return;
                case 210:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B48, 2, ref recValue421);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B4A, 2, ref recValue422);
                    return;
                case 211:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B4C, 2, ref recValue423);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B4E, 2, ref recValue424);
                    return;
                case 212:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B50, 2, ref recValue425);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B52, 2, ref recValue426);
                    return;
                case 213:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B54, 2, ref recValue427);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B56, 2, ref recValue428);
                    return;
                case 214:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B58, 2, ref recValue429);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B5A, 2, ref recValue430);
                    return;
                case 215:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B5C, 2, ref recValue431);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B5E, 2, ref recValue432);
                    return;
                case 216:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B60, 2, ref recValue433);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B62, 2, ref recValue434);
                    return;
                case 217:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B64, 2, ref recValue435);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B66, 2, ref recValue436);
                    return;
                case 218:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B68, 2, ref recValue437);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B6A, 2, ref recValue438);
                    return;
                case 219:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B6C, 2, ref recValue439);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B6E, 2, ref recValue440);
                    return;
                case 220:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B70, 2, ref recValue441);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B72, 2, ref recValue442);
                    return;
                case 221:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B74, 2, ref recValue443);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B76, 2, ref recValue444);
                    return;
                case 222:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B78, 2, ref recValue445);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B7A, 2, ref recValue446);
                    return;
                case 223:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B7C, 2, ref recValue447);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B7E, 2, ref recValue448);
                    return;
                case 224:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B80, 2, ref recValue449);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B82, 2, ref recValue450);
                    return;
                case 225:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B84, 2, ref recValue451);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B86, 2, ref recValue452);
                    return;
                case 226:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B88, 2, ref recValue453);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B8A, 2, ref recValue454);
                    return;
                case 227:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B8C, 2, ref recValue455);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B8E, 2, ref recValue456);
                    return;
                case 228:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B90, 2, ref recValue457);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B92, 2, ref recValue458);
                    return;
                case 229:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B94, 2, ref recValue459);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B96, 2, ref recValue460);
                    return;
                case 230:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B98, 2, ref recValue461);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B9A, 2, ref recValue462);
                    return;
                case 231:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B9C, 2, ref recValue463);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4B9E, 2, ref recValue464);
                    return;
                case 232:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA0, 2, ref recValue465);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA2, 2, ref recValue466);
                    return;
                case 233:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA4, 2, ref recValue467);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA6, 2, ref recValue468);
                    return;
                case 234:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BA8, 2, ref recValue469);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BAA, 2, ref recValue470);
                    return;
                case 235:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BAC, 2, ref recValue471);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BAE, 2, ref recValue472);
                    return;
                case 236:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB0, 2, ref recValue473);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB2, 2, ref recValue474);
                    return;
                case 237:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB4, 2, ref recValue475);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB6, 2, ref recValue476);
                    return;
                case 238:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BB8, 2, ref recValue477);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BBA, 2, ref recValue478);
                    return;
                case 239:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BBC, 2, ref recValue479);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BBE, 2, ref recValue480);
                    return;
                case 240:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC0, 2, ref recValue481);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC2, 2, ref recValue482);
                    return;
                case 241:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC4, 2, ref recValue483);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC6, 2, ref recValue484);
                    return;
                case 242:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BC8, 2, ref recValue485);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BCA, 2, ref recValue486);
                    return;
                case 243:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BCC, 2, ref recValue487);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BCE, 2, ref recValue488);
                    return;
                case 244:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD0, 2, ref recValue489);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD2, 2, ref recValue490);
                    return;
                case 245:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD4, 2, ref recValue491);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD6, 2, ref recValue492);
                    return;
                case 246:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BD8, 2, ref recValue493);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BDA, 2, ref recValue494);
                    return;
                case 247:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BDC, 2, ref recValue495);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BDE, 2, ref recValue496);
                    return;
                case 248:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE0, 2, ref recValue497);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE2, 2, ref recValue498);
                    return;
                case 249:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE4, 2, ref recValue499);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE6, 2, ref recValue500);
                    return;
                case 250:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BE8, 2, ref recValue501);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BEA, 2, ref recValue502);
                    return;
                case 251:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BEC, 2, ref recValue503);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BEE, 2, ref recValue504);
                    return;
                case 252:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF0, 2, ref recValue505);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF2, 2, ref recValue506);
                    return;
                case 253:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF4, 2, ref recValue507);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF6, 2, ref recValue508);
                    return;
                case 254:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BF8, 2, ref recValue509);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BFA, 2, ref recValue510);
                    return;
                case 255:
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BFC, 2, ref recValue511);
                    modbusTCP.ReadHoldingRegister(0, (byte)axisNum1, 0x4BFE, 2, ref recValue512);
                    return;
            }
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
