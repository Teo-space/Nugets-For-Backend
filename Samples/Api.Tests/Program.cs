using Api.Calls.LogTo.SqlServer.ColumnStore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddSerilogLogging(builder.Services, builder.Configuration);
builder.Services.AddCorrelationEnricher();

builder.Services.AddApiCallsLogger(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("LOGS")
        ?? throw new Exception("Не найдена строка подключения LOGS");
    options.TableName = "TEST";
    options.Interval = 5;
    options.BatchSize = 1000;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseApiCallsLogger("TEST");

app.UseAuthorization();
app.MapControllers();

app.Run();
