using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PrimeDecomposition;

var app = WebApplication.Create(args);

app.MapGet("/",
async http =>
{
    var factors = long.Parse(http.Request.Query["number"]);
    http.Response.ContentType = "application/json";
    await http.Response.WriteAsync($@"{{""result"":[{string.Join(",", Calculator.PrimeDecompositionCalc(factors))}]}}");
});

app.MapGet("/health",
    async http =>
    {
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsync(@"{""status"":""ok""}");
    });

await app.RunAsync();