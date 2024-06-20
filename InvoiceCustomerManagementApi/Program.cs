using DataAccessLayer.Model;
using DataAccessLayer.Respository;
using DataAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetSection("ConnectionString").Value.ToString();
string databaseName = builder.Configuration.GetSection("DatabaseName").Value.ToString();
builder.Services.AddControllers();

builder.Services.AddSingleton<ICustomerInterface>(serviceProvider =>
{
    return new CustomerService(connectionString, databaseName);
});

builder.Services.AddSingleton<IInvoiceInterface>(serviceProvider =>
{
    return new InvoiceService(connectionString, databaseName);
});

builder.Services.AddSingleton<IMongoCollection<Invoice>>(provider =>
{
    var mongoClient = new MongoClient(connectionString);
    var database = mongoClient.GetDatabase(databaseName);
    return database.GetCollection<Invoice>("Invoice");
});
builder.Services.AddSingleton<IMongoCollection<Customer>>(provider =>
{
    var mongoClient = new MongoClient(connectionString);
    var database = mongoClient.GetDatabase(databaseName);
    return database.GetCollection<Customer>("Customer");
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseCors(
    options => options.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
