using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace Abner.Extensions.Configuration.Db.Sqlite
{
    internal class SqliteDbConfigurationProvider : DbConfigurationProvider
    {
        public SqliteDbConfigurationProvider(DbConfigurationOption option) : base(option)
        {
        }


        protected override string CreateTable()
        {
            return $@"CREATE TABLE IF NOT EXISTS [{_option.DbSetting.TableName}](
   Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
   {_option.DbSetting.KeyColumn} TEXT NOT NULL,
   {_option.DbSetting.ValueColumn} TEXT NOT NULL
);";
        }

        protected override string InsertToDb(string key, string value, out List<DbParameter> dbParameters)
        {
            dbParameters = new List<DbParameter>();
            dbParameters.Add(new SqliteParameter(_option.DbSetting.KeyColumn, key));
            dbParameters.Add(new SqliteParameter(_option.DbSetting.ValueColumn, value));

            return $@"INSERT INTO {_option.DbSetting.TableName} ({_option.DbSetting.KeyColumn},{_option.DbSetting.ValueColumn}) VALUES (@{_option.DbSetting.KeyColumn},@{_option.DbSetting.ValueColumn})";
        }

        protected override string UpdateToDb(string key, string value, out List<DbParameter> dbParameters)
        {
            dbParameters = new List<DbParameter>();
            dbParameters.Add(new SqliteParameter(_option.DbSetting.KeyColumn, key));
            dbParameters.Add(new SqliteParameter(_option.DbSetting.ValueColumn, value));

            return $@"UPDATE {_option.DbSetting.TableName} SET {_option.DbSetting.ValueColumn} = @{_option.DbSetting.ValueColumn} WHERE {_option.DbSetting.KeyColumn} = @{_option.DbSetting.KeyColumn}";
        }
    }
}
