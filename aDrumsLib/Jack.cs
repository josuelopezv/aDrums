using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
   public class Jack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<TriggerLocation,Pins> Triggers { get; set; }
    }

    public enum TriggerLocation : byte
    {
        Tip = 0,
        Ring1 = 1,
        Ring2 = 2
    }
}
