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
    /// Логика взаимодействия для EmployeeFormWindow.xaml
    /// </summary>
    public partial class EmployeeFormWindow : Window
    {
        private int? employeeId;
        public EmployeeFormWindow(int? employeeId = null)
        {
            InitializeComponent();
            this.employeeId = employeeId;

            if (employeeId.HasValue)
                LoadEmployee(employeeId.Value);
        }

        private void LoadEmployee(int id)
        {
            using var db = new PharmacyDBEntities();
            var emp = db.Employees.Find(id);
            if (emp != null)
            {
                txtFirstName.Text = emp.FirstName;
                txtLastName.Text = emp.LastName;
                txtPosition.Text = emp.Position;
                txtLogin.Text = emp.Login;
                txtPassword.Password = emp.Password;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            using var db = new PharmacyDBEntities();

            var isNew = !employeeId.HasValue;
            var emp = isNew ? new Employees() : db.Employees.Find(employeeId);

            emp.FirstName = txtFirstName.Text;
            emp.LastName = txtLastName.Text;
            emp.Position = txtPosition.Text;
            emp.Login = txtLogin.Text;
            emp.Password = txtPassword.Password;

            if (isNew)
                db.Employees.Add(emp);

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
