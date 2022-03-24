using Microsoft.Extensions.Configuration;

namespace Abner.Extensions.Configuration.Db.SqlServer
{
    internal class SqlServerDbConfigurationSource : DbConfigurationSource
    {
        public SqlServerDbConfigurationSource(DbConfigurationOption option) : base(option)
        {
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            base.EnsureDefaults(builder);
            return new SqlServerDbConfigurationProvider(this);
        }
    }
}
