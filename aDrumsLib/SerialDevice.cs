/*
 * Modified version of: Firmata.Net
 * Arduino.cs - Arduino/firmata library for Visual C# .NET Copyright (C) 2009 Tim Farley
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace aDrumsLib
{
    /**
     * Together with the Firmata 2 firmware (an Arduino sketch uploaded to the
     * Arduino board), this class allows you to control the Arduino board from
     * Processing: reading from and writing to the digital pins and reading the
     * analog inputs.
     */
    internal class SerialDevice
    {
        ISerialPort sp = null;

        private const int BAUDRATE = 115200;

        public Version aDrumVersion { get; private set; }
        public string PortName
        {
            get
            {
                return sp.PortName;
            }
            set
            {
                sp.PortName = value;
            }
        }
        public bool IsOpen
        {
            get
            {
                return sp.IsOpen;
            }
        }

        public SerialDevice(string COM_Port)
        {
            sp.PortName = COM_Port;
            sp.BaudRate = BAUDRATE;
            sp.DtrEnable = true;
            sp.Open();

            var r = RunCommand(SysExMsg.MSG_GET_HANDSHAKE, CommandType.Get);
            if (r.Values.Length != 2) throw new Exception($"Not a valid aDrum response in {COM_Port}");
            aDrumVersion = new Version(r.Values[0], r.Values[1]);

        }

        public void Send(params byte[] message)
        {
            sp.Write(message, 0, message.Length);
        }

        private void SendSysExMsg(SysExMessage sysEx)
        {
            Send(sysEx.ToArray());
        }

        private byte[] ReadExistingBytes()
        {
            var bytes = new byte[sp.BytesToRead];
            if (sp.BytesToRead > 1) sp.Read(bytes, 0, sp.BytesToRead);
            return bytes;
        }

        internal SysExMessage RunCommand(SysExMsg command, CommandType type, params byte[] values)
        {
            return RunCommand(new SysExMessage(command, type, values));
        }

        internal SysExMessage RunCommand(SysExMessage msg, int Timeout = 30)
        {
            if (sp.BytesToRead != 0) //clear the buffer from any sent bytes previously
                sp.ReadExisting();
            Send(msg.ToArray());
            var r = ReadSysEx(Timeout);
            if (r.Command != msg.Command)
                throw new ArrayTypeMismatchException($"Command Mismatch. Command Sent: '{msg.Command}', Command Read: '{r.Command}'");
            return r;
        }

        private SysExMessage ReadSysEx(int TimeoutSeconds)
        {
            return new SysExMessage(ReceiveSysEx(TimeoutSeconds));
        }

        private byte[] ReceiveSysEx(int TimeoutSeconds)
        {
            var storedInputData = new List<byte>();
            bool parsingSysex = false;
            var startTime = DateTime.UtcNow.Ticks;
            while (new TimeSpan(DateTime.UtcNow.Ticks - startTime).Seconds < TimeoutSeconds)
            {
                lock (this)
                {
                    int inputData = sp.ReadByte();
                    if (parsingSysex)
                        if (inputData == SysExMessage.END_SYSEX)
                        {
                            parsingSysex = false;
                            return storedInputData.ToArray();
                        }
                        else
                            storedInputData.Add((byte)inputData);
                    else if (inputData == SysExMessage.START_SYSEX)
                        parsingSysex = true;
                }
            }
            throw new TimeoutException();
        }

        public static SerialDevice getAvailable()
        {
            foreach (var cPort in GetPortNames())
            {
                try { return new SerialDevice(cPort); }
                catch (Exception e)
                {
                    GC.Collect();
                    e.ToString();
                }
            }
            throw new KeyNotFoundException("No valid aDrums device found");
        }

        public static string[] GetPortNames()
        {
            return Factory.GetPortNames();
        }

        public void Close()
        {
            sp.Close();
        }

        ~SerialDevice()
        {
            if (sp.IsOpen) sp.Close();
        }

    } // End class




} // End namespace