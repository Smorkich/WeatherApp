using System;
using System.ComponentModel;
using System.Windows;
using WeatherApp.Model;
using WeatherApp.Service;

namespace WeatherApp
{
    /// <summary>
    /// Логика взаимодействия для AddCityWindow.xaml
    /// </summary>
    public partial class AddCityWindow : Window
    {
        private FileIOService fileIOService;
        private string? PATH;
        private BindingList<City> citiesList;
        public AddCityWindow(string path, BindingList<City> cities)
        {
            InitializeComponent();
            PATH = path;
            citiesList = cities;
            fileIOService = new FileIOService(PATH);
        }

        private void btnAddCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                City newCity = new City(txtAddCity.Text);
                citiesList.Add(newCity);
                fileIOService.SaveData(citiesList);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
