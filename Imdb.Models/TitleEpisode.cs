namespace Imdb.Models;

public class TitleEpisode
{
    public uint TitleId { get; set; }

    public uint ParentTitleId { get; set; }

    public uint? SeasonNumber { get; set; }

    public uint? EpisodeNumber { get; set; }
}
