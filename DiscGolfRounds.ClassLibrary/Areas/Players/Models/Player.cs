using DiscGolfRounds.ClassLibrary.Areas.Rounds.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.Areas.Players.Models
{
    public class Player
    {
        public int Id { get; set; }
        [NotMapped]
        public bool HasPDGANumber { get; set; }
        public int? PDGANumber { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public string FullName {
            get {
                return $"{FirstName} {LastName}";
            }
        }
        [NotMapped]
        public List<Round> Rounds { get; set; }
        
    }
}

