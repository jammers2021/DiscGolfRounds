namespace DiscGolfRounds.API.Areas.Players.Requests
{
    public class UpdatePlayerRequest
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool hasPDGANumber { get; set; }
        public int? pdgaNumber { get; set; }
    }
}
