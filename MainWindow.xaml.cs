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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aptekaaaa
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = false;
            lblStatus.Text = string.Empty;

            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                lblStatus.Text = "Введите логин и пароль";
                btnLogin.IsEnabled = true;
                return;
            }

            try
            {
                using var db = new PharmacyDBEntities();

               
                var user = db.Employees
                                   .FirstOrDefault(u => u.Login == login &&
                                                             u.Password == password);

                if (user == null)
                {
                    lblStatus.Text = "Неверные данные";
                    btnLogin.IsEnabled = true;
                    return;
                }

                MessageBox.Show("Вы авторизованы:" + user.Position);
               
                var main = new MainMenu();  
                main.Show();
                Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе:\n{ex.Message}",
                                "Сбой", MessageBoxButton.OK, MessageBoxImage.Error);
                btnLogin.IsEnabled = true;
            }
        }
    }
}
