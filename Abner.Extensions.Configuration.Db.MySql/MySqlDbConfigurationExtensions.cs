using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abner.Extensions.Configuration.Db.MySql
{
    public static class MySqlDbConfigurationExtensions
    {
        /// <summary>
        /// Adds the Db Configuration provider
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="reloadOnChange"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddMySql(this IConfigurationBuilder configurationBuilder, string connectionString, bool reloadOnChange = false)
            => AddMySql(configurationBuilder, connectionString, DbSetting.Default.TableName, reloadOnChange);

        /// <summary>
        /// Adds the Db Configuration provider
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddMySql(this IConfigurationBuilder configurationBuilder, string connectionString, string tableName, bool reloadOnChange = false)
            => AddMySql(configurationBuilder, options =>
            {
                options.DbSetting.TableName = tableName;
                options.ReloadOnChange = reloadOnChange;
                options.DbSetting.ConnectionString = connectionString;
            });

        /// <summary>
        /// Adds the Db Configuration provider
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddMySql(this IConfigurationBuilder configurationBuilder, Action<DbConfigurationOption> action)
        {
            var option = new DbConfigurationOption();

            action?.Invoke(option);

            if (string.IsNullOrEmpty(option.DbSetting.ConnectionString))
            {
                throw new ArgumentException("ConnectionString cannot be null or empety");
            }

            if (string.IsNullOrEmpty(option.DbSetting.TableName))
            {
                throw new ArgumentException("TableName cannot be null or empety");
            }

            if (string.IsNullOrEmpty(option.DbSetting.KeyColumn))
            {
                throw new ArgumentException("KeyColumn cannot be null or empety");
            }

            if (string.IsNullOrEmpty(option.DbSetting.ValueColumn))
            {
                throw new ArgumentException("ValueColumn cannot be null or empety");
            }

            configurationBuilder.Add(new MySqlDbConfigurationSource(option));

            return configurationBuilder;
        }
    }
}
