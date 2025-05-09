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
    /// Логика взаимодействия для SuppliersFormWindow.xaml
    /// </summary>
    public partial class SuppliersFormWindow : Window
    {
        private int? supplierId;
        public SuppliersFormWindow(int? supplierId = null)
        {
            InitializeComponent();
            this.supplierId = supplierId;

            if (supplierId.HasValue)
                LoadSupplier(supplierId.Value);
        }

        private void LoadSupplier(int id)
        {
            using var db = new PharmacyDBEntities();
            var supplier = db.Suppliers.Find(id);
            if (supplier != null)
            {
                txtName.Text = supplier.Name;
                txtContact.Text = supplier.ContactInformation;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Название обязательно.");
                return;
            }

            using var db = new PharmacyDBEntities();

            var isNew = !supplierId.HasValue;
            var supplier = isNew ? new Suppliers() : db.Suppliers.Find(supplierId);

            supplier.Name = txtName.Text;
            supplier.ContactInformation = txtContact.Text;

            if (isNew)
                db.Suppliers.Add(supplier);

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
