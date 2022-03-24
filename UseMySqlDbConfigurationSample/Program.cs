using Abner.Extensions.Configuration.Db.MySql;
using Abner.Extensions.Configuration.Db.Sqlite;
using Abner.Extensions.Configuration.Db.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// mysql dbconfiguraton source
builder.Configuration.AddMySql(builder.Configuration.GetConnectionString("MySql"));
builder.Services.Configure<DemoOption>(builder.Configuration.GetSection("MySqlJson"));

// sqlserver dbconfiguraton source
//builder.Configuration.AddSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
//builder.Services.Configure<DemoOption>(builder.Configuration.GetSection("SqlServer"));

// sqlite dbconfiguraton source
//builder.Configuration.AddSqlite(builder.Configuration.GetConnectionString("Sqlite"));
//builder.Services.Configure<DemoOption>(builder.Configuration.GetSection("Sqlite"));


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
