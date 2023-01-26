using Microsoft.EntityFrameworkCore.Storage;

namespace Imdb.Common.DbContexts.Utilities
{
    public class SqliteRelationalCommandBuilderFactory : RelationalCommandBuilderFactory
    {
        public SqliteRelationalCommandBuilderFactory(RelationalCommandBuilderDependencies dependencies)
            : base(dependencies) { }

        public override IRelationalCommandBuilder Create() => new SqliteRelationalCommandBuilder(Dependencies);
    }
}
