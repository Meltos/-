using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public interface IDB<T>
    {
        int Count { get; }
        event EventHandler CountChanged;
        List<T> GetData(int start, int count);
        void Save();
        T Add();
        void Remove(T delete);
        void Edit(T edit, string name);
        List<T> GetList();
    }
}
