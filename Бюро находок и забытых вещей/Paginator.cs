using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public class Paginator<T, V> where T : IDB<V>
    {
        private T dataBase; // ссылка на бд с записями, используется через интерфейс IDB
        private int countRowsOnPage; // поле, хранящее кол-во выводимых записей
        private int currentIndex = 0;// поле, храняшее индекс текущей страницы

        public int CurrentIndex
        {// Индекс текущей страницы, при изменении генерирует событие
            get => currentIndex;
            private set { currentIndex = value; CurrentIndexChanged?.Invoke(this, new EventArgs()); }
        }

        public int MaxIndex { get; private set; } = 0; // индекс последней страницы

        public List<V> ShowRows { get; set; } // текущие отображаемые записи

        // события
        public event EventHandler ShowRowsChanges;
        public event EventHandler CountChanged;
        public event EventHandler CurrentIndexChanged;

        // конструктор, требует бд и указание кол-ва отображаемых записей
        public Paginator(T dataBase, int countRowsOnPage)
        {
            this.dataBase = dataBase;
            this.countRowsOnPage = countRowsOnPage;
            // если в бд будет изменено кол-во записей, пагинатор сможет отреагировать на это через подписку на событие
            dataBase.CountChanged += DataBase_CountChanged;
            // первичная настройка кол-ва страниц
            ReIndex();
        }

        private void DataBase_CountChanged(object sender, EventArgs e)
        {
            ReIndex();
        }

        private void ReIndex()
        {
            // индекс последней страницы
            MaxIndex = dataBase.Count / countRowsOnPage;
            // проверка на случай, если общее кол-во записей кратно кол-ву выводимых записей. Чтобы последняя страница не была пустой
            if (MaxIndex > 0 && dataBase.Count % countRowsOnPage == 0)
                MaxIndex--;
            // если кол-во страниц уменьшилось, сбрасываем текущую страницу в начало
            if (CurrentIndex >= MaxIndex)
                CurrentIndex = 0;
            // генерируем событие о изменении кол-ва страниц
            CountChanged?.Invoke(this, new EventArgs());
            // обновляем отображаемые записи
            GetShowRows();
        }

        private void GetShowRows()
        {
            // получаем отображаемые записи из бд, используем текущий индекс страницы
            ShowRows = dataBase.GetData(CurrentIndex * countRowsOnPage, countRowsOnPage);
            // генерируем событие о изменении выводимых данных
            ShowRowsChanges?.Invoke(this, new EventArgs());
        }

        public void Left()
        {
            if (CurrentIndex == 0)
                return;
            CurrentIndex--;
            GetShowRows();
        }

        public void Right()
        {
            if (CurrentIndex == MaxIndex)
                return;
            CurrentIndex++;
            GetShowRows();
        }

        public void Start()
        {
            CurrentIndex = 0;
            GetShowRows();
        }

        public void End()
        {
            CurrentIndex = MaxIndex;
            GetShowRows();
        }
    }
}
