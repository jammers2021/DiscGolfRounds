using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.DataAccess.DTOs
{
    public class ScoreDTO
    {
        [Key]
        public int Id { get; set; }
        public int ScoreOnHole { get; set; }
        public bool Deleted { get; set; }
        public HoleDTO Hole { get; set; }
        public RoundDTO Round { get; set; }
    }
}
