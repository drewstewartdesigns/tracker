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
    
    public partial class Player
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Player()
        {
            this.TournamentScores = new HashSet<TournamentScore>();
        }
    
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string ChatName { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> LeagueID { get; set; }
        public Nullable<int> OffenseRating { get; set; }
    
        public virtual League League { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TournamentScore> TournamentScores { get; set; }
    }
}
