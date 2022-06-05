namespace Imdb.Loader.Helpers;

public static class SqlQueries
{
    public const string CreateMissingImdbTables = @"
CREATE TABLE IF NOT EXISTS [name.basics] (
    NameId           INTEGER NOT NULL
                             CONSTRAINT [PK_name.basics] PRIMARY KEY,
    Name             TEXT    NOT NULL,
    BirthYear        INTEGER,
    DeathYear        INTEGER,
    Professions      TEXT,
    KnownForTitleIds TEXT
)
WITHOUT ROWID;

CREATE TABLE IF NOT EXISTS [title.akas] (
    TitleId         INTEGER NOT NULL,
    [Index]         INTEGER NOT NULL,
    Title           TEXT    NOT NULL,
    Region          TEXT,
    Language        TEXT,
    Types           TEXT,
    Attributes      TEXT,
    IsOriginalTitle INTEGER,
    CONSTRAINT [PK_title.akas] PRIMARY KEY (
        TitleId,
        [Index]
    )
)
WITHOUT ROWID;

CREATE TABLE IF NOT EXISTS [title.basics] (
    TitleId        INTEGER NOT NULL
                           CONSTRAINT [PK_title.basics] PRIMARY KEY,
    TitleType      TEXT    NOT NULL,
    PrimaryTitle   TEXT    NOT NULL,
    OriginalTitle  TEXT,
    IsAdult        BOOLEAN,
    StartYear      INTEGER,
    EndYear        INTEGER,
    RuntimeMinutes INTEGER,
    Genres         TEXT
)
WITHOUT ROWID;

CREATE TABLE IF NOT EXISTS [title.crew] (
    TitleId         INTEGER NOT NULL
                            CONSTRAINT [PK_title.crew] PRIMARY KEY,
    DirectorNameIds TEXT,
    WriterNameIds   TEXT
)
WITHOUT ROWID;

CREATE TABLE IF NOT EXISTS [title.episodes] (
    TitleId       INTEGER NOT NULL
                          CONSTRAINT [PK_title.episodes] PRIMARY KEY,
    ParentTitleId INTEGER NOT NULL,
    SeasonNumber  INTEGER,
    EpisodeNumber INTEGER
)
WITHOUT ROWID;

CREATE TABLE IF NOT EXISTS [title.principals] (
    TitleId    INTEGER NOT NULL,
    [Index]    INTEGER NOT NULL,
    NameId     INTEGER NOT NULL,
    Category   TEXT,
    Job        TEXT,
    Characters TEXT,
    CONSTRAINT [PK_title.principals] PRIMARY KEY (
        TitleId,
        [Index]
    )
)
WITHOUT ROWID;

CREATE TABLE IF NOT EXISTS [title.ratings] (
    TitleId       INTEGER NOT NULL
                          CONSTRAINT [PK_title.ratings] PRIMARY KEY,
    AverageRating DOUBLE  NOT NULL,
    NumberOfVotes INTEGER NOT NULL
)
WITHOUT ROWID;
";

    public const string PragmaStatements = "PRAGMA cache_size = -125;PRAGMA synchronous = NORMAL;PRAGMA journal_mode = WAL";

    public const string Vacuum = "VACUUM";
}
