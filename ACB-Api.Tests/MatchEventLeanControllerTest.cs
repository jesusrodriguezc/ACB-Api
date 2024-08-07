using ACB_Api.Controllers;
using ACB_Api.Models;
using ACB_Api.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace ACB_Api.Tests {
	[TestFixture]
	public class MatchEventLeanControllerTest {
		private Mock<IMatchEventService> _mockMatchEventService;
		private MatchEventLeanController _controller;

		[SetUp]
		public void Setup () {
			_mockMatchEventService = new Mock<IMatchEventService>();
			_controller = new MatchEventLeanController(_mockMatchEventService.Object);
		}

		[Test]
		public async Task GetPbpLean_ReturnsCorrectData () {
			var gameId = 1L;
			var matchEvents = new List<MatchEvent> {
				new MatchEvent { IdTeam = 1, IdLicense = 1, Crono = "00:10", IdPlaybyplaytype = 1 },
				new MatchEvent { IdTeam = 2, IdLicense = 2, Crono = "00:20", IdPlaybyplaytype = 2 }
			};

			_mockMatchEventService.Setup(s => s.GetMatchEventsAsync(gameId))
				.ReturnsAsync(matchEvents);

			var result = await _controller.GetPbpLean(gameId);

			Assert.That(result, Is.InstanceOf<IEnumerable<MatchEventLean>>());
			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public async Task GetGameLeaders_ReturnsCorrectData () {
			var gameId = 1L;
			var matchEvents = new List<MatchEvent> {
				new MatchEvent { IdLicense = 1000, Local = true, Statistics = new Statistics { Points = 12, TotalRebound = 6 } },
				new MatchEvent { IdLicense = 1001, Local = true, Statistics = new Statistics { Points = 1, TotalRebound = 1 } },
				new MatchEvent { IdLicense = 1002, Local = true, Statistics = new Statistics { Points = 6, TotalRebound = 4 } },
				new MatchEvent { IdLicense = 1004, Local = true, Statistics = new Statistics { Points = 3, TotalRebound = 1 } },
				new MatchEvent { IdLicense = 1006, Local = true, Statistics = new Statistics { Points = 0, TotalRebound = 2 } },
				new MatchEvent { IdLicense = 1007, Local = true, Statistics = new Statistics { Points = 15, TotalRebound = 10 } },
				new MatchEvent { IdLicense = 1010, Local = true, Statistics = new Statistics { Points = 12, TotalRebound = 3 } },
				new MatchEvent { IdLicense = 2000, Local = false, Statistics = new Statistics { Points = 0, TotalRebound = 0 } },
				new MatchEvent { IdLicense = 2001, Local = false, Statistics = new Statistics { Points = 3, TotalRebound = 5 } },
				new MatchEvent { IdLicense = 2002, Local = false, Statistics = new Statistics { Points = 18, TotalRebound = 9 } },
				new MatchEvent { IdLicense = 2003, Local = false, Statistics = new Statistics { Points = 5, TotalRebound = 2 } },
				new MatchEvent { IdLicense = 2004, Local = false, Statistics = new Statistics { Points = 8, TotalRebound = 1 } },
				new MatchEvent { IdLicense = 2005, Local = false, Statistics = new Statistics { Points = 13, TotalRebound = 2 } },
				new MatchEvent { IdLicense = 2006, Local = false, Statistics = new Statistics { Points = 18, TotalRebound = 14 } },
			};

			_mockMatchEventService.Setup(s => s.GetMatchEventsAsync(gameId))
				.ReturnsAsync(matchEvents);

			var result = await _controller.GetGameLeaders(gameId);

			Assert.That(result, Is.InstanceOf<GameLeader>());
			Assert.Multiple(() => {
				Assert.That(result.HomeTeamLeaders.Count, Is.EqualTo(7));
				Assert.That(result.AwayTeamleaders.Count, Is.EqualTo(7));
				Assert.That(result.HomeTeamLeaders.First(), Is.EqualTo(1007));  // Most points.
				Assert.That(result.AwayTeamleaders.First(), Is.EqualTo(2006));  // Tied for the most points with 2002 but has more total rebounds.
			});
		}

		[Test]
		public async Task GetGameBiggestLead_ReturnsCorrectData () {
			var gameId = 1L;
			var matchEvents = new List<MatchEvent> {
				new MatchEvent { IdPlaybyplaytype = 92, ScoreLocal = 50, ScoreVisitor = 45 }, // 1-Point Shot Made
				new MatchEvent { IdPlaybyplaytype = 93, ScoreLocal = 60, ScoreVisitor = 70 }, // 2-Point Shot Made
				new MatchEvent { IdPlaybyplaytype = 161, ScoreLocal = 41, ScoreVisitor = 35}, // Foul 2FT
				new MatchEvent { IdPlaybyplaytype = 110, ScoreLocal = 41, ScoreVisitor = 35}, // Foul Received
				new MatchEvent { IdPlaybyplaytype = 94, ScoreLocal = 55, ScoreVisitor = 60 } // 3-Point Shot Made
			};

			_mockMatchEventService.Setup(s => s.GetMatchEventsAsync(gameId))
				.ReturnsAsync(matchEvents);

			var result = await _controller.GetGameBiggestLead(gameId);

			Assert.That(result, Is.InstanceOf<GameScore>());
			Assert.Multiple(() => {
				Assert.That(result.HomeTeam, Is.EqualTo(60));
				Assert.That(result.AwayTeam, Is.EqualTo(70));
			});
		}
	}
}