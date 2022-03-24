using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Abner.Extensions.Configuration.Db.Sqlite
{
    internal class SqliteDbConfigurationProvider : DbConfigurationProvider
    {
        public SqliteDbConfigurationProvider(DbConfigurationSource source) : base(source)
        {
        }

        protected override Func<DbConnection> CreateDbConnection { get => () => new SqliteConnection(option.DbSetting.ConnectionString); }

        protected override string CreateTableScript()
        {
            return $@"CREATE TABLE IF NOT EXISTS [{option.DbSetting.TableName}](
   Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
   [{option.DbSetting.KeyColumn}] TEXT NOT NULL,
   [{option.DbSetting.ValueColumn}] TEXT NOT NULL
);";
        }

        protected override string InsertToDbScript(string key, string value, out List<DbParameter> dbParameters)
        {
            dbParameters = new List<DbParameter>();
            dbParameters.Add(new SqliteParameter(option.DbSetting.KeyColumn, key));
            dbParameters.Add(new SqliteParameter(option.DbSetting.ValueColumn, value));

            return $@"INSERT INTO [{option.DbSetting.TableName}] ([{option.DbSetting.KeyColumn}],[{option.DbSetting.ValueColumn}]) VALUES (@{option.DbSetting.KeyColumn},@{option.DbSetting.ValueColumn})";
        }

        protected override string UpdateToScript(string key, string value, out List<DbParameter> dbParameters)
        {
            dbParameters = new List<DbParameter>();
            dbParameters.Add(new SqliteParameter(option.DbSetting.KeyColumn, key));
            dbParameters.Add(new SqliteParameter(option.DbSetting.ValueColumn, value));

            return $@"UPDATE [{option.DbSetting.TableName}] SET [{option.DbSetting.ValueColumn}] = @{option.DbSetting.ValueColumn} WHERE [{option.DbSetting.KeyColumn}] = @{option.DbSetting.KeyColumn}";
        }
    }
}
