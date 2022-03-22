using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;

namespace Abner.Extensions.Configuration.Db.Sqlite
{
    public static class SqliteDbConfigurationExtensions
    {
        //private static string _sqliteConnectionStr = $"Filename={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration.db")};Cache=Shared";
        private static string _sqliteConnectionStr = $"Data Source=configuration.db;Cache=Shared";

        /// <summary>
        /// Adds the Db Configuration provider
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSqlite(this IConfigurationBuilder configurationBuilder)
            => AddSqlite(configurationBuilder, _sqliteConnectionStr);

        /// <summary>
        /// Adds the Db Configuration provider
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="reloadOnChange"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSqlite(this IConfigurationBuilder configurationBuilder, string connectionString, bool reloadOnChange = false)
            => AddSqlite(configurationBuilder, connectionString, DbSetting.Default.TableName, reloadOnChange);

        /// <summary>
        /// Adds the Db Configuration provider
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSqlite(this IConfigurationBuilder configurationBuilder, string connectionString, string tableName, bool reloadOnChange = false)
            => AddSqlite(configurationBuilder, options =>
            {
                options.DbSetting.TableName = tableName;
                options.ReloadOnChange = reloadOnChange;
                options.DbSetting.ConnectionString = connectionString;
                options.CreateDbConnection = () => new SqliteConnection(connectionString);
            });

        /// <summary>
        /// Adds the Db Configuration provider
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSqlite(this IConfigurationBuilder configurationBuilder, Action<DbConfigurationOption> action)
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

            configurationBuilder.Add(new SqliteDbConfigurationSource(option));

            return configurationBuilder;
        }
    }
}
