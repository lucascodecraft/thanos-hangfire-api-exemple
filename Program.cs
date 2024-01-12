using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration.
builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuration of the monitoring dashboard.
app.UseHangfireDashboard();
app.UseHangfireServer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
