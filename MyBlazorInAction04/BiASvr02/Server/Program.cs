var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(config =>
{
    config.MapBlazorHub();
    config.MapFallbackToPage($"/{nameof(Host)}");
});


app.Run();
