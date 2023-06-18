using Lesson01.API.DTO;
using Lesson01.API.Implementations;
using Lesson01.API.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IExamServiceAdapter, ExamServiceAdapterMock>();
builder.Services.AddScoped<IEmailAdapter, EmailAdapter>();

SetupPaymentGateway();


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