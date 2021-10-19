using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapFallbackToPage("/Host");

app.Run();
