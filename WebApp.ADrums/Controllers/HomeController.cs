using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aDrumsLib;

namespace WebApp.ADrums.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Connect()
        {
            var m = new Models.vm_Connect();
            if (!DrumManager.Current.Ports.Any())
                m.Errors = "No COM Port available are you sure the device is connected";
            return View(m);
        }

        [HttpPost]
        public IActionResult Connect(Models.vm_Connect model)
        {
            return View(model);
        }

        public IActionResult Index()
        {
            //if (!DrumManager.Current.IsConnected)
            //    return RedirectToAction("connect");
            return View(DrumManager.Current.Jacks);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
