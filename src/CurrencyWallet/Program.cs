using CurrenctWallet.Api.Controllers;
using CurrencyWallet.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCurrencyWalletsModule();
builder.Services.AddControllers().
    AddApplicationPart(typeof(WalletController).Assembly);
builder.Services.AddHttpClient();
//var config = builder.Configuration;
//builder.Services.AddDbContext<CurrencyWalletDbContext>(o =>
//{
//    o.UseNpgsql(config.GetConnectionString("MainDbConnection"));
//});

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

