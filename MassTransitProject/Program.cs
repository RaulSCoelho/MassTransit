using MassTransit;
using Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

#region MassTransit

services.AddSingleton(KebabCaseEndpointNameFormatter.Instance);
services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq();

    cfg.AddRequestClient<SubmitOrder>();
});

services.AddOptions<MassTransitHostOptions>().Configure(options =>
{
    options.WaitUntilStarted = true;

    options.StartTimeout = TimeSpan.FromSeconds(10);

    options.StopTimeout = TimeSpan.FromSeconds(30);
});

#endregion

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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
