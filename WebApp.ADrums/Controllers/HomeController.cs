using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ADrums.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Connect()
        {
            return View(Program.DrumManager.Ports);
        }

        public IActionResult Index()
        {
            if (!Program.DrumManager.IsConnected)
                return RedirectToAction("Connect");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
