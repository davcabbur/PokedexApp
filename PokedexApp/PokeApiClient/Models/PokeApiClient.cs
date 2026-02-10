
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace Pokedex.PokeApiClient.Models
{
    public class PokeApiClient
    {
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://pokeapi.co/api/v2/")
        };

        public async Task<PokemonDto> GetPokemonAsync(string nameOrId)
        {
            var response = await _http.GetAsync($"pokemon/{nameOrId.ToLower()}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PokemonDto>(json);
        }
    }
}
