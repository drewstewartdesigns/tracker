﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tracker.Models;
using tracker.ViewModels;

namespace tracker.Controllers
{
    public class TournamentsController : Controller
    {
        private trackerDBEntities db = new trackerDBEntities();

        // GET: Tournaments
        public ActionResult Index()
        {
            var tournaments = db.Tournaments.Include(t => t.League).Where(t => t.LeagueID == 2).OrderByDescending(t => t.TournamentID);
            return View(tournaments.ToList());
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
                                      ts.PointsFor,
                                      ts.PointsAgainst
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
                    tvm.PointsFor = item.PointsFor;
                    tvm.PointsAgainst = item.PointsAgainst;
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
        public ActionResult Create([Bind(Include = "TournamentID,TournamentName,LeagueID")] Tournament tournament)
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
        public ActionResult Edit([Bind(Include = "TournamentID,TournamentName,LeagueID")] Tournament tournament)
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