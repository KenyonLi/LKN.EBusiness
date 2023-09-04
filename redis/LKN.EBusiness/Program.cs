using LKN.EBusiness.Caches;
using LKN.EBusiness.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
string conneStr = builder.Configuration.GetSection("RedisConnStr:DefaultConnection").Value;
//string conneStr = builder.Configuration.GetSection("RedisConnStr:ClusterConnection").Value;

builder.Services.AddDistributedRedisCache(conneStr);

builder.Services.AddDbContext<ProductDbContext>(options => {
          options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion("8.0.32"));
      });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
