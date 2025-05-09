using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aptekaaaa
{
    /// <summary>
    /// Логика взаимодействия для ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
            cbReportType.SelectedIndex = 0;
            dpFrom.SelectedDate = DateTime.Now.AddDays(-30);
            dpTo.SelectedDate = DateTime.Now;
        }

        private void CbReportType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Скрывать/показывать фильтры в зависимости от отчёта (если надо)
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var selectedReport = (cbReportType.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedReport == "Остатки товаров")
            {
                GenerateStockReport();
            }
            else if (selectedReport == "Продажи за период")
            {
                GenerateSalesReport();
            }
        }

        private void GenerateStockReport()
        {
            using var db = new PharmacyDBEntities();

            var report = db.ProductsInWarehouses
                .Select(pi => new
                {
                    Склад = pi.Warehouses.Address,
                    Товар = pi.Products.Name,
                    Количество = pi.Count
                })
                .OrderBy(r => r.Склад)
                .ToList();

            dgReport.ItemsSource = report;
        }

        private void GenerateSalesReport()
        {
            if (dpFrom.SelectedDate == null || dpTo.SelectedDate == null)
            {
                MessageBox.Show("Выберите диапазон дат.");
                return;
            }

            DateTime from = dpFrom.SelectedDate.Value;
            DateTime to = dpTo.SelectedDate.Value;

            using var db = new PharmacyDBEntities();

            var report = db.Orderss
                .Where(o => o.OrderDates >= from && o.OrderDates <= to)
                .Select(o => new
                {
                    Дата = o.OrderDates,
                    Клиент = o.Customers.FirstName + " " + o.Customers.LastName,
                    Сумма = o.TotalCost
                })
                .OrderBy(o => o.Дата)
                .ToList();

            dgReport.ItemsSource = report;
        }
    }
}
