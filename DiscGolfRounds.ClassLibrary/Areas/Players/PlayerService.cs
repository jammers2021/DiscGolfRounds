using DiscGolfRounds.ClassLibrary.Areas.Players.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using DiscGolfRounds.ClassLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.Areas.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly DiscGolfContext _dbContext;

        public PlayerService(DiscGolfContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Player> CreatePlayer(string firstName, string lastName, bool hasPDGANumber, int? pDGANumber)
        {

            var player = new Player()
            {
                FirstName = firstName,
                LastName = lastName,
                HasPDGANumber = hasPDGANumber,
                PDGANumber = pDGANumber
            };
            await _dbContext.Players.AddAsync(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }
        public async Task<List<Player>> ViewAllPlayers()
        {
            var players = await _dbContext.Players.Where(p => p.Deleted == false).ToListAsync();
            foreach (var player in players)
            {
                if (player.PDGANumber != null) player.HasPDGANumber = true;
            }
            return players;
        }
        public async Task<Player> PlayerUpdater(int playerId, string firstName, string lastName, bool hasPDGANumber, int? pDGANumber)
        {
            var player = await _dbContext.Players.FindAsync(playerId);
            if (player == null)
            {
                return null;
            }
            player.FirstName = firstName;
            player.LastName = lastName;
            player.PDGANumber = pDGANumber;
            player.HasPDGANumber = hasPDGANumber;
            _dbContext.Players.Update(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }
        public async Task<Player> PlayerDeleter(int playerID)
        {
            var player = await _dbContext.Players.FindAsync(playerID);
            player.Deleted = true;
            _dbContext.Players.Update(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }
        public async Task<Player> UndoPlayerDeleter(int playerID)
        {
            var player = await _dbContext.Players.FindAsync(playerID);
            player.Deleted = false;
            _dbContext.Players.Update(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }

    }
}
