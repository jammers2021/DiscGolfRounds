using DiscGolfRounds.API.Areas.Rounds.Requests;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Models;
using DiscGolfRounds.ClassLibrary.DataAccess;
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
        public RoundController(DiscGolfContext context, IRoundService roundService)
        {
            _context = context;
            _roundService = roundService;
        }
        [HttpPost(nameof(RoundCreator))]
        public async Task<Round> RoundCreator(NewRoundRequest newRoundRequest)
        {
            var round = await _roundService.RoundFromExistingCourseVariant(newRoundRequest.variantID, newRoundRequest.playerID, newRoundRequest.createdDateTime, newRoundRequest.scoreList);
            return round;
        }
        [HttpGet(nameof(RoundsViewerAtCourseVariant))]
        public async Task<List<Round>> RoundsViewerAtCourseVariant(int variantID, int playerID)
        {
            var rounds = await _roundService.RoundsAtCourseVariant(variantID, playerID);
            return rounds;
        }
        [HttpGet(nameof(AllAcesViewer))]
        public async Task<List<Score>> AllAcesViewer()
        {
            var scores = await _roundService.AceSelectorAllPlayers();
            return scores;
        }
        [HttpGet(nameof(AllAcesViewerByPlayer))]
        public async Task<List<Score>> AllAcesViewerByPlayer(int playerID)
        {
            var scores = await _roundService.AceSelectorIndividualPlayer(playerID);
            return scores;
        }
        [HttpPost(nameof(RoundUpdater))]
        public async Task<Round> RoundUpdater(RoundUpdaterRequest roundUpdaterRequest)
        {
            var round = await _roundService.RoundUpdater(roundUpdaterRequest.roundID, roundUpdaterRequest.variantID, roundUpdaterRequest.playerID, roundUpdaterRequest.dateTime, roundUpdaterRequest.scoreList);
            return round;
        }
        [HttpGet(nameof(RoundDeleter))]
        public async Task<Round> RoundDeleter(int roundID)
        {
            var round = await _roundService.RoundDeleter(roundID);
            return round;
        }
        [HttpGet(nameof(UndoRoundDeleter))]
        public async Task<Round> UndoRoundDeleter(int roundID)
        {
            var round = await _roundService.UndoRoundDeleter(roundID);
            return round;
        }
    } 
}
