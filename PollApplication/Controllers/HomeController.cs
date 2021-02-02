using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PollApplication.Models;

namespace PollApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role.Equals("admin"))
            {
                ViewBag.Name = "Admin";
                return View("AdminIndex");
            }

            return Polls();
        }

        [Authorize(Roles = "user")]
        public IActionResult Polls()
        {
            return RedirectToAction("Index", "Polls");
        }

        [Authorize(Roles = "admin, user")]
        public IActionResult Quit()
        {
            return RedirectPermanent("~/Account/Login");
        }

        [Authorize(Roles = "admin")]
        public IActionResult About()
        {
            return RedirectPermanent("~/PollCreation/Poll");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
