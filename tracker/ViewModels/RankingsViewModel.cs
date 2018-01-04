using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tracker.ViewModels
{
    public class RankingsViewModel
    {
        public int PlayerID { get; set; }
        [DisplayName("Player")]
        public string PlayerName { get; set; }// Player
        public int? OffenseRating { get; set; } 
        public int TournamentID { get; set; }
        public int TournamentScoresID { get; set; }

        public decimal OffenseRanking35 { get; set; }
        public decimal OffenseRanking28 { get; set; }
        public decimal OffenseRanking14 { get; set; }
        public decimal OffenseRanking7 { get; set; }

        public decimal PowerRating { get; set; }

        public decimal DefensePowerRanking30 { get; set; }
    }
}