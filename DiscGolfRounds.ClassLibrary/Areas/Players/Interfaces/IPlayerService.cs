using DiscGolfRounds.ClassLibrary.Areas.Players.Models;

namespace DiscGolfRounds.ClassLibrary.Areas.Players.Interfaces
{
    public interface IPlayerService
    {
        Task<Player> CreatePlayer(string firstName, string lastName, bool hasPDGANumber, int? pDGANumber);
        Task<Player> PlayerDeleter(int playerID);
        Task<Player> PlayerUpdater(int playerId, string firstName, string lastName, bool hasPDGANumber, int? pDGANumber);
        Task<Player> UndoPlayerDeleter(int playerID);
        Task<List<Player>> ViewAllPlayers();
    }
}