using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    public static class Factory
    {
        public static ISerialPort getSerialPort()
        {
            return new winSerialPort();
        }

        public static string[] GetPortNames()
        {
         return   System.IO.Ports.SerialPort.GetPortNames();
        }
    }
}
