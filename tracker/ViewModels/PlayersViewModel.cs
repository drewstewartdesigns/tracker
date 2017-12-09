using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tracker.ViewModels
{
    public class PlayersViewModel
    {
        public int PlayerID { get; set; }
        [DisplayName("Player Name")]
        public string PlayerName { get; set; }// Player
        [DisplayName("Chat Name")]
        public string ChatName { get; set; }// Player
        [DisplayName("Status")]
        public string StatusName { get; set; }// Player
        [DisplayName("League")]
        public string LeagueName { get; set; }// League
        [DisplayName("Opponent")]
        public string TournamentName { get; set; }// Tournament\
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public Nullable<int> Rank { get; set; } 
        [DisplayName("#")]
        public int TournamentID { get; set; }// Tournament
        [DisplayName("Points For")]
        public Nullable<int> PointsFor { get; set; }// TournamentScores
        [DisplayName("Points Against")]
        public Nullable<int> PointsAgainst { get; set; }// TournamentScores
        public bool MissedDrives { get; set; }
        public Nullable<int> TotalPointsFor { get; set; }
        public Nullable<int> TotalPointsAgainst { get; set; }
        public decimal AveragePointsFor { get; set; }
        public decimal AveragePointsAgainst { get; set; }
    }
}