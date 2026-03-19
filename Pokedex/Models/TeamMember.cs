using System.ComponentModel.DataAnnotations;

namespace Pokedex.Models;

public class TeamMember
{
    public int Id { get; set; } // PK
    
    [Range(AppConstants.MinId, AppConstants.MaxId, ErrorMessage = "O PokémonId deve estar entre 1 e 493.")]
    public int PokemonId { get; set; }

    [StringLength(50)]
    public string? Nickname { get; set; }

    
    [Range(AppConstants.minLv, AppConstants.maxLv, ErrorMessage = "O Nível deve estar entre 1 e 100")]
    public int PokemonLevel { get; set; }

    [RegularExpression("Masculino|Feminino|Desconhecido", ErrorMessage = "Gênero inválido.")]
    public string Gender { get; set; } = "Desconhecido";

}