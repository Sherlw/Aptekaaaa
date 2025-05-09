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
    /// Логика взаимодействия для SuppliersWindow.xaml
    /// </summary>
    public partial class SuppliersWindow : Window
    {
        public SuppliersWindow()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            using var db = new PharmacyDBEntities();
            string search = txtSearch.Text.ToLower();

            var suppliers = db.Suppliers
                .Where(s => string.IsNullOrEmpty(search) || s.Name.ToLower().Contains(search))
                .ToList();

            dgSuppliers.ItemsSource = suppliers;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadSuppliers();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = new SuppliersFormWindow();
            if (form.ShowDialog() == true)
                LoadSuppliers();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgSuppliers.SelectedItem is Suppliers selected))
            {
                MessageBox.Show("Выберите поставщика.");
                return;
            }

            var form = new SuppliersFormWindow(selected.SupplierID);
            if (form.ShowDialog() == true)
                LoadSuppliers();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgSuppliers.SelectedItem is Suppliers selected))
            {
                MessageBox.Show("Выберите поставщика.");
                return;
            }

            if (MessageBox.Show($"Удалить поставщика '{selected.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using var db = new PharmacyDBEntities();
                var supplier = db.Suppliers.Find(selected.SupplierID);
                if (supplier != null)
                {
                    db.Suppliers.Remove(supplier);
                    db.SaveChanges();
                    LoadSuppliers();
                }
            }
        }
    }
}
