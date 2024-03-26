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
    public sealed partial class GenrePage : Page
    {
        public GenrePage()
        {
            this.InitializeComponent();
            LoadGenre();
        }

        public async void LoadGenre()
        {
            try
            {
                var apiHandler = new ApiHandler();
                var genres = await apiHandler.GetGenresAsync();

                GenreListView.ItemsSource = genres;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading genres: {ex.Message}");
            }
        }

        private void RefreshGenre_Click(object sender, RoutedEventArgs e)
        {
            LoadGenre();
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = SearchTextBox.Text.ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    this.LoadGenre();
                }
                else
                {
                    var apiHandler = new ApiHandler();
                    var genres = await apiHandler.GetGenresAsync();
                    var filteredGenres = genres.Where(g => g.Name.ToLower().Contains(searchText)).ToList();

                    GenreListView.ItemsSource = filteredGenres;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while searching genres: {ex.Message}");
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashboardPage));
        }

        private void CreateGenre_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateGenrePage));
        }

        private void GenreListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedGenre = (Models.Genre)e.ClickedItem;
            Frame.Navigate(typeof(GenreDetailPage), selectedGenre);
        }
    }
}
