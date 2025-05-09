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
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            var win = new ProductsWindow();
            win.ShowDialog();
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            var win = new OrdersWindow();
            win.ShowDialog();
        }

        private void BtnCustomers_Click(object sender, RoutedEventArgs e)
        {
            var win = new CustomersWindow();
            win.ShowDialog();
        }

        private void BtnSuppliers_Click(object sender, RoutedEventArgs e)
        {
            var win = new SuppliersWindow();
            win.ShowDialog();
        }

        private void BtnEmployees_Click(object sender, RoutedEventArgs e)
        {
            var win = new EmployeesWindow();
            win.ShowDialog();
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            var win = new ReportsWindow();
            win.ShowDialog();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var login = new MainWindow();
            login.Show();
            this.Close();
        }
    }
}
