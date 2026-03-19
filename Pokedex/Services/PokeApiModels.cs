namespace Pokedex.Services;

public class PokemonBasic
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<string> Types { get; set; } = new();
    public int Height { get; set; }
    public int Weight { get; set; }

    // Sprite oficial (alta qualidade)
    public string ImageUrl =>
        $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{Id}.png";
}