using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tracker.Models;
using tracker.ViewModels;
using PagedList;

namespace tracker.Controllers
{
    public class MainPlayersController : Controller
    {
        private trackerDBEntities db = new trackerDBEntities();

        // GET: MainPlayers
        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.League).Include(p => p.Status).Where(p => p.LeagueID == 1);
            return View(players.ToList());
        }

        // GET: MainPlayers/Details/5
        public ActionResult Details(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            var playerStats = db.Players.Where(p => p.TournamentScores.Any(ts => ts.PlayerID == player.PlayerID));

            //return View(playerStats.ToList());

            List<PlayersViewModel> PlayersVW = new List<PlayersViewModel>();

            var playerDetails = (from p in db.Players
                                 join l in db.Leagues on p.LeagueID equals l.LeagueID
                                 join s in db.Status on p.StatusID equals s.StatusID
                                 join ts in db.TournamentScores on p.PlayerID equals ts.PlayerID
                                 join t in db.Tournaments on ts.TournamentID equals t.TournamentID
                                 where p.PlayerID == id
                                 select new
                                 {
                                     p.PlayerID,
                                     p.PlayerName,
                                     p.ChatName,
                                     s.StatusName,
                                     l.LeagueName,
                                     ts.Tournament,
                                     ts.TournamentID,
                                     ts.PointsFor,
                                     ts.PointsAgainst,
                                     t.TournamentName
                                 });

            foreach (var item in playerDetails)
            {
                PlayersViewModel tvm = new PlayersViewModel();
                tvm.PlayerID = item.PlayerID;
                tvm.PlayerName = item.PlayerName;
                tvm.ChatName = item.ChatName;
                tvm.StatusName = item.StatusName;
                tvm.LeagueName = item.LeagueName;
                tvm.TournamentID = item.TournamentID.Value;
                tvm.TournamentName = item.TournamentName;
                tvm.PointsFor = item.PointsFor;
                tvm.PointsAgainst = item.PointsAgainst;

                foreach (var playerStat in playerStats)
                {
                    tvm.TotalPointsFor = playerStat.TournamentScores.Sum(x => x.PointsFor);
                    tvm.TotalPointsAgainst = playerStat.TournamentScores.Sum(x => x.PointsAgainst);
                    tvm.AveragePointsFor = Math.Floor(playerStat.TournamentScores.Average(x => Convert.ToDouble(x.PointsFor)));
                    tvm.AveragePointsAgainst = Math.Floor(playerStat.TournamentScores.Average(x => Convert.ToDouble(x.PointsAgainst)));
                }

                PlayersVW.Add(tvm);
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(PlayersVW.OrderByDescending(x => x.TournamentID).ToPagedList(pageNumber, pageSize));
        }

        // GET: MainPlayers/Create
        public ActionResult Create()
        {
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName");
            return View();
        }

        // POST: MainPlayers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlayerID,PlayerName,ChatName,StatusID,LeagueID")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", player.LeagueID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", player.StatusID);
            return View(player);
        }

        // GET: MainPlayers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", player.LeagueID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", player.StatusID);
            return View(player);
        }

        // POST: MainPlayers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlayerID,PlayerName,ChatName,StatusID,LeagueID")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", player.LeagueID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", player.StatusID);
            return View(player);
        }

        // GET: MainPlayers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: MainPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
