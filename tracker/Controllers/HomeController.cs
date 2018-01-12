using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tracker.Models;
using tracker.ViewModels;

namespace tracker.Controllers
{
    public class HomeController : Controller
    {
        private trackerDBEntities db = new trackerDBEntities();

        public ActionResult Index()
        {

           

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult HowTo()
        {
            ViewBag.Message = "How To Do Things";

            return View();
        }
    }
}