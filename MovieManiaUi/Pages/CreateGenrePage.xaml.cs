using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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
using MovieManiaUi.Models;

namespace MovieManiaUi.Pages
{
    public sealed partial class CreateGenrePage : Page
    {
        public CreateGenrePage()
        {
            this.InitializeComponent();
        }

        private async void CreateGenreButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextbox.Text))
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Please fill in a name.",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
                return;
            }

            var genreName = NameTextbox.Text;

            var genre = new Genre
            {
                Name = genreName
            };

            using var client = new HttpClient();

            var genreJson = JsonSerializer.Serialize(genre);
            var genreContext = new StringContent(genreJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7193/api/Genres", genreContext);

            if (response.IsSuccessStatusCode)
            {
                this.Frame.Navigate(typeof(GenrePage));
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
            this.Frame.Navigate(typeof(GenrePage));
        }
    }
}
