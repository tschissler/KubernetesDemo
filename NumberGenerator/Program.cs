using System;
using System.Threading.Tasks;
using Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NumberGenerator;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Host.UseSerilog((context, configuration)
    => configuration
        .Enrich
        .FromLogContext()
        .WriteTo
        .Console()
    );

var app = builder.Build();
app.MapHub<NumberGeneratorHub>("/signalr");
var hubContext = app.Services.GetRequiredService<IHubContext<NumberGeneratorHub>>();

var generator = new Generator();
var statistics = new Statistics();
var timer = new System.Timers.Timer
{
    Interval = 10000,
    AutoReset = true,
};

timer.Elapsed += (_, _) =>
{
    Log.Information("Updating UI now with {Count}", statistics.Calls);
    hubContext.Clients.All.SendAsync("UpdateStatistics", statistics.Calls);
    statistics.Reset();
};

app.MapGet("/start",
    async http =>
    {
        var max = long.Parse(http.Request.Query["max"]);
        var intervalInMilliseconds = long.Parse(http.Request.Query["interval"]);
        var generatedNumbers = generator.Generate(max, intervalInMilliseconds);
#pragma warning disable 4014
        var caller = new CallPrimeDecomposition(Environment.GetEnvironmentVariable("PRIME_DECOMPOSITION_URL"));
        generatedNumbers.Subscribe(async x =>
        {
            if (await caller.Call(x))
            {
                statistics.Increment();
            }
        });
        timer.Start();
#pragma warning restore 4014
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsJsonAsync(new StatusDto("ok"));
    });

app.MapGet("/stop",
    async http =>
    {
        timer.Stop();
        generator.Stop();
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsJsonAsync(new StatusDto("stopped"));
    });

app.MapGet("/updateinterval",
    async http =>
    {
        generator.IntervalInMilliseconds = long.Parse(http.Request.Query["interval"]);
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsJsonAsync(new StatusDto("ok"));
    });

app.MapGet("/health",
    async http =>
    {
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsJsonAsync(new StatusDto("ok"));
    });

await app.RunAsync();

class NumberGeneratorHub : Hub
{
    public override Task OnConnectedAsync()
    {
        Log.Information("{Id} connected", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception e)
    {
        Log.Information("Disconnected {Message} {ConnectionId}", e?.Message, Context.ConnectionId);
        await base.OnDisconnectedAsync(e);
    }
}
