using Newtonsoft.Json;

namespace ACB_Api.Models {
	public class MatchEvent {
		[JsonProperty("id_competition")]
		public int IdCompetition { get; set; }

		[JsonProperty("competition")]
		public Competition Competition { get; set; }

		[JsonProperty("id_edition")]
		public int IdEdition { get; set; }

		[JsonProperty("edition")]
		public Edition Edition { get; set; }

		[JsonProperty("id_phase")]
		public int IdPhase { get; set; }

		[JsonProperty("id_subphase")]
		public object IdSubphase { get; set; }

		[JsonProperty("id_round")]
		public object IdRound { get; set; }

		[JsonProperty("id_match")]
		public int IdMatch { get; set; }

		[JsonProperty("id_license")]
		public int? IdLicense { get; set; }

		[JsonProperty("license")]
		public License License { get; set; }

		[JsonProperty("id_license_type")]
		public int? IdLicenseType { get; set; }

		[JsonProperty("id_license_subtype")]
		public int? IdLicenseSubtype { get; set; }

		[JsonProperty("id_team")]
		public int? IdTeam { get; set; }

		[JsonProperty("team")]
		public Team Team { get; set; }

		[JsonProperty("order")]
		public int Order { get; set; }

		[JsonProperty("id_playbyplaytype")]
		public int IdPlaybyplaytype { get; set; }

		[JsonProperty("type")]
		public Type Type { get; set; }

		[JsonProperty("shirt_number")]
		public string ShirtNumber { get; set; }

		[JsonProperty("local")]
		public bool Local { get; set; }

		[JsonProperty("period")]
		public int Period { get; set; }

		[JsonProperty("minute")]
		public int Minute { get; set; }

		[JsonProperty("second")]
		public int Second { get; set; }

		[JsonProperty("crono")]
		public string Crono { get; set; }

		[JsonProperty("score_local")]
		public int ScoreLocal { get; set; }

		[JsonProperty("score_visitor")]
		public int ScoreVisitor { get; set; }

		[JsonProperty("posX")]
		public int? PosX { get; set; }

		[JsonProperty("posY")]
		public int? PosY { get; set; }

		[JsonProperty("wall_clock")]
		public int? WallClock { get; set; }

		[JsonProperty("statistics")]
		public Statistics Statistics { get; set; }
	}

	public class Competition {
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("official_name")]
		public string OfficialName { get; set; }

		[JsonProperty("initial_date")]
		public int InitialDate { get; set; }

		[JsonProperty("final_date")]
		public object FinalDate { get; set; }

		[JsonProperty("url_image")]
		public string UrlImage { get; set; }

		[JsonProperty("url_image_negative")]
		public string UrlImageNegative { get; set; }
	}

	public class Edition {
		[JsonProperty("year")]
		public int Year { get; set; }
	}

	public class License {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("id_person")]
		public int IdPerson { get; set; }

		[JsonProperty("licenseStr")]
		public string LicenseStr { get; set; }

		[JsonProperty("licenseStr15")]
		public string LicenseStr15 { get; set; }

		[JsonProperty("licenseAbbrev")]
		public string LicenseAbbrev { get; set; }

		[JsonProperty("licenseNick")]
		public string LicenseNick { get; set; }

		[JsonProperty("id_type")]
		public string IdType { get; set; }

		[JsonProperty("active")]
		public bool Active { get; set; }

		[JsonProperty("media")]
		public List<Medium> Media { get; set; }
	}

	public class Medium {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("id_type")]
		public int IdType { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("from_date")]
		public int FromDate { get; set; }
	}

	
	public class Statistics {
		[JsonProperty("3pt_success")]
		public int _3ptSuccess { get; set; }

		[JsonProperty("3pt_tried")]
		public int _3ptTried { get; set; }

		[JsonProperty("2pt_success")]
		public int _2ptSuccess { get; set; }

		[JsonProperty("2pt_tried")]
		public int _2ptTried { get; set; }

		[JsonProperty("1pt_success")]
		public int _1ptSuccess { get; set; }

		[JsonProperty("1pt_tried")]
		public int _1ptTried { get; set; }

		[JsonProperty("total_rebound")]
		public int TotalRebound { get; set; }

		[JsonProperty("asis")]
		public int Asis { get; set; }

		[JsonProperty("steals")]
		public int Steals { get; set; }

		[JsonProperty("turnovers")]
		public int Turnovers { get; set; }

		[JsonProperty("blocks")]
		public int Blocks { get; set; }

		[JsonProperty("personal_fouls")]
		public int PersonalFouls { get; set; }

		[JsonProperty("received_fouls")]
		public int ReceivedFouls { get; set; }

		[JsonProperty("points")]
		public int Points { get; set; }
	}

	public class Team {
		[JsonProperty("id_team_denomination")]
		public int IdTeamDenomination { get; set; }

		[JsonProperty("team_actual_name")]
		public string TeamActualName { get; set; }

		[JsonProperty("team_actual_short_name")]
		public string TeamActualShortName { get; set; }

		[JsonProperty("team_abbrev_name")]
		public string TeamAbbrevName { get; set; }

		[JsonProperty("media")]
		public List<Medium> Media { get; set; }
	}

	public class Type {
		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("normalized_description")]
		public string NormalizedDescription { get; set; }
	}


}
