using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex.PokeApiClient.Models
{
    public class PokemonDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sprites")]
        public PokemonSpritesDto Sprites { get; set; }

        [JsonPropertyName("types")]
        public List<PokemonTypeSlotDto> Types { get; set; }

        [JsonPropertyName("stats")]
        public List<PokemonStatDto> Stats { get; set; }
    }
}
