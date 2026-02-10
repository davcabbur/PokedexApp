using Pokedex.PokeApiClient.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PokedexApp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml.
    /// Esta es la ventana principal que gestiona la carga de la Pokédex,
    /// la búsqueda de Pokémon y la visualización de detalles estadísticos.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadPokemon();
        }

        /// <summary>
        /// Carga asíncronamente la lista inicial de 1025 Pokémon.
        /// Realiza peticiones a la PokeAPI y añade las tarjetas al panel.
        /// </summary>
        private async void LoadPokemon()
        {
            var apiClient = new PokeApiClient();
            for (int i = 1; i <= 1025; i++)
            {
                try
                {
                    var pokemon = await apiClient.GetPokemonAsync(i.ToString());
                    var card = new PokemonCard();
                    card.SetPokemon(pokemon);

                    card.PokemonSelected += PokemonCard_PokemonSelected;

                    PokemonCardsPanel.Children.Add(card);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error cargando Pokémon #{i}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Maneja el evento de selección de un Pokémon en una tarjeta.
        /// Inicia la carga de los detalles completos del Pokémon seleccionado.
        /// </summary>
        private void PokemonCard_PokemonSelected(object sender, PokemonDto selectedPokemon)
        {
            if (selectedPokemon != null)
            {
                // Inicia la carga asíncrona del detalle
                ShowPokemonDetails(selectedPokemon);
            }
        }

        /// <summary>
        /// Muestra los detalles completos, incluyendo estadísticas, del Pokémon seleccionado.
        /// Realiza una segunda llamada a la API para asegurar que se tienen todos los datos necesarios.
        /// </summary>
        private async void ShowPokemonDetails(PokemonDto pokemon)
        {
            // Ocultamos el panel mientras cargamos (para evitar mostrar datos viejos o incompletos)
            StatsDetailControl.Visibility = Visibility.Collapsed;

            // Instancia del cliente API para la segunda llamada
            var apiClient = new PokeApiClient();
            PokemonDto fullPokemon;

            try
            {
                // 1. LLAMADA DE DETALLE: Usamos el ID del objeto resumido para obtener el detalle completo
                fullPokemon = await apiClient.GetPokemonAsync(pokemon.Id.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle de {pokemon.Name}: {ex.Message}");
                return;
            }

            // 2. Usar los datos completos (que ahora sí tienen 'Stats')
            var statsList = fullPokemon.Stats;

            if (statsList != null && statsList.Any())
            {
                // Pasamos la lista de estadísticas al control de detalle
                StatsDetailControl.SetStats(statsList);

                // Hacemos el control visible y listo
                StatsDetailControl.Visibility = Visibility.Visible;
            }
            else
            {
                // Esto solo ocurriría si la API no devuelve stats incluso en la llamada de detalle
                MessageBox.Show($"Advertencia: Los detalles de estadísticas estaban vacíos para {pokemon.Name}. No se pudo mostrar el panel.");
            }
        }

        /// <summary>
        /// Maneja el evento Click del botón de búsqueda.
        /// Filtra las tarjetas de Pokémon mostradas basándose en el nombre o número introducido.
        /// </summary>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            foreach (var child in PokemonCardsPanel.Children)
            {
                if (child is PokemonCard card)
                {
                    bool isVisible = true;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        var data = card.PokemonData;
                        if (data != null)
                        {
                            bool nameMatch = data.Name.ToLower().Contains(searchText);
                            bool idMatch = data.Id.ToString() == searchText || 
                                           (int.TryParse(searchText, out int searchId) && searchId == data.Id);
                            
                            isVisible = nameMatch || idMatch;
                        }
                    }
                    card.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }
}