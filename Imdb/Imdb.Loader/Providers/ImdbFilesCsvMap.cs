using CsvHelper.Configuration;

using Imdb.Common.Models;

using System.Globalization;

namespace Imdb.Loader.Providers;

public sealed class ImdbFilesCsvMap
{
    public sealed class NameBasicsMap : ClassMap<NameBasics>
    {
        public NameBasicsMap()
        {
            Map((m) => m.NameId).Name("nconst").Convert(c => GetId(c.Row.GetField("nconst")));
            Map((m) => m.Name).Name("primaryName").Convert(c => GetString(c.Row.GetField("primaryName")));
            Map((m) => m.BirthYear).Name("birthYear").Convert(c => GetNumber(c.Row.GetField("birthYear")));
            Map((m) => m.DeathYear).Name("deathYear").Convert(c => GetNumber(c.Row.GetField("deathYear")));
            Map((m) => m.Professions).Name("primaryProfession").Convert(c => GetString(c.Row.GetField("primaryProfession")));
            Map((m) => m.KnownForTitleIds).Name("knownForTitles").Convert(c => GetIds(c.Row.GetField("knownForTitles")));
        }
    }

    public sealed class TitleAkasMap : ClassMap<TitleAkas>
    {
        public TitleAkasMap()
        {
            Map((m) => m.TitleId).Name("titleId").Convert(c => GetId(c.Row.GetField("titleId")));
            Map((m) => m.Index).Name("ordering");
            Map((m) => m.Title).Name("title");
            Map((m) => m.Region).Name("region").Convert(c => GetString(c.Row.GetField("region")));
            Map((m) => m.Language).Name("language").Convert(c => GetString(c.Row.GetField("language")));
            Map((m) => m.Types).Name("types").Convert(c => GetString(c.Row.GetField("types")));
            Map((m) => m.Attributes).Name("attributes").Convert(c => GetString(c.Row.GetField("attributes")));
            Map((m) => m.IsOriginalTitle).Name("isOriginalTitle").Convert(c => GetBool(c.Row.GetField("isOriginalTitle")));
        }
    }

    public sealed class TitleBasicsMap : ClassMap<TitleBasics>
    {
        public TitleBasicsMap()
        {
            Map((m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((m) => m.TitleType).Name("titleType").Convert(c => GetString(c.Row.GetField("titleType")));
            Map((m) => m.PrimaryTitle).Name("primaryTitle").Convert(c => GetString(c.Row.GetField("primaryTitle")));
            Map((m) => m.OriginalTitle).Name("originalTitle").Convert(c => GetString(c.Row.GetField("originalTitle")));
            Map((m) => m.IsAdult).Name("isAdult").Convert(c => GetBool(c.Row.GetField("isAdult")));
            Map((m) => m.StartYear).Name("startYear").Convert(c => GetNumber(c.Row.GetField("startYear")));
            Map((m) => m.EndYear).Name("endYear").Convert(c => GetNumber(c.Row.GetField("endYear")));
            Map((m) => m.RuntimeMinutes).Name("runtimeMinutes").Convert(c => GetNumber(c.Row.GetField("runtimeMinutes")));
            Map((m) => m.Genres).Name("genres").Convert(c => GetString(c.Row.GetField("genres")));
        }
    }

    public sealed class TitleCrewMap : ClassMap<TitleCrew>
    {
        public TitleCrewMap()
        {
            Map((m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((m) => m.DirectorNameIds).Name("directors").Convert(c => GetIds(c.Row.GetField("directors")));
            Map((m) => m.WriterNameIds).Name("writers").Convert(c => GetIds(c.Row.GetField("writers")));
        }
    }

    public sealed class TitleEpisodeMap : ClassMap<TitleEpisode>
    {
        public TitleEpisodeMap()
        {
            Map((m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((m) => m.ParentTitleId).Name("parentTconst").Convert(c => GetId(c.Row.GetField("parentTconst")));
            Map((m) => m.SeasonNumber).Name("seasonNumber").Convert(c => GetNumber(c.Row.GetField("seasonNumber")));
            Map((m) => m.EpisodeNumber).Name("episodeNumber").Convert(c => GetNumber(c.Row.GetField("episodeNumber")));
        }
    }

    public sealed class TitlePrincipalsMap : ClassMap<TitlePrincipals>
    {
        public TitlePrincipalsMap()
        {
            Map((m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((m) => m.Index).Name("ordering");
            Map((m) => m.NameId).Name("nconst").Convert(c => GetId(c.Row.GetField("nconst")));
            Map((m) => m.Category).Name("category").Convert(c => GetString(c.Row.GetField("category")));
            Map((m) => m.Job).Name("job").Convert(c => GetString(c.Row.GetField("job")));
            Map((m) => m.Characters).Name("characters").Convert(c => GetCharactersString(c.Row.GetField("characters")));
        }
    }

    public sealed class TitleRatingMap : ClassMap<TitleRating>
    {
        public TitleRatingMap()
        {
            Map((m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((m) => m.AverageRating).Name("averageRating");
            Map((m) => m.NumberOfVotes).Name("numVotes");
        }
    }

    private static readonly Func<string, uint> GetId = (val) => uint.Parse(val[2..], CultureInfo.InvariantCulture);

    private static readonly Func<string, string> GetIds = (val) => !val.Equals("\\N") ? string.Join(',', from id in val.Split(',') select GetId(id)) : null;

    private static readonly Func<string, uint?> GetNumber = (val) => !val.Equals("\\N") ? new uint?(uint.Parse(val, CultureInfo.InvariantCulture)) : null;

    private static readonly Func<string, string> GetString = (val) => !val.Equals("\\N") ? val : null;

    private static readonly Func<string, string> GetCharactersString = delegate (string val)
    {
        if (val.Equals("\\N"))
        {
            return null;
        }
        string text = val;
        if (val.StartsWith("[\""))
        {
            text = text[2..];
        }
        if (val.EndsWith("\"]"))
        {
            text = text[..(text.Length - 2)];
        }
        return text;
    };

    private static readonly Func<string, bool?> GetBool = delegate (string val)
    {
        if (val.Equals("1"))
        {
            return true;
        }
        return val.Equals("0") ? new bool?(false) : null;
    };
}
