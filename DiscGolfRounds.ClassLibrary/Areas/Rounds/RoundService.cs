using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Models;
using DiscGolfRounds.ClassLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace DiscGolfRounds.ClassLibrary.Areas.Rounds
{
    public class RoundService : IRoundService
    {
        private readonly DiscGolfContext _context;
        public RoundService(DiscGolfContext context)
        {
            _context = context;
        }
        public async Task<Round> CreateRoundFromExistingCourseVariant(int variantID, int? playerID, DateTime dateTime, List<int> scoreList)
        {
            if (variantID == null || variantID < 1 || playerID == null || scoreList == null)
            {
                return null;
            }

            Player? player = await _context.Players.FindAsync(playerID);
            if (player.PDGANumber != null)
            {
                player.HasPDGANumber = true;
            }
            CourseVariant? variant = await _context.CourseVariants.FindAsync(variantID);
            Course course = await _context.Courses.FindAsync(variant.CourseId);
            List<Hole> holes = await _context.Holes.Where(h => h.CourseVariantID == variantID).ToListAsync();
            holes.OrderBy(h => h.Id);
            List<Score> scores = new();
            Round round = new();
            round.CourseVariantID = variantID;
            round.Course = course;
            round.CourseId = variant.CourseId;
            round.CourseVariant = variant;
            round.DatePlayed = dateTime;
            round.Player = player;
            round.PlayerID = player.Id;
            for (int i =0; i < holes.Count; i++)
            {
                Score score = new();
                int value = scoreList.ElementAt(i);
                score.ScoreOnHole = value;
                int holeNumber = i+1;
                var selectedHole = holes.First(h => h.Number == holeNumber);
                score.Hole = selectedHole;
                score.HoleID = selectedHole.Id;
                score.Round = round;
                scores.Add(score);

            }
            await _context.Rounds.AddAsync(round);
            await _context.Scores.AddRangeAsync(scores);
            await _context.SaveChangesAsync();
            return round;
        }
        public async Task<List<Round>> RoundsAtCourseVariant(int variantID, int? playerID)
        {

            Player? player = await _context.Players.FindAsync(playerID);
            if (player.PDGANumber != null)
            {
                player.HasPDGANumber = true;
            }

            CourseVariant? variant = await _context.CourseVariants.Include(cv=>cv.Course).SingleOrDefaultAsync(cv=> cv.Id == variantID);
            List<Round> rounds = await _context.Rounds.Where(r => r.CourseVariantID == variantID && r.PlayerID == playerID && r.Deleted == false).ToListAsync();
            foreach (Round round in rounds)
            {
                round.Course = variant.Course;
                round.CourseVariantID = variantID;
                round.CourseVariant = variant;

            }
            if (player.Deleted == true || variant.Deleted == true)
            {
                return null;
            }
            return rounds;
        }

        /*public async Task<CourseVariant> CourseVariantSelection(int variantID)
        {
            //using var _context = new DiscGolf_context();
            CourseVariant? variant = await _context.CourseVariants.FindAsync(variantID);
            variant.Course = await _context.Courses.FindAsync(variant.CourseId);
            variant.Holes = await _context.Holes.Where(h => h.CourseVariantID == variantID && h.Deleted == false).ToListAsync();
            return variant;
        }*/
        public async Task<Player> AcePlayer(int? playerID)
        {
            if (playerID == null)
            {
                return null;
            }
            var player = await _context.Players.FindAsync(playerID);
            return player;
        }
        public async Task<List<Score>> AceSelectorIndividualPlayer(int? playerID)
        {
            var scores = await AceSelectorAllPlayers();
            List<int> roundIDs= new List<int>();
            foreach (var score in scores)
            {
                bool containsID = roundIDs.Contains(score.RoundID);
                if (!containsID)
                    roundIDs.Add(score.RoundID);
            }
            var player = await AcePlayer(playerID);
            if (player.Deleted == true || player == null)
            {
                return null;
            }
            if (player.PDGANumber != null)
            {
                player.HasPDGANumber = true;
            }
            var Rounds = await _context.Rounds.Include(r=> r.Player).Where(r => roundIDs.Contains(r.Id)).ToListAsync();
            List<Score> playerScore = new();
            foreach (var score in scores)
            {
                if (score.Round.Player.Id == playerID)
                {
                    playerScore.Add(score);
                }
            }
            return playerScore;
        }
        public async Task<List<Score>> AceSelectorAllPlayers()
        {

            //  Not sure why these are giving error message with the includes for round-Possible error with how the database is setup,
            //  however, I cannot see the difference between the round and CourseVariant (which works with the include)
            //Add Checks for deleted courses and holes
            var deletedPlayers = await _context.Players.Where(p => p.Deleted == true).ToListAsync();
            List<int> deletedPlayerIds = new List<int>();
            foreach (var player in deletedPlayers)
            {
                deletedPlayerIds.Add(player.Id);
            }
            
            List<Score> scores = await _context.Scores.
                Where(s => s.ScoreOnHole == 1 && s.Deleted == false).
                ToListAsync();
            List<int> roundIDs = new();
            List<int> holeIDs = new();
            foreach (var score in scores)
            {
                roundIDs.Add(score.RoundID);
                holeIDs.Add(score.HoleID);
            }
            List<Round> rounds = await _context.Rounds
                .Include(r => r.Player).Where(r=> r.Player.Deleted != true)
                .Where(r => roundIDs.Contains(r.Id) && r.Deleted != true).ToListAsync();
            List<int> variantIDs = new();
            foreach (var round in rounds)
            {
                variantIDs.Add(round.CourseVariantID);
            }
            List<CourseVariant> variants = await _context.CourseVariants.Include(cv=> cv.Course).Where(cv=> cv.Course.Deleted != true)
                .Where(cv=> variantIDs.Contains(cv.Id) && cv.Deleted != true).ToListAsync();
            List<Hole> holes = await _context.Holes.Include(r => r.CourseVariant)
                .Where(h=> holeIDs.Contains(h.Id) && h.Deleted != true).ToListAsync();
            foreach (var round in rounds)
            {
                round.CourseVariant = variants.FirstOrDefault(cv => cv.Id == round.CourseVariantID);
            }
            foreach (var score in scores)
            {
                score.Hole = holes.FirstOrDefault(h => h.Id == score.HoleID);
                score.Hole.CourseVariant = variants.FirstOrDefault(cv => cv.Id == score.Hole.CourseVariantID);
                score.Round = rounds.FirstOrDefault(r => r.Id == score.RoundID);
            }
                        //List<Score> scores = await _context.Scores.Where(s => s.ScoreOnHole == 1 && s.Deleted == false).Where(s => !deletedPlayerIds.Contains(s.Round.Player.Id)).ToListAsync();


            //List<Score> scores = await _context.Scores.Where(s => s.ScoreOnHole == 1 && s.Deleted == false).Include(s => s.Hole).Include(s => s.Round).Where(s => !deletedPlayerIds.Contains(s.Round.PlayerID)).ToListAsync();
            return scores;
        }

        public async Task<List<Round>> AceRoundSelector(List<Score>? scores)
        {
            if (scores == null)
            {
                return null;
            }
            List<int> roundIDs = new();
            foreach (var score in scores)
            {
                if (!roundIDs.Contains(score.RoundID))
                {
                    roundIDs.Add(score.RoundID);
                }
            }
            //List<Round> rounds = await _context.Rounds.Where(r => r.PlayerID == playerID && roundIDs.Contains(r.Id)).ToListAsync();
            List<Round> rounds = await _context.Rounds.Where(r => roundIDs.Contains(r.Id) && r.Deleted == false).Include(r => r.Player).ToListAsync();
            return rounds;
        }
        
        //Untested method
        public async Task<Round> RoundDeleter(int roundID)
        {
            var round = await _context.Rounds.FindAsync(roundID);
            if (round == null)
                return null;
            round.Deleted = true;
            var scores = await _context.Scores.Where(s => s.RoundID == roundID).ToListAsync();
            foreach (var score in scores)
            {
                score.Deleted = true;
            }
            await _context.SaveChangesAsync();
            return round;
        }
        public async Task<Round> UndoRoundDeleter(int roundID)
        {
            var round = await _context.Rounds.FindAsync(roundID);
            if (round == null)
                return null;
            round.Deleted = false;
            return round;
        }
        public async Task<Round> RoundUpdater(int roundID, int variantID, int? playerID, DateTime dateTime, List<int> scoreList)
        {
            var round = await _context.Rounds.FindAsync(roundID);
            if (round == null)
                return round;
            if (variantID == null || variantID < 1 || playerID == null || scoreList == null)
            {
                return null;
            }

            Player? player = await _context.Players.FindAsync(playerID);
            if (player.PDGANumber != null)
            {
                player.HasPDGANumber = true;
            }
            CourseVariant? variant = await _context.CourseVariants.FindAsync(variantID);
            Course course = await _context.Courses.FindAsync(variant.CourseId);
            List<Hole> holes = await _context.Holes.Where(h => h.CourseVariantID == variantID).ToListAsync();
            holes.OrderBy(h => h.Id);
            List<Score> scores = await _context.Scores.Where(s => s.RoundID == roundID).ToListAsync();
            _context.Scores.RemoveRange(scores);
            round.CourseVariantID = variantID;
            round.Course = course;
            round.CourseId = variant.CourseId;
            round.CourseVariant = variant;
            round.DatePlayed = dateTime;
            round.Player = player;
            round.PlayerID = player.Id;
            //Fix using replace par
            scores.Clear();
            for (int i = 1; i <= holes.Count; i++)
            {
                Score score = new();
                int value = scoreList.ElementAt(i - 1);
                score.ScoreOnHole = value;
                int holeNumber = i;
                var selectedHole = holes.First(h => h.Number == holeNumber);
                score.Hole = selectedHole;
                score.HoleID = selectedHole.Id;
                score.Round = round;
                score.RoundID = round.Id;
                scores.Add(score);

            }
            _context.Scores.AddRange(scores);
            _context.Rounds.Update(round);
            await _context.SaveChangesAsync();
            return round;
        }

    }
}
