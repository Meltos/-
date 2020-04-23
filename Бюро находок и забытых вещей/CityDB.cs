using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public class CityDB : IDB<City>
    {
        CountryDB dB;
        Country country;
        public int Count { get { return country.Cities.Count; } }

        public event EventHandler CountChanged;

        public CityDB(CountryDB dB, Country country)
        {
            this.dB = dB;
            this.country = country;
        }

        public City Add()
        {
            City city = dB.AddCity(country);
            Save();
            return city;
        }

        public void Edit(City edit, string name)
        {
            dB.EditCity(edit, name);
            Save();
        }

        public List<City> GetData(int start, int count)
        {
            if (Count > start + count)
                return country.Cities.GetRange(start, count);
            else if (Count > start)
                return country.Cities.GetRange(start, Count - start);
            else
                return new List<City>();
        }

        public List<City> GetList()
        {
            return null;
        }

        public void Remove(City delete)
        {
            dB.RemoveCity(country, delete);
            Save();
        }

        public void Save()
        {
            dB.Save();
            CountChanged?.Invoke(this, new EventArgs());
        }

        public List<City> Transformation(Country country)
        {
            return country.Cities;
        }

        public List<Country> GetCountries()
        {
            return dB.GetList();
        }

        
    }
}
