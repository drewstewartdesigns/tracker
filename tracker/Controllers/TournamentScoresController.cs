using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using tracker.Models;
using OfficeOpenXml;
using tracker.ViewModels;

namespace tracker.Controllers
{
    public class TournamentScoresController : Controller
    {
        private trackerDBEntities db = new trackerDBEntities();

        // GET: TournamentScores
        public ActionResult Index()
        {
            var tournamentScores = db.TournamentScores.Include(t => t.League).Where(t => t.LeagueID == 2).Include(t => t.Player).Where(t => t.Player.StatusID == 1).Include(t => t.Tournament).OrderBy(t => t.TournamentID);
            return View(tournamentScores.ToList().OrderByDescending(x => x.TournamentID));
        }

        // GET: TournamentScores/Details/5
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

        // GET: TournamentScores/Create
        public ActionResult Create(int? id)
        {
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", 2);
            ViewBag.PlayerID = new SelectList(db.Players.Where(t => t.StatusID == 1).Include(t => t.League).Where(t => t.LeagueID == 2), "PlayerID", "PlayerName");
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "TournamentName", id);
            ViewBag.MissedDrives = false;
            return View();
        }

        // POST: TournamentScores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TournamentScoresID,TournamentID,PlayerID,PointsFor,MissedDrives,PointsAgainst,LeagueID,DefenseAgainst")] TournamentScore tournamentScore)
        {
            if (ModelState.IsValid)
            {
                db.TournamentScores.Add(tournamentScore);
                db.SaveChanges();
                return RedirectToAction("Index", "Tournaments");
                //return RedirectToAction("Index");
            }

            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", tournamentScore.LeagueID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "PlayerName", tournamentScore.PlayerID);
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "TournamentName", tournamentScore.TournamentID);
            return View(tournamentScore);
        }


        public ActionResult Upload()
        {
            return View();
        }
        // POST: TournamentScores/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                ExcelPackage package = new ExcelPackage(postedFile.InputStream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                TournamentScore tournamentScore = new TournamentScore();
                
                int rowLength = worksheet.Dimension.End.Row;

                // plus 1 to avoid headers
                for (int rowIndex = worksheet.Dimension.Start.Row + 1; rowIndex <= rowLength; rowIndex++)
                {
                    var playerId = worksheet.Cells[rowIndex, 3].Value;
                    // don't include benched players or rows without a playerID
                    if (worksheet.Cells[rowIndex, 9].GetValue<int>() != 1 && playerId != null)
                    {
                        tournamentScore.TournamentID = worksheet.Cells[rowIndex, 1].GetValue<int>();
                        tournamentScore.LeagueID = worksheet.Cells[rowIndex, 2].GetValue<int>();
                        tournamentScore.PlayerID = worksheet.Cells[rowIndex, 3].GetValue<int>();
                        tournamentScore.PointsFor = worksheet.Cells[rowIndex, 5].GetValue<int>();
                        tournamentScore.PointsAgainst = worksheet.Cells[rowIndex, 6].GetValue<int>();
                        tournamentScore.MissedDrives = worksheet.Cells[rowIndex, 8].GetValue<bool>();
                        tournamentScore.DefenseAgainst = worksheet.Cells[rowIndex, 7].GetValue<int>();

                        db.TournamentScores.Add(tournamentScore);
                        db.SaveChanges();
                    }
                }

                //ViewBag.Message = "File uploaded successfully.";
            }

            return View();
        }

        // GET: TournamentScores/Edit/5
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

        // POST: TournamentScores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TournamentScoresID,TournamentID,PlayerID,PointsFor,MissedDrives,PointsAgainst,LeagueID,DefenseAgainst")] TournamentScore tournamentScore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournamentScore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tournaments", new {id=tournamentScore.TournamentID});
            }
            ViewBag.LeagueID = new SelectList(db.Leagues, "LeagueID", "LeagueName", tournamentScore.LeagueID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "PlayerName", tournamentScore.PlayerID);
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "TournamentName", tournamentScore.TournamentID);
            return View(tournamentScore);
        }

        // GET: TournamentScores/Delete/5
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

        // POST: TournamentScores/Delete/5
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
