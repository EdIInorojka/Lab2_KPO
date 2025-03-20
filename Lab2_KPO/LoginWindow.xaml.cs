using Lab2_KPO.Data;
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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);

                if (user == null || !VerifyPassword(password, user.PasswordHash, user.Salt))
                {
                    MessageBox.Show("Неверное имя пользователя или пароль.");
                    return;
                }

                if (!user.IsActive)
                {
                    MessageBox.Show("Пользователь недоступен.");
                    return;
                }

                MessageBox.Show("Успешный вход.");
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            Close();
        }

        private bool VerifyPassword(string password, string passwordHash, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToBase64String(hashedBytes) == passwordHash;
            }
        }
    }
}
