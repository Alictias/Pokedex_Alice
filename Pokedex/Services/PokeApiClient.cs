using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Pokedex.Services;

public interface IPokeApiClient
{
    Task<PokemonBasic?> GetBasicAsync(int id, CancellationToken ct = default);
}

public class PokeApiClient : IPokeApiClient
{
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;

    public PokeApiClient(HttpClient http, IMemoryCache cache)
    {
        _http = http;
        _cache = cache;
    }

    public async Task<PokemonBasic?> GetBasicAsync(int id, CancellationToken ct = default)
    {
        if (id < AppConstants.MinId || id > AppConstants.MaxId) return null;

        if (_cache.TryGetValue($"poke-basic-{id}", out PokemonBasic? cached))
            return cached;

        // PokeAPI v2 - GET /pokemon/{id} (somente leitura)
        var raw = await _http.GetFromJsonAsync<PokeApiPokemon>($"pokemon/{id}", ct);
        if (raw is null) return null;

        var basic = new PokemonBasic
        {
            Id = raw.id,
            Name = ToTitle(raw.name),
            Types = raw.types.OrderBy(t => t.slot).Select(t => ToTitle(t.type.name)).ToList(),
            Height = raw.height,
            Weight = raw.weight
        };

        _cache.Set($"poke-basic-{id}", basic, TimeSpan.FromHours(12));
        return basic;
    }

    private static string ToTitle(string s) =>
        string.IsNullOrWhiteSpace(s) ? s : char.ToUpper(s[0]) + s[1..];

    // DTO mínimo do JSON de /pokemon/{id}
    private class PokeApiPokemon
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public int height { get; set; }
        public int weight { get; set; }
        public List<TypeSlot> types { get; set; } = new();
        public class TypeSlot
        {
            public int slot { get; set; }
            public Named type { get; set; } = new();
        }
        public class Named { public string name { get; set; } = ""; public string url { get; set; } = ""; }
    }
}