using Microsoft.Extensions.Configuration;
using Xunit;

namespace Abner.Extensions.Configuration.Db.Sqlite.Test
{
    public class SqliteDbConfigurationProviderTest
    {
        [Fact]
        public void CanLoadDataFromSqlite()
        {
            var builder = new ConfigurationBuilder().AddSqlite();

            var config = builder.Build();

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

        }
    }
}