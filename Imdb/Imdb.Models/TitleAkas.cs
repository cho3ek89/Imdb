namespace Imdb.Models;

public class TitleAkas
{
    public uint TitleId { get; set; }

    public uint Index { get; set; }

    public string Title { get; set; }

    public string Region { get; set; }

    public string Language { get; set; }

    public string Types { get; set; }

    public string Attributes { get; set; }

    public bool? IsOriginalTitle { get; set; }
}
