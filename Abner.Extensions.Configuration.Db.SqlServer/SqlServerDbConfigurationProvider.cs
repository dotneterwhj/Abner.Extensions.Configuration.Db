using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace Abner.Extensions.Configuration.Db.SqlServer
{
    internal class SqlServerDbConfigurationProvider : DbConfigurationProvider
    {
        public SqlServerDbConfigurationProvider(DbConfigurationSource source) : base(source)
        {
        }

        protected override Func<DbConnection> CreateDbConnection => () => new SqlConnection(option.DbSetting.ConnectionString);

        protected override string CreateTableScript()
        {
            var tableName = option.DbSetting.TableName;

            var keyColumn = option.DbSetting.KeyColumn;

            var valueColumn = option.DbSetting.ValueColumn;

            var sqlScript = $@"IF OBJECT_ID(N'{tableName}',N'U') IS NULL
BEGIN
CREATE TABLE {tableName}(
	[Id] [int] IDENTITY(1,1) NOT NULL,
    [{keyColumn}] [nvarchar](255) NOT NULL,
	[{valueColumn}] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
END;";

            return sqlScript;
        }

        protected override string SelectFromDbScript()
        {
            return $@"SELECT [{option.DbSetting.KeyColumn}], [{option.DbSetting.ValueColumn}] FROM [{option.DbSetting.TableName}]";
        }

        protected override string InsertToDbScript(string key, string value, out List<DbParameter> dbParameters)
        {
            dbParameters = new List<DbParameter>();
            dbParameters.Add(new SqlParameter(option.DbSetting.KeyColumn, key));
            dbParameters.Add(new SqlParameter(option.DbSetting.ValueColumn, value));

            return $@"INSERT INTO [{option.DbSetting.TableName}] ([{option.DbSetting.KeyColumn}],[{option.DbSetting.ValueColumn}]) VALUES (@{option.DbSetting.KeyColumn},@{option.DbSetting.ValueColumn})";
        }

        protected override string UpdateToScript(string key, string value, out List<DbParameter> dbParameters)
        {
            dbParameters = new List<DbParameter>();
            dbParameters.Add(new SqlParameter(option.DbSetting.KeyColumn, key));
            dbParameters.Add(new SqlParameter(option.DbSetting.ValueColumn, value));

            return $@"UPDATE [{option.DbSetting.TableName}] SET [{option.DbSetting.ValueColumn}] = @{option.DbSetting.ValueColumn} WHERE [{option.DbSetting.KeyColumn}] = @{option.DbSetting.KeyColumn}";
        }
    }
}
