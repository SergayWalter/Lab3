using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
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

namespace Kyrs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshPreparationDataGrid();
            ComboType.ItemsSource = KyrsEntities1.GetContext().Type_.ToList();
            Vis();
        }

        private void RefreshPreparationDataGrid()
        {
            var context = KyrsEntities1.GetContext();
            var requestWithRelations = context.Preparation
                .Include(r => r.Recipe)
                .Include(r => r.Manufacturer)
                .ToList();

            Preparation.ItemsSource = requestWithRelations;
        }

        private void Vis()
        {
            switch (Authorization.authorizationRole)
            {
                case "Админ":
                    break;
                case "Модер":
                    BtnDelet.Visibility = Visibility.Collapsed;
                    break;
                case "Юзер":
                    BtnDelet.Visibility = Visibility.Collapsed;
                    BtnAdd.Visibility = Visibility.Collapsed;
                    break;
                default:
                    return;
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Authorization.authorizationRole == "Юзер") MessageBox.Show("Вы не можете редактировать данные таблицы!");
            else
            {
                var selectedRequest = (sender as Button)?.DataContext as Preparation;
                if (selectedRequest != null)
                {
                    RefreshWindow addEditWindow = new RefreshWindow(selectedRequest);
                    if (addEditWindow.ShowDialog() == true)
                    {
                        RefreshPreparationDataGrid();
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow addEditWindow = new AddEditWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                RefreshPreparationDataGrid();
            }
        }

        private void BtnDelet_Click(object sender, RoutedEventArgs e)
        {
            var servisForRemoving = Preparation.SelectedItems.Cast<Preparation>().ToList();
            if (servisForRemoving.Any() && MessageBox.Show($"Вы точно хотите удалить следующее {servisForRemoving.Count()} элемент?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var context = KyrsEntities1.GetContext();
                context.Preparation.RemoveRange(servisForRemoving);
                context.SaveChanges();
                MessageBox.Show("Данные удалены");
                RefreshPreparationDataGrid();
            }
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            RefreshPreparationDataGrid();
        }


        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedItem is Type_ selectedType)
            {
                int selectedTypeID = selectedType.TypeID;
                var context = KyrsEntities1.GetContext();
                Preparation.ItemsSource = context.Preparation
                    .Include(r => r.Recipe)
                    .Include(r => r.Manufacturer)
                    .Where(r => r.TypeID == selectedTypeID)
                    .ToList();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            var context = KyrsEntities1.GetContext();
            try
            {
                Preparation.ItemsSource = context.Preparation
                .Include(r => r.Recipe)
                .Include(r => r.Manufacturer)
                .Where(r =>
                r.Manufacturer.MName.ToLower().Contains(searchText) ||
                r.Type_.TName.ToLower().Contains(searchText) ||
                r.PName.ToLower().Contains(searchText))
                .ToList();

            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
            {
                // Логировка или отладка исключения
                Console.WriteLine(ex.InnerException?.Message);
            }
        }
        private void BtnAuthorization_Click(object sender, EventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }
    }
}
