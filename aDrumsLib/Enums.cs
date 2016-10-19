using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    public enum ConnStatus
    {
        Disconnected = 0,
        Connected = 1,
        Fault = 3,
    }

    public enum TriggerType : byte
    {
        Disabled = 0,
        Piezo = 1,
        Trimpot = 2,
        Switch = 3,
    }
    public enum CurveType : byte
    {
        Linear,
        Log,
        Exp,
        Max,
        Mid,
    }

    public enum Pins: byte
    {
        HiHat_Pedal = 0,
        HiHat_Bow = 1,
        HiHat_Bell = 2,
        Kick_Drum = 3,
        Snare_Head = 4,
        Snare_Shell = 5,
        Tom_1_Head = 6,
        Tom_1_Shell = 7,
        Tom_2_Head = 8,
        Tom_2_Shell = 9,
        Tom_3_Head = 10,
        Tom_3_Shell = 11,
        Tom_4_Head = 12,
        Tom_4_Shell = 13,
        Crash_1_Bow = 14,
        Crash_1_Edge = 15,
        Crash_2_Bow = 16,
        Crash_2_Edge = 17,
        Ride_1_Bow = 18,
        Ride_1_Bell = 19,
        Ride_1_Edge = 20,
        Ride_2_Bow = 21,
        Ride_2_Bell = 22,
        Ride_2_Edge = 23,
        Ride_3_Bow = 24,
        Ride_3_Bell = 25,
        Ride_3_Edge = 26,
        Aux_1_Head = 27,
        Aux_1_Shell = 28,
        Aux_2_Head = 29,
        Aux_2_Shell = 30,
        Aux_3_Head = 31,
        Aux_3_Shell = 32,
        Aux_4_Head = 33,
        Aux_4_Shell = 34,
        Aux_5_Head = 35,
        Aux_5_Shell = 36,

    }
}
