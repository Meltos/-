using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Бюро_находок_и_забытых_вещей.Forms
{
    public partial class ReportForm : Form
    {
        Paginator<ReportDB, Advertisement> paginator;
        ListViewViewer viewer;
        ReportDB reportDB;
        AdvertisementDB dB;

        public ReportForm()
        {
            InitializeComponent();
            LoadBox();
            dB = new AdvertisementDB();
            reportDB = new ReportDB();
            // создаем экземпляр пагинатора для отображения 10 записей на странице. Число 10 можно сделать переменной и вынести в настройки
            paginator = new Paginator<ReportDB, Advertisement>(reportDB, 20);
            paginator.CountChanged += Paginator_CountChanged; ;
            paginator.CurrentIndexChanged += Paginator_CurrentIndexChanged;
            paginator.ShowRowsChanges += Paginator_ShowRowsChanges;
            // для отображения данных в листвью я сделал отдельный класс
            // в нем кэшируются строки
            viewer = new ListViewViewer(listView1, 1, 20);
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
    }
}
