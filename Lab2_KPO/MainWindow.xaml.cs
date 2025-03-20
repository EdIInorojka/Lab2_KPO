using System.Linq;
using System.Windows;
using Lab2_KPO.Models;
using Lab2_KPO.Data;
using Lab2_KPO.Models;
using Lab2_KPO;
using Microsoft.EntityFrameworkCore;

namespace Lab2_KPO
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();

            // Подписываемся на событие изменения данных
            AppDbContext.DataChanged += OnDataChanged;
        }

        // Метод для загрузки данных
        private void LoadData()
        {
            using (var context = new AppDbContext())
            {
                var faculties = context.Faculties
                    .Include(f => f.Groups)
                    .ThenInclude(g => g.Students)
                    .ToList();
                MainTreeView.ItemsSource = faculties;
            }
        }

        // Обработчик события изменения данных
        private void OnDataChanged()
        {
            LoadData();
        }

        // CRUD операции для факультетов
        private void AddFaculty_Click(object sender, RoutedEventArgs e)
        {
            var form = new FacultyForm();
            if (form.ShowDialog() == true)
            {
                AppDbContext.NotifyDataChanged();
            }
        }

        private void EditFaculty_Click(object sender, RoutedEventArgs e)
        {
            var selectedFaculty = MainTreeView.SelectedItem as Faculty;
            if (selectedFaculty != null)
            {
                var form = new FacultyForm(selectedFaculty);
                if (form.ShowDialog() == true)
                {
                    AppDbContext.NotifyDataChanged();
                }
            }
        }

        private void DeleteFaculty_Click(object sender, RoutedEventArgs e)
        {
            var selectedFaculty = MainTreeView.SelectedItem as Faculty;
            if (selectedFaculty != null)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить факультет '{selectedFaculty.Title}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Faculties.Remove(selectedFaculty);
                        context.SaveChanges();
                    }
                    AppDbContext.NotifyDataChanged();
                }
            }
        }

        // CRUD операции для групп
        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            var selectedFaculty = MainTreeView.SelectedItem as Faculty;
            if (selectedFaculty != null)
            {
                var form = new GroupForm();
                if (form.ShowDialog() == true)
                {
                    AppDbContext.NotifyDataChanged();
                }
            }
            else
            {
                MessageBox.Show("Выберите факультет для добавления группы.");
            }
        }

        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = MainTreeView.SelectedItem as Group;
            if (selectedGroup != null)
            {
                var form = new GroupForm(selectedGroup);
                if (form.ShowDialog() == true)
                {
                    AppDbContext.NotifyDataChanged();
                }
            }
        }

        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = MainTreeView.SelectedItem as Group;
            if (selectedGroup != null)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить группу '{selectedGroup.Title}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Groups.Remove(selectedGroup);
                        context.SaveChanges();
                    }
                    AppDbContext.NotifyDataChanged();
                }
            }
        }

        // CRUD операции для студентов
        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = MainTreeView.SelectedItem as Group;
            if (selectedGroup != null)
            {
                var form = new StudentForm();
                if (form.ShowDialog() == true)
                {
                    AppDbContext.NotifyDataChanged();
                }
            }
            else
            {
                MessageBox.Show("Выберите группу для добавления студента.");
            }
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = MainTreeView.SelectedItem as Student;
            if (selectedStudent != null)
            {
                var form = new StudentForm(selectedStudent);
                if (form.ShowDialog() == true)
                {
                    AppDbContext.NotifyDataChanged();
                }
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = MainTreeView.SelectedItem as Student;
            if (selectedStudent != null)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить студента '{selectedStudent.Name} {selectedStudent.Surname}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Students.Remove(selectedStudent);
                        context.SaveChanges();
                    }
                    AppDbContext.NotifyDataChanged();
                }
            }
        }

        // Отписываемся от события при закрытии окна
        protected override void OnClosed(EventArgs e)
        {
            AppDbContext.DataChanged -= OnDataChanged;
            base.OnClosed(e);
        }
    }
}