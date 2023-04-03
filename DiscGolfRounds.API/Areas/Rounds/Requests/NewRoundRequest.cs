namespace DiscGolfRounds.API.Areas.Rounds.Requests
{
    public class NewRoundRequest
    {
        public int variantID { get; set; }
        public int playerID { get; set; }
        public DateTime createdDateTime { get; set; }
        public List<int> scoreList { get; set; }

    }
}
