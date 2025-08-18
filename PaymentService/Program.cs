using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Exceptions;
using PaymentService.Interfaces;
using PaymentService.MessageBroker;
using PaymentService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentDataService, PaymentDataService>();
builder.Services.AddScoped<IMessagingDataService, MessagingDataService>();
builder.Services.AddScoped<IMessagingService, MessagingService>();

builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddHostedService<KafkaConsumer>();

builder.Services.AddLogging();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run EF Core migrations safely
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        await context.Database.MigrateAsync(); // Creates DB if it doesn't exist and applies migrations
        Console.WriteLine("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database migration failed: " + ex.Message);
        throw; // Optional: stop app if DB can't migrate
    }
}

// Start the app
app.Run("http://0.0.0.0:80");