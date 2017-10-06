using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tracker.ViewModels
{
    public class HomeViewModel
    {
        public int PlayerID { get; set; }// Player
        [DisplayName("Player Name")]
        public string PlayerName { get; set; }// Player
        [DisplayName("Status")]
        public string StatusName { get; set; }// Player
        [DisplayName("League")]
        public string LeagueName { get; set; }// League
        [DisplayName("Opponent")]
        public string TournamentName { get; set; }// Tournament
        [DisplayName("#")]
        public int TournamentID { get; set; }// Tournament
        [DisplayName("Points For")]
        public int TournamentScoresID { get; set; }// TournamentScores
        public Nullable<int> PointsFor { get; set; }// TournamentScores
        [DisplayName("Points Against")]
        public Nullable<int> PointsAgainst { get; set; }// TournamentScores

        public Nullable<int> TotalPointsFor { get; set; }// TournamentScores
        public Nullable<int> TotalPointsAgainst { get; set; }// TournamentScores

        public int TotalTournamentsPlayed { get; set; }
        public int TotalTournamentsWon { get; set; }
        public int TotalTournamentsLost { get; set; }

        public double AveragePointsFor { get; set; }
        public double AveragePointsAgainst { get; set; }
    }
}