using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public class FilterDB : IDB<Advertisement>
    {
        List<Advertisement> filter = new List<Advertisement>();
        public int Count { get { return filter.Count; } }

        public event EventHandler CountChanged;

        public Advertisement Add()
        {
            return null;
        }

        public void Edit(Advertisement edit, string name)
        {
        }

        public List<Advertisement> GetData(int start, int count)
        {
            if (Count > start + count)
                return filter.GetRange(start, count);
            else if (Count > start)
                return filter.GetRange(start, Count - start);
            else
                return new List<Advertisement>();
        }

        public List<Advertisement> GetList()
        {
            return filter;
        }

        public void Remove(Advertisement delete)
        {
        }

        public void Save()
        {
        }

        public void SetCurrentData(List<Advertisement> advertisements)
        {
            filter = advertisements;
        }
    }
}
