using System.Text.Json.Serialization;

namespace Pokedex.PokeApiClient.Models
{
    public class PokemonSpritesDto
    {
        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; }
    }
}
