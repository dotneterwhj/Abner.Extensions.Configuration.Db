using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;

namespace Abner.Extensions.Configuration.Db
{
    public class SqlHelper
    {
        public static void ExcuteNonQuery(DbConnection dbConn, string commandText, List<DbParameter> dbParameters)
        {
            using (DbCommand command = dbConn.CreateCommand())
            {
                command.CommandText = commandText;
                if (dbParameters != null)
                {
                    command.Parameters.AddRange(dbParameters.ToArray());
                }
                try
                {
                    dbConn.Open();
                    command.ExecuteNonQuery();
                    dbConn.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
                finally
                {
                    dbConn.Dispose();
                }
            }
        }


        public static void ExcuteQuery(DbConnection dbConn, string commandText, Action<DbDataReader> action)
        {
            using (DbCommand command = dbConn.CreateCommand())
            {
                command.CommandText = commandText;

                try
                {
                    dbConn.Open();
                    DbDataReader reader = command.ExecuteReader();
                    action?.Invoke(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
                finally
                {
                    dbConn.Dispose();
                }
            }
        }
    }
}
