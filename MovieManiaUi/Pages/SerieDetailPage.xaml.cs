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
using System.Text;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MovieManiaUi.Pages
{
    public sealed partial class SerieDetailPage : Page
    {
        private Models.Serie selectedSerie;

        public SerieDetailPage()
        {
            this.InitializeComponent();
            LoadGenre(); 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Models.Serie serie)
            {
                selectedSerie = serie;
            }

            LoadSerieInfo();
        }

        private void LoadSerieInfo()
        {
            TitleTextBlock.Text = selectedSerie.Title;
            GenreTextBlock.Text = "Genres: " + selectedSerie.GenresAsString;
            PlatformTextBlock.Text = "Platform: " + selectedSerie.Platform;
            ReleaseYearTextBlock.Text = "Release Year: " + selectedSerie.ReleaseYear;
            RatingTextBlock.Text = "Rating: " + selectedSerie.Rating;
        }

        private async void LoadGenre()
        {
            try
            {
                var apiHandler = new ApiHandler();
                var genres = await apiHandler.GetGenresAsync();

                GenreComboBox.Items.Clear();

                foreach (var genre in genres)
                {
                    GenreComboBox.Items.Add(new ComboBoxItem { Content = genre.Name });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading genres: {ex.Message}");
            }
        }

        private async void EditSerieButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenreName = (GenreComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedSerie == null || selectedSerie.Id <= 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(SerieTitle.Text) &&
                string.IsNullOrWhiteSpace(SeriePlatform.Text) &&
                string.IsNullOrWhiteSpace(SerieReleaseYear.Text) &&
                string.IsNullOrWhiteSpace(SerieRating.Text) &&
                selectedGenreName == null)
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Nothing to update!",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
                return;
            }

            if (!string.IsNullOrWhiteSpace(SerieReleaseYear.Text) &&
                (!int.TryParse(SerieReleaseYear.Text, out int releaseYear) || releaseYear < 1800 || releaseYear > DateTime.Now.Year))
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Invalid release year. Release year should be between 1800 and the current year.",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
                return;
            }

            if (!string.IsNullOrWhiteSpace(SerieRating.Text) &&
                (!int.TryParse(SerieRating.Text, out int rating) || rating < 1 || rating > 5))
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Invalid rating value. Rating should be between 1 and 5.",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
                return;
            }

            var updatedSerie = new Models.Serie
            {
                Id = selectedSerie.Id,
                Title = string.IsNullOrWhiteSpace(SerieTitle.Text) ? selectedSerie.Title : SerieTitle.Text,
                Platform = string.IsNullOrWhiteSpace(SeriePlatform.Text) ? selectedSerie.Platform : SeriePlatform.Text,
                ReleaseYear = int.TryParse(SerieReleaseYear.Text, out releaseYear) ? releaseYear : selectedSerie.ReleaseYear,
                Rating = int.TryParse(SerieRating.Text, out rating) ? rating : selectedSerie.Rating,
                Genres = string.IsNullOrWhiteSpace(selectedGenreName) ? selectedSerie.Genres : new List<string> { selectedGenreName }
            };

            var serieJson = JsonSerializer.Serialize(updatedSerie);

            using (var client = new HttpClient())
            {
                var apiUrl = $"https://localhost:7193/api/Series/{selectedSerie.Id}";
                var content = new StringContent(serieJson, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    selectedSerie = updatedSerie;
                    LoadSerieInfo();
                }
                else
                {
                    ContentDialog ErrorDialog = new ContentDialog
                    {
                        Title = "Editing failed!",
                        Content = "Click 'Ok' to continue",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot,
                    };

                    ContentDialogResult result = await ErrorDialog.ShowAsync();
                    return;
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSerie == null || selectedSerie.Id <= 0)
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Invalid Serie Id",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
                return;
            }

            using (var client = new HttpClient())
            {
                var apiUrl = $"https://localhost:7193/api/Series/{selectedSerie.Id}";
                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    this.Frame.Navigate(typeof(SeriePage));
                }
                else
                {
                    ContentDialog ErrorDialog = new ContentDialog
                    {
                        Title = "Deletion Failed!",
                        Content = "Click 'Ok' to continue",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot,
                    };

                    ContentDialogResult result = await ErrorDialog.ShowAsync();
                    return;
                }
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SeriePage));
        }
    }
}
