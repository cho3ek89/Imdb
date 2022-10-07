namespace Imdb.Common.Models;

public class TitleRating
{
    public uint TitleId { get; set; }

    public double AverageRating { get; set; }

    public uint NumberOfVotes { get; set; }
}
