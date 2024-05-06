using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace MailBoxd
{
    public partial class MainWindow : Window
    {
        public List<Film> Films { get; set; } = new List<Film>(); // Initialisation ici
        public List<Film> FilteredFilms { get; set; }
        public List<Serie> Series { get; set; } = new List<Serie>(); // Initialisation ici
        public List<Serie> FilteredSeries { get; set; }
        public DataManager DataManager { get; set; }

        public MainWindow()
        {
            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();

            Task.Delay(5000).ContinueWith(t =>
            {
                Dispatcher.Invoke(() =>
                {
                    // Initialisation des données après la fermeture du SplashScreen
                    InitializeComponent();
                    DataManager = new DataManager();
                    DataContext = DataManager;

                    DataManager.LoadData();
                    UpdateFilteredLists();
                    UpdateUI();

                    // Fermer l'écran de chargement
                    splashScreen.Close();
                });
            });

            InitializeComponent();
            DataManager = new DataManager();
            DataContext = DataManager;

            DataManager.LoadData();

            // Mettez à jour les listes filtrées après le chargement des données
            UpdateFilteredLists();
            UpdateUI();
        }



        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterContent();
        }

        private void FilterContent()
        {
            string searchText = SearchBox.Text?.ToLower() ?? string.Empty;

            // Vérifiez d'abord si DataManager n'est pas nul
            if (DataManager != null)
            {
                if (DataManager.Films != null) // Vérifiez que la liste n'est pas nulle
                {
                    FilteredFilms = DataManager.Films
                        .Where(film => film.Titre.ToLower().Contains(searchText))
                        .ToList();
                }

                if (DataManager.Series != null) // Vérifiez que la liste n'est pas nulle
                {
                    FilteredSeries = DataManager.Series
                        .Where(serie => serie.Titre.ToLower().Contains(searchText))
                        .ToList();
                }

                UpdateUI();
            }
        }

        public void SaveData()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(appDirectory, "Data");
            string dataFilePath = Path.Combine(dataDirectory, "data.json");

            FileManager fileManager = new FileManager(dataFilePath);

            if (fileManager.CanWriteToFile())
            {
                DataManager.SaveData(); // Assume this method writes to the dataFilePath
                MessageBox.Show("Data saved successfully!");
            }
            else
            {
                MessageBox.Show("Write permission is not granted for data.json");
            }
        }



        private void ShowAddMediaForm(object sender, RoutedEventArgs e)
        {
            AddMediaWindow addMediaWindow = new AddMediaWindow();
            if (addMediaWindow.ShowDialog() == true)
            {
                if (addMediaWindow.NewMediaItem is Film film)
                {
                    DataManager.Films.Add(film);
                }
                else if (addMediaWindow.NewMediaItem is Serie serie)
                {
                    DataManager.Series.Add(serie);
                }

                DataManager.SaveData();
                UpdateFilteredLists();
                UpdateUI();
            }
        }

        private void UpdateFilteredLists()
        {
            // Assurez-vous que les listes sont initialisées
            FilteredFilms = new List<Film>(DataManager.Films);
            FilteredSeries = new List<Serie>(DataManager.Series);
        }

        private void UpdateUI()
        {
            Dispatcher.Invoke(() =>
            {
                if (FilmsListView != null)
                {
                    
                    FilmsListView.ItemsSource = FilteredFilms;
                }
                if (SeriesListView != null)
                {
                    
                    SeriesListView.ItemsSource = FilteredSeries;
                }
                FilmsListView.Items.Refresh();  // Assurez-vous que les ListView sont rafraîchies
                SeriesListView.Items.Refresh();
            });
        }

        

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = Brushes.Gray;
            }
        }
        private void Star_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton button)
            {
                var film = button.DataContext as Film;
                var serie = button.DataContext as Serie;

                int newRating = Convert.ToInt32(button.Tag);

                if (film != null)
                {
                    film.Note = newRating;
                }
                else if (serie != null)
                {
                    serie.Note = newRating;
                }

                UpdateUI(); 
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.DataContext is Film film)
            {
                film.IsInWishlist = true;
                DataManager.SaveData(); // Sauvegarde après changement
            }
            else if (checkBox.DataContext is Serie serie)
            {
                serie.IsInWishlist = true;
                DataManager.SaveData(); // Sauvegarde après changement
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.DataContext is Film film)
            {
                film.IsInWishlist = false;
                DataManager.SaveData(); // Sauvegarde après changement
            }
            else if (checkBox.DataContext is Serie serie)
            {
                serie.IsInWishlist = false;
                DataManager.SaveData(); // Sauvegarde après changement
            }
        }
        private void OpenWishlistPage_Click(object sender, RoutedEventArgs e)
        {
            // Mise à jour des listes peut-être inutile ici si elle ne modifie pas les données pertinentes
            var wishlistFilms = DataManager.Films.Where(f => f.IsInWishlist).ToList();
            var wishlistSeries = DataManager.Series.Where(s => s.IsInWishlist).ToList();
            var allWishlistItems = wishlistFilms.Cast<IMediaItem>().Concat(wishlistSeries.Cast<IMediaItem>()).ToList();

            if (allWishlistItems.Any())
            {
                var wishlistWindow = new WishlistPage(allWishlistItems);
                wishlistWindow.Show();
            }
            else
            {
                MessageBox.Show("Your wishlist is empty");
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DataManager.SaveData();
            MessageBox.Show("Modification completed successfully !");
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = (int)value;
            return count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double rating)
            {
                var stars = new bool[5];
                int fullStarsCount = (int)Math.Round(rating);

                for (int i = 0; i < fullStarsCount; i++)
                {
                    stars[i] = true;
                }

                return stars;
            }

            return Enumerable.Repeat(false, 5).ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NoteToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double rating && parameter is string starIndex)
            {
                int index = int.Parse(starIndex);
                return index <= rating ? Brushes.Gold : Brushes.Gray;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



}