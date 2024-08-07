namespace ACB_Api.Models {
	/// <summary>
	/// Clase que contiene de manera reducida la información del play by play.
	/// </summary>
	public class MatchEventLean {
		public long? TeamId { get; set; }
		public long? PlayerLicenceId { get; set; }
		public string ActionTime { get; set; }
		public int ActionType { get; set; }
	}
}
