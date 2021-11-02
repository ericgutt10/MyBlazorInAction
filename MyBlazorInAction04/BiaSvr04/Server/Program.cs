var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(app =>
{
    app.MapBlazorHub();
    app.MapRazorPages();
    app.MapFallbackToPage("/Host");
});

app.Run();
