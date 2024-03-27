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
    public sealed partial class GenreDetailPage : Page
    {
        private Models.Genre selectedGenre;

        public GenreDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Models.Genre genre)
            {
                selectedGenre = genre;
            }

            LoadGenreInfo();
        }

        private void LoadGenreInfo()
        {
            NameTextBlock.Text = selectedGenre.Name;
        }

        private async void EditGenreButton_Click(object sender, RoutedEventArgs e)
        {

            if (selectedGenre == null || selectedGenre.Id <= 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(GenreName.Text))
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

            var updatedGenre = new Models.Genre
            {
                Id = selectedGenre.Id,
                Name = GenreName.Text,
            };

            var genreJson = JsonSerializer.Serialize(updatedGenre);

            using (var client = new HttpClient())
            {
                var apiUrl = $"https://localhost:7193/api/Genres/{selectedGenre.Id}";
                var content = new StringContent(genreJson, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    selectedGenre = updatedGenre;
                    LoadGenreInfo();
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
            if (selectedGenre == null || selectedGenre.Id <= 0)
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
                var apiUrl = $"https://localhost:7193/api/Genres/{selectedGenre.Id}";
                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    this.Frame.Navigate(typeof(GenrePage));
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
            this.Frame.Navigate(typeof(GenrePage));
        }
    }
}
