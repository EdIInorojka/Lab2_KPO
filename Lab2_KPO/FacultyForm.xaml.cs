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
    /// Логика взаимодействия для FacultyForm.xaml
    /// </summary>
    public partial class FacultyForm : Window
    {
        private Faculty _faculty;

        public FacultyForm(Faculty faculty = null)
        {
            InitializeComponent();
            _faculty = faculty ?? new Faculty();
            TitleTextBox.Text = _faculty.Title;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _faculty.Title = TitleTextBox.Text;

            using (var context = new AppDbContext())
            {
                if (_faculty.Id == 0)
                    context.Faculties.Add(_faculty);
                else
                    context.Faculties.Update(_faculty);

                context.SaveChanges();
            }

            AppDbContext.NotifyDataChanged();

            Close();
        }
    }
}
