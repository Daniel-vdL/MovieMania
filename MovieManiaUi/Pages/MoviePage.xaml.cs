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
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MovieManiaUi.Pages
{
    public sealed partial class MoviePage : Page
    {
        public MoviePage()
        {
            this.InitializeComponent();
            LoadFilms();
            LoadReleaseYears();
            LoadGenre();
        }

        public async void LoadFilms()
        {
            try
            {
                var apiHandler = new ApiHandler();
                var films = await apiHandler.GetFilmsAsync();

                FilmListView.ItemsSource = films;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading films: {ex.Message}");
            }
        }

        private void RefreshFilms_Click(object sender, RoutedEventArgs e)
        {
            LoadFilms();
        }

        private async void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = searchTextBox.Text.ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    this.LoadFilms();
                }
                else
                {
                    var apiHandler = new ApiHandler();
                    var films = await apiHandler.GetFilmsAsync();
                    var filteredFilms = films.Where(f => f.Title.ToLower().Contains(searchText)).ToList();

                    FilmListView.ItemsSource = filteredFilms;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while searching films: {ex.Message}");
            }
        }

        private async void LoadReleaseYears()
        {
            var apiHandler = new ApiHandler();
            var films = await apiHandler.GetFilmsAsync();
            var distinctYears = films.Select(f => f.ReleaseYear).Distinct().OrderByDescending(year => year);

            releaseYearComboBox.Items.Clear();

            releaseYearComboBox.Items.Add(new ComboBoxItem { Content = "All" });

            foreach (var year in distinctYears)
            {
                releaseYearComboBox.Items.Add(new ComboBoxItem { Content = year.ToString() });
            }
        }

        private async void releaseYearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var apiHandler = new ApiHandler();
            var films = await apiHandler.GetFilmsAsync();
            string selectedYear = (releaseYearComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedYear == "All")
            {
                LoadFilms();
            }
            else if (int.TryParse(selectedYear, out int year))
            {
                var filteredFilms = films.Where(f => f.ReleaseYear == year).ToList();
                FilmListView.ItemsSource = filteredFilms;
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
                var films = await apiHandler.GetFilmsAsync();
                string selectedGenre = (genreComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (selectedGenre == "All")
                {
                    LoadFilms();
                }
                else
                {
                    var filteredFilms = films.Where(f => f.Genres.Contains(selectedGenre)).ToList();
                    FilmListView.ItemsSource = filteredFilms;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while filtering films by genre: {ex.Message}");
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashboardPage));
        }

        private void CreateFilm_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateFilmPage));
        }
    }
}
