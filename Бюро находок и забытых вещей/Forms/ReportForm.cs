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
    public partial class ReportForm : Form
    {
        Paginator<ReportDB, Advertisement> paginator;
        ListViewViewer viewer;
        ReportDB reportDB;
        AdvertisementDB dB;
        CategoryDB categoryDB;
        CountryDB countryDB;
        DiscoveredDB discoveredDB;

        public ReportForm(CategoryDB categoryDB, CountryDB countryDB, DiscoveredDB discoveredDB)
        {
            InitializeComponent();
            reportDB = new ReportDB();
            LoadBox();
            dB = new AdvertisementDB();
            this.categoryDB = categoryDB;
            this.countryDB = countryDB;
            this.discoveredDB = discoveredDB;
            // создаем экземпляр пагинатора для отображения 10 записей на странице. Число 10 можно сделать переменной и вынести в настройки
            paginator = new Paginator<ReportDB, Advertisement>(reportDB, 30);
            paginator.CountChanged += Paginator_CountChanged; ;
            paginator.CurrentIndexChanged += Paginator_CurrentIndexChanged;
            paginator.ShowRowsChanges += Paginator_ShowRowsChanges;
            // для отображения данных в листвью я сделал отдельный класс
            // в нем кэшируются строки
            viewer = new ListViewViewer(listView1, 1, 30);
            dB.Save();
        }

        private void LoadBox()
        {
            comboBox1.DataSource = null;
            comboBox1.DataSource = reportDB.SetFilterBox();
            
            comboBox2.DataSource = null;
            comboBox2.DataSource = reportDB.SetTimeBox();
        }

        private void Paginator_ShowRowsChanges(object sender, EventArgs e)
        {
            viewer.ViewData(paginator.ShowRows);
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

        private void button2_Click(object sender, EventArgs e)
        {
            paginator.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paginator.End();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue)
                paginator.Right();
            else if (e.NewValue < e.OldValue)
                paginator.Left();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Своя дата")
            {
                dateTimePicker1.Visible = true;
                label1.Visible = true;
            }
            else
            {
                dateTimePicker1.Visible = false;
                label1.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Advertisement> filter = new List<Advertisement>();
            TimeSpan span = new TimeSpan(0, 0, 0, 0);
            DateTime dateTime = new DateTime();
            if (comboBox2.Text == "За неделю")
            {
                span = span.Add(new TimeSpan(7, 0, 0, 0));
            }
            else if (comboBox2.Text == "За месяц")
            { 
                span = span.Add(new TimeSpan(30, 0, 0, 0));
            }
            else if (comboBox2.Text == "За год")
            {
                span = span.Add(new TimeSpan(365, 0, 0, 0));
            }
            else if (comboBox2.Text == "За всё время")
            {
                span = span.Add(new TimeSpan(10000, 0, 0, 0));
            }
            else
            {
                dateTime = dateTimePicker1.Value;
            }
            foreach (var row in dB.GetList())
            {
                if (comboBox2.Text == "Своя дата")
                {
                    if (comboBox1.Text == "Все объявления" && row.Time.Date == dateTime.Date)
                        filter.Add(row);
                    else if (comboBox1.Text == "Открытые объявления" && row.Time.Date == dateTime.Date && row.Close == false)
                        filter.Add(row);
                    else if (comboBox1.Text == "Закрытые объявления" && row.Time.Date == dateTime.Date && row.Close == true)
                        filter.Add(row);
                }
                else
                {
                    if (comboBox1.Text == "Все объявления" && span > DateTime.Now - row.Time)
                        filter.Add(row);
                    else if (comboBox1.Text == "Открытые объявления" && span > DateTime.Now - row.Time && row.Close == false)
                        filter.Add(row);
                    else if (comboBox1.Text == "Закрытые объявления" && span > DateTime.Now - row.Time && row.Close == true)
                        filter.Add(row);
                }
            }
            label2.Text = $"Количество найденных объявлений:{filter.Count}";
            reportDB.SetCurrentData(filter);
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
    }
}
