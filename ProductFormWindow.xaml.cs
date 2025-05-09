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
    /// Логика взаимодействия для ProductFormWindow.xaml
    /// </summary>
    public partial class ProductFormWindow : Window
    {
        private int? productId;
        public ProductFormWindow(int? productId = null)
        {
            InitializeComponent();
            this.productId = productId;
            LoadSuppliers();
            if (productId.HasValue)
                LoadProduct(productId.Value);
        }
        private void LoadSuppliers()
        {
            using var db = new PharmacyDBEntities();
            cbSupplier.ItemsSource = db.Suppliers.ToList();
        }

        private void LoadProduct(int id)
        {
            using var db = new PharmacyDBEntities();
            var product = db.Products.Find(id);
            if (product != null)
            {
                txtName.Text = product.Name;
                cbSupplier.SelectedValue = product.SupplierID;
                txtQuantity.Text = product.QuantityInStock?.ToString();
                txtCost.Text = product.Cost?.ToString();
                dpExpiration.SelectedDate = product.ExpirationDate;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using var db = new PharmacyDBEntities();

            if (string.IsNullOrWhiteSpace(txtName.Text) || cbSupplier.SelectedValue == null)
            {
                MessageBox.Show("Заполните обязательные поля.");
                return;
            }

            var isNew = !productId.HasValue;
            var product = isNew ? new Products() : db.Products.Find(productId);

            product.Name = txtName.Text;
            product.SupplierID = (int)cbSupplier.SelectedValue;
            product.QuantityInStock = int.TryParse(txtQuantity.Text, out var q) ? q : 0;
            product.Cost = decimal.TryParse(txtCost.Text, out var c) ? c : 0;
            product.ExpirationDate = dpExpiration.SelectedDate;

            if (isNew)
                db.Products.Add(product);

            db.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
