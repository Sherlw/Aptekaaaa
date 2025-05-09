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
    /// Логика взаимодействия для EmployeesWindow.xaml
    /// </summary>
    public partial class EmployeesWindow : Window
    {
        public EmployeesWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using var db = new PharmacyDBEntities();
            string search = txtSearch.Text.ToLower();

            var employees = db.Employees
                .Where(e =>
                    string.IsNullOrEmpty(search) ||
                    e.FirstName.ToLower().Contains(search) ||
                    e.LastName.ToLower().Contains(search) ||
                    e.Position.ToLower().Contains(search))
                .ToList();

            dgEmployees.ItemsSource = employees;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = new EmployeeFormWindow();
            if (form.ShowDialog() == true)
                LoadEmployees();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgEmployees.SelectedItem is Employees selected))
            {
                MessageBox.Show("Выберите сотрудника.");
                return;
            }

            var form = new EmployeeFormWindow(selected.EmployeeID);
            if (form.ShowDialog() == true)
                LoadEmployees();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgEmployees.SelectedItem is Employees selected))
            {
                MessageBox.Show("Выберите сотрудника.");
                return;
            }

            if (MessageBox.Show($"Удалить сотрудника '{selected.FirstName} {selected.LastName}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using var db = new PharmacyDBEntities();
                var emp = db.Employees.Find(selected.EmployeeID);
                if (emp != null)
                {
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                    LoadEmployees();
                }
            }
        }
    }
}
