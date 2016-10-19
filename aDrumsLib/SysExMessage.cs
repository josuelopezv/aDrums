using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    internal class SysExMessage
    {
        internal const byte START_SYSEX = 0xF0; // start a MIDI SysEx message
        internal const byte END_SYSEX = 0xF7; // end a MIDI SysEx message

        public byte Command { get; set; }

        public byte[] Values { get; set; }

        public SysExMessage(SysExMsg command, CommandType type, params byte[] values)
        {
            Command = (byte)((((byte)command) << 1) | (byte)type);
            Values = values;
        }

        public SysExMessage(IEnumerable<Byte> Msg)
        {
            var l = Msg.ToList();
            Command = l.First();
            l.RemoveAt(0);
            Values = l.ToArray();
        }

        public SysExMessage() { }

        public byte[] ToArray()
        {
            var r = new byte[Values.Length + 3];
            r[0] = START_SYSEX;
            r[1] = Command;
            for (int i = 0; i < Values.Length; i++)
            {
                r[i + 2] = Values[i];
            }
            r[r.Length - 1] = END_SYSEX;
            return r.ToArray();
        }

    }


    #region enums

    public enum CommandType : byte
    {
        Get = 0,
        Set = 1,
    }

    public enum SysExMsg : byte
    {
        MSG_GET_HANDSHAKE = 0,
        MSG_GET_PINCOUNT = 8,
        MSG_EEPROM = 100,
        MSG_pinType = 1,
        MSG_pinThreshold = 2,
        MSG_pinNoteOnThreshold = 3,
        MSG_pinPitch = 4,
    }
    #endregion
}
