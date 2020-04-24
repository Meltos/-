using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Бюро_находок_и_забытых_вещей
{
    public partial class MainForm : Form
    {
        Paginator<AdvertisementDB, Advertisement> paginator;
        ListViewViewer viewer;
        CategoryDB categoryDB;
        CountryDB countryDB;
        DiscoveredDB discoveredDB;
        AdvertisementDB dB;
        public MainForm()
        {
            InitializeComponent();

            countryDB = new CountryDB();
            comboBox1.DataSource = null;
            comboBox1.DataSource = countryDB.GetListCombobox();
            comboBox1.DisplayMember = "NameCountry";

            categoryDB = new CategoryDB();
            comboBox3.DataSource = null;
            comboBox3.DataSource = categoryDB.GetListCombobox();
            comboBox3.DisplayMember = "NameCategory";

            discoveredDB = new DiscoveredDB();
            comboBox5.DataSource = null;
            comboBox5.DataSource = discoveredDB.GetDiscoveredBox();
            comboBox5.DisplayMember = "Status";

            dB = new AdvertisementDB();
            // создаем экземпляр пагинатора для отображения 10 записей на странице. Число 10 можно сделать переменной и вынести в настройки
            paginator = new Paginator<AdvertisementDB, Advertisement>(dB, 10);
            // для отображения данных в листвью я сделал отдельный класс
            // в нем кэшируются строки
            viewer = new ListViewViewer(listView1, 5, 10);


            // вызываем обновление всех данных и событий
            // за счет того, что данный метод вызывается ПОСЛЕ создания пагинатора интерфейс успевает подписаться на события пагинатора и нормально отобразить все данные
            dB.Save();
        }

        private void Paginator_CurrentIndexChanged(object sender, EventArgs e)
        {
            hScrollBar1.Value = paginator.CurrentIndex;
        }

        private void Paginator_CountChanged(object sender, EventArgs e)
        {
            hScrollBar1.Maximum = paginator.MaxIndex;
            hScrollBar1.Value = paginator.CurrentIndex;
        }

        private void Paginator_ShowRowsChanges(object sender, EventArgs e)
        {
            ShowAdvertisementData(paginator.ShowRows);
        }

        private void ShowAdvertisementData(List<Advertisement> rows)
        {
            List<Advertisement> filter = new List<Advertisement>();
            Country combocountry = new Country();
            combocountry = (Country)comboBox1.SelectedItem;
            City combocity = new City();
            combocity = (City)comboBox2.SelectedItem;
            Category combocategory = new Category();
            combocategory = (Category)comboBox3.SelectedItem;
            SubCategory combosubCategory = new SubCategory();
            combosubCategory = (SubCategory)comboBox4.SelectedItem;
            string combodiscovered = (string)comboBox5.SelectedItem;
            if (combocountry.NameCountry == "" && combocategory.NameCategory == "" && combodiscovered == "")
            {
                viewer.ViewData(rows);
                return;
            }
            foreach (var row in rows)
            {
                if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == row.Discovered && combocity.NameCity == row.City.NameCity && combosubCategory.NameSubcategory == row.Subcategory.NameSubcategory)
                {
                    filter.Add(row);
                }
                //city
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == row.Discovered && combosubCategory.NameSubcategory == row.Subcategory.NameSubcategory && combocity.NameCity == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == "" && combosubCategory.NameSubcategory == row.Subcategory.NameSubcategory && combocity.NameCity == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == "" && combosubCategory.NameSubcategory == "" && combocity.NameCity == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == "" && combodiscovered == "" && combocity.NameCity == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == "" && combodiscovered == row.Discovered && combocity.NameCity == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == row.Discovered && combocity.NameCity == "" && combosubCategory.NameSubcategory == "")
                {
                    filter.Add(row);
                }
                //subCategory
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == row.Discovered && combocity.NameCity == row.City.NameCity && combosubCategory.NameSubcategory == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == "" && combocity.NameCity == row.City.NameCity && combosubCategory.NameSubcategory == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == "" && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == "" && combosubCategory.NameSubcategory == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == "" && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == row.Discovered && combosubCategory.NameSubcategory == "")
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == "" && combocategory.NameCategory == "" && combodiscovered == row.Discovered)
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == "" && combodiscovered == "" && combocity.NameCity == row.City.NameCity)
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == row.Country.NameCountry && combocategory.NameCategory == "" && combodiscovered == row.Discovered && combocity.NameCity == row.City.NameCity)
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == "" && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == "" && combocity.NameCity == "" && combosubCategory.NameSubcategory == row.Subcategory.NameSubcategory)
                {
                    filter.Add(row);
                }
                else if (combocountry.NameCountry == "" && combocategory.NameCategory == row.Category.NameCategory && combodiscovered == row.Discovered && combocity.NameCity == "" && combosubCategory.NameSubcategory == row.Subcategory.NameSubcategory)
                {
                    filter.Add(row);
                }
            }
            viewer.ViewData(filter);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            paginator.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            paginator.End();
        }

        private void добавитьСтрануToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CountryForm countryForm = new CountryForm();
            countryForm.ShowDialog();
        }

        private void добавитьГородToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var countries = countryDB.GetList();
            if (countries.Count == 0)
            {
                MessageBox.Show("Вы ещё не добавили страну!");
                return;
            }
            CityForm cityForm = new CityForm();
            cityForm.ShowDialog();
        }

        private void добавитьКатегориюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm();
            categoryForm.ShowDialog();
        }

        private void добавитьПодкатегориюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var categories = categoryDB.GetList();
            if (categories.Count == 0)
            {
                MessageBox.Show("Вы ещё не создали категорию!");
                return;
            }
            SubcategoryForm subcategoryForm = new SubcategoryForm();
            subcategoryForm.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Country combocountry = new Country();
            combocountry = (Country)comboBox1.SelectedItem;
            if (combocountry.NameCountry == "" || combocountry.Cities.Count == 0)
            {
                label2.Visible = false;
                comboBox2.Visible = false;
                comboBox2.DataSource = null;
                comboBox2.DataSource = countryDB.GetListComboboxCity(combocountry);
                comboBox2.DisplayMember = "NameCity";
                return;
            }
            label2.Visible = true;
            comboBox2.Visible = true;
            comboBox2.DataSource = null;
            comboBox2.DataSource = countryDB.GetListComboboxCity(combocountry);
            comboBox2.DisplayMember = "NameCity";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category combocategory = new Category();
            combocategory = (Category)comboBox3.SelectedItem;
            if (combocategory.NameCategory == "" || combocategory.Subcategories.Count == 0)
            {
                label4.Visible = false;
                comboBox4.Visible = false;
                comboBox4.DataSource = null;
                comboBox4.DataSource = categoryDB.GetListComboboxSubCategory(combocategory);
                comboBox4.DisplayMember = "NameSubcategory";
                return;
            }
            label4.Visible = true;
            comboBox4.Visible = true;
            comboBox4.DataSource = null;
            comboBox4.DataSource = categoryDB.GetListComboboxSubCategory(combocategory);
            comboBox4.DisplayMember = "NameSubcategory";
        }

        private void добавитьОбъявлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var categories = categoryDB.GetList();
            var countries = countryDB.GetList();
            if (categories.Count == 0)
            {
                MessageBox.Show("Вы ещё не создали категорию!");
                return;
            }
            else if (countries.Count == 0)
            {
                MessageBox.Show("Вы ещё не добавили страну!");
                return;
            }
            paginator.ShowRowsChanges -= Paginator_ShowRowsChanges;
            AddAdvertisementForm addAdvertisementForm = new AddAdvertisementForm(dB, categoryDB, countryDB, discoveredDB, dB.Add(), 1);
            addAdvertisementForm.ShowDialog();
            dB.Save();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;
            Advertisement advertisement1 = (Advertisement)listView1.SelectedItems[0].Tag;
            AddAdvertisementForm addAdvertisementForm = new AddAdvertisementForm(dB, categoryDB, countryDB, discoveredDB, advertisement1, 0);
            addAdvertisementForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (i == 1)
            {
                // подписываемся на событие изменения выводимых записей
                paginator.ShowRowsChanges -= Paginator_ShowRowsChanges;
                // подписываемся на изменение кол-ва страниц
                paginator.CountChanged -= Paginator_CountChanged;
                // подписываемся на изменение текущего индекса
                paginator.CurrentIndexChanged -= Paginator_CurrentIndexChanged;
                i--;
            }
            // подписываемся на событие изменения выводимых записей
            paginator.ShowRowsChanges += Paginator_ShowRowsChanges;
            // подписываемся на изменение кол-ва страниц
            paginator.CountChanged += Paginator_CountChanged;
            // подписываемся на изменение текущего индекса
            paginator.CurrentIndexChanged += Paginator_CurrentIndexChanged;
            i++;
            dB.Save();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue)
                paginator.Right();
            else if (e.NewValue < e.OldValue)
                paginator.Left();
        }
    }
}
