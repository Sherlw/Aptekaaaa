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
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        public ProductsWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using var db = new PharmacyDBEntities();

            string search = txtSearch.Text.ToLower();
            var products = db.Products
    .Where(p => string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search))
    .Select(p => new
    {
        p.ProductID,
        p.Name,
        SupplierName = p.Suppliers != null ? p.Suppliers.Name : "",
        p.QuantityInStock,
        p.Cost,
        p.ExpirationDate
    })
    .ToList();

            dgProducts.ItemsSource = products;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = new ProductFormWindow();
            if (form.ShowDialog() == true)
                LoadProducts();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Products selected)
            {
                var form = new ProductFormWindow(selected.ProductID);
                if (form.ShowDialog() == true)
                    LoadProducts();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgProducts.SelectedItem is Products selected))
            {
                MessageBox.Show("Выберите товар для удаления.");
                return;
            }

            if (MessageBox.Show($"Удалить товар '{selected.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using var db = new PharmacyDBEntities();
                var product = db.Products.Find(selected.ProductID);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    LoadProducts();
                }
            }
        }
    }
}
