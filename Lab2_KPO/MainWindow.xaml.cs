using Lab2_KPO.Data;
using Lab2_KPO.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2_KPO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            SetupContextMenu();

            AppDbContext.DataChanged += OnDataChanged;
        }

        private void OnDataChanged()
        {
            LoadData();
        }

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

        protected override void OnClosed(EventArgs e)
        {
            AppDbContext.DataChanged -= OnDataChanged;
            base.OnClosed(e);
        }

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenu();

            // Меню для факультетов
            var addFacultyMenuItem = new MenuItem { Header = "Добавить факультет" };
            addFacultyMenuItem.Click += AddFaculty_Click;
            contextMenu.Items.Add(addFacultyMenuItem);

            var editFacultyMenuItem = new MenuItem { Header = "Изменить данные факультета" };
            editFacultyMenuItem.Click += EditFaculty_Click;
            contextMenu.Items.Add(editFacultyMenuItem);

            var deleteFacultyMenuItem = new MenuItem { Header = "Удалить факультет" };
            deleteFacultyMenuItem.Click += DeleteFaculty_Click;
            contextMenu.Items.Add(deleteFacultyMenuItem);

            // Меню для групп
            var addGroupMenuItem = new MenuItem { Header = "Добавить группу" };
            addGroupMenuItem.Click += AddGroup_Click;
            contextMenu.Items.Add(addGroupMenuItem);

            var editGroupMenuItem = new MenuItem { Header = "Изменить данные группы" };
            editGroupMenuItem.Click += EditGroup_Click;
            contextMenu.Items.Add(editGroupMenuItem);

            var deleteGroupMenuItem = new MenuItem { Header = "Удалить группу" };
            deleteGroupMenuItem.Click += DeleteGroup_Click;
            contextMenu.Items.Add(deleteGroupMenuItem);

            // Меню для студентов
            var addStudentMenuItem = new MenuItem { Header = "Добавить студента" };
            addStudentMenuItem.Click += AddStudent_Click;
            contextMenu.Items.Add(addStudentMenuItem);

            var editStudentMenuItem = new MenuItem { Header = "Изменить данные студента" };
            editStudentMenuItem.Click += EditStudent_Click;
            contextMenu.Items.Add(editStudentMenuItem);

            var deleteStudentMenuItem = new MenuItem { Header = "Удалить студента" };
            deleteStudentMenuItem.Click += DeleteStudent_Click;
            contextMenu.Items.Add(deleteStudentMenuItem);

            MainTreeView.ContextMenu = contextMenu;
        }

        // CRUD для факультетов
        private void AddFaculty_Click(object sender, RoutedEventArgs e)
        {
            var form = new FacultyForm();
            if (form.ShowDialog() == true)
            {
                LoadData();
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
                    LoadData();
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
                    "Подтвердить",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Faculties.Remove(selectedFaculty);
                        context.SaveChanges();
                    }
                    LoadData();
                }
            }
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            var selectedFaculty = MainTreeView.SelectedItem as Faculty;
            if (selectedFaculty != null)
            {
                var form = new GroupForm();
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберете факультет для добавления группы.");
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
                    LoadData();
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
                    "Подтвердить",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Groups.Remove(selectedGroup);
                        context.SaveChanges();
                    }
                    LoadData();
                }
            }
        }

        // CRUD для студентов
        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = MainTreeView.SelectedItem as Group;
            if (selectedGroup != null)
            {
                var form = new StudentForm();
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберете группу для добавления студента.");
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
                    LoadData();
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
                    "Подтвердить",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Students.Remove(selectedStudent);
                        context.SaveChanges();
                    }
                    LoadData();
                }
            }
        }
    }
}