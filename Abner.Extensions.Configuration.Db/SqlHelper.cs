using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace Abner.Extensions.Configuration.Db
{
    public class SqlHelper
    {
        public static void ExcuteNonQuery(DbConnection dbConn, string commandText, List<DbParameter> dbParameters)
        {
            DbCommand command = dbConn.CreateCommand();
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
}
