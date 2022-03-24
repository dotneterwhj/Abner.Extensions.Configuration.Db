### Abner.Extensions.Configuration.DbProvider

扩展.net core配置框架，从数据库中读取配置文件（目前支持的数据库 mysql,sqlserver,sqlite）

#### 第一步

以mysql为例，环境以vs2022+.net6为例，通过nuget安装包

```bash
Install-Package Abner.Extensions.Configuration.Db.MySql
```

#### 第二步

打开appsettings.json文件添加以下节点

```json
"ConnectionStrings": {
    "MySql": "Server=localhost;Port=3306;Database=Demo;User=root;Password=123456;",
    "SqlServer": "Data Source=(localdb)\\ProjectModels;Initial Catalog=demo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
    "Sqlite": "Data Source=configuration.db;Cache=Shared"
  }
```

需先在指定的dbms中创建连接字符串中的数据库“Demo”，使用sqlite时可忽略

在program中添加以下代码即可

```c#
using Abner.Extensions.Configuration.Db.MySql;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddMySql(builder.Configuration.GetConnectionString("MySql"));
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
```

#### 第三步

通过运行程序，会在指定的数据库中创建默认的配置表“Db_Configs”,有三个表字段，默认为Id,Name,Value, 其中Id为自增主键，也可通过以下代码进行配置表名跟表字段；

```C#
builder.Configuration.AddMySql(options =>
{
    options.DbSetting.ConnectionString = builder.Configuration.GetConnectionString("MySql");
    options.DbSetting.TableName = "YourTableName";
    options.DbSetting.KeyColumn = "YourKeyField";
    options.DbSetting.ValueColumn = "YourValueField";
});
```

也可自行在数据库中创建对应的表，配置时按照手动创建表时指定的名称即可；

可以支持以下形式的value:

![image-20220324150357879](https://github.com/dotneterwhj/Abner.Extensions.Configuration.Db/blob/main/public/image/image-20220324150357879.png)



#### 第四步

在Controller中添加以下代码：

```C#
private readonly ILogger<WeatherForecastController> _logger;
private readonly IOptions<DemoOption> _demoOption;
private readonly IConfiguration _configuration;

public WeatherForecastController(ILogger<WeatherForecastController> logger, 
                                 IOptions<DemoOption> demoOption,
                                 IConfiguration configuration)
{
    _logger = logger;
    this._demoOption = demoOption;
    this._configuration = configuration;
}

[HttpGet]
public IActionResult Get()
{
    return new JsonResult(new
                          {
                              json = _demoOption.Value,
                              array = _configuration["MySqlArray:0:myKey"],
                              text = _configuration["MysqlText"]
                          });
}
```

在浏览器中访问时返回

![image-20220324150429587](https://github.com/dotneterwhj/Abner.Extensions.Configuration.Db/blob/main/public/image/image-20220324150429587.png)
