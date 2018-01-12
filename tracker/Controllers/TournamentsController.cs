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
    public class TournamentsController : Controller
    {
        private trackerDBEntities db = new trackerDBEntities();

        public ActionResult Rankings()
        {
            List<RankingsViewModel> TournamentsVW = new List<RankingsViewModel>();

            var playerList =
                db.Players.Where(p => p.StatusID == 1 && p.LeagueID == 2).Distinct();


            foreach (var player in playerList)
            {
                RankingsViewModel tvm = new RankingsViewModel();

                tvm.PlayerName = player.PlayerName;
                tvm.OffenseRating = player.OffenseRating;

                var playerTournamentList = player.TournamentScores.ToList();

                if (playerTournamentList.Count > 0)
                {
                    tvm.OffenseRanking35 =
                        playerTournamentList.OrderByDescending(ts => ts.TournamentID)
                            .Take(35)
                            .Average(x => Convert.ToDecimal(x.PointsFor));

                    tvm.OffenseRanking28 =
                        playerTournamentList.OrderByDescending(ts => ts.TournamentID)
                            .Take(28)
                            .Average(x => Convert.ToDecimal(x.PointsFor));

                    tvm.OffenseRanking14 =
                        playerTournamentList.OrderByDescending(ts => ts.TournamentID)
                            .Take(14)
                            .Average(x => Convert.ToDecimal(x.PointsFor));

                    tvm.OffenseRanking7 =
                        playerTournamentList.OrderByDescending(ts => ts.TournamentID)
                            .Take(7)
                            .Average(x => Convert.ToDecimal(x.PointsFor));

                    int? _offenseRating = 0;
                    if (player.OffenseRating.HasValue)
                    {
                        _offenseRating = player.OffenseRating;
                    }
                    tvm.PowerRating = Convert.ToDecimal(tvm.OffenseRanking14 * 5 + _offenseRating);

                    TournamentsVW.Add(tvm);   
                }
            }


            return View(TournamentsVW.ToList());
        }

        // GET: Tournaments
        public ActionResult Index(int? page)
        {
            List<TournamentsViewModel> TournamentsVW = new List<TournamentsViewModel>();

            var tournaments = db.Tournaments.Include(t => t.League).Where(t => t.LeagueID == 2).OrderByDescending(t => t.TournamentID);

            foreach (var tournament in tournaments)
            {
                TournamentsViewModel tvm = new TournamentsViewModel();
                tvm.TournamentID = tournament.TournamentID;
                tvm.TournamentName = tournament.TournamentName;
                tvm.Rank = tournament.Rank;
                tvm.TotalPointsFor = tournament.TournamentScores.Sum(x => x.PointsFor);
                tvm.TotalPointsAgainst = tournament.TournamentScores.Sum(x => x.PointsAgainst);
                TournamentsVW.Add(tvm);
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(TournamentsVW.ToPagedList(pageNumber, pageSize));
        }

        // GET: Tournaments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            List<TournamentsViewModel> TournamentsVW = new List<TournamentsViewModel>();

            var tourneyDetails = (from ts in db.TournamentScores
                                  join p in db.Players on ts.PlayerID equals p.PlayerID
                                  join l in db.Leagues on p.LeagueID equals l.LeagueID
                                  join t in db.Tournaments on ts.TournamentID equals t.TournamentID
                                  select new
                                  {
                                      p.PlayerID,
                                      p.PlayerName,
                                      l.LeagueName,
                                      t.TournamentID,
                                      t.TournamentName,
                                      t.Rank,
                                      ts.TournamentScoresID,
                                      ts.PointsFor,
                                      ts.PointsAgainst,
                                      ts.MissedDrives
                                  });

            foreach (var item in tourneyDetails)
            {
                if (item.TournamentID == id)
                {
                    TournamentsViewModel tvm = new TournamentsViewModel();
                    tvm.PlayerID = item.PlayerID;
                    tvm.PlayerName = item.PlayerName;
                    tvm.LeagueName = item.LeagueName;
                    tvm.TournamentID = item.TournamentID;
                    tvm.TournamentName = item.TournamentName;
                    tvm.Rank = item.Rank;
                    tvm.TournamentScoresID = item.TournamentScoresID;
                    tvm.PointsFor = item.PointsFor;
                    tvm.PointsAgainst = item.PointsAgainst;
                    tvm.MissedDrives = item.MissedDrives;
                    TournamentsVW.Add(tvm);
                }
            }


            return View(TournamentsVW.ToList());
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", 2);
            return View();
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TournamentID,TournamentName,LeagueID,Rank")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", tournament.LeagueID);
            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", tournament.LeagueID);
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TournamentID,TournamentName,LeagueID,Rank")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", tournament.LeagueID);
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tournament tournament = db.Tournaments.Find(id);
            db.Tournaments.Remove(tournament);
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
