using DiscGolfRounds.API.Areas.Players.Requests;
using DiscGolfRounds.ClassLibrary.Areas.DataAccess;
using DiscGolfRounds.ClassLibrary.Areas.Players.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscGolfRounds.API.Areas.Players
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly DiscGolfContext _context;
        private readonly IPlayerService _playerService;
        public PlayerController(DiscGolfContext context, IPlayerService playerService)
        {
            _context = context;
            _playerService = playerService;
        }
        [HttpPost(nameof(CreateNewPlayer))]
        public async Task<Player> CreateNewPlayer(NewPlayerRequest newPlayerRequest)
        {
            var player = await _playerService.CreatePlayer(newPlayerRequest.firstName, newPlayerRequest.lastName, newPlayerRequest.hasPDGANumber, newPlayerRequest.pdgaNumber);
            return player;
        }
        [HttpGet(nameof(ViewAllPlayers))]
        public async Task<List<Player>> ViewAllPlayers()
        {
            var players = await _playerService.ViewAllPlayers();
            return players;

        }
        [HttpPost(nameof(UpdatePlayer))]
        public async Task<Player> UpdatePlayer(UpdatePlayerRequest updatePlayerRequest)
        {
            var player = await _playerService.PlayerUpdater(updatePlayerRequest.Id, updatePlayerRequest.firstName, updatePlayerRequest.lastName,
                updatePlayerRequest.hasPDGANumber, updatePlayerRequest.pdgaNumber);
            return player;
        }
        [HttpPost(nameof(PlayerDeleter))]
        public async Task<Player> PlayerDeleter(int playerID)
        {
            return await _playerService.PlayerDeleter(playerID);
        }
        [HttpPostAttribute(nameof(UndoPlayerDeleter))]
        public async Task<Player> UndoPlayerDeleter(int playerID)
        {
            return await _playerService.UndoPlayerDeleter(playerID);
        }

    }
} 
