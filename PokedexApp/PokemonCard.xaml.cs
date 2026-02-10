using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Pokedex.PokeApiClient.Models;

namespace PokedexApp
{
    /// <summary>
    /// Lógica de interacción para PokemonCard.xaml.
    /// Representa una tarjeta individual de Pokémon con información resumida.
    /// </summary>
    public partial class PokemonCard : UserControl
    {
        /// <summary>
        /// Evento que se dispara cuando el usuario selecciona esta tarjeta.
        /// </summary>
        public event EventHandler<PokemonDto> PokemonSelected;

        /// <summary>
        /// Almacena el Pokémon actual mostrado en esta tarjeta.
        /// </summary>
        private PokemonDto _currentPokemon;

        /// <summary>
        /// Obtiene los datos del Pokémon asociado a esta tarjeta.
        /// Útil para operaciones de filtrado o búsqueda.
        /// </summary>
        public PokemonDto PokemonData => _currentPokemon;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PokemonCard"/>.
        /// </summary>
        public PokemonCard()
        {
            InitializeComponent();
            // Asociar el evento de clic de ratón a la tarjeta
            this.MouseLeftButtonUp += PokemonCard_MouseLeftButtonUp;
        }

        /// <summary>
        /// Establece los datos del Pokémon en la tarjeta y actualiza la interfaz gráfica.
        /// </summary>
        /// <param name="pokemon">El DTO con la información del Pokémon.</param>
        public void SetPokemon(PokemonDto pokemon)
        {
            if (pokemon == null) return;

            _currentPokemon = pokemon; // Almacenar para el clic

            // Establecer numero
            NumeroPokemon.Text = $"#{pokemon.Id:D3}";

            //Establecer nombre
            NombrePokemon.Text = CapitalizeName(pokemon.Name);

            //Establecer sprite
            if (pokemon.Sprites?.FrontDefault != null)
            {
                try
                {
                    PokemonSprite.Source = new BitmapImage(new Uri(pokemon.Sprites.FrontDefault));
                }
                catch
                {
                    // Si falla, dejar vacio
                }
            }

            if (pokemon.Types != null && pokemon.Types.Any())
            {
                var typesList = pokemon.Types
                    .OrderBy(t => t.Slot)
                    .Select(t => new TypeBadge
                    {
                        Name = CapitalizeName(t.Type.Name),
                        Color = GetTypeColor(t.Type.Name)
                    });

                // Se asume que TypesContainer existe en XAML
                TypesContainer.ItemsSource = typesList;

                // Establecer color de fondo segun el tipo
                var primaryType = pokemon.Types.OrderBy(t => t.Slot).First().Type.Name;
                HeaderBorder.Background = GetTypeColor(primaryType);
            }
        }

        /// <summary>
        /// Maneja el evento de clic izquierdo en la tarjeta.
        /// Dispara el evento <see cref="PokemonSelected"/> con los datos del Pokémon actual.
        /// </summary>
        private void PokemonCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Disparar el evento, enviando el Pokémon seleccionado
            PokemonSelected?.Invoke(this, _currentPokemon);
        }

        /// <summary>
        /// Capitaliza la primera letra de una cadena de texto.
        /// </summary>
        private string CapitalizeName(String name)
        {
            if (String.IsNullOrEmpty(name)) return name;
            return char.ToUpper(name[0]) + name.Substring(1).ToLower();
        }

        /// <summary>
        /// Obtiene el color asociado a un tipo de Pokémon.
        /// </summary>
        private SolidColorBrush GetTypeColor(string type)
        {
            Color color;
            switch (type.ToLower())
            {
                case "normal": color = Color.FromRgb(168, 167, 122); break;
                case "fire": color = Color.FromRgb(238, 129, 48); break;
                case "water": color = Color.FromRgb(99, 144, 240); break;
                case "electric": color = Color.FromRgb(247, 208, 44); break;
                case "grass": color = Color.FromRgb(122, 199, 76); break;
                case "ice": color = Color.FromRgb(150, 217, 214); break;
                case "fighting": color = Color.FromRgb(194, 46, 40); break;
                case "poison": color = Color.FromRgb(163, 62, 161); break;
                case "ground": color = Color.FromRgb(226, 191, 101); break;
                case "flying": color = Color.FromRgb(169, 143, 243); break;
                case "psychic": color = Color.FromRgb(249, 85, 135); break;
                case "bug": color = Color.FromRgb(166, 185, 26); break;
                case "rock": color = Color.FromRgb(182, 161, 54); break;
                case "ghost": color = Color.FromRgb(115, 87, 151); break;
                case "dragon": color = Color.FromRgb(111, 53, 252); break;
                case "dark": color = Color.FromRgb(112, 87, 70); break;
                case "steel": color = Color.FromRgb(183, 183, 206); break;
                case "fairy": color = Color.FromRgb(214, 133, 173); break;
                default: color = Color.FromRgb(104, 144, 240); break;
            }
            return new SolidColorBrush(color);
        }

        /// <summary>
        /// Clase interna para representar una insignia de tipo con su nombre y color.
        /// </summary>
        private class TypeBadge
        {
            /// <summary>
            /// Obtiene o establece el nombre del tipo.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Obtiene o establece el color asociado al tipo.
            /// </summary>
            public SolidColorBrush Color { get; set; }
        }
    }
}