using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    public class AvrUploader
    {
        public static IEnumerable<string> getPorts()
        {
            return SerialDevice.GetPortNames();
        }

        private static string ResetBoard(string comPort)
        {
            var oldPorts = getPorts();
            var s = Factory.getSerialPort();
            s.PortName = comPort;
            s.BaudRate = 1200;
            s.Open();
            s.Close();
            System.Threading.Thread.Sleep(2500);
            return getPorts().Where(x => !oldPorts.Contains(x)).FirstOrDefault();
        }

        public static string Upload(string comPort, string UploaderExePath, string configFilePath, string HexFilePath)
        {
            if (string.IsNullOrWhiteSpace(comPort))
                throw new Exception("COM Port not found");

            var ProgrammerPort = ResetBoard(comPort);

            if (string.IsNullOrWhiteSpace(ProgrammerPort))
                throw new Exception("Programmer Port not found");

            return Execute($"{UploaderExePath} -V -C \"{configFilePath}\" -v -p atmega32u4 -c avr109 -P {ProgrammerPort} -b 57600 -D -U flash:w:\"{HexFilePath}:i\"");

        }

        public static string Upload(string comPort)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "avrUploader";
            return Upload(comPort,
                 $"{path}\\avrdude",
                 $"{path}\\avrdude.conf",
                 $"{path}\\aDrums.ino.hex"
                 );
        }

        public static string Upload()
        {
            return Upload(getPorts().FirstOrDefault());
        }

        private static string Execute(string command)
        {
            var avrprog = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo("cmd")
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                },
            };

            avrprog.Start();
            avrprog.StandardInput.AutoFlush = true;
            avrprog.StandardInput.WriteLine(command);
            avrprog.StandardInput.Close();

            return
                $"{command}\r\n"+
                avrprog.StandardError.ReadToEnd().Trim();
        }
    }
}
