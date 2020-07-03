using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MINASA6SF_Rev.Models
{
    public class ModbusTCP_
    {
        private const int READ_BUFFER_SIZE = 2048; // 2KB.
        private const int WRITE_BUFFER_SIZE = 2048; // 2KB.

        private byte[] bufferReceiver = null;
        private byte[] bufferSender = null;
        int rs;  //수신 바이트 수
        private Socket mSocket = null;

        private string IP = "127.0.0.1";
        private int Port = 502;

        string txtReceiMsg;

        private const byte fctReadCoil = 1;
        private const byte fctReadHoldingRegister = 3;
        private const byte fctReadInputRegister = 4;
        private const byte fctWriteSingleCoil = 5;
        private const byte fctWriteSingleRegister = 6;
        private const byte fctWriteMultipleCoils = 15;
        private const byte fctWriteMultipleRegister = 16;


        byte slaveAddress = 1;
        byte function = 3;
        ushort id = 0;
        ushort startAddress = 0;
        uint numberOfPoints = 2;
        const bool ON = true;


        public ModbusTCP_()
        {

        }

        public void Connect(IPAddress IP, int Port)
        {
            this.mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.bufferReceiver = new byte[READ_BUFFER_SIZE];
            this.bufferSender = new byte[WRITE_BUFFER_SIZE];
            this.mSocket.SendBufferSize = READ_BUFFER_SIZE;
            this.mSocket.ReceiveBufferSize = WRITE_BUFFER_SIZE;
            IPEndPoint server = new IPEndPoint(IP, Port);
            this.mSocket.Connect(server);
        }

        /// <summary>
        /// Disconnect with device
        /// </summary>
        public void Disconnect()
        {
            if (this.mSocket == null) return;
            if (this.mSocket.Connected)
            {
                this.mSocket.Close();
            }
        }



        private void btnReadCoils_Click(object sender, EventArgs e)
        {
            try
            {
             
                byte[] frame = ReadCoilsMsg(id, slaveAddress, startAddress, function, numberOfPoints);
                txtReceiMsg = Display(frame); //Show Message sent
                this.Write(frame); // Send message
                Thread.Sleep(100); // Delay 100ms
                byte[] buffReceiver = this.Read(); // Receive messages                         

                // Process data.
                int SizeByte = buffReceiver[8]; // The data bytes received.
                bool[] temp = null;

                if (function != buffReceiver[7])
                {
                    byte[] byteMsg = new byte[9];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                    txtReceiMsg = Display(byteMsg);

                    byte[] errorbytes = new byte[3];
                    Array.Copy(buffReceiver, 6, errorbytes, 0, errorbytes.Length);
                    this.CheckValidate(errorbytes); // Check validate
                }
                else
                {
                    byte[] byteMsg = new byte[9 + SizeByte];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                    byte[] data = new byte[SizeByte];
                    txtReceiMsg = Display(byteMsg); // Show received messages
                    Array.Copy(buffReceiver, 9, data, 0, data.Length);
                   // temp = ByteToBool(data);
                }

                // Result
                if (temp == null) return;
                string result = string.Empty;
                foreach (var item in temp)
                {
                    result += string.Format("{0} ", item);
                }
                txtReceiMsg = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void btnReadHoldingRegisters_Click(object sender, EventArgs e)
        {
            try
            {             
                byte[] frame = ReadHoldingRegistersMsg(id, slaveAddress, startAddress, function, numberOfPoints);
                this.Write(frame); // Send message
                Thread.Sleep(100); // Delay 100ms
                byte[] buffReceiver = this.Read(); // Receive messages                         

                // Process data.
                int SizeByte = buffReceiver[8]; // The data bytes received.
                           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnON_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] frame = WriteSingleCoilMsg(id, slaveAddress, startAddress, function, ON);
                txtReceiMsg = Display(frame); //Show Message sent
                this.Write(frame); // Send message
                Thread.Sleep(100); // Delay 100ms   

                // Process data.
                byte[] buffReceiver = this.Read(); // Receive messages  
                int SizeByte = buffReceiver[8]; // The data bytes received.    
                byte[] byteMsg = null;

                if (function != buffReceiver[7])
                {
                    byte[] errorbytes = new byte[3];
                    Array.Copy(buffReceiver, 6, errorbytes, 0, errorbytes.Length);
                    this.CheckValidate(errorbytes); // Check validate

                    byteMsg = new byte[9];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }
                else
                {
                    byteMsg = new byte[READ_BUFFER_SIZE];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }

                // Result
                txtReceiMsg = Display(byteMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOFF_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] frame = WriteSingleCoilMsg(id, slaveAddress, startAddress, function, false);
                txtReceiMsg = Display(frame); //Show Message sent
                this.Write(frame); // Send message
                Thread.Sleep(100); // Delay 100ms                                         

                // Process data.
                byte[] buffReceiver = this.Read(); // Receive messages  
                int SizeByte = buffReceiver[8]; // The data bytes received.    
                byte[] byteMsg = null;

                if (function != buffReceiver[7])
                {
                    byte[] errorbytes = new byte[3];
                    Array.Copy(buffReceiver, 6, errorbytes, 0, errorbytes.Length);
                    this.CheckValidate(errorbytes); // Check validate

                    byteMsg = new byte[9];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }
                else
                {
                    byteMsg = new byte[READ_BUFFER_SIZE];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }

                // Result
                if (byteMsg == null) return;
                txtReceiMsg = Display(byteMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnWriteSingleRegister_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveAddress = 1;
                byte function = 6;
                ushort id = function;
                ushort startAddress = 40002;
                ushort value=0; //레지스트리에 쓸 값
                txtReceiMsg = string.Empty;

                byte[] frame = WriteSingleRegisterMsg(id, slaveAddress, startAddress, function, value);
                txtReceiMsg = Display(frame);
                this.Write(frame); // send frame
                Thread.Sleep(100); // Delay 100ms                                         

                // Process data.
                byte[] buffReceiver = this.Read(); // Receive messages  
                int SizeByte = buffReceiver[8]; // The data bytes received.    
                byte[] byteMsg = null;

                if (function != buffReceiver[7])
                {
                    byte[] errorbytes = new byte[3];
                    Array.Copy(buffReceiver, 6, errorbytes, 0, errorbytes.Length);
                    this.CheckValidate(errorbytes); // Check validate

                    byteMsg = new byte[9];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }
                else
                {
                    byteMsg = new byte[READ_BUFFER_SIZE];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }

                // Result
                if (byteMsg == null) return;
                txtReceiMsg = Display(byteMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btnWriteMultipleCoils_Click(object sender, EventArgs e)
        {
            try
            {
                bool[] input = new bool[16] { false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false };
                byte[] values = ConvertBoolArrayToByteArray(input);

                byte[] frame = WriteMultipleCoilsMsg(id, slaveAddress, startAddress, function, values);
                txtReceiMsg = Display(frame);
                this.Write(frame); // send frame
                Thread.Sleep(100); // Delay 100ms                                         

                // Process data.
                byte[] buffReceiver = this.Read(); // Receive messages  
                int SizeByte = buffReceiver[8]; // The data bytes received.    
                byte[] byteMsg = null;

                if (function != buffReceiver[7])
                {
                    byte[] errorbytes = new byte[3];
                    Array.Copy(buffReceiver, 6, errorbytes, 0, errorbytes.Length);
                    this.CheckValidate(errorbytes); // Check validate

                    byteMsg = new byte[9];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }
                else
                {
                    byteMsg = new byte[READ_BUFFER_SIZE];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }

                // Result
                if (byteMsg == null) return;
                txtReceiMsg = Display(byteMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btnWriteMultipleRegisters_Click(object sender, EventArgs e)
        {
            try
            {
                ushort[] input = new ushort[2] { 1983, 1993 };
                byte[] values = ConvertUInt16ArrayToByteArray(input);

                byte[] frame = WriteMultipleRegistersMsg(id, slaveAddress, startAddress, function, values);
                txtReceiMsg = Display(frame);
                this.Write(frame); // send frame
                Thread.Sleep(100); // Delay 100ms                                         

                // Process data.
                byte[] buffReceiver = this.Read(); // Receive messages  
                int SizeByte = buffReceiver[8]; // The data bytes received.    
                byte[] byteMsg = null;

                if (function != buffReceiver[7])
                {
                    byte[] errorbytes = new byte[3];
                    Array.Copy(buffReceiver, 6, errorbytes, 0, errorbytes.Length);
                    this.CheckValidate(errorbytes); // Check validate

                    byteMsg = new byte[9];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }
                else
                {
                    byteMsg = new byte[READ_BUFFER_SIZE];
                    Array.Copy(buffReceiver, 0, byteMsg, 0, byteMsg.Length);
                }

                // Result
                if (byteMsg == null) return;
                txtReceiMsg = Display(byteMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        //FC01 
        public byte[] ReadCoilsMsg(ushort id, byte slaveAddress, ushort startAddress, byte function, uint numberOfPoints)
        {
            byte[] frame = new byte[12];
            frame[0] = (byte)(id >> 8);	// Transaction Identifier High
            frame[1] = (byte)id; // Transaction Identifier Low
            frame[2] = 0; // Protocol Identifier High
            frame[3] = 0; // Protocol Identifier Low
            frame[4] = 0; // Message Length High
            frame[5] = 6; // Message Length Low(6 bytes to follow)
            frame[6] = slaveAddress; // Slave address(Unit Identifier)
            frame[7] = function; // Function             
            frame[8] = (byte)(startAddress >> 8); // Starting Address High
            frame[9] = (byte)startAddress; // Starting Address Low           
            frame[10] = (byte)(numberOfPoints >> 8); // Quantity of Coils High
            frame[11] = (byte)numberOfPoints; // Quantity of Coils Low
            return frame;
        }

        //FC03
        public byte[] ReadHoldingRegistersMsg(ushort id, byte slaveAddress, ushort startAddress, byte function, uint numberOfPoints)
        {
            byte[] frame = new byte[12];
            frame[0] = (byte)(id >> 8);	// Transaction Identifier High
            frame[1] = (byte)id; // Transaction Identifier Low
            frame[2] = 0; // Protocol Identifier High
            frame[3] = 0; // Protocol Identifier Low
            frame[4] = 0; // Message Length High
            frame[5] = 6; // Message Length Low(6 bytes to follow)
            frame[6] = slaveAddress; // Slave address(Unit Identifier)
            frame[7] = function; // Function             
            frame[8] = (byte)(startAddress >> 8); // Starting Address High
            frame[9] = (byte)startAddress; // Starting Address Low           
            frame[10] = (byte)(numberOfPoints >> 8); // Quantity of Registers High
            frame[11] = (byte)numberOfPoints; // Quantity of Registers Low
            return frame;
        }

        //FC05 
        public byte[] WriteSingleCoilMsg(ushort id, byte slaveAddress, ushort startAddress, byte function, bool value)
        {
            byte[] frame = new byte[12];
            frame[0] = (byte)(id >> 8); // Slave id high byte
            frame[1] = (byte)id; // Slave id low byte
            frame[2] = 0; // Protocol Identifier High
            frame[3] = 0; // Protocol Identifier Low
            frame[4] = 0; // Message Length High
            frame[5] = 6; // Message Length Low(6 bytes to follow)
            frame[6] = slaveAddress; // Slave address(Unit Identifier)
            frame[7] = function; // Function               
            frame[8] = (byte)(startAddress >> 8); // Starting Address High
            frame[9] = (byte)startAddress; // Starting Address Low 
            frame[10] = (byte)(value ? 0xFF : 0x0); //Write Data High
            frame[11] = 0x00; //Write Data Low
            return frame;
        }

        //FC06 
        public byte[] WriteSingleRegisterMsg(ushort id, byte slaveAddress, ushort startAddress, byte function, ushort value)
        {
            byte[] frame = new byte[12];
            frame[0] = (byte)(id >> 8); // Transaction Identifier High
            frame[1] = (byte)id; // Transaction Identifier Low
            frame[2] = 0; // Protocol Identifier High
            frame[3] = 0; // Protocol Identifier Low
            frame[4] = 0; // Message Length High
            frame[5] = 6; // Message Length Low(6 bytes to follow)
            frame[6] = slaveAddress; // Slave address(Unit Identifier)
            frame[7] = function; // Function               
            frame[8] = (byte)(startAddress >> 8); // Starting Address High
            frame[9] = (byte)startAddress; // Starting Address Low 
            frame[10] = (byte)(value >> 8); //Write Data High
            frame[11] = (byte)value; //Write Data Low
            return frame;
        }

        //FC15
        public byte[] WriteMultipleCoilsMsg(ushort id, byte slaveAddress, ushort startAddress, byte function, byte[] values)
        {
            int byteCount = values.Length;
            byte[] frame = new byte[13 + byteCount];
            frame[0] = (byte)(id >> 8); // Transaction Identifier High
            frame[1] = (byte)id; // Transaction Identifier Low
            frame[2] = 0; // Protocol Identifier High
            frame[3] = 0; // Protocol Identifier Low
            frame[4] = 0; // Message Length High
            frame[5] = (byte)(7 + byteCount); //Message Length Low
            frame[6] = slaveAddress; // Slave Address(The Unit Identifier)
            frame[7] = function; // Function	           
            frame[8] = (byte)(startAddress >> 8); // Starting Address High	     
            frame[9] = (byte)startAddress; // Starting Address Low	  
            ushort amount = (ushort)(byteCount * 8); ;
            frame[10] = (byte)(amount >> 8); // Quantity of Coils High
            frame[11] = (byte)amount; // Quantity of Coils Low
            frame[12] = (byte)byteCount; // Byte Count
            Array.Copy(values, 0, frame, 13, byteCount); // Write Data
            return frame;
        }

        //FC16
        public byte[] WriteMultipleRegistersMsg(ushort id, byte slaveAddress, ushort startAddress, byte function, byte[] values)
        {
            int byteCount = values.Length;
            byte[] frame = new byte[13 + byteCount];
            frame[0] = (byte)(id >> 8); // Transaction Identifier High
            frame[1] = (byte)id; // Transaction Identifier Low
            frame[2] = 0; // Protocol Identifier High
            frame[3] = 0; // Protocol Identifier Low
            frame[4] = 0; // Message Length High
            frame[5] = (byte)(7 + byteCount); //Message Length Low
            frame[6] = slaveAddress; // Slave Address(The Unit Identifier)
            frame[7] = function; // Function	           
            frame[8] = (byte)(startAddress >> 8); // Starting Address High	     
            frame[9] = (byte)startAddress; // Starting Address Low	  
            ushort amount = (ushort)(byteCount / 2);
            frame[10] = (byte)(amount >> 8); // Quantity of Registers High
            frame[11] = (byte)amount; // Quantity of Registers Low
            frame[12] = (byte)byteCount; // Byte Count
            Array.Copy(values, 0, frame, 13, byteCount); // Write Data
            return frame;
        }


        public static byte[] ConvertUInt16ToByteArray(UInt16 value)
        {
            byte[] array = BitConverter.GetBytes(value);
            Array.Reverse(array);
            return array;
        }

        public static byte[] ConvertUInt16ArrayToByteArray(UInt16[] value)
        {
            ByteArray arr = new ByteArray();
            foreach (UInt16 val in value)
                arr.Add(ConvertUInt16ToByteArray(val));
            return arr.array;
        }

        public static byte[] ConvertBoolArrayToByteArray(bool[] bits)
        {
            int numBytes = bits.Length / 8;
            int bitEven = bits.Length % 8;
            if (bitEven != 0)
            {
                numBytes++;
            }
            Array.Reverse(bits);
            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }
            Array.Reverse(bytes);
            return bytes;
        }

        public int Write(byte[] frame)
        {
            return this.mSocket.Send(frame, frame.Length, SocketFlags.None);
        }
               
        public byte[] Read()
        {
            NetworkStream ns = new NetworkStream(this.mSocket);
            if (ns.CanRead)
            {
                rs = this.mSocket.Receive(this.bufferReceiver, this.bufferReceiver.Length, SocketFlags.None);
            }
            return this.bufferReceiver;
        }
              
        public void CheckValidate(byte[] messageReceived)
        {
            try
            {
                switch (messageReceived[1])
                {

                    case 129: // Hex: 81                     
                    case 130: // Hex: 82 
                    case 131: // Hex: 83 
                    case 132: // Hex: 83 
                    case 133: // Hex: 84 
                    case 134: // Hex: 86 
                    case 143: // Hex: 8F 
                    case 144: // Hex: 90
                        switch (messageReceived[2])
                        {
                            case 1:
                                throw new Exception("01/0x01: Illegal Function.");
                            case 2:
                                throw new Exception("02/0x02: Illegal Data Address.");
                            case 3:
                                throw new Exception("03/0x03: Illegal Data Value.");
                            case 4:
                                throw new Exception("04/0x04: Failure In Associated Device.");
                            case 5:
                                throw new Exception("05/0x05: Acknowledge.");
                            case 6:
                                throw new Exception("06/0x06: Slave Device Busy.");
                            case 7:
                                throw new Exception("07/0x07: NAK – Negative Acknowledgements.");
                            case 8:
                                throw new Exception("08/0x08: Memory Parity Error.");
                            case 10:
                                throw new Exception("10/0x0A: Gateway Path Unavailable.");
                            case 11:
                                throw new Exception("11/0x0B: Gateway Target Device Failed to respond.");
                            default:
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                txtReceiMsg = ex.Message;
            }
        }

        /// <summary>
        /// Display Data
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Message</returns>
        private string Display(byte[] data)
        {
            string result = string.Empty;
            foreach (var item in data)
            {
                result += string.Format("{0:X2}", item);
            }
            return result;
        }
    }
}
