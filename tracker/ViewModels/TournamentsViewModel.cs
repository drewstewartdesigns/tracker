using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace tracker.ViewModels
{
    public class TournamentsViewModel
    {
        public int PlayerID { get; set; }
        [DisplayName("Player")]
        public string PlayerName { get; set; }// Player
        [DisplayName("League")]
        public string LeagueName { get; set; }// League
        [DisplayName("Opponent")]
        public string TournamentName { get; set; }// Tournament
        public int TournamentID { get; set; }
        [DisplayName("Points For")]
        public Nullable<int> PointsFor { get; set; }
        [DisplayName("Points Against")]
        public Nullable<int> PointsAgainst { get; set; }
    }
}