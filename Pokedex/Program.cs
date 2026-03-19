using Pokedex.Components;

using Microsoft.EntityFrameworkCore;
using Pokedex.Data;
using Pokedex.Services;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=pokedex.db";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(conn));

// após builder.Services.AddDbContext...
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IPokeApiClient, PokeApiClient>(c =>
{
    c.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
    c.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddScoped<ITeamService, TeamService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


// Auto-migração do banco (prático para demo)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


app.Run();
