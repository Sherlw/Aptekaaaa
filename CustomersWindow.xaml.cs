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
    /// Логика взаимодействия для CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow : Window
    {
        public CustomersWindow()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            using var db = new PharmacyDBEntities();
            string search = txtSearch.Text.ToLower();

            var customers = db.Customers
                .Where(c =>
                    string.IsNullOrEmpty(search) ||
                    c.FirstName.ToLower().Contains(search) ||
                    c.LastName.ToLower().Contains(search))
                .ToList();

            dgCustomers.ItemsSource = customers;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = new CustomersFormWindow();
            if (form.ShowDialog() == true)
                LoadCustomers();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgCustomers.SelectedItem is Customers selected))
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            var form = new CustomersFormWindow(selected.CustomerID);
            if (form.ShowDialog() == true)
                LoadCustomers();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgCustomers.SelectedItem is Customers selected))
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            if (MessageBox.Show($"Удалить клиента '{selected.FirstName} {selected.LastName}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using var db = new PharmacyDBEntities();
                var customer = db.Customers.Find(selected.CustomerID);
                if (customer != null)
                {
                    db.Customers.Remove(customer);
                    db.SaveChanges();
                    LoadCustomers();
                }
            }
        }
    }
}
