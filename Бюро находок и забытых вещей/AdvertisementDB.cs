using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public class AdvertisementDB : IDB<Advertisement>
    {
        CategoryDB categoryDB;
        CountryDB countryDB;
        DiscoveredDB discoveredDB;
        List<Advertisement> advertisements = new List<Advertisement>();
        string fileName = "advertisementsdb.bin";

        public int Count { get { return advertisements.Count; } }

        public event EventHandler CountChanged;

        public AdvertisementDB()
        {
            if (!File.Exists(fileName))
                return;
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                advertisements = (List<Advertisement>)bf.Deserialize(fs);
            }
        }

        public Advertisement Add()
        {
            categoryDB = new CategoryDB();
            countryDB = new CountryDB();
            discoveredDB = new DiscoveredDB();
            var catigories = categoryDB.GetList();
            var countries = countryDB.GetList();
            var cities = countryDB.GetCities(countries[0]);
            var subcatigories = categoryDB.GetSubCategories(catigories[0]);
            var discovereds = discoveredDB.GetDiscovered();
            Advertisement advertisement = new Advertisement
            {
                Address = "",
                Close = false,
                ContactPerson = "",
                Description = "",
                Header = "",
                Phone = 0,
                Time = DateTime.Now,
                Category = catigories[0],
                City = cities[0],
                Country = countries[0],
                Discovered = discovereds[0],
                Subcategory = subcatigories[0]
            };
            advertisements.Add(advertisement);
            Save();
            return advertisement;
        }

        public void Edit(Advertisement edit, string name)
        {
            
        }

        public List<Advertisement> GetData(int start, int count)
        {
            if (Count > start + count)
                return advertisements.GetRange(start, count);
            else if (Count > start)
                return advertisements.GetRange(start, Count - start);
            else
                return new List<Advertisement>();
        }

        public List<Advertisement> GetList()
        {
            return advertisements;
        }

        public void Remove(Advertisement delete)
        {
            advertisements.Remove(delete);
            Save();
        }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, advertisements);
            }
            CountChanged?.Invoke(this, new EventArgs());
        }
    }
}
