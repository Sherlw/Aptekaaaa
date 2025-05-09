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
    /// Логика взаимодействия для CustomersFormWindow.xaml
    /// </summary>
    public partial class CustomersFormWindow : Window
    {
        private int? customerId;
        public CustomersFormWindow(int? customerId = null)
        {
            InitializeComponent();
            this.customerId = customerId;
            if (customerId.HasValue)
                LoadCustomer(customerId.Value);
        }

        private void LoadCustomer(int id)
        {
            using var db = new PharmacyDBEntities();
            var customer = db.Customers.Find(id);
            if (customer != null)
            {
                txtFirstName.Text = customer.FirstName;
                txtLastName.Text = customer.LastName;
                txtContact.Text = customer.ContactInformation;
                dpBirthDate.SelectedDate = customer.DateOfBirth;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Имя и фамилия обязательны.");
                return;
            }

            using var db = new PharmacyDBEntities();

            var isNew = !customerId.HasValue;
            var customer = isNew ? new Customers() : db.Customers.Find(customerId);

            customer.FirstName = txtFirstName.Text;
            customer.LastName = txtLastName.Text;
            customer.ContactInformation = txtContact.Text;
            customer.DateOfBirth = dpBirthDate.SelectedDate;

            if (isNew)
                db.Customers.Add(customer);

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

