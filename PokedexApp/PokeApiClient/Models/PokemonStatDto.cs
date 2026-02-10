using System.Text.Json.Serialization;

namespace Pokedex.PokeApiClient.Models
{
    public class PokemonStatDto
    {
        [JsonPropertyName("base_stat")]
        public int BaseStat { get; set; }

        [JsonPropertyName("stat")]
        public StatDto Stat { get; set; }
    }

    public class StatDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
