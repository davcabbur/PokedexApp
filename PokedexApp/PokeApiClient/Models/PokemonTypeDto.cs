using System.Text.Json.Serialization;

namespace Pokedex.PokeApiClient.Models
{
    public class PokemonTypeSlotDto
    {
        [JsonPropertyName("slot")]
        public int Slot { get; set; }

        [JsonPropertyName("type")]
        public PokemonTypeDto Type { get; set; }
    }

    public class PokemonTypeDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
