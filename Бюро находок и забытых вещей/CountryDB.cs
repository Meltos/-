using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public class CountryDB : IDB<Country>
    {
        List<Country> countries = new List<Country>();
        string fileName = "contrydb.bin";

        public event EventHandler CountChanged;

        public int Count { get { return countries.Count; } }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, countries);
            }
            CountChanged?.Invoke(this, new EventArgs());
        }

        public CountryDB()
        {
            if (!File.Exists(fileName))
                return;
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                countries = (List<Country>)bf.Deserialize(fs);
            }
        }

        public City AddCity(Country selectedcountry)
        {
            City newcity = new City
            {
                NameCity = "",
                Country = selectedcountry
            };
            selectedcountry.Cities.Add(newcity);
            Save();
            return newcity;
        }

        public void EditCity(City selectedcity, string namecity)
        {
            selectedcity.NameCity = namecity;
            Save();
        }

        public List<City> GetCities(Country selectedcountry)
        {
            return selectedcountry.Cities;
        }

        public void RemoveCity(Country country, City city)
        {
            country.Cities.Remove(city);
            Save();
        }

        public List<Country> GetData(int start, int count)
        {
            if (Count > start + count)
                return countries.GetRange(start, count);
            else if (Count > start)
                return countries.GetRange(start, Count - start);
            else
                return new List<Country>();
        }

        public Country Add()
        {
            Country country = new Country { NameCountry = "" };
            countries.Add(country);
            Save();
            return country;
        }

        public void Remove(Country delete)
        {
            if (delete.Cities.Count > 0)
                return;
            countries.Remove(delete);
            Save();
        }

        public void Edit(Country edit, string name)
        {
            edit.NameCountry = name;
            Save();
        }

        public List<Country> GetList()
        {
            return countries;
        }

        public List<Country> GetListCombobox()
        {
            List<Country> countriesCombo = new List<Country>();
            Country nullcountry = new Country { NameCountry = "" };
            countriesCombo.Add(nullcountry);
            countriesCombo.AddRange(countries);
            return countriesCombo;
        }

        public List<City> GetListComboboxCity(Country country)
        {
            List<City> citybox = new List<City>();
            City city = new City { NameCity = "", Country = country };
            citybox.Add(city);
            citybox.AddRange(country.Cities);
            return citybox;
        }
    }
}
