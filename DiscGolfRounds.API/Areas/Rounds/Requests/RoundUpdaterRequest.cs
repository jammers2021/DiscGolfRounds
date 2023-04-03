namespace DiscGolfRounds.API.Areas.Rounds.Requests
{
    public class RoundUpdaterRequest
    {
        public int roundID { get; set; }
        public int variantID { get; set; }
        public int? playerID { get; set; }
        public DateTime dateTime { get; set; }
        public List<int> scoreList { get; set; }
    }
}
