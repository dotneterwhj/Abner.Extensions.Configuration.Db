using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abner.Extensions.Configuration.Db
{
    public static class DbConfigurationExtensions
    {
        public const string DbProviderKey = "DbProvider";

        public static IConfigurationBuilder SetDbProvider(this IConfigurationBuilder builder, IDbProvider dbProvider)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Properties[DbProviderKey] = dbProvider ?? throw new ArgumentNullException(nameof(dbProvider));
            return builder;
        }

        public static IDbProvider GetDbProvider(this IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Properties.TryGetValue(DbProviderKey, out object provider))
            {
                return provider as IDbProvider;
            }

            var dbProvider = new DefaultDbProvider(builder.Sources);

            SetDbProvider(builder, dbProvider);

            return dbProvider;
        }
    }
}
