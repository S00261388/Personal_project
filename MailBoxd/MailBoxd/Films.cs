using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MailBoxd
{
    public class Film : INotifyPropertyChanged, IMediaItem
    {
        private string titre;
        private string genre;
        private int annee;
        private double note;
        private string imagePath;
        private string review;
        private bool isInWishlist;

        public string Titre { get => titre; set { titre = value; OnPropertyChanged(); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(); } }
        public int Annee { get => annee; set { annee = value; OnPropertyChanged(); } }
        public double Note { get => note; set { note = value; OnPropertyChanged(); } }
        public string ImagePath { get => imagePath; set { imagePath = value; OnPropertyChanged(); } }
        public string Review { get => review; set { review = value; OnPropertyChanged(); } }
        public bool IsInWishlist { get => isInWishlist; set { isInWishlist = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class Serie : INotifyPropertyChanged, IMediaItem
    {
        private string titre;
        private string genre;
        private int annee;
        private double note;
        private string imagePath;
        private string review;
        private bool isInWishlist;

        public string Titre { get => titre; set { titre = value; OnPropertyChanged(); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(); } }
        public int Annee { get => annee; set { annee = value; OnPropertyChanged(); } }
        public double Note { get => note; set { note = value; OnPropertyChanged(); } }
        public string ImagePath { get => imagePath; set { imagePath = value; OnPropertyChanged(); } }
        public string Review { get => review; set { review = value; OnPropertyChanged(); } }
        public bool IsInWishlist { get => isInWishlist; set { isInWishlist = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
