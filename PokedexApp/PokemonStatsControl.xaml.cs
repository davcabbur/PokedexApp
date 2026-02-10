using Pokedex.PokeApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PokedexApp
{
    /// <summary>
    /// Lógica de interacción para PokemonStatsControl.xaml.
    /// Este control se encarga de mostrar de forma visual y numérica las estadísticas base de un Pokémon.
    /// Soporta la visualización de HP, Ataque, Defensa, Ataque Especial, Defensa Especial y Velocidad.
    /// </summary>
    public partial class PokemonStatsControl : UserControl
    {
        /// <summary>
        /// Valor máximo posible para una estadística base (usado para calcular porcentajes de barras).
        /// Basado en el máximo teórico (aproximado) de los juegos originales.
        /// </summary>
        private const int MAX_STAT_VALUE = 255; // Valor máximo para calcular porcentajes

        public PokemonStatsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Establece y visualiza las estadísticas del Pokémon en el control.
        /// Actualiza tanto los textos como las barras de progreso, y destaca la estadística más alta con una estrella.
        /// </summary>
        public void SetStats(List<PokemonStatDto> stats)
        {
            if (stats == null || !stats.Any()) return;

            
            // Encontrar el valor máximo de las estadísticas base
            int maxStat = stats.Max(stats => stats.BaseStat);

            int total = 0;

            // Para borrar estrellas previas (?)
            HpStar.Text = HpStar.Text = AttackStar.Text = DefenseStar.Text = SpAttackStar.Text = SpDefenseStar.Text = SpeedStar.Text = string.Empty;

            foreach (var stat in stats) {
                total += stat.BaseStat;
                bool isMax = stat.BaseStat == maxStat;

                switch (stat.Stat.Name.ToLower())
                {
                    case "hp":
                        HpValue.Text = stat.BaseStat.ToString();
                        SetBarWidth(HpBar, stat.BaseStat);
                        if (isMax) HpStar.Text = "\u2605";
                        break;

                    case "attack":
                        AttackValue.Text = stat.BaseStat.ToString();
                        SetBarWidth(AttackBar, stat.BaseStat);
                        if (isMax) AttackStar.Text = "\u2605";
                        break;

                    case "defense":
                        DefenseValue.Text = stat.BaseStat.ToString();
                        SetBarWidth(DefenseBar, stat.BaseStat);
                        if (isMax) DefenseStar.Text = "\u2605";
                        break;

                    case "special-attack":
                        SpAttackValue.Text = stat.BaseStat.ToString();
                        SetBarWidth(SpAttackBar, stat.BaseStat);
                        if (isMax) SpAttackStar.Text = "\u2605";
                        break;

                    case "special-defense":
                        SpDefenseValue.Text = stat.BaseStat.ToString();
                        SetBarWidth(SpDefenseBar, stat.BaseStat);
                        if (isMax) SpDefenseStar.Text = "\u2605";
                        break;

                    case "speed":
                        SpeedValue.Text = stat.BaseStat.ToString();
                        SetBarWidth(SpeedBar, stat.BaseStat);
                        if (isMax) SpeedStar.Text = "\u2605";
                        break;
                }

            }
            
            TotalValue.Text = total.ToString();
        }
        /// <summary>
        /// Calcula y establece el ancho de la barra de estadística.
        /// Utiliza un Binding con convertidor para ajustar el ancho relativo al contenedor.
        /// </summary>
        private void SetBarWidth(Border bar, int statValue)
        {
            // Calculamos el porcentaje del valor respecto al máximo
            double percentage = (double)statValue / MAX_STAT_VALUE;

            // Aseguramos que el porcentaje esté entre 0 y 1
            percentage = Math.Min(Math.Max(percentage, 0), 1);

            // Establecemos el ancho como porcentaje del ancho disponible
            bar.Width = double.NaN; // Reset
            bar.SetBinding(WidthProperty, new Binding
            {
                Path = new PropertyPath("ActualWidth"),
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Border), 1),
                Converter = new StatBarWidthConverter(),
                ConverterParameter = percentage
            });
        }
    }

    /// <summary>
    /// Convertidor de valor que calcula el ancho final de la barra en píxeles.
    /// Multiplica el ancho del contenedor por el porcentaje de la estadística.
    /// </summary>
    public class StatBarWidthConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double containerWidth && parameter is double percentage)
            {
                return containerWidth * percentage;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}