using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.ViewModels
{
    public partial class MainPanelViewModel
    {
        partial void BlockParameterRec3()
        {

            //145
            cmdCode = Convert.ToInt32(parameter7_4byte1_291[1]);
            if (Convert.ToInt32(parameter7_4byte1_291[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_292, 0);                    //블록데이터 구성

                BlockParaModel1s[145].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_292, 0);

                BlockParaModel1s[145].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[145].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[145].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[145].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[145].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[145].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[145].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[145].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[145].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[145].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[145].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[145].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[145].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_292, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[145].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_291[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_291[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_291[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[145].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[145].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_292, 0);                     //비교값   hiki7

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

                BlockParaModel1s[145].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_292, 0);                     //비교값   hiki7

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

                BlockParaModel1s[145].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_291[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_291[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_291[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_291[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_292, 0);                     //비교값   hiki7

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

                BlockParaModel1s[145].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //146
            cmdCode = Convert.ToInt32(parameter7_4byte1_293[1]);
            if (Convert.ToInt32(parameter7_4byte1_293[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_294, 0);                    //블록데이터 구성

                BlockParaModel1s[146].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_294, 0);

                BlockParaModel1s[146].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[146].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[146].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[146].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[146].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[146].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[146].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[146].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[146].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[146].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[146].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[146].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[146].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_294, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[146].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_293[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_293[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_293[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[146].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[146].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_294, 0);                     //비교값   hiki7

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

                BlockParaModel1s[146].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_294, 0);                     //비교값   hiki7

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

                BlockParaModel1s[146].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_293[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_293[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_293[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_293[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_294, 0);                     //비교값   hiki7

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

                BlockParaModel1s[146].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //147
            cmdCode = Convert.ToInt32(parameter7_4byte1_295[1]);
            if (Convert.ToInt32(parameter7_4byte1_295[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_296, 0);                    //블록데이터 구성

                BlockParaModel1s[147].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_296, 0);

                BlockParaModel1s[147].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[147].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[147].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[147].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[147].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[147].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[147].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[147].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[147].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[147].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[147].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[147].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[147].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_296, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[147].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_295[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_295[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_295[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[147].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[147].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_296, 0);                     //비교값   hiki7

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

                BlockParaModel1s[147].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_296, 0);                     //비교값   hiki7

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

                BlockParaModel1s[147].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_295[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_295[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_295[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_295[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_296, 0);                     //비교값   hiki7

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

                BlockParaModel1s[147].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //148
            cmdCode = Convert.ToInt32(parameter7_4byte1_297[1]);
            if (Convert.ToInt32(parameter7_4byte1_297[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_298, 0);                    //블록데이터 구성

                BlockParaModel1s[148].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_298, 0);

                BlockParaModel1s[148].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[148].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[148].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[148].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[148].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[148].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[148].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[148].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[148].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[148].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[148].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[148].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[148].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_298, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[148].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_297[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_297[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_297[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[148].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[148].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_298, 0);                     //비교값   hiki7

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

                BlockParaModel1s[148].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_298, 0);                     //비교값   hiki7

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

                BlockParaModel1s[148].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_297[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_297[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_297[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_297[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_298, 0);                     //비교값   hiki7

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

                BlockParaModel1s[148].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //149
            cmdCode = Convert.ToInt32(parameter7_4byte1_299[1]);
            if (Convert.ToInt32(parameter7_4byte1_299[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_300, 0);                    //블록데이터 구성

                BlockParaModel1s[149].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_300, 0);

                BlockParaModel1s[149].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[149].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[149].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[149].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[149].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[149].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[149].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[149].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[149].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[149].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[149].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[149].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[149].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 7)                                       //디크리멘트 카운트 기동
            {
               BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);       //천이조건 hiki5
              TargetPosition = BitConverter.ToInt32(parameter7_4byte1_300, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[149].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_299[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_299[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_299[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[149].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[149].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_300, 0);                     //비교값   hiki7

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

                BlockParaModel1s[149].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_300, 0);                     //비교값   hiki7

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

                BlockParaModel1s[149].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_299[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_299[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_299[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_299[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_300, 0);                     //비교값   hiki7

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

                BlockParaModel1s[149].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //150
            cmdCode = Convert.ToInt32(parameter7_4byte1_301[1]);
            if (Convert.ToInt32(parameter7_4byte1_301[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_302, 0);                    //블록데이터 구성

                BlockParaModel1s[150].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_302, 0);

                BlockParaModel1s[150].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[150].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[150].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[150].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[150].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[150].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[150].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[150].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[150].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[150].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[150].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[150].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[150].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_302, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[150].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_301[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_301[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_301[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[150].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[150].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_302, 0);                     //비교값   hiki7

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

                BlockParaModel1s[150].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_302, 0);                     //비교값   hiki7

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

                BlockParaModel1s[150].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_301[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_301[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_301[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_301[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_302, 0);                     //비교값   hiki7

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

                BlockParaModel1s[150].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }





            //151
            cmdCode = Convert.ToInt32(parameter7_4byte1_303[1]);
            if (Convert.ToInt32(parameter7_4byte1_303[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_304, 0);                    //블록데이터 구성

                BlockParaModel1s[151].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_304, 0);

                BlockParaModel1s[151].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[151].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[151].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[151].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[151].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[151].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[151].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[151].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[151].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[151].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[151].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[151].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[151].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_304, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[151].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_303[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_303[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_303[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[151].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[151].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_304, 0);                     //비교값   hiki7

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

                BlockParaModel1s[151].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_304, 0);                     //비교값   hiki7

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

                BlockParaModel1s[151].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_303[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_303[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_303[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_303[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_304, 0);                     //비교값   hiki7

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

                BlockParaModel1s[151].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //152
            cmdCode = Convert.ToInt32(parameter7_4byte1_305[1]);
            if (Convert.ToInt32(parameter7_4byte1_305[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_306, 0);                    //블록데이터 구성

                BlockParaModel1s[152].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_306, 0);

                BlockParaModel1s[152].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[152].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[152].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[152].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[152].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[152].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[152].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[152].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[152].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[152].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[152].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[152].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[152].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_306, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[152].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_305[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_305[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_305[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[152].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[152].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_306, 0);                     //비교값   hiki7

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

                BlockParaModel1s[152].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_306, 0);                     //비교값   hiki7

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

                BlockParaModel1s[152].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_305[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_305[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_305[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_305[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_306, 0);                     //비교값   hiki7

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

                BlockParaModel1s[152].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //153
            cmdCode = Convert.ToInt32(parameter7_4byte1_307[1]);
            if (Convert.ToInt32(parameter7_4byte1_307[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_308, 0);                    //블록데이터 구성

                BlockParaModel1s[153].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_308, 0);

                BlockParaModel1s[153].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[153].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[153].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[153].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[153].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[153].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[153].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[153].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[153].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[153].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[153].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[153].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[153].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_308, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[153].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_307[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_307[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_307[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[153].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[153].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_308, 0);                     //비교값   hiki7

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

                BlockParaModel1s[153].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_308, 0);                     //비교값   hiki7

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

                BlockParaModel1s[153].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_307[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_307[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_307[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_307[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_308, 0);                     //비교값   hiki7

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

                BlockParaModel1s[153].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //154
            cmdCode = Convert.ToInt32(parameter7_4byte1_309[1]);
            if (Convert.ToInt32(parameter7_4byte1_309[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_310, 0);                    //블록데이터 구성

                BlockParaModel1s[154].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_310, 0);

                BlockParaModel1s[154].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[154].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[154].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 4)                                      //원점복귀
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[0]) >> 4);                 //검출방법 hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);//방향     hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[154].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[154].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[154].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[154].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[154].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[154].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[154].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[154].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[154].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[154].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_310, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[154].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_309[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_309[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_309[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[154].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[154].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_310, 0);                     //비교값   hiki7

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

                BlockParaModel1s[154].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_310, 0);                     //비교값   hiki7

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

                BlockParaModel1s[154].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_309[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_309[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_309[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_309[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_310, 0);                     //비교값   hiki7

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

                BlockParaModel1s[154].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //155
            cmdCode = Convert.ToInt32(parameter7_4byte1_311[1]);
            if (Convert.ToInt32(parameter7_4byte1_311[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_312, 0);                    //블록데이터 구성

                BlockParaModel1s[155].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_312, 0);

                BlockParaModel1s[155].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[155].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[155].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[155].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[155].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[155].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[155].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[155].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[155].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[155].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[155].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[155].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[155].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_312, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[155].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_311[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_311[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_311[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[155].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[155].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_312, 0);                     //비교값   hiki7

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

                BlockParaModel1s[155].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_312, 0);                     //비교값   hiki7

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

                BlockParaModel1s[155].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_311[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_311[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_311[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_311[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_312, 0);                     //비교값   hiki7

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

                BlockParaModel1s[155].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //156
            cmdCode = Convert.ToInt32(parameter7_4byte1_313[1]);
            if (Convert.ToInt32(parameter7_4byte1_313[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_314, 0);                    //블록데이터 구성

                BlockParaModel1s[156].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_314, 0);

                BlockParaModel1s[156].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[156].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[156].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 4)                                      //원점복귀
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[0]) >> 4);                 //검출방법 hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);//방향     hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[156].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[156].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[156].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[156].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[156].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[156].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[156].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[156].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[156].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[156].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_314, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[156].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_313[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_313[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_313[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[156].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[156].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_314, 0);                     //비교값   hiki7

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

                BlockParaModel1s[156].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_314, 0);                     //비교값   hiki7

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

                BlockParaModel1s[156].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_313[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_313[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_313[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_313[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_314, 0);                     //비교값   hiki7

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

                BlockParaModel1s[156].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //157
            cmdCode = Convert.ToInt32(parameter7_4byte1_315[1]);
            if (Convert.ToInt32(parameter7_4byte1_315[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_316, 0);                    //블록데이터 구성

                BlockParaModel1s[157].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_316, 0);

                BlockParaModel1s[157].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[157].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[157].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[157].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[157].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[157].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[157].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[157].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[157].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[157].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[157].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[157].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[157].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_316, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[157].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_315[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_315[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_315[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[157].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[157].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_316, 0);                     //비교값   hiki7

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

                BlockParaModel1s[157].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_316, 0);                     //비교값   hiki7

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

                BlockParaModel1s[157].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_315[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_315[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_315[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_315[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_316, 0);                     //비교값   hiki7

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

                BlockParaModel1s[157].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //158
            cmdCode = Convert.ToInt32(parameter7_4byte1_317[1]);
            if (Convert.ToInt32(parameter7_4byte1_317[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_318, 0);                    //블록데이터 구성

                BlockParaModel1s[158].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_318, 0);

                BlockParaModel1s[158].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[158].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[158].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 4)                                      //원점복귀
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[0]) >> 4);                 //검출방법 hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);//방향     hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[158].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[158].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[158].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[158].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[158].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[158].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[158].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[158].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 6)                                       //속도갱신
            {
                   SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[0]) >> 4);                 //속도번호  hiki1
                  Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[158].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[158].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_318, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[158].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_317[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_317[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_317[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[158].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[158].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_318, 0);                     //비교값   hiki7

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

                BlockParaModel1s[158].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_318, 0);                     //비교값   hiki7

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

                BlockParaModel1s[158].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_317[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_317[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_317[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_317[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_318, 0);                     //비교값   hiki7

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

                BlockParaModel1s[158].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //159
            cmdCode = Convert.ToInt32(parameter7_4byte1_319[1]);
            if (Convert.ToInt32(parameter7_4byte1_319[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_320, 0);                    //블록데이터 구성

                BlockParaModel1s[159].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_320, 0);

                BlockParaModel1s[159].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[159].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[159].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[159].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[159].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[159].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[159].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[159].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[159].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[159].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[159].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[159].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[159].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_320, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[159].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_319[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_319[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_319[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[159].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[159].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_320, 0);                     //비교값   hiki7

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

                BlockParaModel1s[159].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_320, 0);                     //비교값   hiki7

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

                BlockParaModel1s[159].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_319[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_319[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_319[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_319[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_320, 0);                     //비교값   hiki7

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

                BlockParaModel1s[159].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }







            //160
            cmdCode = Convert.ToInt32(parameter7_4byte1_321[1]);
            if (Convert.ToInt32(parameter7_4byte1_321[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_322, 0);                    //블록데이터 구성

                BlockParaModel1s[160].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_322, 0);

                BlockParaModel1s[160].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[160].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[160].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[160].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[160].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[160].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[160].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[160].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[160].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[160].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[160].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[160].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[160].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_322, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[160].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_321[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_321[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_321[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[160].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[160].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_322, 0);                     //비교값   hiki7

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

                BlockParaModel1s[160].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_322, 0);                     //비교값   hiki7

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

                BlockParaModel1s[160].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_321[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_321[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_321[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_321[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_322, 0);                     //비교값   hiki7

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

                BlockParaModel1s[160].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //161
            cmdCode = Convert.ToInt32(parameter7_4byte1_323[1]);
            if (Convert.ToInt32(parameter7_4byte1_323[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_324, 0);                    //블록데이터 구성

                BlockParaModel1s[161].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_324, 0);

                BlockParaModel1s[161].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[161].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[161].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[161].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[161].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[161].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[161].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[161].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[161].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[161].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[161].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[161].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[161].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_324, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[161].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_323[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_323[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_323[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[161].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[161].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_324, 0);                     //비교값   hiki7

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

                BlockParaModel1s[161].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_324, 0);                     //비교값   hiki7

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

                BlockParaModel1s[161].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_323[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_323[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_323[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_323[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_324, 0);                     //비교값   hiki7

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

                BlockParaModel1s[161].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //162
            cmdCode = Convert.ToInt32(parameter7_4byte1_325[1]);
            if (Convert.ToInt32(parameter7_4byte1_325[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_326, 0);                    //블록데이터 구성

                BlockParaModel1s[162].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_326, 0);

                BlockParaModel1s[162].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[162].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[162].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[162].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[162].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[162].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[162].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[162].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[162].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[162].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[162].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[162].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[162].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_326, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[162].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_325[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_325[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_325[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[162].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[162].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_326, 0);                     //비교값   hiki7

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

                BlockParaModel1s[162].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_326, 0);                     //비교값   hiki7

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

                BlockParaModel1s[162].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_325[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_325[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_325[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_325[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_326, 0);                     //비교값   hiki7

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

                BlockParaModel1s[162].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //163
            cmdCode = Convert.ToInt32(parameter7_4byte1_327[1]);
            if (Convert.ToInt32(parameter7_4byte1_327[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_328, 0);                    //블록데이터 구성

                BlockParaModel1s[163].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_328, 0);

                BlockParaModel1s[163].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[163].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[163].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[163].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[163].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[163].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[163].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[163].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[163].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[163].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[163].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[163].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[163].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_328, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[163].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_327[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_327[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_327[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[163].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[163].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_328, 0);                     //비교값   hiki7

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

                BlockParaModel1s[163].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_328, 0);                     //비교값   hiki7

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

                BlockParaModel1s[163].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_327[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_327[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_327[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_327[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_328, 0);                     //비교값   hiki7

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

                BlockParaModel1s[163].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //164
            cmdCode = Convert.ToInt32(parameter7_4byte1_329[1]);
            if (Convert.ToInt32(parameter7_4byte1_329[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_330, 0);                    //블록데이터 구성

                BlockParaModel1s[164].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_330, 0);

                BlockParaModel1s[164].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[164].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[164].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[164].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[164].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[164].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[164].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[164].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[164].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[164].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[164].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[164].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[164].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_330, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[164].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_329[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_329[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_329[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[164].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[164].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_330, 0);                     //비교값   hiki7

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

                BlockParaModel1s[164].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_330, 0);                     //비교값   hiki7

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

                BlockParaModel1s[164].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_329[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_329[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_329[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_329[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_330, 0);                     //비교값   hiki7

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

                BlockParaModel1s[164].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //165
            cmdCode = Convert.ToInt32(parameter7_4byte1_331[1]);
            if (Convert.ToInt32(parameter7_4byte1_331[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_332, 0);                    //블록데이터 구성

                BlockParaModel1s[165].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_332, 0);

                BlockParaModel1s[165].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[165].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[165].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[165].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[165].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[165].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[165].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[165].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[165].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[165].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[165].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[165].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[165].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_332, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[165].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_331[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_331[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_331[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[165].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[165].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_332, 0);                     //비교값   hiki7

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

                BlockParaModel1s[165].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_332, 0);                     //비교값   hiki7

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

                BlockParaModel1s[165].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_331[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_331[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_331[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_331[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_332, 0);                     //비교값   hiki7

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

                BlockParaModel1s[165].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //166
            cmdCode = Convert.ToInt32(parameter7_4byte1_333[1]);
            if (Convert.ToInt32(parameter7_4byte1_333[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_334, 0);                    //블록데이터 구성

                BlockParaModel1s[166].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_334, 0);

                BlockParaModel1s[166].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[166].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[166].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[166].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[166].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[166].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[166].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[166].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[166].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[166].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[166].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[166].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[166].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_334, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[166].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_333[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_333[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_333[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[166].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[166].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_334, 0);                     //비교값   hiki7

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

                BlockParaModel1s[166].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);  // hiki4   
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);      //천이조건 hiki5
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) >> 4);                      // 비교대상  hiki1
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_334, 0);                     //비교값   hiki7

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

                BlockParaModel1s[166].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_333[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_333[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_333[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_333[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_334, 0);                     //비교값   hiki7

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

                BlockParaModel1s[166].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //167
            cmdCode = Convert.ToInt32(parameter7_4byte1_335[1]);
            if (Convert.ToInt32(parameter7_4byte1_335[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_336, 0);                    //블록데이터 구성

                BlockParaModel1s[167].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_336, 0);

                BlockParaModel1s[167].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[167].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[167].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[167].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[167].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[167].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[167].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[167].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[167].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[167].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[167].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[167].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[167].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_336, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[167].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_335[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_335[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_335[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[167].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[167].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_336, 0);                     //비교값   hiki7

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

                BlockParaModel1s[167].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_336, 0);                     //비교값   hiki7

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

                BlockParaModel1s[167].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_335[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_335[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_335[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_335[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_336, 0);                     //비교값   hiki7

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

                BlockParaModel1s[167].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //168
            cmdCode = Convert.ToInt32(parameter7_4byte1_337[1]);
            if (Convert.ToInt32(parameter7_4byte1_337[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_338, 0);                    //블록데이터 구성

                BlockParaModel1s[168].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_338, 0);

                BlockParaModel1s[168].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[168].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[168].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[168].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[168].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[168].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[168].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[168].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[168].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[168].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[168].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[168].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[168].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_338, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[168].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_337[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_337[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_337[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[168].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[168].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_338, 0);                     //비교값   hiki7

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

                BlockParaModel1s[168].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_338, 0);                     //비교값   hiki7

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

                BlockParaModel1s[168].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_337[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_337[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_337[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_337[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_338, 0);                     //비교값   hiki7

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

                BlockParaModel1s[168].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //169
            cmdCode = Convert.ToInt32(parameter7_4byte1_339[1]);
            if (Convert.ToInt32(parameter7_4byte1_339[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_340, 0);                    //블록데이터 구성

                BlockParaModel1s[169].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_340, 0);

                BlockParaModel1s[169].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[169].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[169].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[169].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[169].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[169].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[169].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[169].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[169].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[169].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[169].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[169].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[169].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_340, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[169].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_339[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_339[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_339[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[169].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[169].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_340, 0);                     //비교값   hiki7

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

                BlockParaModel1s[169].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_340, 0);                     //비교값   hiki7

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

                BlockParaModel1s[169].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_339[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_339[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_339[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_339[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_340, 0);                     //비교값   hiki7

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

                BlockParaModel1s[169].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //170
            cmdCode = Convert.ToInt32(parameter7_4byte1_341[1]);
            if (Convert.ToInt32(parameter7_4byte1_341[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_342, 0);                    //블록데이터 구성

                BlockParaModel1s[170].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_342, 0);

                BlockParaModel1s[170].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[170].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[170].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[170].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[170].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[170].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[170].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[170].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[170].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[170].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[170].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[170].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[170].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_342, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[170].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_341[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_341[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_341[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[170].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[170].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_342, 0);                     //비교값   hiki7

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

                BlockParaModel1s[170].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_342, 0);                     //비교값   hiki7

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

                BlockParaModel1s[170].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_341[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_341[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_341[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_341[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_342, 0);                     //비교값   hiki7

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

                BlockParaModel1s[170].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //171
            cmdCode = Convert.ToInt32(parameter7_4byte1_343[1]);
            if (Convert.ToInt32(parameter7_4byte1_343[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_344, 0);                    //블록데이터 구성

                BlockParaModel1s[171].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_344, 0);

                BlockParaModel1s[171].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[171].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[171].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[171].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[171].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[171].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[171].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[171].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[171].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[171].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[171].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[171].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[171].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_344, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[171].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_343[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_343[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_343[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[171].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[171].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_344, 0);                     //비교값   hiki7

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

                BlockParaModel1s[171].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_344, 0);                     //비교값   hiki7

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

                BlockParaModel1s[171].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_343[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_343[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_343[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_343[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_344, 0);                     //비교값   hiki7

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

                BlockParaModel1s[171].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //172
            cmdCode = Convert.ToInt32(parameter7_4byte1_345[1]);
            if (Convert.ToInt32(parameter7_4byte1_345[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_346, 0);                    //블록데이터 구성

                BlockParaModel1s[172].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_346, 0);

                BlockParaModel1s[172].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[172].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[172].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[172].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[172].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[172].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[172].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[172].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[172].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[172].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[172].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 6)                                       //속도갱신
            {
                 SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[0]) >> 4);                 //속도번호  hiki1
                Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
              BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[172].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[172].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_346, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[172].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_345[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_345[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_345[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[172].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[172].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_346, 0);                     //비교값   hiki7

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

                BlockParaModel1s[172].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_346, 0);                     //비교값   hiki7

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

                BlockParaModel1s[172].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_345[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_345[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_345[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_345[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_346, 0);                     //비교값   hiki7

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

                BlockParaModel1s[172].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //173
            cmdCode = Convert.ToInt32(parameter7_4byte1_347[1]);
            if (Convert.ToInt32(parameter7_4byte1_347[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_348, 0);                    //블록데이터 구성

                BlockParaModel1s[173].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_348, 0);

                BlockParaModel1s[173].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[173].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[173].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[173].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[173].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[173].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[173].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[173].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[173].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[173].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[173].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[173].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[173].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_348, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[173].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_347[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_347[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_347[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[173].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[173].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_348, 0);                     //비교값   hiki7

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

                BlockParaModel1s[173].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_348, 0);                     //비교값   hiki7

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

                BlockParaModel1s[173].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_347[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_347[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_347[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_347[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_348, 0);                     //비교값   hiki7

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

                BlockParaModel1s[173].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //174
            cmdCode = Convert.ToInt32(parameter7_4byte1_349[1]);
            if (Convert.ToInt32(parameter7_4byte1_349[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_350, 0);                    //블록데이터 구성

                BlockParaModel1s[174].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_350, 0);

                BlockParaModel1s[174].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[174].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[174].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[174].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[174].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[174].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[174].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[174].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[174].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[174].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[174].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[174].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[174].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_350, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[174].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_349[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_349[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_349[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[174].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[174].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_350, 0);                     //비교값   hiki7

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

                BlockParaModel1s[174].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_350, 0);                     //비교값   hiki7

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

                BlockParaModel1s[174].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_349[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_349[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_349[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_349[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_350, 0);                     //비교값   hiki7

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

                BlockParaModel1s[174].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //175
            cmdCode = Convert.ToInt32(parameter7_4byte1_351[1]);
            if (Convert.ToInt32(parameter7_4byte1_351[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_352, 0);                    //블록데이터 구성

                BlockParaModel1s[175].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_352, 0);

                BlockParaModel1s[175].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[175].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[175].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[175].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[175].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[175].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[175].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[175].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[175].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[175].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[175].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[175].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[175].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_352, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[175].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_351[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_351[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_351[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[175].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[175].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_352, 0);                     //비교값   hiki7

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

                BlockParaModel1s[175].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_352, 0);                     //비교값   hiki7

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

                BlockParaModel1s[175].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_351[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_351[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_351[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_351[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_352, 0);                     //비교값   hiki7

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

                BlockParaModel1s[175].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //176
            cmdCode = Convert.ToInt32(parameter7_4byte1_353[1]);
            if (Convert.ToInt32(parameter7_4byte1_353[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_354, 0);                    //블록데이터 구성

                BlockParaModel1s[176].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_354, 0);

                BlockParaModel1s[176].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[176].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[176].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[176].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[176].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[176].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[176].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[176].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[176].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[176].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[176].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[176].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[176].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_354, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[176].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_353[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_353[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_353[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[176].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[176].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_354, 0);                     //비교값   hiki7

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

                BlockParaModel1s[176].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_354, 0);                     //비교값   hiki7

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

                BlockParaModel1s[176].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_353[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_353[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_353[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_353[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_354, 0);                     //비교값   hiki7

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

                BlockParaModel1s[176].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //177
            cmdCode = Convert.ToInt32(parameter7_4byte1_355[1]);
            if (Convert.ToInt32(parameter7_4byte1_355[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_356, 0);                    //블록데이터 구성

                BlockParaModel1s[177].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_356, 0);

                BlockParaModel1s[177].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[177].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[177].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[177].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[177].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[177].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[177].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[177].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[177].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[177].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[177].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[177].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[177].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_356, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[177].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_355[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_355[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_355[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[177].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[177].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_356, 0);                     //비교값   hiki7

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

                BlockParaModel1s[177].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_356, 0);                     //비교값   hiki7

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

                BlockParaModel1s[177].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_355[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_355[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_355[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_355[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_356, 0);                     //비교값   hiki7

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

                BlockParaModel1s[177].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //178
            cmdCode = Convert.ToInt32(parameter7_4byte1_357[1]);
            if (Convert.ToInt32(parameter7_4byte1_357[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_358, 0);                    //블록데이터 구성

                BlockParaModel1s[178].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_358, 0);

                BlockParaModel1s[178].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[178].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[178].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[178].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[178].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[178].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[178].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[178].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[178].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[178].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[178].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[178].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[178].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_358, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[178].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_357[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_357[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_357[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[178].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[178].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_358, 0);                     //비교값   hiki7

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

                BlockParaModel1s[178].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_358, 0);                     //비교값   hiki7

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

                BlockParaModel1s[178].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_357[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_357[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_357[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_357[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_358, 0);                     //비교값   hiki7

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

                BlockParaModel1s[178].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //179
            cmdCode = Convert.ToInt32(parameter7_4byte1_359[1]);
            if (Convert.ToInt32(parameter7_4byte1_359[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_360, 0);                    //블록데이터 구성

                BlockParaModel1s[179].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_360, 0);

                BlockParaModel1s[179].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[179].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[179].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[179].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[179].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[179].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[179].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[179].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[179].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[179].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[179].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[179].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[179].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_360, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[179].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_359[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_359[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_359[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[179].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[179].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_360, 0);                     //비교값   hiki7

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

                BlockParaModel1s[179].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_360, 0);                     //비교값   hiki7

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

                BlockParaModel1s[179].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_359[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_359[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_359[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_359[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_360, 0);                     //비교값   hiki7

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

                BlockParaModel1s[179].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //180
            cmdCode = Convert.ToInt32(parameter7_4byte1_361[1]);
            if (Convert.ToInt32(parameter7_4byte1_361[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_362, 0);                    //블록데이터 구성

                BlockParaModel1s[180].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_362, 0);

                BlockParaModel1s[180].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[180].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[180].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[180].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[180].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[180].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[180].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[180].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[180].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 5)                                       //감속정지
            {
               StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[0]) >> 4);                 //정지방법 hiki1
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[180].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[180].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[180].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[180].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_362, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[180].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_361[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_361[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_361[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[180].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[180].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_362, 0);                     //비교값   hiki7

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

                BlockParaModel1s[180].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_362, 0);                     //비교값   hiki7

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

                BlockParaModel1s[180].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_361[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_361[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_361[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_361[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_362, 0);                     //비교값   hiki7

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

                BlockParaModel1s[180].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }


            //181
            cmdCode = Convert.ToInt32(parameter7_4byte1_363[1]);
            if (Convert.ToInt32(parameter7_4byte1_363[1]) == 1)                                       //상대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) >> 4);           //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) & 0b_0000_1111); //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[3]) >> 4);           //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt16(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);  //  방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[3]) & 0b_0000_0011);//천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_364, 0);                    //블록데이터 구성

                BlockParaModel1s[181].BlockData = "상대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 상대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 2)                                        //절대위치결정
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[0]) >> 4);                 //속도번호  hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[0]) & 0b_0000_1111);       //가속번호  hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) >> 4);                 //감속번호  hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);//방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);       //천이조건  hiki5
            TargetPosition = BitConverter.ToInt32(parameter7_4byte1_364, 0);

                BlockParaModel1s[181].BlockData = "절대위치결정" +
                    ", 속도번호:V" + SpdNum.ToString() +
                    ", 가속설정번호:A" + AccNum.ToString() +
                    ", 감속설정번호:D" + Decnum.ToString() +
                    ", 천이조건:" + BlockChif.ToString() +
                    ", 절대이동량:" + TargetPosition.ToString();

            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 3)                                       //JOG운전
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[0]) >> 4);                 //속도번호 hiki1
                AccNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[0]) & 0b_0000_1111);       //가속번호 hiki2
                Decnum = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) >> 4);                 //감속번호 hiki3
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (Movidr == 0)
                {
                    BlockParaModel1s[181].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:정방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[181].BlockData = "JOG" +
                   ", 속도번호:V" + SpdNum.ToString() +
                   ", 가속설정번호:A" + AccNum.ToString() +
                   ", 감속설정번호:D" + Decnum.ToString() +
                   ", JOG방향:부방향" +
                   ", 천이조건:" + BlockChif.ToString();
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 4)                                      //원점복귀
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[0]) >> 4);                 //검출방법 hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);//방향     hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);       //천이조건 hiki5

                if (SpdNum == 1)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[181].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[181].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME+Z상" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else if (SpdNum == 2)
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[181].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[181].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:HOME" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }
                else
                {
                    if (Movidr == 0)
                    {
                        BlockParaModel1s[181].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:정방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                    else if (Movidr == 1)
                    {
                        BlockParaModel1s[181].BlockData = "원점복귀" +
                        ", 원점 복귀 방법:제조사 사용" +
                        ", 복귀방향:부방향" +
                        ", 천이조건:" + BlockChif.ToString();
                    }
                }

            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 5)                                       //감속정지
            {
                StopMethod = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[0]) >> 4);                 //정지방법 hiki1
                 BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);       //천이조건 hiki5


                if (StopMethod == 0)
                {
                    BlockParaModel1s[181].BlockData = "감속정지" +
                    ", 정지방법:감속정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[181].BlockData = "감속정지" +
                    ", 정지방법:즉시정지" +
                   ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 6)                                       //속도갱신
            {
                SpdNum = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[0]) >> 4);                 //속도번호  hiki1
               Movidr = (UInt16)((Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);//동작방향  hiki4
             BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);       //천이조건  hiki5

                if (Movidr == 0)
                {
                    BlockParaModel1s[181].BlockData = "속도갱신" +
                       ", 속도번호:V" + SpdNum.ToString() +
                      ", JOG방향:정방향" +
                      ", 천이조건:" + BlockChif.ToString();
                }
                else
                {
                    BlockParaModel1s[181].BlockData = "속도갱신" +
                      ", 속도번호:V" + SpdNum.ToString() +
                     ", JOG방향:부방향" +
                     ", 천이조건:" + BlockChif.ToString();
                }
            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);       //천이조건 hiki5
               TargetPosition = BitConverter.ToInt32(parameter7_4byte1_364, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[181].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 8)                                       //출력신호조작            
            {
                b_CTRL1_2 = (Convert.ToUInt16(parameter7_4byte1_363[0]) >> 4);                       //hiki1
                b_CTRL3_4 = (Convert.ToUInt16(parameter7_4byte1_363[0]) & 0b_0000_1111);             //hiki2
                b_CTRL5_6 = (Convert.ToUInt16(parameter7_4byte1_363[3]) >> 4);                       //hiki3
         BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);       //천이 조건hiki5

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

                BlockParaModel1s[181].BlockData = "출력신호조작" +
                ", B-CTRL1:" + bctrl1 +
                ", B-CTRL2:" + bctrl2 +
                ", B-CTRL3:" + bctrl3 +
                ", B-CTRL4:" + bctrl4 +
                ", B-CTRL5:" + bctrl5 +
                ", B-CTRL6:" + bctrl6 +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 9)                                       //점프
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);  //   hiki4
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);          //천이조건 hiki5

                JumpBlockNum = (ushort)((hiki2local << 6) + (hiki3local << 2) + hiki4local);

                BlockParaModel1s[181].BlockData = "점프" +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 10)      // 조건분기(=)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_364, 0);                     //비교값   hiki7

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

                BlockParaModel1s[181].BlockData = "조건분기(=)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 11)      // 조건분기(>)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);  // hiki4   
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) >> 4);                      // 비교대상  hiki1
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);      //천이조건 hiki5
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_364, 0);                     //비교값   hiki7

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

                BlockParaModel1s[181].BlockData = "조건분기(>)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_363[1]) == 12)      // 조건분기(<)
            {
                ushort hiki2local = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) & 0b_0000_1111); // hiki2
                ushort hiki3local = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[3]) >> 4);           // hiki3
               ushort hiki4local = (UInt16)((Convert.ToInt16(parameter7_4byte1_363[3]) & 0b_0000_1111) >> 2);  // hiki4
                           SpdNum = (UInt16)(Convert.ToInt16(parameter7_4byte1_363[0]) >> 4);                      // 비교대상  hiki1              
                        BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_363[3]) & 0b_0000_0011);      //천이조건 hiki5   
                       TargetPosition = BitConverter.ToInt32(parameter7_4byte1_364, 0);                     //비교값   hiki7

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

                BlockParaModel1s[181].BlockData = "조건분기(<)" +
                ", 비교대상:" + comp +
                ", 블록번호:" + JumpBlockNum.ToString() +
                ", 천이조건:" + BlockChif.ToString() +
                ", 비교값(역치):" + TargetPosition.ToString();
            }



            //182
            cmdCode = Convert.ToInt32(parameter7_4byte1_365[1]);
            if (Convert.ToInt32(parameter7_4byte1_365[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_365[1]) == 12)      // 조건분기(<)
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



            //183
            cmdCode = Convert.ToInt32(parameter7_4byte1_367[1]);
            if (Convert.ToInt32(parameter7_4byte1_367[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_367[1]) == 12)      // 조건분기(<)
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



            //184
            cmdCode = Convert.ToInt32(parameter7_4byte1_369[1]);
            if (Convert.ToInt32(parameter7_4byte1_369[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_369[1]) == 12)      // 조건분기(<)
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



            //185
            cmdCode = Convert.ToInt32(parameter7_4byte1_371[1]);
            if (Convert.ToInt32(parameter7_4byte1_371[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_371[1]) == 12)      // 조건분기(<)
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



            //186
            cmdCode = Convert.ToInt32(parameter7_4byte1_373[1]);
            if (Convert.ToInt32(parameter7_4byte1_373[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_373[1]) == 12)      // 조건분기(<)
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



            //187
            cmdCode = Convert.ToInt32(parameter7_4byte1_375[1]);
            if (Convert.ToInt32(parameter7_4byte1_375[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_375[1]) == 12)      // 조건분기(<)
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


            //188
            cmdCode = Convert.ToInt32(parameter7_4byte1_377[1]);
            if (Convert.ToInt32(parameter7_4byte1_377[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_377[1]) == 12)      // 조건분기(<)
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



            //189
            cmdCode = Convert.ToInt32(parameter7_4byte1_379[1]);
            if (Convert.ToInt32(parameter7_4byte1_379[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_379[1]) == 12)      // 조건분기(<)
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



            //190
            cmdCode = Convert.ToInt32(parameter7_4byte1_381[1]);
            if (Convert.ToInt32(parameter7_4byte1_381[1]) == 1)                                       //상대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 2)                                        //절대위치결정
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 3)                                       //JOG운전
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 4)                                      //원점복귀
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 5)                                       //감속정지
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 6)                                       //속도갱신
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 7)                                       //디크리멘트 카운트 기동
            {
                BlockChif = (UInt16)(Convert.ToInt32(parameter7_4byte1_247[3]) & 0b_0000_0011);       //천이조건 hiki5
                TargetPosition = BitConverter.ToInt32(parameter7_4byte1_248, 0);                           //카운트 설정값 hiki7

                BlockParaModel1s[123].BlockData = "디크리멘트 카운터 기동" +
                     ", 천이조건:" + BlockChif.ToString() +
                     ", 카운터 설정지[1ms]:" + TargetPosition.ToString();
            }
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 8)                                       //출력신호조작            
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 9)                                       //점프
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 10)      // 조건분기(=)
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 11)      // 조건분기(>)
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
            else if (Convert.ToInt32(parameter7_4byte1_381[1]) == 12)      // 조건분기(<)
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

        }
    }
}
