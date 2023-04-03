namespace DiscGolfRounds.API.Areas.Players.Requests
{
    public class NewPlayerRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool hasPDGANumber { get; set; }
        public int? pdgaNumber { get; set; }
    }
}
