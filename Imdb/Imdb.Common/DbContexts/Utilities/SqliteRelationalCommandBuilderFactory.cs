using Microsoft.EntityFrameworkCore.Storage;

namespace Imdb.Common.DbContexts.Utilities;

public class SqliteRelationalCommandBuilderFactory(RelationalCommandBuilderDependencies dependencies) : RelationalCommandBuilderFactory(dependencies)
{
    public override IRelationalCommandBuilder Create() => new SqliteRelationalCommandBuilder(Dependencies);
}
