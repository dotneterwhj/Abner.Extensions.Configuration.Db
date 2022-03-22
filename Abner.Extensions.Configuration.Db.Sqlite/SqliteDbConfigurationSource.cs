using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abner.Extensions.Configuration.Db.Sqlite
{
    internal class SqliteDbConfigurationSource : DbConfigurationSource
    {
        public SqliteDbConfigurationSource(DbConfigurationOption option) : base(option)
        {
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SqliteDbConfigurationProvider(base.Option);
        }
    }
}
