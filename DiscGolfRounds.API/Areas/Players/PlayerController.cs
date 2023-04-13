using AutoMapper;
using DiscGolfRounds.API.Areas.Players.Requests;
using DiscGolfRounds.ClassLibrary.Areas.Players.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using DiscGolfRounds.ClassLibrary.DataAccess;
using DiscGolfRounds.ClassLibrary.DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace DiscGolfRounds.API.Areas.Players
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly DiscGolfContext _context;
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;
        public PlayerController(DiscGolfContext context, IPlayerService playerService, IMapper mapper)
        {
            _context = context;
            _playerService = playerService;
            _mapper = mapper;
        }
        [HttpPost(nameof(CreateNewPlayer))]
        public async Task<PlayerDTO> CreateNewPlayer(NewPlayerRequest newPlayerRequest)
        {
            var player = await _playerService.CreatePlayer(newPlayerRequest.firstName, newPlayerRequest.lastName, newPlayerRequest.hasPDGANumber, newPlayerRequest.pdgaNumber);
            return _mapper.Map<PlayerDTO>(player);
        }
        [HttpGet(nameof(ViewAllPlayers))]
        public async Task<List<PlayerDTO>> ViewAllPlayers()
        {
            var players = await _playerService.ViewAllPlayers();
            return players.Select(p=> _mapper.Map<PlayerDTO>(p)).ToList();

        }
        [HttpPost(nameof(UpdatePlayer))]
        public async Task<PlayerDTO> UpdatePlayer(UpdatePlayerRequest updatePlayerRequest)
        {
            var player = await _playerService.PlayerUpdater(updatePlayerRequest.Id, updatePlayerRequest.firstName, updatePlayerRequest.lastName,
                updatePlayerRequest.hasPDGANumber, updatePlayerRequest.pdgaNumber);
            return _mapper.Map<PlayerDTO>(player);
        }
        [HttpPost(nameof(PlayerDeleter))]
        public async Task<PlayerDTO> PlayerDeleter(int playerID)
        {
            var player = await _playerService.PlayerDeleter(playerID);
            return _mapper.Map<PlayerDTO>(player);
        }
        [HttpPostAttribute(nameof(UndoPlayerDeleter))]
        public async Task<PlayerDTO> UndoPlayerDeleter(int playerID)
        {
            var player = await _playerService.UndoPlayerDeleter(playerID);
            return _mapper.Map<PlayerDTO>(player);
        }

    }
} 
