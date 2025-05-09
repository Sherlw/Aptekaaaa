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
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        public OrdersWindow()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            using var db = new PharmacyDBEntities();

            var orders = db.Orderss
                .Select(o => new
                {
                    o.OrderID,
                    o.OrderDates,
                    CustomerName = o.Customers.FirstName + " " + o.Customers.LastName,
                    WarehouseName = o.Warehouses.Address,
                    o.TotalCost
                })
                .ToList();

            dgOrders.ItemsSource = orders;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrders.SelectedItem == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }

            // Получаем ID заказа
            var selected = dgOrders.SelectedItem;
            var orderId = (int)selected.GetType().GetProperty("OrderID").GetValue(selected);

            var win = new OrderFormWindow(orderId); // окно просмотра заказа
            win.ShowDialog();
        }
    }
}
