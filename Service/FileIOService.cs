using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using WeatherApp.Model;

namespace WeatherApp.Service
{
    internal class FileIOService
    {
        private readonly string? PATH;
        public FileIOService(string? path)
        {
            PATH = path;
        }
        public BindingList<City> LoadData()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                City city = new City("Туапсе");
                BindingList<City> citiesList = new BindingList<City>();
                citiesList.Add(city);
                SaveData(citiesList);
                return citiesList;
            }
            using(var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<City>>(fileText);
            }

        } 
        public void SaveData(object cities)
        {
            using(StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(cities);
                writer.Write(output);
            }
        }
    }
}
