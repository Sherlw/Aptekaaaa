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
    /// Логика взаимодействия для OrderFormWindow.xaml
    /// </summary>
    public partial class OrderFormWindow : Window
    {
        private List<Products> allProducts;
        private int orderId;
        public OrderFormWindow(int orderId)
        {
            InitializeComponent(); 
            this.orderId = orderId;
            LoadData();
        }
        private void LoadData()
        {
            using var db = new PharmacyDBEntities();
            cbCustomer.ItemsSource = db.Customers.ToList();
            cbWarehouse.ItemsSource = db.Warehouses.ToList();

            allProducts = db.Products.ToList();

            // Настроим выпадающий список товаров
            var productColumn = dgOrderItems.Columns[0] as DataGridComboBoxColumn;
            productColumn.ItemsSource = allProducts;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbCustomer.SelectedValue == null || cbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента и склад.");
                return;
            }

            var items = dgOrderItems.Items.OfType<dynamic>()
                .Where(i => i.ProductID != null && i.Quantity != null && i.Quantity > 0)
                .ToList();

            if (items.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один товар.");
                return;
            }

            using var db = new PharmacyDBEntities();

            var order = new Orderss
            {
                CustomerID = (int)cbCustomer.SelectedValue,
                WarehouseID = (int)cbWarehouse.SelectedValue,
                OrderDates = DateTime.Now,
                TotalCost = 0, // пересчитаем ниже
                EmployeeID = 1 // TODO: подставлять авторизованного
            };

            db.Orderss.Add(order);
            db.SaveChanges(); // Сохраняем, чтобы получить OrderID

            decimal total = 0;

            foreach (var item in items)
            {
                int pid = item.ProductID;
                int qty = item.Quantity;

                var product = db.Products.Find(pid);
                if (product == null || product.QuantityInStock < qty)
                {
                    MessageBox.Show($"Недостаточно товара: {product?.Name}");
                    return;
                }

                // Создаём позицию заказа
                var od = new OrderDetails
                {
                    OrderID = order.OrderID,
                    ProductID = pid,
                    Quantity = qty
                };
                db.OrderDetails.Add(od);

                // Списываем со склада
                product.QuantityInStock -= qty;
                total += product.Cost.GetValueOrDefault() * qty;
            }

            order.TotalCost = total;
            db.SaveChanges();

            MessageBox.Show($"Заказ успешно оформлен. Сумма: {total} руб.");
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
