using aDrumsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ADrums.Models
{
    public class vm_Connect
    {
        [Display(Name = "COM Port"), Required]
        public string COM_Port { get; set; }

        public string Errors { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectList getCOM_List()
        {
            return string.IsNullOrWhiteSpace(COM_Port) ?
                new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DrumManager.Current.Ports) :
                new Microsoft.AspNetCore.Mvc.Rendering.SelectList(DrumManager.Current.Ports, COM_Port);

        }
    }
}
