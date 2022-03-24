using Microsoft.Extensions.Configuration;

namespace Abner.Extensions.Configuration.Db
{
    public abstract class DbConfigurationSource : IConfigurationSource
    {
        public DbConfigurationSource(DbConfigurationOption option)
        {
            Option = option;
        }

        public DbConfigurationOption Option { get; }

        public IDbProvider DbProvider { get; set; }

        public abstract IConfigurationProvider Build(IConfigurationBuilder builder);

        public void EnsureDefaults(IConfigurationBuilder builder)
        {
            DbProvider = DbProvider ?? builder.GetDbProvider();
        }
    }
}
