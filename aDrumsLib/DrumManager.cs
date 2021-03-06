﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    public class DrumManager : IDisposable
    {
        private static volatile DrumManager instance;
        private static object syncRoot = new Object();

        public static DrumManager Current
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DrumManager();
                    }
                }

                return instance;
            }
        }

        private SerialDevice SerialD { get; set; }

        public IEnumerable<string> Ports { get { return Factory.GetPortNames(); } }

        public string Jacks { get; private set; }

        public Version FW_Version
        {
            get
            {
                return SerialD != null ? SerialD.aDrumVersion : null;
            }
        }

        public int PinCount { get; set; }

        public List<MidiTrigger> Triggers { get; set; }

        public bool IsConnected
        {
            get
            {
                return SerialD != null;
            }
        }

        public DrumManager()
        {
            SerialD = null;
        }

        public void Connect(string ComPort)
        {
            GC.Collect(); // avoid open connections
            if (IsConnected) throw new Exception($"Already connected to device {SerialD.PortName}");
            SerialD = ComPort == null ? SerialDevice.getAvailable() : new SerialDevice(ComPort);

            PinCount = SerialD.RunCommand(SysExMsg.MSG_GET_PINCOUNT, CommandType.Get).Values[0];

            LoadSettings();
        }

        public void Connect()
        {
            Connect(null);
        }

        public void Disconnect()
        {
            SerialD = null;
            Triggers = null;
        }

        public void LoadSettings()
        {
            var lTriggers = new List<MidiTrigger>();
            for (int i = 0; i < PinCount; i++)
            {
                var t = new MidiTrigger((Pins)i);
                t.getValues(SerialD);
                lTriggers.Add(t);
            }
            Triggers = lTriggers;
        }

        public void SaveSettings()
        {
            foreach (var t in Triggers)
            {
                t.setValues(SerialD);
            }
        }

        public void WriteSettingsToEEPROM()
        {
            SerialD.Send(new SysExMessage(SysExMsg.MSG_EEPROM, CommandType.Set).ToArray());
        }

        public void LoadSettingsFromEEPROM()
        {
            SerialD.Send(new SysExMessage(SysExMsg.MSG_EEPROM, CommandType.Get).ToArray());
            LoadSettings();
        }

        public static IEnumerable<string> getCOMPorts()
        {
            return SerialDevice.GetPortNames();
        }

        public void Dispose()
        {
            if (SerialD != null && SerialD.IsOpen) SerialD.Close();
            SerialD = null;
        }

    }
}
