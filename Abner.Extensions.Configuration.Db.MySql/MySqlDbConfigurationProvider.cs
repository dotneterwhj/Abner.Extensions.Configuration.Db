using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Abner.Extensions.Configuration.Db.MySql
{
    internal class MySqlDbConfigurationProvider : DbConfigurationProvider
    {
        public MySqlDbConfigurationProvider(DbConfigurationSource source) : base(source)
        {
        }

        protected override Func<DbConnection> CreateDbConnection => () => new MySqlConnection(option.DbSetting.ConnectionString);

        protected override string CreateTableScript()
        {
            var sqlScript =
    $@"
CREATE TABLE IF NOT EXISTS `{option.DbSetting.TableName}` (
  `Id` INT(11) PRIMARY KEY AUTO_INCREMENT NOT NULL,
  `{option.DbSetting.KeyColumn}` varchar(255) DEFAULT NULL,
  `{option.DbSetting.ValueColumn}` varchar(4000) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";

            return sqlScript;
        }

        protected override string SelectFromDbScript()
        {
            return $@"SELECT `{option.DbSetting.KeyColumn}`, `{option.DbSetting.ValueColumn}` FROM `{option.DbSetting.TableName}`";
        }

        protected override string InsertToDbScript(string key, string value, out List<DbParameter> dbParameters)
        {
            dbParameters = new List<DbParameter>();
            dbParameters.Add(new MySqlParameter(option.DbSetting.KeyColumn, key));
            dbParameters.Add(new MySqlParameter(option.DbSetting.ValueColumn, value));

            return $@"INSERT INTO `{option.DbSetting.TableName}` (`{option.DbSetting.KeyColumn}`,`{option.DbSetting.ValueColumn}`) VALUES (?{option.DbSetting.KeyColumn},?{option.DbSetting.ValueColumn})";

        }

        protected override string UpdateToScript(string key, string value, out List<DbParameter> dbParameters)
        {
            dbParameters = new List<DbParameter>();
            dbParameters.Add(new MySqlParameter(option.DbSetting.KeyColumn, key));
            dbParameters.Add(new MySqlParameter(option.DbSetting.ValueColumn, value));

            return $@"UPDATE `{option.DbSetting.TableName}` SET `{option.DbSetting.ValueColumn}` = ?{option.DbSetting.ValueColumn} WHERE `{option.DbSetting.KeyColumn}` = ?{option.DbSetting.KeyColumn}";

        }
    }
}
