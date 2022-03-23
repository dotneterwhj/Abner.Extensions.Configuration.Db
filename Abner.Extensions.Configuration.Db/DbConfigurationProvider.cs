using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text.Json;

namespace Abner.Extensions.Configuration.Db
{
    public abstract class DbConfigurationProvider : ConfigurationProvider, IDisposable
    {
        protected readonly DbConfigurationOption _option;

        public DbConfigurationProvider(DbConfigurationOption option)
        {
            this._option = option;

            SqlHelper.ExcuteNonQuery(_option.CreateDbConnection(), CreateTable(), null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            FillData(SelectFromDb());
        }

        /// <summary>
        /// 填充IDictionary<string, string> Data数据
        /// </summary>
        /// <param name="queryString"></param>
        private void FillData(string queryString)
        {
            using (var dbConnection = _option.CreateDbConnection.Invoke())
            {
                DbCommand command = dbConnection.CreateCommand();
                command.CommandText = queryString;

                try
                {
                    dbConnection.Open();
                    DbDataReader reader = command.ExecuteReader();
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
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }
        }

        public override void Set(string key, string value)
        {
            if (_option.SetPersistent)
            {
                string sqlstring = "";
                List<DbParameter> parameters = new List<DbParameter>();

                if (Data.ContainsKey(key))
                {
                    // 更新配置
                    sqlstring = UpdateToDb(key, value, out parameters);
                }
                else
                {
                    // 插入配置
                    sqlstring = InsertToDb(key, value, out parameters);
                }

                SqlHelper.ExcuteNonQuery(_option.CreateDbConnection(), sqlstring, parameters);
            }
            base.Set(key, value);
        }

        /// <summary>
        /// 创建配置表
        /// </summary>
        protected abstract string CreateTable();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string SelectFromDb()
        {
            return $@"SELECT {_option.DbSetting.KeyColumn}, {_option.DbSetting.ValueColumn} FROM {_option.DbSetting.TableName}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract string InsertToDb(string key, string value, out List<DbParameter> dbParameters);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract string UpdateToDb(string key, string value, out List<DbParameter> dbParameters);

    }
}
