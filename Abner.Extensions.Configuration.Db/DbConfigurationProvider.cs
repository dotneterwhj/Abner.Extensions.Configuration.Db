using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

namespace Abner.Extensions.Configuration.Db
{
    public abstract class DbConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private IDisposable _changeRegister;

        protected abstract Func<DbConnection> CreateDbConnection { get; }

        protected DbConfigurationSource Source { get; }

        protected DbConfigurationOption option { get; }

        public DbConfigurationProvider(DbConfigurationSource source)
        {
            this.Source = source;

            this.option = source.Option;

            SqlHelper.ExcuteNonQuery(CreateDbConnection(), CreateTableScript(), null);

            if (option.ReloadOnChange)
            {
                _changeRegister = ChangeToken.OnChange(
                    () =>
                    {
                        return Source.DbProvider.Watch(option.DbSetting.TableName);
                    },
                    () =>
                    {
                        Load(true);
                    });
            }
        }

        public override void Load()
        {
            Load(false);
        }

        private void Load(bool reload)
        {
            var old = Clone(Data);

            FillData(SelectFromDbScript());

            if (reload && IsChanged(old, Data))
            {
                OnReload();
            }
        }

        /// <summary>
        /// 填充IDictionary<string, string> Data数据
        /// </summary>
        /// <param name="queryString"></param>
        private void FillData(string queryString)
        {
            SqlHelper.ExcuteQuery(CreateDbConnection(), queryString, reader =>
            {
                while (reader.Read())
                {
                    string key = reader.GetString(0);
                    string value = reader.GetString(1);

                    if (string.IsNullOrEmpty(value))
                    {
                        Data[key] = null;
                        continue;
                    }

                    foreach (var kv in JsonParser.Parse(key, value))
                    {
                        Data[kv.Key] = kv.Value;
                    }
                }
            });
        }

        public override void Set(string key, string value)
        {
            if (option.DbSetting.SetPersistent)
            {
                string sqlstring = "";
                List<DbParameter> parameters = new List<DbParameter>();

                if (Data.ContainsKey(key))
                {
                    // 更新配置
                    sqlstring = UpdateToScript(key, value, out parameters);
                }
                else
                {
                    // 插入配置
                    sqlstring = InsertToDbScript(key, value, out parameters);
                }

                SqlHelper.ExcuteNonQuery(CreateDbConnection(), sqlstring, parameters);
            }
            base.Set(key, value);
        }

        /// <summary>
        /// 创建配置表sql语句
        /// </summary>
        /// <returns>
        /// such as sqlite:
        /// CREATE TABLE IF NOT EXISTS [Db_Configs](
        /// Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
        /// Name TEXT NOT NULL,
        /// Value TEXT NOT NULL
        /// );</returns>
        protected abstract string CreateTableScript();

        /// <summary>
        /// 查询配置表sql语句
        /// </summary>
        /// <returns>
        /// such as sqlite:
        /// SELECT NAME,VALUE FROM Db_Configs
        /// </returns>
        protected virtual string SelectFromDbScript()
        {
            return $@"SELECT {option.DbSetting.KeyColumn}, {option.DbSetting.ValueColumn} FROM {option.DbSetting.TableName}";
        }

        /// <summary>
        /// 插入配置表sql语句
        /// </summary>
        /// <returns>
        /// such as sqlite:
        /// INSERT INTO Db_Configs (Name,Value) VALUES (@Name,@Value)
        /// </returns>
        protected abstract string InsertToDbScript(string key, string value, out List<DbParameter> dbParameters);

        /// <summary>
        /// 更新配置表sql语句
        /// </summary>
        /// <returns>
        /// such as sqlite:
        /// UPDATE Db_Configs SET Vaule = @Value WHERE Name = @Name
        /// </returns>
        protected abstract string UpdateToScript(string key, string value, out List<DbParameter> dbParameters);


        private IDictionary<string, string> Clone(IDictionary<string, string> dic)
        {
            var previewData = new Dictionary<string, string>();

            foreach (var item in dic)
            {
                previewData[item.Key] = item.Value;
            }

            return previewData;
        }

        private bool IsChanged(IDictionary<string, string> old, IDictionary<string, string> @new)
        {
            if (old.Count != @new.Count)
            {
                return true;
            }
            foreach (var item in old)
            {
                if (item.Value != @new[item.Key])
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            _changeRegister?.Dispose();
        }
    }
}
