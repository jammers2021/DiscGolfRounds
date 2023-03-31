using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Models;

namespace DiscGolfRounds.ClassLibrary.Areas.Rounds.Interfaces
{
    public interface IRoundService
    {
        Task<List<Course>> AceCourseSelector(List<CourseVariant> variants);
        Task<Player> AcePlayer(int? playerID);
        Task<List<Round>> AceRoundSelector(List<Score>? scores);
        Task<List<Score>> AceSelectorAllPlayers();
        Task<List<Score>> AceSelectorIndividualPlayer(int? playerID);
        Task<List<CourseVariant>> AceVariantSelector(List<Round> rounds);
        Task<CourseVariant> CourseVariantSelection(int variantID);
        Task<Round> RoundFromExistingCourseVariant(int variantID, int? playerID, DateTime dateTime, List<int> scoreList);
        Task<List<Round>> RoundsAtCourseVariant(int variantID, int? playerID);
    }
}