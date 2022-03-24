using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics;
using Xunit;

namespace Abner.Extensions.Configuration.Db.Sqlite.Test
{
    public class SqliteDbConfigurationProviderTest
    {
        [Fact]
        public void CanLoadDataFromSqlite()
        {
            var builder = new ConfigurationBuilder().AddSqlite(options =>
            {
                options.DbSetting.ConnectionString = @"Data Source=configuration.db;Cache=Shared";
                options.ReloadOnChange = true;
            });

            var config = builder.Build();

            ChangeToken.OnChange(
                    () =>
                    {
                        return config.GetReloadToken();
                    },
                    () =>
                    {
                        Debug.WriteLine("配置发生了变化");
                    });

            //            var json = @"
            //{
            //    ""firstname"": ""test"",
            //    ""test.last.name"": ""last.name"",
            //        ""residential.address"": {
            //            ""street.name"": ""Something street"",
            //            ""zipcode"": ""12345""
            //        }
            //}";

            //            var json1 = @"
            //[{
            //    ""firstname"": ""test"",
            //    ""test.last.name"": ""last.name"",
            //        ""residential.address"": {
            //            ""street.name"": ""Something street"",
            //            ""zipcode"": ""12345""
            //        }
            //},{
            //    ""firstname"": ""test1"",
            //    ""test.last.name"": ""last.name1"",
            //        ""residential.address"": {
            //            ""street.name"": ""Something street1"",
            //            ""zipcode"": ""123456""
            //        }
            //}]";


            //            config["persons"] = json1;
            //            config["person"] = json;

            //            var value2 = config["person:firstname"];

            Assert.Equal("Something street", config["person:residential.address:0:street.name"]);
            Assert.Equal("123456222", config["person:residential.address:1:zipcode"]);
            Assert.Equal("test", config["person:firstname"]);
            Assert.Equal("value1", config["test"]);

            Console.ReadKey();
        }
    }
}