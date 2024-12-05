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

namespace Kyrs
{
    /// <summary>
    /// Логика взаимодействия для RefreshWindow.xaml
    /// </summary>
    public partial class RefreshWindow : Window
    {
        private Preparation _currentPreparation;
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _)) e.Handled = true;
        }
        public RefreshWindow(Preparation preparation)
        {
            InitializeComponent();
            _currentPreparation = preparation;
            TypeComboBox.ItemsSource = KyrsEntities1.GetContext().Type_.ToList();
            ManufacturerComboBox.ItemsSource = KyrsEntities1.GetContext().Manufacturer.ToList();
            RecipeComboBox.ItemsSource = KyrsEntities1.GetContext().Recipe.ToList();
            // Презагрузка данных
            TypeComboBox.SelectedItem = preparation.Type_;
            ManufacturerComboBox.SelectedItem = preparation.Manufacturer;
            RecipeComboBox.SelectedItem = preparation.Recipe;
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь код для обновления данных в базе
            var context = KyrsEntities1.GetContext();
            _currentPreparation.TypeID =
           ((Type_)TypeComboBox.SelectedItem).TypeID;
            _currentPreparation.ManufacturerID =
           ((Manufacturer)ManufacturerComboBox.SelectedItem).ManufacturerID;
            _currentPreparation.RecipeID =
           ((Recipe)RecipeComboBox.SelectedItem).RecipeID;
            context.SaveChanges();
            MessageBox.Show("Данные заявки обновлены");
            this.Close();
        }
    }
}
