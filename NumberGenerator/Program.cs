﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NumberGenerator;

var app = WebApplication.Create(args);

var generator = new Generator();
var caller = new CallPrimeDecomposition(Environment.GetEnvironmentVariable("PRIME_DECOMPOSITION_URL"));

app.MapGet("/start",
    async http =>
    {
        var max = long.Parse(http.Request.Query["max"]);
        var intervalInMilliseconds = long.Parse(http.Request.Query["interval"]);
        var generatedNumbers = generator.Generate(max, intervalInMilliseconds);
#pragma warning disable 4014
        generatedNumbers.Subscribe(x => caller.Call(x));
#pragma warning restore 4014
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsync(@"{""status"":""running""}");
    });

app.MapGet("/stop",
    async http =>
    {
        generator.Stop();
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsync(@"{""status"":""stopped""}");
    });

app.MapGet("/stats",
    async http =>
    {
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsJsonAsync(caller.CallStatistics);
    });

app.MapGet("/health",
    async http =>
    {
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsync(@"{""status"":""ok""}");
    });

await app.RunAsync();