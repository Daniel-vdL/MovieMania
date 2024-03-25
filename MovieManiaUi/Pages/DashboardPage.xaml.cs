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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MovieManiaUi.Pages
{
    public sealed partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            this.InitializeComponent();
        }

        private void MoviePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MoviePage));
        }

        private void SeriePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SeriePage));
        }

        private void GenrePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GenrePage));
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
