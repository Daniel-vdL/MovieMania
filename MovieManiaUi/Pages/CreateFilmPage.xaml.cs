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
using Windows.System;

namespace MovieManiaUi.Pages
{
    public sealed partial class CreateFilmPage : Page
    {
        private Genre selectedGenreName;

        public CreateFilmPage()
        {
            this.InitializeComponent();
            LoadGenre();
        }

        private async void CreateFilmButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextbox.Text) ||
                GenreComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(PlatformTextbox.Text) ||
                string.IsNullOrWhiteSpace(ReleaseYearTextbox.Text) ||
                string.IsNullOrWhiteSpace(RatingTextbox.Text))
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Please fill in all fields.",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
                return;
            }

            if (!int.TryParse(ReleaseYearTextbox.Text, out _) || !int.TryParse(RatingTextbox.Text, out _))
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Invalid input format. Release year and rating should be numeric values.",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
                return;
            }

            int releaseYear = int.Parse(ReleaseYearTextbox.Text);
            int rating = int.Parse(RatingTextbox.Text);

            if (releaseYear < 1800 || releaseYear > DateTime.Now.Year)
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

            if (rating < 1 || rating > 5)
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

            var title = TitleTextbox.Text;
            var selectedGenreName = (GenreComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            var platform = PlatformTextbox.Text;

            var film = new Film
            {
                Title = title,
                Platform = platform,
                ReleaseYear = releaseYear,
                Rating = rating,
                Genres = new List<string> { selectedGenreName }
            };

            using var client = new HttpClient();

            var filmJson = JsonSerializer.Serialize(film);
            var context = new StringContent(filmJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7193/api/Films", context);

            if (response.IsSuccessStatusCode)
            {
                this.Frame.Navigate(typeof(FilmPage));
            }
            else
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Creation failed!",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FilmPage));
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
    }
}
