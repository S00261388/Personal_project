using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MailBoxd
{
    /// <summary>
    /// Interaction logic for AddMediaWindow.xaml
    /// </summary>
    public partial class AddMediaWindow : Window
    {
        public IMediaItem NewMediaItem { get; private set; }

        public AddMediaWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool isFilm = ((ComboBoxItem)mediaTypeComboBox.SelectedItem).Content.ToString() == "Film";
            if (ValidateInputs())
                {
                    if (isFilm)
                    {
                        NewMediaItem = new Film
                        {
                            Titre = txtTitle.Text,
                            Genre = txtGenre.Text,
                            Annee = int.Parse(txtYear.Text),
                            Note = double.Parse(txtNote.Text),
                            Review = txtReview.Text,
                            IsInWishlist = false
                        };
                    }
                    else
                    {
                        NewMediaItem = new Serie
                        {
                            Titre = txtTitle.Text,
                            Genre = txtGenre.Text,
                            Annee = int.Parse(txtYear.Text),
                            Note = double.Parse(txtNote.Text),
                            Review = txtReview.Text,
                            IsInWishlist = false
                        };
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please ensure all fields are correctly filled and year and note are numbers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }



        private bool ValidateInputs()
        {
            return !string.IsNullOrWhiteSpace(txtTitle.Text) &&
                   !string.IsNullOrWhiteSpace(txtGenre.Text) &&
                   int.TryParse(txtYear.Text, out int year) &&
                   double.TryParse(txtNote.Text, out double note) &&
                   year > 1900 && year <= DateTime.Now.Year &&
                   note >= 0 && note <= 5;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
