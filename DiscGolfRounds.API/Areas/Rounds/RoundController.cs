using AutoMapper;
using DiscGolfRounds.API.Areas.Rounds.Requests;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Models;
using DiscGolfRounds.ClassLibrary.DataAccess;
using DiscGolfRounds.ClassLibrary.DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DiscGolfRounds.API.Areas.Rounds
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly DiscGolfContext _context;
        private readonly IRoundService _roundService;
        private readonly IMapper _mapper;
        public RoundController(DiscGolfContext context, IRoundService roundService, IMapper mapper)
        {
            _context = context;
            _roundService = roundService;
            _mapper = mapper;
        }
        [HttpPost(nameof(RoundCreator))]
        public async Task<RoundDTO> RoundCreator(NewRoundRequest newRoundRequest)
        {
            var round = await _roundService.CreateRoundFromExistingCourseVariant(newRoundRequest.variantID, newRoundRequest.playerID, newRoundRequest.createdDateTime, newRoundRequest.scoreList);
            return _mapper.Map<RoundDTO>(round);
        }
        [HttpGet(nameof(RoundsViewerAtCourseVariant))]
        public async Task<List<RoundDTO>> RoundsViewerAtCourseVariant(int variantID, int playerID)
        {
            var rounds = await _roundService.RoundsAtCourseVariant(variantID, playerID);
            return rounds.Select(r=> _mapper.Map<RoundDTO>(r)).ToList();
        }
        [HttpGet(nameof(AllAcesViewer))]
        public async Task<List<ScoreDTO>> AllAcesViewer()
        {
            var scores = await _roundService.AceSelectorAllPlayers();
            return scores.Select(s=>_mapper.Map<ScoreDTO>(s)).ToList();
        }
        [HttpGet(nameof(AllAcesViewerByPlayer))]
        public async Task<List<ScoreDTO>> AllAcesViewerByPlayer(int playerID)
        {
            var scores = await _roundService.AceSelectorIndividualPlayer(playerID);
            return scores.Select(s => _mapper.Map<ScoreDTO>(s)).ToList();
        }
        [HttpPost(nameof(RoundUpdater))]
        public async Task<RoundDTO> RoundUpdater(RoundUpdaterRequest roundUpdaterRequest)
        {
            var round = await _roundService.RoundUpdater(roundUpdaterRequest.roundID, roundUpdaterRequest.variantID, roundUpdaterRequest.playerID, roundUpdaterRequest.dateTime, roundUpdaterRequest.scoreList);
            return _mapper.Map<RoundDTO>(round);
        }
        [HttpGet(nameof(RoundDeleter))]
        public async Task<Round> RoundDeleter(int roundID)
        {
            var round = await _roundService.RoundDeleter(roundID);
            return round;
        }
        [HttpGet(nameof(UndoRoundDeleter))]
        public async Task<RoundDTO> UndoRoundDeleter(int roundID)
        {
            var round = await _roundService.UndoRoundDeleter(roundID);
            return _mapper.Map<RoundDTO>(round);
        }
    } 
}
