using Microsoft.EntityFrameworkCore.Storage;

namespace Imdb.Common.DbContexts.Utilities
{
    public class SqliteRelationalCommandBuilder : RelationalCommandBuilder
    {
        public SqliteRelationalCommandBuilder(RelationalCommandBuilderDependencies dependencies)
            : base(dependencies) { }

        public override IRelationalCommand Build()
        {
            return new RelationalCommand(Dependencies, GetCommandText(), Parameters);
        }

        /// <summary>
        /// Gets SQL command.
        /// If it is 'CREATE TABLE' command, 'WITHOUT ROWID' phrase will be added to it.
        /// </summary>
        /// <see href="https://www.sqlite.org/withoutrowid.html"/>
        private string GetCommandText()
        {
            var commandText = base.ToString();

            return !commandText.StartsWith("CREATE TABLE") || commandText.Contains("AUTOINCREMENT")
                ? commandText
                : commandText[..commandText.LastIndexOf(");")] + ") WITHOUT ROWID;";
        }
    }
}
