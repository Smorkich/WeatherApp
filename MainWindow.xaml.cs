using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WeatherApp.Model;
using WeatherApp.Service;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly string apiKey = "9a226768560941c1b51225957241901";
        private readonly string? PATH = $"{Environment.CurrentDirectory}\\cities.json"; 
        private string cityName;
        private BindingList<City> cities;
        private FileIOService fileIOService;

        private DateTime now;

        public DateTime Now
        {
            get
            {
                return now;
            }
            set
            {
                now = value;
                OnPropertyChanged(nameof(Now));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            fileIOService = new FileIOService(PATH);
            try
            {
                cities = fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            CmbBox.ItemsSource = cities.Select(x => x.Name);
            CmbBox.Text = CmbBox.Items[0].ToString();
            DataContext = this;

            lblDigitalClock.Visibility = Visibility.Hidden;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (sender, args) =>
            {
                Now = DateTime.Now;
            };
            timer.Start();

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void btnGetWeather_Click(object sender, RoutedEventArgs e)
        {
            cityName = txtCityName.Text.Trim();
            if (string.IsNullOrEmpty(cityName))
            {
                MessageBox.Show("Enter city");
                return;
            }
            string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}";

            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(apiUrl);
                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using(Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jsonResponse = reader.ReadToEnd();
                            WeatherData? weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResponse);
                            DisplayWeatherData(weatherData);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            lblDigitalClock.Visibility = Visibility.Visible;
            txtCityName.Text = "";
        }

        private void DisplayWeatherData(WeatherData? weatherData)
        {
            lblCityName.Content = weatherData.Location.Name;
            lblTemperature.Content = weatherData.Current.TempC + "C";
            lblCondition.Content = weatherData.Current.Condition.Text;
            lblHumidity.Content = weatherData.Current.Humidity + "%";
            lblWindSpeed.Content = weatherData.Current.WindKph + "km/h";

            BitmapImage weatherIcon = new BitmapImage();
            weatherIcon.BeginInit();
            weatherIcon.UriSource = new Uri("http:" + weatherData.Current.Condition.Icon);
            weatherIcon.EndInit();
            imgWeatherIcon.Source = weatherIcon;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void CmbBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            txtCityName.Text = CmbBox.SelectedItem.ToString();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCityWindow addCityWindow = new AddCityWindow(PATH, cities);
            addCityWindow.Show();
        }
    }
}
