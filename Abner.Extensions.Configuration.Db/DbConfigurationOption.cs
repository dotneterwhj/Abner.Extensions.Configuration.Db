namespace Abner.Extensions.Configuration.Db
{
    public class DbConfigurationOption
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        public DbSetting DbSetting { get; set; } = DbSetting.Default;

        /// <summary>
        /// Determines whether the source will be loaded if the underlying file changes.
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// Number of milliseconds that reload will wait before calling Load.  This helps
        /// avoid triggering reload before a file is completely written. Default is 1000.
        /// </summary>
        public int ReloadDelay { get; set; } = 1000;
    }

    public class DbSetting
    {
        public static DbSetting Default { get; } = new DbSetting();

        /// <summary>
        /// 配置的更改是否持久化到数据库,目前只支持整体保存更改
        /// </summary>
        public bool SetPersistent { get; set; } = false;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 映射到数据库的配置表名
        /// </summary>
        public string TableName { get; set; } = "Db_Configs";

        /// <summary>
        /// 映射到数据库表的键字段列名
        /// </summary>
        public string KeyColumn { get; set; } = "Name";

        /// <summary>
        /// 映射到数据库表的值字段列名
        /// </summary>
        public string ValueColumn { get; set; } = "Value";

    }
}
