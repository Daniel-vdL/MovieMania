using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManiaUi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MovieManiaUi.Pages
{
    public sealed partial class SeriePage : Page
    {
        public SeriePage()
        {
            this.InitializeComponent();
            LoadSeries();
            LoadReleaseYears();
            LoadGenre();
        }

        public async void LoadSeries()
        {
            try
            {
                var apiHandler = new ApiHandler();
                var series = await apiHandler.GetSeriesAsync();

                SerieListView.ItemsSource = series;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading films: {ex.Message}");
            }
        }

        private void RefreshSeries_Click(object sender, RoutedEventArgs e)
        {
            LoadSeries();
        }

        private async void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = searchTextBox.Text.ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    this.LoadSeries();
                }
                else
                {
                    var apiHandler = new ApiHandler();
                    var series = await apiHandler.GetSeriesAsync();
                    var filteredSeries = series.Where(s => s.Title.ToLower().Contains(searchText)).ToList();

                    SerieListView.ItemsSource = filteredSeries;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while searching series: {ex.Message}");
            }
        }

        private async void LoadReleaseYears()
        {
            try
            {
                var apiHandler = new ApiHandler();
                var series = await apiHandler.GetSeriesAsync();
                var distinctYears = series.Select(s => s.ReleaseYear).Distinct().OrderByDescending(year => year);

                releaseYearComboBox.Items.Clear();

                releaseYearComboBox.Items.Add(new ComboBoxItem { Content = "All" });

                foreach (var year in distinctYears)
                {
                    releaseYearComboBox.Items.Add(new ComboBoxItem { Content = year.ToString() });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading releaseYears: {ex.Message}");
            }
        }

        private async void releaseYearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var apiHandler = new ApiHandler();
                var series = await apiHandler.GetSeriesAsync();
                string selectedYear = (releaseYearComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (selectedYear == "All")
                {
                    LoadSeries();
                }
                else if (int.TryParse(selectedYear, out int year))
                {
                    var filteredSeries = series.Where(s => s.ReleaseYear == year).ToList();
                    SerieListView.ItemsSource = filteredSeries;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while filtering series by releaseYear: {ex.Message}");
            }

        }

        private async void LoadGenre()
        {
            try
            {
                var apiHandler = new ApiHandler();
                var genres = await apiHandler.GetGenresAsync();

                genreComboBox.Items.Clear();

                genreComboBox.Items.Add(new ComboBoxItem { Content = "All" });

                foreach (var genre in genres)
                {
                    genreComboBox.Items.Add(new ComboBoxItem { Content = genre.Name });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading genres: {ex.Message}");
            }
        }

        private async void genreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var apiHandler = new ApiHandler();
                var series = await apiHandler.GetSeriesAsync();
                string selectedGenre = (genreComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (selectedGenre == "All")
                {
                    LoadSeries();
                }
                else
                {
                    var filteredSeries = series.Where(s => s.Genres.Contains(selectedGenre)).ToList();
                    SerieListView.ItemsSource = filteredSeries;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while filtering series by genre: {ex.Message}");
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashboardPage));
        }

        private void CreateSerie_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateSeriePage));
        }
    }
}
