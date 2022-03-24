using Microsoft.Extensions.Configuration;
using Xunit;

namespace Abner.Extensions.Configuration.Db.MySql.Test
{
    public class MySqlDbConfigurationProviderTest
    {
        private readonly static string _connectionStr = "Server=localhost;Port=23306;Database=Demo;User=root;Password=123456;";

        [Fact]
        public void CanLoadDataFromMySql()
        {
            var builder = new ConfigurationBuilder().AddMySql(_connectionStr);

            var config = builder.Build();

            Assert.Equal("Something street", config["person:residential.address:0:street.name"]);
            Assert.Equal("123456222", config["person:residential.address:1:zipcode"]);
            Assert.Equal("test", config["person:firstname"]);
            Assert.Equal("value1", config["test"]);

        }

        [Fact]
        public void CanSetDataPersistent()
        {
            var builder = new ConfigurationBuilder()
                .AddMySql(options =>
                {
                    options.DbSetting.SetPersistent = true;
                    options.DbSetting.ConnectionString = _connectionStr;
                });

            var config = builder.Build();

            config["test2"] = "value2";
            Assert.Equal("value2", config["test2"]);

            Assert.Equal("Something street", config["person:residential.address:0:street.name"]);
            Assert.Equal("123456222", config["person:residential.address:1:zipcode"]);
            Assert.Equal("test", config["person:firstname"]);
            Assert.Equal("value1", config["test"]);
            config["test"] = "value";

        }

        [Fact]
        public void LoadData()
        {
            var builder = new ConfigurationBuilder()
                .AddMySql(options =>
                {
                    options.DbSetting.ConnectionString = _connectionStr;
                    options.DbSetting.SetPersistent = true;
                });

            var config = builder.Build();

            var json = @"
            {
                ""firstname"": ""test"",
                ""test.last.name"": ""last.name"",
                    ""residential.address"": [{
                        ""street.name"": ""Something street"",
                        ""zipcode"": ""12345""
                    },{
                        ""street.name"": ""Something street222"",
                        ""zipcode"": ""123456222""
                    }]
            }";
            config["person"] = json;
            config["test"] = "value1";
        }

        [Fact]
        public void CanChangeTableOrColumnName()
        {
            var builder = new ConfigurationBuilder()
                .AddMySql(options =>
                {
                    options.DbSetting.ConnectionString = _connectionStr;
                    options.DbSetting.TableName = "MyConfig";
                    options.DbSetting.KeyColumn = "key";
                    options.DbSetting.ValueColumn = "valu1";
                    options.DbSetting.SetPersistent = true;
                });

            var config = builder.Build();

            var json = @"
            {
                ""firstname"": ""test"",
                ""test.last.name"": ""last.name"",
                    ""residential.address"": [{
                        ""street.name"": ""Something street"",
                        ""zipcode"": ""12345""
                    },{
                        ""street.name"": ""Something street222"",
                        ""zipcode"": ""123456222""
                    }]
            }";
            config["person"] = json;
        }

        [Fact]
        public void CanChangeTableOrColumnName2()
        {
            var builder = new ConfigurationBuilder()
                .AddMySql(options =>
                {
                    options.DbSetting.ConnectionString = _connectionStr;
                    options.DbSetting.TableName = "MyConfig";
                    options.DbSetting.KeyColumn = "key";
                    options.DbSetting.ValueColumn = "valu1";
                });

            var config = builder.Build();

            Assert.Equal("test", config["person:firstname"]);
        }
    }
}
