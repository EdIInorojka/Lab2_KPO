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
    /// Логика взаимодействия для StudentForm.xaml
    /// </summary>
    public partial class StudentForm : Window
    {
        private Student _student;

        public StudentForm(Student student = null)
        {
            InitializeComponent();
            _student = student ?? new Student();

            using (var context = new AppDbContext())
            {
                GroupComboBox.ItemsSource = context.Groups.ToList();
                if (_student.GroupId > 0)
                {
                    GroupComboBox.SelectedItem = context.Groups.FirstOrDefault(g => g.Id == _student.GroupId);
                }
            }

            NameTextBox.Text = _student.Name;
            SurnameTextBox.Text = _student.Surname;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _student.Name = NameTextBox.Text;
            _student.Surname = SurnameTextBox.Text;
            _student.GroupId = (GroupComboBox.SelectedItem as Group)?.Id ?? 0;

            using (var context = new AppDbContext())
            {
                if (_student.Id == 0)
                    context.Students.Add(_student);
                else
                    context.Students.Update(_student);

                context.SaveChanges();
            }

            AppDbContext.NotifyDataChanged();

            Close();
        }
    }
}