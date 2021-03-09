namespace Imdb.Models
{
    public class TitleBasics
    {
		public uint TitleId { get; set; }

		public string TitleType { get; set; }

		public string PrimaryTitle { get; set; }

		public string OriginalTitle { get; set; }

		public bool? IsAdult { get; set; }

		public uint? StartYear { get; set; }

		public uint? EndYear { get; set; }

		public uint? RuntimeMinutes { get; set; }

		public string Genres { get; set; }
	}
}
