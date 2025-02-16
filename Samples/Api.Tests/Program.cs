WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddSerilogLogging(builder.Services, builder.Configuration, "Api.Tests");
builder.Services.AddCorrelationEnricher().AddHttpContextEnricher();


builder.AddDistributedConfiguration();

//builder.Services.AddCorrelationEnricher();

/*
builder.Services.AddApiCallsLogger(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("LOGS")
        ?? throw new Exception("Не найдена строка подключения LOGS");
    options.TableName = "TEST";
    options.Interval = 5;
    options.BatchSize = 1000;
});
*/

//OTelDependencyInjection
//builder.Services.AddOTel("Test");
//builder.Logging.AddOTel();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseApiCallsLogger("TEST");

app.UseAuthorization();
app.MapControllers();

app.Run();
