using Microsoft.Extensions.Configuration;
using Xunit;

namespace Abner.Extensions.Configuration.Db.SqlServer.Test
{
    public class SqlServerDbConfigurationProviderTest
    {
        [Fact]
        public void CanLoadDataFromSqlServer()
        {
            var builder = new ConfigurationBuilder().AddSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=demo1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

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
                .AddSqlServer(options =>
                {
                    options.DbSetting.SetPersistent = true;
                    options.DbSetting.ConnectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=demo1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                });

            var config = builder.Build();



            Assert.Equal("Something street", config["person:residential.address:0:street.name"]);
            Assert.Equal("123456222", config["person:residential.address:1:zipcode"]);
            Assert.Equal("test", config["person:firstname"]);
            Assert.Equal("value1", config["test"]);

            config["test2"] = "value2";
            Assert.Equal("value2", config["test2"]);

            config["test"] = "value";

        }

        [Fact]
        public void CanChangeTableOrColumnName()
        {
            var builder = new ConfigurationBuilder()
                .AddSqlServer(options =>
                {
                    options.DbSetting.ConnectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=demo1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
                    ""residential.address"": {
                        ""street.name"": ""Something street"",
                        ""zipcode"": ""12345""
                    }
            }";
            config["person"] = json;
        }

        [Fact]
        public void CanChangeTableOrColumnName2()
        {
            var builder = new ConfigurationBuilder()
                .AddSqlServer(options =>
                {
                    options.DbSetting.ConnectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=demo1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    options.DbSetting.TableName = "MyConfig";
                    options.DbSetting.KeyColumn = "key";
                    options.DbSetting.ValueColumn = "valu1";
                });

            var config = builder.Build();

            Assert.Equal("test", config["person:firstname"]);
        }
    }
}