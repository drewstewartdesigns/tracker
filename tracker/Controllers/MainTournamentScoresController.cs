using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using tracker.Models;

namespace tracker.Controllers
{
    public class MainTournamentScoresController : Controller
    {
        private trackerDBEntities db = new trackerDBEntities();

        // GET: MainTournamentScores
        public ActionResult Index()
        {
            var tournamentScores = db.TournamentScores.Include(t => t.League).Where(t => t.LeagueID == 1).Include(t => t.Player).Where(t => t.Player.StatusID == 1).Include(t => t.Tournament).OrderBy(t => t.TournamentID);
            return View(tournamentScores.ToList().OrderByDescending(x => x.TournamentID));
        }

        // GET: MainTournamentScores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentScore tournamentScore = db.TournamentScores.Find(id);
            if (tournamentScore == null)
            {
                return HttpNotFound();
            }
            return View(tournamentScore);
        }

        // GET: MainTournamentScores/Create
        public ActionResult Create(int? id)
        {
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", 1);
            ViewBag.PlayerID = new SelectList(db.Players.Where(t => t.StatusID == 1).Include(t => t.League).Where(t => t.LeagueID == 1), "PlayerID", "PlayerName");
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "TournamentName", id);
            return View();
        }

        // POST: MainTournamentScores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TournamentScoresID,TournamentID,PlayerID,PointsFor,PointsAgainst,LeagueID")] TournamentScore tournamentScore)
        {
            if (ModelState.IsValid)
            {
                db.TournamentScores.Add(tournamentScore);
                db.SaveChanges();
                return RedirectToAction("Index", "MainTournaments");
            }

            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", tournamentScore.LeagueID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "PlayerName", tournamentScore.PlayerID);
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "TournamentName", tournamentScore.TournamentID);
            return View(tournamentScore);
        }

        // GET: MainTournamentScores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentScore tournamentScore = db.TournamentScores.Find(id);
            if (tournamentScore == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", tournamentScore.LeagueID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "PlayerName", tournamentScore.PlayerID);
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "TournamentName", tournamentScore.TournamentID);
            return View(tournamentScore);
        }

        // POST: MainTournamentScores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TournamentScoresID,TournamentID,PlayerID,PointsFor,PointsAgainst,LeagueID")] TournamentScore tournamentScore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournamentScore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "MainTournaments", new { id = tournamentScore.TournamentID });
            }
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", tournamentScore.LeagueID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "PlayerName", tournamentScore.PlayerID);
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "TournamentName", tournamentScore.TournamentID);
            return View(tournamentScore);
        }

        // GET: MainTournamentScores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentScore tournamentScore = db.TournamentScores.Find(id);
            if (tournamentScore == null)
            {
                return HttpNotFound();
            }
            return View(tournamentScore);
        }

        // POST: MainTournamentScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TournamentScore tournamentScore = db.TournamentScores.Find(id);
            db.TournamentScores.Remove(tournamentScore);
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
