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
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MovieManiaUi.Pages
{
    public sealed partial class FilmDetailPage : Page
    {
        private Models.Film selectedFilm;

        public FilmDetailPage()
        {
            this.InitializeComponent();
            LoadGenre(); 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Models.Film film)
            {
                selectedFilm = film;
            }

            LoadFilmInfo();
        }

        private void LoadFilmInfo()
        {
            TitleTextBlock.Text = selectedFilm.Title;
            GenreTextBlock.Text = "Genres: " + selectedFilm.GenresAsString;
            PlatformTextBlock.Text = "Platform: " + selectedFilm.Platform;
            ReleaseYearTextBlock.Text = "Release Year: " + selectedFilm.ReleaseYear;
            RatingTextBlock.Text = "Rating: " + selectedFilm.Rating;
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

        private async void EditFilmButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenreName = (GenreComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedFilm == null || selectedFilm.Id <= 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(FilmTitle.Text) &&
                string.IsNullOrWhiteSpace(FilmPlatform.Text) &&
                string.IsNullOrWhiteSpace(FilmReleaseYear.Text) &&
                string.IsNullOrWhiteSpace(FilmRating.Text) &&
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

            if (!string.IsNullOrWhiteSpace(FilmReleaseYear.Text) &&
                (!int.TryParse(FilmReleaseYear.Text, out int releaseYear) || releaseYear < 1800 || releaseYear > DateTime.Now.Year))
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

            if (!string.IsNullOrWhiteSpace(FilmRating.Text) &&
                (!int.TryParse(FilmRating.Text, out int rating) || rating < 1 || rating > 5))
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

            var updatedFilm = new Models.Film
            {
                Id = selectedFilm.Id,
                Title = string.IsNullOrWhiteSpace(FilmTitle.Text) ? selectedFilm.Title : FilmTitle.Text,
                Platform = string.IsNullOrWhiteSpace(FilmPlatform.Text) ? selectedFilm.Platform : FilmPlatform.Text,
                ReleaseYear = int.TryParse(FilmReleaseYear.Text, out releaseYear) ? releaseYear : selectedFilm.ReleaseYear,
                Rating = int.TryParse(FilmRating.Text, out rating) ? rating : selectedFilm.Rating,
                Genres = string.IsNullOrWhiteSpace(selectedGenreName) ? selectedFilm.Genres : new List<string> { selectedGenreName }
            };

            var filmJson = JsonSerializer.Serialize(updatedFilm);

            using (var client = new HttpClient())
            {
                var apiUrl = $"https://localhost:7193/api/Films/{selectedFilm.Id}";
                var content = new StringContent(filmJson, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    selectedFilm = updatedFilm;
                    LoadFilmInfo();
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
            if (selectedFilm == null || selectedFilm.Id <= 0)
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Invalid Film Id",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
                return;
            }

            using (var client = new HttpClient())
            {
                var apiUrl = $"https://localhost:7193/api/Films/{selectedFilm.Id}";
                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    this.Frame.Navigate(typeof(FilmPage));
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
            this.Frame.Navigate(typeof(FilmPage));
        }
    }
}
