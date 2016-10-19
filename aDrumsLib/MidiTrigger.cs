using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    public class MidiTrigger
    {
        public Pins PinNumber { get; set; }
        public TriggerType Type { get; set; }
        public CurveType Curve { get; set; }
        public int Channel { get; set; }
        public int Note { get; set; }
        public byte Threshold { get; set; }//
        public byte Duration_Threshold { get; set; }//
        public int Gain { get; set; }

        public MidiTrigger() { }
        public MidiTrigger(Pins pinNumber) { PinNumber = pinNumber; }

        internal IEnumerable<SysExMessage> getSysExMsg()
        {
            var Msg = new List<SysExMessage>();
            Msg.Add(new SysExMessage(SysExMsg.MSG_PIN_TYPE, CommandType.Set, (byte)PinNumber, (byte)Type));
            Msg.Add(new SysExMessage(SysExMsg.MSG_PIN_THRESHOLD, CommandType.Set, (byte)PinNumber, Threshold));
            Msg.Add(new SysExMessage(SysExMsg.MSG_PIN_NOTEON_THRESHOLD, CommandType.Set, (byte)PinNumber, Duration_Threshold));
            return Msg;
        }
    }

}
