namespace Imdb.Loader.Options;

public class DatabaseSettings
{
    public int BatchSize { get; set; }

    public string ConnectionString { get; set; }

    public bool VacuumDatabase { get; set; }
}
