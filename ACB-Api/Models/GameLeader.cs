namespace ACB_Api.Models {
	/// <summary>
	/// Clase que contiene dos listas con los líderes de juego de cada equipo.
	/// </summary>
	public class GameLeader {
		public List<long> HomeTeamLeaders { get; set; }
		public List<long> AwayTeamleaders { get; set; }
	}
}
