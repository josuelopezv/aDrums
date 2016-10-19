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
        Log1,
        Log2,
        Log3,
        Exp1,
        Exp2,
        S1,
        S2,
        Strong1,
        Strong2,
        Max,
    }

    public enum Pins: byte
    {
        HiHat_Pedal = 4,
        HiHat_Bow = 6,
        HiHat_Bell = 8,
        Kick_Drum = 9,
        Snare_Head = 0,
        Snare_Shell = 32,
        Tom_1_Head = 64,
        Tom_1_Shell = 96,
        Tom_2_Head = 128,
        Tom_2_Shell = 160,
        Tom_3_Head = 192,
        Tom_3_Shell = 224,
        Tom_4_Head = 1,
        Tom_4_Shell = 33,
        Crash_1_Bow = 65,
        Crash_1_Edge = 97,
        Crash_2_Bow = 129,
        Crash_2_Edge = 161,
        Ride_1_Bow = 193,
        Ride_1_Bell = 225,
        Ride_1_Edge = 2,
        Ride_2_Bow = 34,
        Ride_2_Bell = 66,
        Ride_2_Edge = 98,
        Ride_3_Bow = 130,
        Ride_3_Bell = 162,
        Ride_3_Edge = 194,
        Aux_1_Head = 226,
        Aux_1_Shell = 3,
        Aux_2_Head = 35,
        Aux_2_Shell = 67,
        Aux_3_Head = 99,
        Aux_3_Shell = 131,
        Aux_4_Head = 163,
        Aux_4_Shell = 195,
        Aux_5_Head = 227,
        Aux_5_Shell = 10,
    }
}
