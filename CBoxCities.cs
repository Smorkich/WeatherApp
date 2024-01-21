using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class CBoxCities
    {
        private ArrayList cities = new ArrayList();
        public CBoxCities() 
        {
            cities.Add("Туапсе");
            cities.Add("Краснодар");
        }
        public ArrayList GetCities()
        {
            return cities;
        }
    }
}
