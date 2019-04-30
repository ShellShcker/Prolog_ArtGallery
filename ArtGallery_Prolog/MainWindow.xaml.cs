using System;
using SbsSW.SwiPlCs;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using ArtGallery_Prolog.Models;
using ArtGallery_Prolog.ViewModels;

namespace ArtGallery_Prolog
{
    public partial class MainWindow : Window
    {
        List<ListViewModel> items;

        public MainWindow()
        {
            InitializeComponent();

            ComboBoxViewModel comboBoxViewModel = new ComboBoxViewModel();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            mainPanel.Visibility = Visibility.Collapsed;

            artistComboBox.DisplayMemberPath = "_Key";
            artistComboBox.SelectedValuePath = "_Value";

            artistComboBox.ItemsSource = comboBoxViewModel.ArtistComboBox();

            paintingListView.SelectedItem = null;
        }

        private void enterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Environment Variables
                Environment.SetEnvironmentVariable("SWI_HOME_DIR", @"C:\\Program Files (x86)\\swipl");
                Environment.SetEnvironmentVariable("Path", @"C:\\Program Files (x86)\\swipl\\bin");
                string[] p = { "-q", "-f", @"ArtGallery.pl" };

                // Connect to Prolog Engine
                PlEngine.Initialize(p);

                // Change to Main Panel
                index.Visibility = Visibility.Collapsed;
                mainPanel.Visibility = Visibility.Visible;

                // Close Prolog Connection
                PlEngine.PlCleanup();
            }
            catch
            {
                indexErrorMessage.Text = "Connection Error: Could not connect to Prolog Engine";
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {            
            // Initialize ListView
            paintingListView.ItemsSource = null;
            items = new List<ListViewModel>();

            // ComboBox Variables
            ComboBoxPairsModel artist, museum, technique, year, description;
            string artistValue, museumValue, techniqueValue, yearValue, descriptionValue;

            // Connect to Prolog Engine
            string[] p = { "-q", "-f", @"ArtGallery.pl" };            
            PlEngine.Initialize(p);

            // Prolog Initialization
            PlQuery consult;
            PlQuery cargar = new PlQuery("cargar('ArtGallery.bd')");
            cargar.NextSolution();

            // Get ComboBox Values
            try { artist = (ComboBoxPairsModel)artistComboBox.SelectedItem;
                artistValue = artist._Value; } catch { artistValue = "Artist";  }

            consult = new PlQuery("paintingsOf(Name," + artistValue + ",Museum,Technique,Year,Description).");
            foreach (PlQueryVariables data in consult.SolutionVariables)
            {
                items.Add(new ListViewModel()
                {                    
                    Name = data["Name"].ToString(),
                    Artist = artistValue,
                    Museum = data["Museum"].ToString(),
                    Technique = data["Technique"].ToString(),
                    Year = data["Year"].ToString(),
                    Description = data["Description"].ToString(),
                });
            }

            // Close Prolog Connection
            PlEngine.PlCleanup();

            // Render Images
            foreach (ListViewModel item in items)
            {
                item.ImageSource = "Resources/" + item.Name + ".png";
            }


            paintingListView.ItemsSource = items;
            ComboBoxNull();
        }

        private void PaintingListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Cast SelectedItem to ListViewModel
            ListViewModel item = (ListViewModel)paintingListView.SelectedItem;            

            // String to ImageSource (Uri)
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(item.ImageSource, UriKind.Relative);
            bimage.EndInit();
            selectedImage.Source = bimage;

            // Asign values to Painting Detail Panel
            selectedPaint.Text = item.Name;
            selectedArtist.Text = item.Artist;
            selectedMuseum.Text = item.Museum;
            selectedTechnique.Text = item.Technique;
            selectedYear.Text = item.Year;
            selectedDescription.Text = item.Description;
        }

        private void ComboBoxNull()
        {
            artistComboBox.SelectedItem = null;
        }
    }
}
