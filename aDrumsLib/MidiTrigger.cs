using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    public class MidiTrigger
    {
        public Pins PinNumber { get; private set; }
        public TriggerType Type { get; set; }
        public CurveType Curve { get; set; }
        //public int Channel { get; set; }
        public byte Pitch { get; set; }
        public byte Threshold { get; set; }//
        public byte Duration_Threshold { get; set; }//
        //public int Gain { get; set; }

        public MidiTrigger() { }
        public MidiTrigger(Pins pinNumber) { PinNumber = pinNumber; }

        internal IEnumerable<SysExMessage> getSysExMsg(CommandType ct = CommandType.Set)
        {
            var Msg = new List<SysExMessage>();
            Msg.Add(new SysExMessage(SysExMsg.MSG_pinType, ct, (byte)PinNumber, (byte)Type));
            Msg.Add(new SysExMessage(SysExMsg.MSG_pinThreshold, ct, (byte)PinNumber, Threshold));
            Msg.Add(new SysExMessage(SysExMsg.MSG_pinNoteOnThreshold, ct, (byte)PinNumber, Duration_Threshold));
            Msg.Add(new SysExMessage(SysExMsg.MSG_pinPitch, ct, (byte)PinNumber, Pitch));
            return Msg;
        }

        internal void setValues(SerialDevice sd)
        {
            foreach (var item in getSysExMsg(CommandType.Set))
            {
                sd.Send(item.ToArray());
            }
        }

        internal void getValues(SerialDevice sd)
        {
            foreach (var item in getSysExMsg(CommandType.Get))
            {
                Parse(sd.RunCommand(item));
            }
        }

        void Parse(SysExMessage msg)
        {
            if (msg.Values[0] != (byte)PinNumber) throw new Exception($"Pin mismatch {PinNumber} not equal to {msg.Values[1]}");
            var c = (SysExMsg)(msg.Command >> 1);
            switch (c)
            {
                case SysExMsg.MSG_pinType:
                    Type = (TriggerType)msg.Values[1];
                    break;
                case SysExMsg.MSG_pinThreshold:
                    Threshold = msg.Values[1];
                    break;
                case SysExMsg.MSG_pinNoteOnThreshold:
                    Duration_Threshold = msg.Values[1];
                    break;
                case SysExMsg.MSG_pinPitch:
                    Pitch = msg.Values[1];
                    break;
                default:
                    throw new Exception ($"Parse error Command:'{msg.Command}' not valid");
                    break;
            }
        }
    }

}
