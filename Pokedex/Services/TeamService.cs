using Microsoft.EntityFrameworkCore;
using Pokedex.Data;
using Pokedex.Models;

namespace Pokedex.Services;

public interface ITeamService
{
    Task<List<TeamMember>> GetAllAsync();
    Task<int> CountAsync();
    Task<(bool ok, string? error)> AddAsync(TeamMember m);
    Task<(bool ok, string? error)> UpdateAsync(TeamMember m);
    Task DeleteAsync(int id);
}

public class TeamService : ITeamService
{
    private readonly AppDbContext _db;
    public TeamService(AppDbContext db) => _db = db;

    public Task<List<TeamMember>> GetAllAsync() =>
        _db.Team.AsNoTracking().OrderBy(t => t.Id).ToListAsync();

    public Task<int> CountAsync() => _db.Team.CountAsync();

    public async Task<(bool ok, string? error)> AddAsync(TeamMember m)
    {
        if (await _db.Team.CountAsync() >= 6)
            return (false, "O time já tem 6 Pokémon.");


        if (m.PokemonLevel < AppConstants.MinLv || m.PokemonLevel > AppConstants.MaxLv)
        return (false, $"O nível deve estar entre {AppConstants.MinLv} e {AppConstants.MaxLv}.");

        m.Nickname = string.IsNullOrWhiteSpace(m.Nickname) ? null : m.Nickname.Trim();
        _db.Team.Add(m);
        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool ok, string? error)> UpdateAsync(TeamMember m)
    {
        var e = await _db.Team.FindAsync(m.Id);
        if (e is null) return (false, "Registro não encontrado.");
        
        if (m.PokemonLevel < AppConstants.MinLv || m.PokemonLevel > AppConstants.MaxLv)
        return (false, $"O nível deve estar entre {AppConstants.MinLv} e {AppConstants.MaxLv}.");

        e.Nickname = string.IsNullOrWhiteSpace(m.Nickname) ? null : m.Nickname.Trim();
        e.Gender = m.Gender;
        e.PokemonLevel = m.PokemonLevel;
        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _db.Team.FindAsync(id);
        if (e is null) return;
        _db.Team.Remove(e);
        await _db.SaveChangesAsync();
    }
}