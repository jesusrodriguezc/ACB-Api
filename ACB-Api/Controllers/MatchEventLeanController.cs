using ACB_Api.Models;
using ACB_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ACB_Api.Controllers {
	[ApiController]
	[Route("acb-api")]
	public class MatchEventLeanController : ControllerBase {

		private readonly IMatchEventService _matchEventService; 
		public MatchEventLeanController (IMatchEventService matchEventService) {
			_matchEventService = matchEventService;
		}
		/// <summary>
		/// Returns a reduced version of the play by play information for the match specified by its ID (game_id) 
		/// with only the team(id_team_denomination), player(Licence_id), timing and play type(id_playbyplaytype) information.
		/// </summary>
		/// <param name="game_id"></param>
		/// <returns></returns>
		[HttpGet("pbp-lean/{game_id}")]
		
		public async Task<IEnumerable<MatchEventLean>> GetPbpLean (long game_id) {

			var matchEventList = await _matchEventService.GetMatchEventsAsync(game_id);

			var matchEventLeanList = matchEventList.Select(fullEvent => new MatchEventLean() {
				TeamId = fullEvent.IdTeam,
				PlayerLicenceId = fullEvent.IdLicense,
				ActionTime = fullEvent.Crono,
				ActionType = fullEvent.IdPlaybyplaytype
			});

			return matchEventLeanList;
		}
		/// <summary>
		/// Returns the leaders of both teams in points and rebounds for the match specified by their ID (game_id). 
		/// </summary>
		/// <param name="game_id"></param>
		/// <returns></returns>

		[HttpGet("game-leaders/{game_id}")]

		public async Task<GameLeader> GetGameLeaders (long game_id) {

			var matchEventList = await _matchEventService.GetMatchEventsAsync(game_id);

			// We take the last event chronologically for each player, as it will have their updated statistics. Then, we sort them by points and total rebounds.
			var gameLeaders = matchEventList
				.Where(mev => mev.IdLicense != null && mev.Statistics != null)
				.GroupBy(mev => mev.IdLicense)
				.Select(mevPlayer => mevPlayer.Last())
				.OrderByDescending(playerLastPlay => playerLastPlay.Statistics.Points)
				.ThenByDescending(playerLastPlay => playerLastPlay.Statistics.TotalRebound);

			// Then, we separate it into home and away, and take their licence numbers.
			return new GameLeader {
				HomeTeamLeaders = gameLeaders.Where(player => player.Local).Select(player => (long)player.IdLicense.GetValueOrDefault()).ToList(),
				AwayTeamleaders = gameLeaders.Where(player => !player.Local).Select(player => (long)player.IdLicense.GetValueOrDefault()).ToList()
			};
		}

		/// <summary>
		/// Returns the highest point difference between the two teams during the match specified by their ID (game_id).
		/// </summary>
		/// <param name="game_id"></param>
		/// <returns></returns>

		[HttpGet("game-biggest_lead/{game_id}")]

		public async Task<GameScore> GetGameBiggestLead (long game_id) {
			var matchEventList = await _matchEventService.GetMatchEventsAsync(game_id);

			// Only events that change the score, i.e. shots, will be taken into account. These events are type 92, 93 and 94.
			var gameBiggestLead = matchEventList
				.Where(mev => new int[] { 92, 93, 94 }.Contains(mev.IdPlaybyplaytype))
				.MaxBy(canasta => Math.Abs(canasta.ScoreLocal - canasta.ScoreVisitor));

			return new GameScore {
				HomeTeam = gameBiggestLead.ScoreLocal,
				AwayTeam = gameBiggestLead.ScoreVisitor
			};
		}
	}
}