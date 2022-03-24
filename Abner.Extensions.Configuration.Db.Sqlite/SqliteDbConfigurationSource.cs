using Microsoft.Extensions.Configuration;

namespace Abner.Extensions.Configuration.Db.Sqlite
{
    internal class SqliteDbConfigurationSource : DbConfigurationSource
    {
        public SqliteDbConfigurationSource(DbConfigurationOption option) : base(option)
        {
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            base.EnsureDefaults(builder);
            return new SqliteDbConfigurationProvider(this);
        }
    }
}
