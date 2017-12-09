//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace tracker.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TournamentScore
    {
        public int TournamentScoresID { get; set; }
        public Nullable<int> TournamentID { get; set; }
        public Nullable<int> PlayerID { get; set; }
        public Nullable<int> PointsFor { get; set; }
        public Nullable<int> PointsAgainst { get; set; }
        public Nullable<int> LeagueID { get; set; }
        public bool MissedDrives { get; set; }
    
        public virtual League League { get; set; }
        public virtual Player Player { get; set; }
        public virtual Tournament Tournament { get; set; }
    }
}
