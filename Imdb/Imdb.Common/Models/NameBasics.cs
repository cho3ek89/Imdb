namespace Imdb.Common.Models;

public class NameBasics
{
    public uint NameId { get; set; }

    public string Name { get; set; }

    public uint? BirthYear { get; set; }

    public uint? DeathYear { get; set; }

    public string Professions { get; set; }

    public string KnownForTitleIds { get; set; }
}
