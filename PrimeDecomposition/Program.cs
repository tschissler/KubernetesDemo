using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PrimeDecomposition;

var app = WebApplication.Create(args);

app.MapGet("/",
async http =>
{
    var factors = long.Parse(http.Request.Query["number"]);
    await http.Response.WriteAsync(string.Join(" * ", Calculator.PrimeDecompositionCalc(factors)));
});

await app.RunAsync();