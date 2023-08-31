using LKN.EBusiness.Models;
using LKN.EBusiness.MongoDBs;
using LKN.EBusiness.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 1、注册商品服务到数据库
builder.Services.AddSingleton<IProductService, ProductService>();
// 2、配置商品MongoDB选项
builder.Services.Configure<ProductMongoDBOptions>(builder.Configuration.GetSection(nameof(ProductMongoDBOptions)));
//builder.Services.AddMongoDB(builder.Configuration);

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
