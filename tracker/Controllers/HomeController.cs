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
            List<HomeViewModel> HomeVM = new List<HomeViewModel>();

            var tournamentScores = db.TournamentScores.Include(ts => ts.Tournament);

            foreach (var tournamentScore in tournamentScores)
            {
                HomeViewModel hvm = new HomeViewModel();



                hvm.TotalPointsFor = tournamentScore.Player.TournamentScores.Sum(ts => ts.PointsFor);

                HomeVM.Add(hvm);
            }

            return View(HomeVM.ToList());
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