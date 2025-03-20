using Lab2_KPO.Data;
using Lab2_KPO.Models;
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

namespace Lab2_KPO
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            using (var context = new AppDbContext())
            {
                if (context.Users.Any(u => u.Username == username))
                {
                    MessageBox.Show("Имя пользователя занято.");
                    return;
                }

                string salt = Guid.NewGuid().ToString();
                string passwordHash = HashPassword(password, salt);

                var user = new User
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    Salt = salt,
                    RegistrationDate = DateTime.Now,
                    IsActive = true
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            MessageBox.Show("Успешная регистрация.");
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close(); 
        }

        private void ReturnToLogin_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}