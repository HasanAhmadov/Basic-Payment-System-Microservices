using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Exceptions;
using OrderService.Interfaces;
using OrderService.MessageBroker;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<KafkaConfiguration>(
    builder.Configuration.GetSection("Kafka"));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDataService, OrderDataService>();
builder.Services.AddScoped<IMessagingService, MessagingService>();
builder.Services.AddScoped<IMessagingDataService, MessagingDataService>();

builder.Services.AddSingleton<IKafkaPublisher, KafkaPublisher>();
builder.Services.AddHostedService<KafkaConsumer>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run migrations safely
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync(); // <--- Use MigrateAsync instead of EnsureCreatedAsync
        Console.WriteLine("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database migration failed: " + ex.Message);
        throw;
    }
}

app.Run("http://0.0.0.0:80");