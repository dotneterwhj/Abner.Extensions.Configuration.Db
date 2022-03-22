using Microsoft.Extensions.Configuration;
using System;
using System.Data.Common;
using System.Text;

namespace Abner.Extensions.Configuration.Db
{
    public abstract class DbConfigurationSource : IConfigurationSource
    {

        public DbConfigurationSource(DbConfigurationOption option)
        {
            Option = option;
        }

        public DbConfigurationOption Option { get; }

        public abstract IConfigurationProvider Build(IConfigurationBuilder builder);
    }
}
