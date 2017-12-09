﻿using System;
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
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public Nullable<int> Rank { get; set; } 
        public int TournamentID { get; set; }
        public int TournamentScoresID { get; set; }
        [DisplayName("Points For")]
        public Nullable<int> PointsFor { get; set; }
        [DisplayName("Points Against")]
        public Nullable<int> PointsAgainst { get; set; }
        public Nullable<int> TotalPointsFor { get; set; }
        public Nullable<int> TotalPointsAgainst { get; set; }
        public bool MissedDrives { get; set; }
    }
}