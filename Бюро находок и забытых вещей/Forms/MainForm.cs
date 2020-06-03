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
        Paginator<FilterDB, Advertisement> paginator;
        ListViewViewer viewer;
        CategoryDB categoryDB;
        CountryDB countryDB;
        DiscoveredDB discoveredDB;
        AdvertisementDB dB;
        FilterDB filterDB;

        public MainForm()
        {
            InitializeComponent();
            LoadBox();

            dB = new AdvertisementDB();
            filterDB = new FilterDB();
            // создаем экземпляр пагинатора для отображения 10 записей на странице. Число 10 можно сделать переменной и вынести в настройки
            paginator = new Paginator<FilterDB, Advertisement>(filterDB, 20);
            paginator.CountChanged += Paginator_CountChanged;
            paginator.CurrentIndexChanged += Paginator_CurrentIndexChanged;
            paginator.ShowRowsChanges += Paginator_ShowRowsChanges;
            // для отображения данных в листвью я сделал отдельный класс
            // в нем кэшируются строки
            viewer = new ListViewViewer(listView1, 5, 20);
            dB.Save();
        }

        private void LoadBox()
        {
            countryDB = new CountryDB();
            categoryDB = new CategoryDB();
            discoveredDB = new DiscoveredDB();
            comboBox1.DataSource = null;
            comboBox1.DataSource = countryDB.GetListCombobox();
            comboBox1.DisplayMember = "NameCountry";

            comboBox3.DataSource = null;
            comboBox3.DataSource = categoryDB.GetListCombobox();
            comboBox3.DisplayMember = "NameCategory";

            comboBox5.DataSource = null;
            comboBox5.DataSource = discoveredDB.GetDiscoveredBox();
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
            viewer.ViewData(paginator.ShowRows);
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
            CountryForm countryForm = new CountryForm(countryDB, dB);
            countryForm.ShowDialog();
            LoadBox();
        }

        private void добавитьГородToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var countries = countryDB.GetList();
            if (countries.Count == 0)
            {
                MessageBox.Show("Вы ещё не добавили страну!");
                return;
            }
            CityForm cityForm = new CityForm(countryDB, dB);
            cityForm.ShowDialog();
            LoadBox();
        }

        private void добавитьКатегориюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm(categoryDB, dB);
            categoryForm.ShowDialog();
            LoadBox();
        }

        private void добавитьПодкатегориюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var categories = categoryDB.GetList();
            if (categories.Count == 0)
            {
                MessageBox.Show("Вы ещё не создали категорию!");
                return;
            }
            SubcategoryForm subcategoryForm = new SubcategoryForm(categoryDB, dB);
            subcategoryForm.ShowDialog();
            LoadBox();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;
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
            if (comboBox3.SelectedIndex == -1)
                return;
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
            AddAdvertisementForm addAdvertisementForm = new AddAdvertisementForm(dB, categoryDB, countryDB, discoveredDB, dB.Add(), 1);
            addAdvertisementForm.ShowDialog();
            dB.Save();
            viewer.ViewData(paginator.ShowRows);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;
            Advertisement advertisement1 = (Advertisement)listView1.SelectedItems[0].Tag;
            AddAdvertisementForm addAdvertisementForm = new AddAdvertisementForm(dB, categoryDB, countryDB, discoveredDB, advertisement1, 0);
            addAdvertisementForm.ShowDialog();
            viewer.ViewData(paginator.ShowRows);
        }

        private void button1_Click(object sender, EventArgs e)
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
                filterDB.SetCurrentData(dB.GetList());
            }
            else
            {
                foreach (var row in dB.GetList())
                {
                    if (row.Country.NameCountry.Contains(combocountry?.NameCountry) &&
                    row.Category.NameCategory.Contains(combocategory?.NameCategory) &&
                    row.Discovered.Contains(combodiscovered) &&
                    row.City.NameCity.Contains(combocity?.NameCity) &&
                    row.Subcategory.NameSubcategory.Contains(combosubCategory?.NameSubcategory))
                    {
                        filter.Add(row);
                    }
                }
                filterDB.SetCurrentData(filter);
            }
            viewer.ViewData(paginator.ShowRows);

            dB.Save();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue)
                paginator.Right();
            else if (e.NewValue < e.OldValue)
                paginator.Left();
        }

        private void отчётToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm(categoryDB, countryDB, discoveredDB);
            reportForm.ShowDialog();
        }
    }
}
