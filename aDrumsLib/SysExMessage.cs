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

        public byte[] Parameters { get; set; }

        public SysExMessage(SysExMsg command, CommandType type, params byte[] values)
        {
            Command = (byte)((int)Command << 1 & (int)type);
            Parameters = values;
        }

        public SysExMessage(IEnumerable<Byte> Msg)
        {
            var l = Msg.ToList();
            Command = l.First();
            l.RemoveAt(0);
            Parameters = l.ToArray();
        }

        public SysExMessage() { }

        public byte[] ToArray()
        {
            var r = new byte[Parameters.Length + 3];
            r[0] = START_SYSEX;
            r[1] = Command;
            for (int i = 0; i < Parameters.Length; i++)
            {
                r[i + 2] = Parameters[1];
            }
            r[r.Length - 1] = END_SYSEX;
            return r.ToArray();
        }

    }


    #region enums

    public enum CommandType
    {
        Get = 0,
        Set = 1,
    }

    public enum SysExMsg : byte
    {
        MSG_HANDSHAKE = 0,
        MSG_PIN_TYPE = 1,
        MSG_PIN_THRESHOLD = 2,
        MSG_PIN_NOTEON_THRESHOLD = 3,
        MSG_PIN_PITCH = 4,
    }
    #endregion
}
