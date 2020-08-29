using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;
using MINASA6SF_Rev.ViewModels;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.Models
{
    public class Master
    {
        // ------------------------------------------------------------------------
        // Constants for access
        private const byte fctReadCoil = 1;
        private const byte fctReadDiscreteInputs = 2;
        private const byte fctReadHoldingRegister = 3;
        private const byte fctReadInputRegister = 4;
        private const byte fctWriteSingleCoil = 5;
        private const byte fctWriteSingleRegister = 6;
        private const byte fctWriteMultipleCoils = 15;
        private const byte fctWriteMultipleRegister = 16;
        private const byte fctReadWriteMultipleRegister = 23;

        /// <summary>Constant for exception illegal function.</summary>
        public const byte excIllegalFunction = 1;
        /// <summary>Constant for exception illegal data address.</summary>
        public const byte excIllegalDataAdr = 2;
        /// <summary>Constant for exception illegal data value.</summary>
        public const byte excIllegalDataVal = 3;
        /// <summary>Constant for exception slave device failure.</summary>
        public const byte excSlaveDeviceFailure = 4;
        /// <summary>Constant for exception acknowledge. This is triggered if a write request is executed while the watchdog has expired.</summary>
        public const byte excAck = 5;
        /// <summary>Constant for exception slave is busy/booting up.</summary>
        public const byte excSlaveIsBusy = 6;
        /// <summary>Constant for exception gate path unavailable.</summary>
        public const byte excGatePathUnavailable = 10;
        /// <summary>Constant for exception not connected.</summary>
        public const byte excExceptionNotConnected = 253;
        /// <summary>Constant for exception connection lost.</summary>
        public const byte excExceptionConnectionLost = 254;
        /// <summary>Constant for exception response timeout.</summary>
        public const byte excExceptionTimeout = 255;
        /// <summary>Constant for exception wrong offset.</summary>
        private const byte excExceptionOffset = 128;
        /// <summary>Constant for exception send failt.</summary>
        private const byte excSendFailt = 100;

        // ------------------------------------------------------------------------
        // Private declarations
        private static ushort _timeout = 10000;
        private static ushort _refresh = 50;
        private static bool _connected = false;
        private static bool _no_sync_connection = false;
        public bool connected1 = false;
        public bool connected2 = false;

        private static Socket tcpAsyCl;
        private byte[] tcpAsyClBuffer = new byte[2048];

        private static Socket tcpSynCl;
        private byte[] tcpSynClBuffer = new byte[2048];
      

        byte[] data_WriteSingleCoils;
        ushort numBytes_WriteMultipleCoils;
        byte[] data_WriteMultipleCoils;
        byte[] data_WriteSinglRegister;
        ushort numBytes_WriteMultipleReg;
        byte[] data_WriteMultipleRegister;
        ushort numBytes_ReadWirteMultipleRegister;
        byte[] data_ReadWriteMultipleRegister;
        ushort numBytes_ReadWriteMultipleRegister2;
        byte[] data_ReadWriteMultipleRegister2;

        byte[] _id_CreateReadHeader;
        byte[] data_CreateReadHeader = new byte[12];
        byte[] _adr_CreateReadHeader;
        byte[] _length_CreateReadHeader;

        byte[] data_CrateWriteHeader;
        byte[] _id_CreateWriteHeader;
        byte[] _adr_CreateWriteHeader;
        byte[] _size_CreateWriteheader;
        byte[] _cnt_CreateWriteheader;

        byte[] data_CreateReadWriteHeader;
        byte[] _id_CreateReadWriteheader;
        byte[] _size_CreateReadWriteHeader;
        byte[] _adr_read_CreateReadWriteHeader;
        byte[] _cnt_read_CreateReadWriteHeader;
        byte[] _adr_wrtie_CreateReadWriteHeader;
        byte[] _cnt_Wrtie_CreateReadWriteHeader;

        Int32 size_OnSend;
        ushort id_OnReceive;
        byte unit_OnReceive;
        byte function_OnReceive;

        int result_WriteSyncData;
        byte unit_WriteSyncData;
        byte function_WriteSyncData;
        byte[] data_WriteSyndData;
        MainPanelViewModel mainPanelViewModel;

        // ------------------------------------------------------------------------
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public delegate void ResponseData(ushort id, byte unit, byte function, byte[] data);
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public event ResponseData OnResponseData;
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public delegate void ExceptionData(ushort id, byte unit, byte function, byte exception);
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public event ExceptionData OnException;

        // ------------------------------------------------------------------------
        /// <summary>Response timeout. If the slave didn't answers within in this time an exception is called.</summary>
        /// <value>The default value is 500ms.</value>
        public ushort timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Refresh timer for slave answer. The class is polling for answer every X ms.</summary>
        /// <value>The default value is 10ms.</value>
        public ushort refresh
        {
            get { return _refresh; }
            set { _refresh = value; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Displays the state of the synchronous channel</summary>
        /// <value>True if channel was diabled during connection.</value>
        public bool NoSyncConnection
        {
            get { return _no_sync_connection; }
        }

        // ------------------------------------------------------------------------
        /// <summary>Shows if a connection is active.</summary>
        public bool connected
        {
            get { return _connected; }
        }


        // ------------------------------------------------------------------------
        /// <summary>Create master instance without parameters.</summary>
       
        public Master(MainPanelViewModel _mainPanelViewModel)
        {
            mainPanelViewModel = _mainPanelViewModel;
        }

        // ------------------------------------------------------------------------
        /// <summary>Create master instance with parameters.</summary>
        /// <param name="ip">IP adress of modbus slave.</param>
        /// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
        public Master(string ip, ushort port)
        {
            connect(ip, port, false);
        }

        // ------------------------------------------------------------------------
        /// <summary>Create master instance with parameters.</summary>
        /// <param name="ip">IP adress of modbus slave.</param>
        /// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
        /// <param name="no_sync_connection">Disable second connection for synchronous requests</param>
        public Master(string ip, ushort port, bool no_sync_connection)
        {           
            connect(ip, port, no_sync_connection);
        }

        // ------------------------------------------------------------------------
        /// <summary>Start connection to slave.</summary>
        /// <param name="ip">IP adress of modbus slave.</param>
        /// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
        /// <param name="no_sync_connection">Disable sencond connection for synchronous requests</param>
        public void connect(string ip, ushort port, bool no_sync_connection)
        {
            connected1 = true;
            connected2 = true;
            try
            {
                IPAddress _ip;
                _no_sync_connection = no_sync_connection;
                if (IPAddress.TryParse(ip, out _ip) == false)
                {
                    IPHostEntry hst = Dns.GetHostEntry(ip);
                    ip = hst.AddressList[0].ToString();
                }
                // ----------------------------------------------------------------
                //Connect asynchronous client
                tcpAsyCl = new Socket(IPAddress.Parse(ip).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tcpAsyCl.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, _timeout);
                tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, _timeout);
                tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);
                // ----------------------------------------------------------------
                // Connect synchronous client
                if (!_no_sync_connection)
                {
                    tcpSynCl = new Socket(IPAddress.Parse(ip).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    tcpSynCl.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, _timeout);
                    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, _timeout);
                    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);
                }
                _connected = true;
            }
            catch (SocketException ex)
            {
                _connected = false;
                connected1 = false;
                connected2 = false;
                MessageBox.Show(ex.Message, "예외발생", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        // ------------------------------------------------------------------------
        /// <summary>Stop connection to slave.</summary>
        public void disconnect()
        {
            connected1 = false;
            connected2 = false;
            Dispose();
            _connected = false;
        }

        // ------------------------------------------------------------------------
        /// <summary>Destroy master instance.</summary>
        ~Master()
        {
            connected1 = false;
            connected2 = false;
            Dispose();
            _connected = false;
        }

        // ------------------------------------------------------------------------
        /// <summary>Destroy master instance</summary>
        public void Dispose()
        {
            connected1 = false;
            connected2 = false;
            if (tcpAsyCl != null)
            {
                if (tcpAsyCl.Connected)
                {
                    try { tcpAsyCl.Shutdown(SocketShutdown.Both); }
                    catch { }
                    tcpAsyCl.Close();                    
                }
                tcpAsyCl = null;
            }
            if (tcpSynCl != null)
            {
                if (tcpSynCl.Connected)
                {
                    try { tcpSynCl.Shutdown(SocketShutdown.Both); }
                    catch { }
                    tcpSynCl.Close();
                }
                tcpSynCl = null;
            }
            _connected = false;
        }

        internal void CallException(ushort id, byte unit, byte function, byte exception)
        {
            if ((tcpAsyCl == null) || (tcpSynCl == null && !_no_sync_connection)) return;
            if (exception == excExceptionConnectionLost)
            {
                tcpSynCl = null;
                tcpAsyCl = null;
            }
            if (OnException != null) OnException(id, unit, function, exception);
        }

        internal static UInt16 SwapUInt16(UInt16 inValue)
        {
            return (UInt16)(((inValue & 0xff00) >> 8) |
                     ((inValue & 0x00ff) << 8));
        }

        // ------------------------------------------------------------------------
        /// <summary>Read coils from slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        public void ReadCoils(ushort id, byte unit, ushort startAddress, ushort numInputs)
        {
            if (numInputs > 2000)
            {
                CallException(id, unit, fctReadCoil, excIllegalDataVal);
                return;
            }
            WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadCoil), id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Read coils from slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        /// <param name="values">Contains the result of function.</param>
        public void ReadCoils(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            if (numInputs > 2000)
            {
                CallException(id, unit, fctReadCoil, excIllegalDataVal);
                return;
            }
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadCoil), id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Read discrete inputs from slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        public void ReadDiscreteInputs(ushort id, byte unit, ushort startAddress, ushort numInputs)
        {
            if (numInputs > 2000)
            {
                CallException(id, unit, fctReadDiscreteInputs, excIllegalDataVal);
                return;
            }
            WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadDiscreteInputs), id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Read discrete inputs from slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        /// <param name="values">Contains the result of function.</param>
        public void ReadDiscreteInputs(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            if (numInputs > 2000)
            {
                CallException(id, unit, fctReadDiscreteInputs, excIllegalDataVal);
                return;
            }
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadDiscreteInputs), id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Read holding registers from slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        public void ReadHoldingRegister(ushort id, byte unit, ushort startAddress, ushort numInputs)
        {
            if (numInputs > 125)
            {
                CallException(id, unit, fctReadHoldingRegister, excIllegalDataVal);
                return;
            }
            WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadHoldingRegister), id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Read holding registers from slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        /// <param name="values">Contains the result of function.</param>
        public void ReadHoldingRegister(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            if (numInputs > 125)
            {
                CallException(id, unit, fctReadHoldingRegister, excIllegalDataVal);
                return;
            }
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadHoldingRegister), id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Read input registers from slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        public void ReadInputRegister(ushort id, byte unit, ushort startAddress, ushort numInputs)
        {
            if (numInputs > 125)
            {
                CallException(id, unit, fctReadInputRegister, excIllegalDataVal);
                return;
            }
            WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadInputRegister), id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Read input registers from slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        /// <param name="values">Contains the result of function.</param>
        public void ReadInputRegister(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            if (numInputs > 125)
            {
                CallException(id, unit, fctReadInputRegister, excIllegalDataVal);
                return;
            }
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadInputRegister), id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Write single coil in slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="OnOff">Specifys if the coil should be switched on or off.</param>
        /// 
        //public void WriteSingleCoils(ushort id, byte unit, ushort startAddress, bool OnOff)
        //{
        //    byte[] data;
        //    data = CreateWriteHeader(id, unit, startAddress, 1, 1, fctWriteSingleCoil);
        //    if (OnOff == true) data[10] = 255;
        //    else data[10] = 0;
        //    WriteAsyncData(data, id);
        //}

        // ------------------------------------------------------------------------
        /// <summary>Write single coil in slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="OnOff">Specifys if the coil should be switched on or off.</param>
        /// <param name="result">Contains the result of the synchronous write.</param>
        /// 

        //public void WriteSingleCoils(ushort id, byte unit, ushort startAddress, bool OnOff, ref byte[] result)
        //{
        //    byte[] data;
        //    data = CreateWriteHeader(id, unit, startAddress, 1, 1, fctWriteSingleCoil);
        //    if (OnOff == true) data[10] = 255;
        //    else data[10] = 0;
        //    result = WriteSyncData(data, id);
        //}

        public void WriteSingleCoils(ushort id, byte unit, ushort startAddress, bool OnOff)
        {           
            data_WriteSingleCoils = CreateWriteHeader(id, unit, startAddress, 1, 1, fctWriteSingleCoil);
            if (OnOff == true) data_WriteSingleCoils[10] = 255;
            else data_WriteSingleCoils[10] = 0;
            WriteSyncData(data_WriteSingleCoils, id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Write multiple coils in slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numBits">Specifys number of bits.</param>
        /// <param name="values">Contains the bit information in byte format.</param>
        public void WriteMultipleCoils(ushort id, byte unit, ushort startAddress, ushort numBits, byte[] values)
        {
            numBytes_WriteMultipleCoils = Convert.ToUInt16(values.Length);
            if (numBytes_WriteMultipleCoils > 250 || numBits > 2000)
            {
                CallException(id, unit, fctWriteMultipleCoils, excIllegalDataVal);
                return;
            }
                       
            data_WriteMultipleCoils = CreateWriteHeader(id, unit, startAddress, numBits, (byte)(numBytes_WriteMultipleCoils + 2), fctWriteMultipleCoils);
            Array.Copy(values, 0, data_WriteMultipleCoils, 13, numBytes_WriteMultipleCoils);
            WriteSyncData(data_WriteMultipleCoils, id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Write multiple coils in slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numBits">Specifys number of bits.</param>
        /// <param name="values">Contains the bit information in byte format.</param>
        /// <param name="result">Contains the result of the synchronous write.</param>
        public void WriteMultipleCoils(ushort id, byte unit, ushort startAddress, ushort numBits, byte[] values, ref byte[] result)
        {
             numBytes_WriteMultipleCoils = Convert.ToUInt16(values.Length);
            if (numBytes_WriteMultipleCoils > 250 || numBits > 2000)
            {
                CallException(id, unit, fctWriteMultipleCoils, excIllegalDataVal);
                return;
            }

            data_WriteMultipleCoils = CreateWriteHeader(id, unit, startAddress, numBits, (byte)(numBytes_WriteMultipleCoils + 2), fctWriteMultipleCoils);
            Array.Copy(values, 0, data_WriteMultipleCoils, 13, numBytes_WriteMultipleCoils);
            result = WriteSyncData(data_WriteMultipleCoils, id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Write single register in slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address to where the data is written.</param>
        /// <param name="values">Contains the register information.</param>
        public void WriteSingleRegister(ushort id, byte unit, ushort startAddress, byte[] values)
        {
            if (values.GetUpperBound(0) != 1)
            {
                CallException(id, unit, fctReadCoil, excIllegalDataVal);
                return;
            }
            data_WriteSinglRegister = null;
            data_WriteSinglRegister = CreateWriteHeader(id, unit, startAddress, 1, 1, fctWriteSingleRegister);
            data_WriteSinglRegister[10] = values[0];
            data_WriteSinglRegister[11] = values[1];
            WriteSyncData(data_WriteSinglRegister, id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Write single register in slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address to where the data is written.</param>
        /// <param name="values">Contains the register information.</param>
        /// <param name="result">Contains the result of the synchronous write.</param>


        //public void WriteSingleRegister(ushort id, byte unit, ushort startAddress, byte[] values, ref byte[] result)
        //{
        //    if (values.GetUpperBound(0) != 1)
        //    {
        //        CallException(id, unit, fctReadCoil, excIllegalDataVal);
        //        return;
        //    }

        //    data = CreateWriteHeader(id, unit, startAddress, 1, 1, fctWriteSingleRegister);
        //    data[10] = values[0];
        //    data[11] = values[1];
        //    result = WriteSyncData(data, id);
        //    Debug.WriteLine("WriteSingleRegister 실행");
        //}

        // ------------------------------------------------------------------------
        /// <summary>Write multiple registers in slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address to where the data is written.</param>
        /// <param name="values">Contains the register information.</param>
        public void WriteMultipleRegister(ushort id, byte unit, ushort startAddress, byte[] values)
        {
            numBytes_WriteMultipleReg = Convert.ToUInt16(values.Length);
            if (numBytes_WriteMultipleReg > 250)
            {
                CallException(id, unit, fctWriteMultipleRegister, excIllegalDataVal);
                return;
            }

            if (numBytes_WriteMultipleReg % 2 > 0) numBytes_WriteMultipleReg++;
            data_WriteMultipleRegister = CreateWriteHeader(id, unit, startAddress, Convert.ToUInt16(numBytes_WriteMultipleReg / 2), Convert.ToUInt16(numBytes_WriteMultipleReg + 2), fctWriteMultipleRegister);
            Array.Copy(values, 0, data_WriteMultipleRegister, 13, values.Length);
            WriteSyncData(data_WriteMultipleRegister, id);          
        }

        // ------------------------------------------------------------------------
        /// <summary>Write multiple registers in slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address to where the data is written.</param>
        /// <param name="values">Contains the register information.</param>
        /// <param name="result">Contains the result of the synchronous write.</param>
        /// 
        //public void WriteMultipleRegister(ushort id, byte unit, ushort startAddress, byte[] values, ref byte[] result)
        //{
        //    ushort numBytes = Convert.ToUInt16(values.Length);
        //    if (numBytes > 250)
        //    {
        //        CallException(id, unit, fctWriteMultipleRegister, excIllegalDataVal);
        //        return;
        //    }

        //    if (numBytes % 2 > 0) numBytes++;

        //    data = CreateWriteHeader(id, unit, startAddress, Convert.ToUInt16(numBytes / 2), Convert.ToUInt16(numBytes + 2), fctWriteMultipleRegister);
        //    Array.Copy(values, 0, data, 13, values.Length);
        //    result = WriteSyncData(data, id);
        //}

        // ------------------------------------------------------------------------
        /// <summary>Read/Write multiple registers in slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startReadAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        /// <param name="startWriteAddress">Address to where the data is written.</param>
        /// <param name="values">Contains the register information.</param>
        public void ReadWriteMultipleRegister(ushort id, byte unit, ushort startReadAddress, ushort numInputs, ushort startWriteAddress, byte[] values)
        {
            numBytes_ReadWirteMultipleRegister = Convert.ToUInt16(values.Length);
            if (numBytes_ReadWirteMultipleRegister > 250)
            {
                CallException(id, unit, fctReadWriteMultipleRegister, excIllegalDataVal);
                return;
            }
            if (numBytes_ReadWirteMultipleRegister % 2 > 0) numBytes_ReadWirteMultipleRegister++;

            data_ReadWriteMultipleRegister = CreateReadWriteHeader(id, unit, startReadAddress, numInputs, startWriteAddress, Convert.ToUInt16(numBytes_ReadWirteMultipleRegister / 2));
            Array.Copy(values, 0, data_ReadWriteMultipleRegister, 17, values.Length);
            WriteSyncData(data_ReadWriteMultipleRegister, id);
        }

        // ------------------------------------------------------------------------
        /// <summary>Read/Write multiple registers in slave synchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startReadAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        /// <param name="startWriteAddress">Address to where the data is written.</param>
        /// <param name="values">Contains the register information.</param>
        /// <param name="result">Contains the result of the synchronous command.</param>
        public void ReadWriteMultipleRegister(ushort id, byte unit, ushort startReadAddress, ushort numInputs, ushort startWriteAddress, byte[] values, ref byte[] result)
        {
            numBytes_ReadWriteMultipleRegister2 = Convert.ToUInt16(values.Length);
            if (numBytes_ReadWriteMultipleRegister2 > 250)
            {
                CallException(id, unit, fctReadWriteMultipleRegister, excIllegalDataVal);
                return;
            }

            if (numBytes_ReadWriteMultipleRegister2 % 2 > 0) numBytes_ReadWriteMultipleRegister2++;
            data_ReadWriteMultipleRegister2 = CreateReadWriteHeader(id, unit, startReadAddress, numInputs, startWriteAddress, Convert.ToUInt16(numBytes_ReadWriteMultipleRegister2 / 2));
            Array.Copy(values, 0, data_ReadWriteMultipleRegister2, 17, values.Length);
            result = WriteSyncData(data_ReadWriteMultipleRegister2, id);
        }

        // ------------------------------------------------------------------------
        // Create modbus header for read action
        private byte[] CreateReadHeader(ushort id, byte unit, ushort startAddress, ushort length, byte function)
        {

            _id_CreateReadHeader = BitConverter.GetBytes((short)id);
            data_CreateReadHeader[0] = _id_CreateReadHeader[1];			    // Slave id high byte
            data_CreateReadHeader[1] = _id_CreateReadHeader[0];				// Slave id low byte
            data_CreateReadHeader[5] = 6;					// Message size
            data_CreateReadHeader[6] = unit;					// Slave address
            data_CreateReadHeader[7] = function;				// Function code
            _adr_CreateReadHeader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startAddress));
            data_CreateReadHeader[8] = _adr_CreateReadHeader[0];				// Start address
            data_CreateReadHeader[9] = _adr_CreateReadHeader[1];                // Start address
            _length_CreateReadHeader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)length));
            data_CreateReadHeader[10] = _length_CreateReadHeader[0];			// Number of data to read
            data_CreateReadHeader[11] = _length_CreateReadHeader[1];			// Number of data to read
            return data_CreateReadHeader;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for write action
        private byte[] CreateWriteHeader(ushort id, byte unit, ushort startAddress, ushort numData, ushort numBytes, byte function)
        {
            data_CrateWriteHeader = new byte[numBytes + 11];

            _id_CreateWriteHeader = BitConverter.GetBytes((short)id);
            data_CrateWriteHeader[0] = _id_CreateWriteHeader[1];				// Slave id high byte
            data_CrateWriteHeader[1] = _id_CreateWriteHeader[0];				// Slave id low byte
            _size_CreateWriteheader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)(5 + numBytes)));
            data_CrateWriteHeader[4] = _size_CreateWriteheader[0];				// Complete message size in bytes
            data_CrateWriteHeader[5] = _size_CreateWriteheader[1];				// Complete message size in bytes
            data_CrateWriteHeader[6] = unit;					// Slave address
            data_CrateWriteHeader[7] = function;                // Function code
            _adr_CreateWriteHeader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startAddress));
            data_CrateWriteHeader[8] = _adr_CreateWriteHeader[0];				// Start address
            data_CrateWriteHeader[9] = _adr_CreateWriteHeader[1];				// Start address
            if (function >= fctWriteMultipleCoils)
            {
                _cnt_CreateWriteheader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)numData));
                data_CrateWriteHeader[10] = _cnt_CreateWriteheader[0];			// Number of bytes
                data_CrateWriteHeader[11] = _cnt_CreateWriteheader[1];			// Number of bytes
                data_CrateWriteHeader[12] = (byte)(numBytes - 2);
            }
            return data_CrateWriteHeader;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for read/write action
        private byte[] CreateReadWriteHeader(ushort id, byte unit, ushort startReadAddress, ushort numRead, ushort startWriteAddress, ushort numWrite)
        {
            data_CreateReadWriteHeader = new byte[numWrite * 2 + 17];

            _id_CreateReadWriteheader = BitConverter.GetBytes((short)id);
            data_CreateReadWriteHeader[0] = _id_CreateReadWriteheader[1];						// Slave id high byte
            data_CreateReadWriteHeader[1] = _id_CreateReadWriteheader[0];						// Slave id low byte
            _size_CreateReadWriteHeader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)(11 + numWrite * 2)));
            data_CreateReadWriteHeader[4] = _size_CreateReadWriteHeader[0];						// Complete message size in bytes
            data_CreateReadWriteHeader[5] = _size_CreateReadWriteHeader[1];						// Complete message size in bytes
            data_CreateReadWriteHeader[6] = unit;							// Slave address
            data_CreateReadWriteHeader[7] = fctReadWriteMultipleRegister;	// Function code
            _adr_read_CreateReadWriteHeader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startReadAddress));
            data_CreateReadWriteHeader[8] = _adr_read_CreateReadWriteHeader[0];					// Start read address
            data_CreateReadWriteHeader[9] = _adr_read_CreateReadWriteHeader[1];					// Start read address
            _cnt_read_CreateReadWriteHeader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)numRead));
            data_CreateReadWriteHeader[10] = _cnt_read_CreateReadWriteHeader[0];				// Number of bytes to read
            data_CreateReadWriteHeader[11] = _cnt_read_CreateReadWriteHeader[1];				// Number of bytes to read
            _adr_wrtie_CreateReadWriteHeader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startWriteAddress));
            data_CreateReadWriteHeader[12] = _adr_wrtie_CreateReadWriteHeader[0];				// Start write address
            data_CreateReadWriteHeader[13] = _adr_wrtie_CreateReadWriteHeader[1];				// Start write address
            _cnt_Wrtie_CreateReadWriteHeader = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)numWrite));
            data_CreateReadWriteHeader[14] = _cnt_Wrtie_CreateReadWriteHeader[0];				// Number of bytes to write
            data_CreateReadWriteHeader[15] = _cnt_Wrtie_CreateReadWriteHeader[1];				// Number of bytes to write
            data_CreateReadWriteHeader[16] = (byte)(numWrite * 2);

            return data_CreateReadWriteHeader;
        }

        // ------------------------------------------------------------------------
        // Write asynchronous data
        private void WriteAsyncData(byte[] write_data, ushort id)
        {
            if ((tcpAsyCl != null) && (tcpAsyCl.Connected))
            {
                try
                {
                    tcpAsyCl.BeginSend(write_data, 0, write_data.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                }
                catch (SystemException)
                {
                    CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
                }
            }
            else CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
        }

        // ------------------------------------------------------------------------
        // Write asynchronous data acknowledge
        private void OnSend(System.IAsyncResult result)
        {
            size_OnSend = tcpAsyCl.EndSend(result);
            if (result.IsCompleted == false) CallException(0xFFFF, 0xFF, 0xFF, excSendFailt);
            else tcpAsyCl.BeginReceive(tcpAsyClBuffer, 0, tcpAsyClBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), tcpAsyCl);
        }

        // ------------------------------------------------------------------------
        // Write asynchronous data response
        private void OnReceive(System.IAsyncResult result)
        {
            if (tcpAsyCl == null) return;

            try
            {
                tcpAsyCl.EndReceive(result);
                if (result.IsCompleted == false) CallException(0xFF, 0xFF, 0xFF, excExceptionConnectionLost);
            }
            catch (Exception) { }

            id_OnReceive = SwapUInt16(BitConverter.ToUInt16(tcpAsyClBuffer, 0));
            unit_OnReceive = tcpAsyClBuffer[6];
            function_OnReceive = tcpAsyClBuffer[7];
            byte[] data;

            // ------------------------------------------------------------
            // Write response data
            if ((function_OnReceive >= fctWriteSingleCoil) && (function_OnReceive != fctReadWriteMultipleRegister))
            {
                data = new byte[2];
                Array.Copy(tcpAsyClBuffer, 10, data, 0, 2);
            }
            // ------------------------------------------------------------
            // Read response data
            else
            {
                data = new byte[tcpAsyClBuffer[8]];
                Array.Copy(tcpAsyClBuffer, 9, data, 0, tcpAsyClBuffer[8]);
            }
            // ------------------------------------------------------------
            // Response data is slave exception
            if (function_OnReceive > excExceptionOffset)
            {
                function_OnReceive -= excExceptionOffset;
                CallException(id_OnReceive, unit_OnReceive, function_OnReceive, tcpAsyClBuffer[8]);
            }
            // ------------------------------------------------------------
            // Response data is regular data
            else if (OnResponseData != null) OnResponseData(id_OnReceive, unit_OnReceive, function_OnReceive, data);
        }

        // ------------------------------------------------------------------------
        // Write data and and wait for response
        private byte[] WriteSyncData(byte[] write_data, ushort id)
        {
            try
            {
                if (connected1 && write_data != null)
                {
                    tcpSynCl.Send(write_data, 0, write_data.Length, SocketFlags.None);
                    result_WriteSyncData = tcpSynCl.Receive(tcpSynClBuffer, 0, tcpSynClBuffer.Length, SocketFlags.None);

                    if (tcpSynClBuffer == null)
                    {
                        return null;
                    }
                    else
                    {
                        unit_WriteSyncData = tcpSynClBuffer[6];
                        function_WriteSyncData = tcpSynClBuffer[7];
                    }

                    if (result_WriteSyncData == 0) CallException(id, unit_WriteSyncData, write_data[7], excExceptionConnectionLost);

                    // ------------------------------------------------------------
                    // Response data is slave exception
                    if (function_WriteSyncData > excExceptionOffset)
                    {
                        function_WriteSyncData -= excExceptionOffset;
                        CallException(id, unit_WriteSyncData, function_WriteSyncData, tcpSynClBuffer[8]);
                        return null;
                    }
                    // ------------------------------------------------------------
                    // Write response data
                    else if ((function_WriteSyncData >= fctWriteSingleCoil) && (function_WriteSyncData != fctReadWriteMultipleRegister))
                    {
                        data_WriteSyndData = new byte[2];
                        Array.Copy(tcpSynClBuffer, 10, data_WriteSyndData, 0, 2);
                    }
                    // ------------------------------------------------------------
                    // Read response data
                    else
                    {
                        data_WriteSyndData = new byte[tcpSynClBuffer[8]];
                        Array.Copy(tcpSynClBuffer, 9, data_WriteSyndData, 0, tcpSynClBuffer[8]);
                    }
                    return data_WriteSyndData;

                }
                else
                    return null;
            }
            catch
            {
                MessageBox.Show("확인");
                mainPanelViewModel.ExecuteSettingsConfirm(this);
                
                //Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                //{
                //}));
                return null;
            }
        }
    }
}

