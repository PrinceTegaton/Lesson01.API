using Lesson01.API.Database;
using Lesson01.API.DTO;
using Lesson01.API.Implementations;
using Lesson01.API.Interfaces;
using MicroServices.Shared;
using MicroServices.Shared.Adapters;
using MicroServices.Shared.DataAccess;
using MicroServices.Shared.Extensions;
using MicroServices.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

AppSettings settings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();


// mock user
builder.Services.AddScoped<CurrentUser>(a => new CurrentUser
{
    UserId = 1,
    Name = "System Admin",
    Role = MicroServices.UserRoleName.SystemAdministrator,
    EmailAddress = "admin@school.com",
    MobileNo = "09099999919"
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IExamServiceAdapter, ExamServiceAdapterMock>();
builder.Services.AddScoped<IEmailAdapter, EmailAdapter>();

AddDataStore();
AddRepositories();

SetupPaymentGateway();

builder.Services.AddSingleton<IDateTimeAdapter>(a => new DateTimeAdapter(1));
//builder.Services.AddSingleton <typeof(ILogAdapter<>), typeof(LogAdapter<>)) > ();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (settings.EnableAutoMigration)
{
    ConfigureAutoMigration();
}

if (settings.InjectSqlScriptsOnStartup)
{
    ConfigureSqlInjector();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void AddDataStore()
{
    builder.Services.AddScoped<DbContext, AppDbContext>();
    builder.Services.AddDbContextPool<AppDbContext>(options =>
    {
        string connStr = builder.Configuration.GetConnectionString("AppConnection");
        options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
    });
}

void AddRepositories()
{
    builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    // register all domain entities to a generic repository instance
    Assembly.GetExecutingAssembly().GetTypes().Where(a => a.Name == typeof(IGenericRepository<>).Name && a.BaseType == typeof(AuditableEntity)).ToList()
        .ForEach((t) =>
        {
            builder.Services.AddTransient(typeof(IGenericRepository<>), t);
        });
}

void SetupPaymentGateway()
{
    // setup payment gateway providers based on config
    var paymentGatewayConfig = builder.Configuration.GetSection("PaymentGatewayConfig").Get<PaymentGatewayConfig>();
    var activePaymentProvider = paymentGatewayConfig.Providers.FirstOrDefault(a => a.Name == paymentGatewayConfig.ActiveProvider);
    if (activePaymentProvider == null)
    {
        throw new ArgumentException("Active payment gateway provider configuration not found in config");
    }

    builder.Services.AddSingleton(activePaymentProvider);

    switch (paymentGatewayConfig.ActiveProvider)
    {
        case PaymentGatewayProvider.Stripe:
            builder.Services.AddScoped<IPaymentGatewayAdapter, StripePaymentAdapter>();
            break;
        case PaymentGatewayProvider.Paystack:
            builder.Services.AddScoped<IPaymentGatewayAdapter, PaystackPaymentAdapter>();
            break;
        default:
            throw new ArgumentException("Unable to create a payment gateway provider implementation");
    }
}

void ConfigureAutoMigration()
{
    try
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception)
    {
        // log error
    }
}

void ConfigureSqlInjector()
{
    try
    {
        // execute sql queries like views and storedprocedures
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        SqlMigrationUtil.InjectAllViews(context.Database);
    }
    catch (Exception)
    {
        // log error
    }
}