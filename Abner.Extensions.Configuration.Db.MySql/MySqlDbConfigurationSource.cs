using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abner.Extensions.Configuration.Db.MySql
{
    internal class MySqlDbConfigurationSource : DbConfigurationSource
    {
        public MySqlDbConfigurationSource(DbConfigurationOption option) : base(option)
        {
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new MySqlDbConfigurationProvider(this);
        }
    }
}
