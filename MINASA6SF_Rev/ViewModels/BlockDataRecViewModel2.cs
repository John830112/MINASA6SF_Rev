﻿using System;
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
    public partial class MainPanelViewModel
    {
        partial void BlockParameterRec2()
        {
            
            //101번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_203[1]);
                 if (Convert.ToInt32(parameter7_4byte1_203[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_204, 0);                    //블록데이터 구성
              
                BlockParaModel1s[101].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_204, 0);                           //블록데이터 구성

                BlockParaModel1s[101].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[101].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[101].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[101].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[101].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[101].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[101].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[101].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[101].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[101].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[101].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[101].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[101].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_204, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[101].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_203[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_203[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_203[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[101].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[101].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_204, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[101].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_204, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[101].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_203[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_203[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_203[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_203[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_204, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[101].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //102번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_205[1]);
                 if (Convert.ToInt32(parameter7_4byte1_205[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_206, 0);                    //블록데이터 구성

                BlockParaModel1s[102].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_206, 0);                           //블록데이터 구성

                BlockParaModel1s[102].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[102].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[102].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[102].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[102].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[102].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[102].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[102].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[102].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[102].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[102].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[102].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[102].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_206, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[102].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_205[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_205[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_205[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[102].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[102].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_206, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[102].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_206, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[102].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_205[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_205[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_205[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_205[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_206, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[102].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //103번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_207[1]);
                 if (Convert.ToInt32(parameter7_4byte1_207[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_208, 0);                    //블록데이터 구성

                BlockParaModel1s[103].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_208, 0);                           //블록데이터 구성

                BlockParaModel1s[103].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[103].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[103].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[103].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[103].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[103].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[103].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[103].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[103].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[103].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[103].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[103].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[103].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_208, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[103].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_207[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_207[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_207[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[103].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[103].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_208, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[103].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_208, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[103].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_207[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_207[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_207[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_207[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_208, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[103].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //104번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_209[1]);
                 if (Convert.ToInt32(parameter7_4byte1_209[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_210, 0);                    //블록데이터 구성

                BlockParaModel1s[104].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_210, 0);                           //블록데이터 구성

                BlockParaModel1s[104].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[104].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[104].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[104].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[104].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[104].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[104].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[104].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[104].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[104].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[104].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[104].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[104].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_210, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[104].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_209[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_209[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_209[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[104].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[104].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_210, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[104].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_210, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[104].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_209[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_209[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_209[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_209[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_210, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[104].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //105번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_211[1]);
                 if (Convert.ToInt32(parameter7_4byte1_211[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_212, 0);                    //블록데이터 구성

                BlockParaModel1s[105].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_212, 0);                           //블록데이터 구성

                BlockParaModel1s[105].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[105].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[105].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[105].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[105].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[105].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[105].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[105].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[105].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[105].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[105].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[105].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[105].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_212, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[105].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_211[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_211[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_211[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[105].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[105].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_212, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[105].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_212, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[105].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_211[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_211[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_211[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_211[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_212, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[105].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //106번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_213[1]);
                 if (Convert.ToInt32(parameter7_4byte1_213[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_214, 0);                    //블록데이터 구성

                BlockParaModel1s[106].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_214, 0);                           //블록데이터 구성

                BlockParaModel1s[106].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[106].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[106].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[106].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[106].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[106].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[106].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[106].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[106].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[106].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[106].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[106].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[106].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_214, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[106].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_213[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_213[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_213[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[106].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[106].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_214, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[106].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_214, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[106].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_213[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_213[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_213[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_213[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_214, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[106].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //107번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_215[1]);
                 if (Convert.ToInt32(parameter7_4byte1_215[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_216, 0);                    //블록데이터 구성

                BlockParaModel1s[107].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_216, 0);                           //블록데이터 구성

                BlockParaModel1s[107].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[107].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[107].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 4)                                      //원점복귀
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[0]) >> 4);                 //검출방법 hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);//방향     hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[107].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[107].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[107].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[107].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[107].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[107].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[107].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[107].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[107].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[107].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_216, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[107].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_215[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_215[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_215[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[107].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[107].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_216, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[107].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_216, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[107].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_215[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_215[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_215[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_215[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_216, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[107].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }

            //108번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_217[1]);
                 if (Convert.ToInt32(parameter7_4byte1_217[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_218, 0);                    //블록데이터 구성

                BlockParaModel1s[108].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_218, 0);                           //블록데이터 구성

                BlockParaModel1s[108].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[108].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[108].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[108].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[108].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[108].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[108].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[108].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[108].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[108].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[108].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[108].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[108].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_218, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[108].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_217[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_217[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_217[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[108].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[108].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_218, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[108].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_218, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[108].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_217[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_217[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_217[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_217[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_218, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[108].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //109번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_219[1]);
                 if (Convert.ToInt32(parameter7_4byte1_219[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_220, 0);                    //블록데이터 구성

                BlockParaModel1s[109].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_220, 0);                           //블록데이터 구성

                BlockParaModel1s[109].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[109].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[109].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 4)                                      //원점복귀
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[0]) >> 4);                 //검출방법 hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);//방향     hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[109].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[109].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[109].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[109].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[109].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[109].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[109].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[109].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[109].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[109].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_220, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[109].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_219[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_219[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_219[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[109].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[109].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_220, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[109].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_220, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[109].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_219[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_219[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_219[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_219[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_220, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[109].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //110번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_221[1]);
                 if (Convert.ToInt32(parameter7_4byte1_221[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_222, 0);                    //블록데이터 구성

                BlockParaModel1s[110].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_222, 0);                           //블록데이터 구성

                BlockParaModel1s[110].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[110].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[110].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[110].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[110].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[110].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[110].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[110].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[110].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[110].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[110].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[110].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[110].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_222, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[110].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_221[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_221[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_221[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[110].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[110].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_222, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[110].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_222, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[110].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_221[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_221[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_221[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_221[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_222, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[110].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //111번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_223[1]);
                 if (Convert.ToInt32(parameter7_4byte1_223[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_224, 0);                    //블록데이터 구성

                BlockParaModel1s[111].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_224, 0);                           //블록데이터 구성

                BlockParaModel1s[111].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[111].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[111].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[111].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[111].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[111].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[111].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[111].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[111].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[111].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[111].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[111].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[111].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_224, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[111].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_223[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_223[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_223[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[111].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[111].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_224, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[111].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_224, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[111].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_223[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_223[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_223[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_223[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_224, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[111].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //112번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_225[1]);
                 if (Convert.ToInt32(parameter7_4byte1_225[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_226, 0);                    //블록데이터 구성

                BlockParaModel1s[112].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_226, 0);                           //블록데이터 구성

                BlockParaModel1s[112].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[112].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[112].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[112].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[112].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[112].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[112].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[112].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[112].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[112].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[112].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[112].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[112].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_226, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[112].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_225[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_225[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_225[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[112].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[112].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_226, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[112].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_226, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[112].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_225[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_225[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_225[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_225[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_226, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[112].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //113번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_227[1]);
                 if (Convert.ToInt32(parameter7_4byte1_227[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_228, 0);                    //블록데이터 구성

                BlockParaModel1s[113].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_228, 0);                           //블록데이터 구성

                BlockParaModel1s[113].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[113].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[113].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[113].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[113].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[113].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[113].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[113].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[113].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[113].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[113].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[113].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[113].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_228, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[113].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_227[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_227[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_227[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[113].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[113].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_228, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[113].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_228, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[113].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_227[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_227[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_227[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_227[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_228, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[113].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //114번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_229[1]);
                 if (Convert.ToInt32(parameter7_4byte1_229[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_230, 0);                    //블록데이터 구성

                BlockParaModel1s[114].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_230, 0);                           //블록데이터 구성

                BlockParaModel1s[114].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[114].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[114].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[114].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[114].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[114].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[114].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[114].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[114].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[114].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[114].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[114].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[114].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_230, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[114].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_229[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_229[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_229[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[114].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[114].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_230, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[114].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_230, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[114].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_229[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_229[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_229[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_229[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_230, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[114].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //115번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_231[1]);
                 if (Convert.ToInt32(parameter7_4byte1_231[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_232, 0);                    //블록데이터 구성

                BlockParaModel1s[115].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_232, 0);                           //블록데이터 구성

                BlockParaModel1s[115].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[115].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[115].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 4)                                      //원점복귀
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[0]) >> 4);                 //검출방법 hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);//방향     hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[115].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[115].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[115].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[115].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[115].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[115].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[115].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[115].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 6)                                       //속도갱신
            {
                   SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[0]) >> 4);                 //속도번호  hiki1
                  Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[115].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[115].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 7)                                       //디크리멘트 카운트 기동
            {
               BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);       //천이조건 hiki5
              TargetPosition = BitConverter.ToInt32(parameter7_4byte1_232, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[115].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_231[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_231[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_231[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[115].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[115].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_232, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[115].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_232, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[115].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_231[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_231[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_231[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_231[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_232, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[115].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //116번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_233[1]);
                 if (Convert.ToInt32(parameter7_4byte1_233[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_234, 0);                    //블록데이터 구성

                BlockParaModel1s[116].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_234, 0);                           //블록데이터 구성

                BlockParaModel1s[116].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[116].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[116].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[116].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[116].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[116].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[116].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[116].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[116].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[116].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[116].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[116].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[116].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_234, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[116].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_233[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_233[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_233[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[116].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[116].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_234, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[116].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_234, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[116].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_233[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_233[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_233[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_233[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_234, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[116].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //117번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_235[1]);
                 if (Convert.ToInt32(parameter7_4byte1_235[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_236, 0);                    //블록데이터 구성

                BlockParaModel1s[117].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_236, 0);                           //블록데이터 구성

                BlockParaModel1s[117].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[117].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[117].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[117].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[117].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[117].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[117].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[117].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[117].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[117].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[117].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[117].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[117].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_236, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[117].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_235[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_235[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_235[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[117].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[117].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_236, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[117].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_236, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[117].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_235[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_235[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_235[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_235[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_236, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[117].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //118번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_237[1]);
                 if (Convert.ToInt32(parameter7_4byte1_237[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_238, 0);                    //블록데이터 구성

                BlockParaModel1s[118].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_238, 0);                           //블록데이터 구성

                BlockParaModel1s[118].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[118].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[118].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[118].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[118].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[118].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[118].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[118].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[118].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[118].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[118].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[118].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[118].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_238, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[118].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_237[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_237[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_237[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[118].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);          //천이조건 hiki5
                  
                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[118].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_238, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[118].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_238, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[118].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_237[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_237[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_237[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_237[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_238, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[118].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //119번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_239[1]);
                 if (Convert.ToInt32(parameter7_4byte1_239[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_240, 0);                    //블록데이터 구성

                BlockParaModel1s[119].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_240, 0);                           //블록데이터 구성

                BlockParaModel1s[119].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[119].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[119].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[119].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[119].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[119].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[119].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[119].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[119].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[119].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[119].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[119].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[119].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_240, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[119].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_239[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_239[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_239[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[119].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[119].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_240, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[119].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_240, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[119].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_239[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_239[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_239[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_239[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_240, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[119].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //120번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_241[1]);
                 if (Convert.ToInt32(parameter7_4byte1_241[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_242, 0);                    //블록데이터 구성

                BlockParaModel1s[120].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_242, 0);                           //블록데이터 구성

                BlockParaModel1s[120].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[120].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[120].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 4)                                      //원점복귀
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[0]) >> 4);                 //검출방법 hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);//방향     hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[120].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[120].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[120].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[120].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[120].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[120].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[120].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[120].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[120].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[120].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_242, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[120].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_241[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_241[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_241[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[120].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[120].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_242, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[120].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_242, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[120].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_241[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_241[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_241[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_241[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_242, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[120].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //121번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_243[1]);
                 if (Convert.ToInt32(parameter7_4byte1_243[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_244, 0);                    //블록데이터 구성

                BlockParaModel1s[121].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_244, 0);                           //블록데이터 구성

                BlockParaModel1s[121].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[121].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[121].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[121].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[121].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[121].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[121].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[121].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[121].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[121].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[121].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[121].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[121].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_244, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[121].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_243[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_243[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_243[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[121].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[121].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_244, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[121].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_244, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[121].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_243[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_243[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_243[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_243[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_244, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[121].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //122번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_245[1]);
                 if (Convert.ToInt32(parameter7_4byte1_245[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_246, 0);                    //블록데이터 구성

                BlockParaModel1s[122].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_246, 0);                           //블록데이터 구성

                BlockParaModel1s[122].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[122].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[122].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[122].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[122].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[122].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[122].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[122].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[122].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[122].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[122].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[122].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[122].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_246, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[122].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_245[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_245[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_245[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[122].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[122].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_246, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[122].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_246, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[122].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_245[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_245[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_245[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_245[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_246, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[122].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //123번 블록
           cmdCode = Convert.ToInt32(parameter7_4byte1_247[1]);
                 if (Convert.ToInt32(parameter7_4byte1_247[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                    //블록데이터 구성

                BlockParaModel1s[123].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);  

                BlockParaModel1s[123].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[123].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[123].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[123].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[123].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[123].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[123].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[123].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[123].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[123].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[123].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[123].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[123].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_247[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_247[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_247[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이 조건hiki5

                OutPutsignalcombo1 = (int)(((b_CTRL1_2) & 0b_1100) >> 2);
                OutPutsignalcombo2 = (int)((b_CTRL1_2) & 0b_0011);
                OutPutsignalcombo3 = (int)(((b_CTRL3_4) & 0b_1100) >> 2);
                OutPutsignalcombo4 = (int)((b_CTRL3_4) & 0b_0011);
                OutPutsignalcombo5 = (int)(((b_CTRL5_6) & 0b_1100) >> 2);
                OutPutsignalcombo6 = (int)((b_CTRL5_6) & 0b_0011);

                string bctrl1 = "";
                string bctrl2 = "";
                string bctrl3 = "";
                string bctrl4 = "";
                string bctrl5 = "";
                string bctrl6 = "";

                switch (OutPutsignalcombo1)
                {
                    case 0:
                        bctrl1 = "유지";
                        break;
                    case 2:
                        bctrl1 = "오프";
                        break;
                    case 3:
                        bctrl1 = "온";
                        break;
                }

                switch (OutPutsignalcombo2)
                {
                    case 0:
                        bctrl2 = "유지";
                        break;
                    case 2:
                        bctrl2 = "오프";
                        break;
                    case 3:
                        bctrl2 = "온";
                        break;
                }

                switch (OutPutsignalcombo3)
                {
                    case 0:
                        bctrl3 = "유지";
                        break;
                    case 2:
                        bctrl3 = "오프";
                        break;
                    case 3:
                        bctrl3 = "온";
                        break;
                }

                switch (OutPutsignalcombo4)
                {
                    case 0:
                        bctrl4 = "유지";
                        break;
                    case 2:
                        bctrl4 = "오프";
                        break;
                    case 3:
                        bctrl4 = "온";
                        break;
                }

                switch (OutPutsignalcombo5)
                {
                    case 0:
                        bctrl5 = "유지";
                        break;
                    case 2:
                        bctrl5 = "오프";
                        break;
                    case 3:
                        bctrl5 = "온";
                        break;
                }

                switch (OutPutsignalcombo6)
                {
                    case 0:
                        bctrl6 = "유지";
                        break;
                    case 2:
                        bctrl6 = "오프";
                        break;
                    case 3:
                        bctrl6 = "온";
                        break;
                }

                BlockParaModel1s[123].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 9)                                       //점프
            {
                 ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) & 0b_0000_1111); // hiki2
                 ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[3]) >> 4);           // hiki3
                ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);  //   hiki4
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[123].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[123].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);
                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[123].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_247[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_247[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_247[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                     //비교값   hiki7

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                string comp = "";
                switch (SpdNum)
                {
                    case 0:
                        comp = "지령위치";
                        break;
                    case 1:
                        comp = "현재위치";
                        break;
                    case 2:
                        comp = "위치편차";
                        break;
                    case 3:
                        comp = "지령속도";
                        break;
                    case 4:
                        comp = "모터속도";
                        break;
                    case 5:
                        comp = "지령토크";
                        break;
                    case 6:
                        comp = "디크리멘트카운트";
                        break;
                    case 7:
                        comp = "입력신호";
                        break;
                    case 8:
                        comp = "출력신호";
                        break;
                }

                BlockParaModel1s[123].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }

            Debug.WriteLine("123번 실행 완료");

        }
    }
}