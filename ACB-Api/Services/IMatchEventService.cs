using ACB_Api.Models;

namespace ACB_Api.Services {
	public interface IMatchEventService {
		Task<IEnumerable<MatchEvent>> GetMatchEventsAsync (long gameId);
	}
}
