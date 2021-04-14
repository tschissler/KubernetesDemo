using Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PrimeDecomposition;

var app = WebApplication.Create(args);

app.MapGet("/",
async http =>
{
    var factors = long.Parse(http.Request.Query["number"]);
    http.Response.ContentType = "application/json";
    await http.Response.WriteAsJsonAsync(new ResultDto(Calculator.PrimeDecompositionCalc(factors)));
});

app.MapGet("/health",
    async http =>
    {
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsJsonAsync(new StatusDto("ok"));
    });

await app.RunAsync();