namespace Imdb.Loader.Helpers;

using CsvHelper.Configuration;

using Imdb.Models;

using System;
using System.Globalization;
using System.Linq;

public sealed class ImdbFilesCsvMap
{
    public sealed class NameBasicsMap : ClassMap<NameBasics>
    {
        public NameBasicsMap()
        {
            Map((NameBasics m) => m.NameId).Name("nconst").Convert(c => GetId(c.Row.GetField("nconst")));
            Map((NameBasics m) => m.Name).Name("primaryName").Convert(c => GetString(c.Row.GetField("primaryName")));
            Map((NameBasics m) => m.BirthYear).Name("birthYear").Convert(c => GetNumber(c.Row.GetField("birthYear")));
            Map((NameBasics m) => m.BirthYear).Name("deathYear").Convert(c => GetNumber(c.Row.GetField("deathYear")));
            Map((NameBasics m) => m.Professions).Name("primaryProfession").Convert(c => GetString(c.Row.GetField("primaryProfession")));
            Map((NameBasics m) => m.Professions).Name("primaryProfession").Convert(c => GetString(c.Row.GetField("primaryProfession")));
            Map((NameBasics m) => m.KnownForTitleIds).Name("knownForTitles").Convert(c => GetIds(c.Row.GetField("knownForTitles")));
        }
    }

    public sealed class TitleAkasMap : ClassMap<TitleAkas>
    {
        public TitleAkasMap()
        {
            Map((TitleAkas m) => m.TitleId).Name("titleId").Convert(c => GetId(c.Row.GetField("titleId")));
            Map((TitleAkas m) => m.Index).Name("ordering");
            Map((TitleAkas m) => m.Title).Name("title");
            Map((TitleAkas m) => m.Region).Name("region").Convert(c => GetString(c.Row.GetField("region")));
            Map((TitleAkas m) => m.Language).Name("language").Convert(c => GetString(c.Row.GetField("language")));
            Map((TitleAkas m) => m.Types).Name("types").Convert(c => GetString(c.Row.GetField("types")));
            Map((TitleAkas m) => m.Attributes).Name("attributes").Convert(c => GetString(c.Row.GetField("attributes")));
            Map((TitleAkas m) => m.IsOriginalTitle).Name("isOriginalTitle").Convert(c => GetBool(c.Row.GetField("isOriginalTitle")));
        }
    }

    public sealed class TitleBasicsMap : ClassMap<TitleBasics>
    {
        public TitleBasicsMap()
        {
            Map((TitleBasics m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((TitleBasics m) => m.TitleType).Name("titleType").Convert(c => GetString(c.Row.GetField("titleType")));
            Map((TitleBasics m) => m.PrimaryTitle).Name("primaryTitle").Convert(c => GetString(c.Row.GetField("primaryTitle")));
            Map((TitleBasics m) => m.OriginalTitle).Name("originalTitle").Convert(c => GetString(c.Row.GetField("originalTitle")));
            Map((TitleBasics m) => m.IsAdult).Name("isAdult").Convert(c => GetBool(c.Row.GetField("isAdult")));
            Map((TitleBasics m) => m.StartYear).Name("startYear").Convert(c => GetNumber(c.Row.GetField("startYear")));
            Map((TitleBasics m) => m.EndYear).Name("endYear").Convert(c => GetNumber(c.Row.GetField("endYear")));
            Map((TitleBasics m) => m.RuntimeMinutes).Name("runtimeMinutes").Convert(c => GetNumber(c.Row.GetField("runtimeMinutes")));
            Map((TitleBasics m) => m.Genres).Name("genres").Convert(c => GetString(c.Row.GetField("genres")));
        }
    }

    public sealed class TitleCrewMap : ClassMap<TitleCrew>
    {
        public TitleCrewMap()
        {
            Map((TitleCrew m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((TitleCrew m) => m.DirectorNameIds).Name("directors").Convert(c => GetIds(c.Row.GetField("directors")));
            Map((TitleCrew m) => m.WriterNameIds).Name("writers").Convert(c => GetIds(c.Row.GetField("writers")));
        }
    }

    public sealed class TitleEpisodeMap : ClassMap<TitleEpisode>
    {
        public TitleEpisodeMap()
        {
            Map((TitleEpisode m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((TitleEpisode m) => m.ParentTitleId).Name("parentTconst").Convert(c => GetId(c.Row.GetField("parentTconst")));
            Map((TitleEpisode m) => m.SeasonNumber).Name("seasonNumber").Convert(c => GetNumber(c.Row.GetField("seasonNumber")));
            Map((TitleEpisode m) => m.EpisodeNumber).Name("episodeNumber").Convert(c => GetNumber(c.Row.GetField("episodeNumber")));
        }
    }

    public sealed class TitlePrincipalsMap : ClassMap<TitlePrincipals>
    {
        public TitlePrincipalsMap()
        {
            Map((TitlePrincipals m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((TitlePrincipals m) => m.Index).Name("ordering");
            Map((TitlePrincipals m) => m.NameId).Name("nconst").Convert(c => GetId(c.Row.GetField("nconst")));
            Map((TitlePrincipals m) => m.Category).Name("category").Convert(c => GetString(c.Row.GetField("category")));
            Map((TitlePrincipals m) => m.Job).Name("job").Convert(c => GetString(c.Row.GetField("job")));
            Map((TitlePrincipals m) => m.Characters).Name("characters").Convert(c => GetCharactersString(c.Row.GetField("characters")));
        }
    }

    public sealed class TitleRatingMap : ClassMap<TitleRating>
    {
        public TitleRatingMap()
        {
            Map((TitleRating m) => m.TitleId).Name("tconst").Convert(c => GetId(c.Row.GetField("tconst")));
            Map((TitleRating m) => m.AverageRating).Name("averageRating");
            Map((TitleRating m) => m.NumberOfVotes).Name("numVotes");
        }
    }

    private static readonly Func<string, uint> GetId = (string val) => uint.Parse(val[2..], CultureInfo.InvariantCulture);

    private static readonly Func<string, string> GetIds = (string val) => (!val.Equals("\\N")) ? string.Join(',', from id in val.Split(',') select GetId(id)) : null;

    private static readonly Func<string, uint?> GetNumber = (string val) => (!val.Equals("\\N")) ? new uint?(uint.Parse(val, CultureInfo.InvariantCulture)) : null;

    private static readonly Func<string, string> GetString = (string val) => (!val.Equals("\\N")) ? val : null;

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
