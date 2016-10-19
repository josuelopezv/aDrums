using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    public class DrumManager
    {
        private SerialDevice SerialD { get; set; }

        public IEnumerable<MidiTrigger> Triggers { get; set; }

        public bool IsConnected { get { return SerialD != null; } }

        public DrumManager() { SerialD = null; }

        public void Connect(string ComPort)
        {
            if (IsConnected) throw new Exception($"Already connected to device {SerialD.PortName}");
            SerialD = ComPort == null ? SerialDevice.getAvailable() : new SerialDevice(ComPort);

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



        public static IEnumerable<string> getCOMPorts()
        {
           return SerialDevice.GetPortNames();
        }

    }
}
