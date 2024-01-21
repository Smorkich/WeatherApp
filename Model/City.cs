using Newtonsoft.Json;

namespace WeatherApp.Model
{
    public class City
    {
        [JsonProperty (PropertyName = "name")]
        public string? Name { get; set; }

        public City(string? name)
        {
            Name = name;
        }

        public City()
        {

        }
    }
}
