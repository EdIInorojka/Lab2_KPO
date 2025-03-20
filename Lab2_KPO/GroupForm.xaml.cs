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
    /// Логика взаимодействия для GroupForm.xaml
    /// </summary>
    public partial class GroupForm : Window
    {
        private Group _group;

        public GroupForm(Group group = null)
        {
            InitializeComponent();
            _group = group ?? new Group();

            using (var context = new AppDbContext())
            {
                FacultyComboBox.ItemsSource = context.Faculties.ToList();
                if (_group.FacultyId > 0)
                {
                    FacultyComboBox.SelectedItem = context.Faculties.FirstOrDefault(f => f.Id == _group.FacultyId);
                }
            }

            TitleTextBox.Text = _group.Title;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _group.Title = TitleTextBox.Text;
            _group.FacultyId = (FacultyComboBox.SelectedItem as Faculty)?.Id ?? 0;

            using (var context = new AppDbContext())
            {
                if (_group.Id == 0)
                    context.Groups.Add(_group);
                else
                    context.Groups.Update(_group);

                context.SaveChanges();
            }

            AppDbContext.NotifyDataChanged();

            Close();
        }
    }
}
