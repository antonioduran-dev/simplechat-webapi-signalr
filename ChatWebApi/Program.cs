using ChatWebApi.Hubs;
using ChatWebApi.Services;
using DB.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// inject context
builder.Services.AddDbContext<ChatContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection"))
);

// add signalR
builder.Services.AddSignalR();

// add the services
builder.Services.AddScoped<IChatService, ChatService>();

// add CORS options, creatiing a new default cors Policy.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(app =>
    {
        app.WithOrigins("http://127.0.0.1:5500") // the url that access to server
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// indicate that use CORS
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// map the Hub created, and set the url
app.MapHub<ChatHub>("/chatHub");

app.Run();
