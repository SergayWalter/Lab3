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
using Kyrs;

namespace Kyrs
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private Preparation preparation = new Preparation();
        private Manufacturer manufacturer = new Manufacturer();
        private Type_ type_  = new Type_();
        public AddEditWindow()
        {
            InitializeComponent();
            DataContext = preparation;
            ComboRecipe.ItemsSource = KyrsEntities1.GetContext().Recipe.ToList();
            ManufacturerComboBox.ItemsSource = KyrsEntities1.GetContext().Manufacturer.ToList();
            TypeComboBox.ItemsSource = KyrsEntities1.GetContext().Type_.ToList();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _)) e.Handled = true;
        }
        private void BtnSave_Click(Object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            if (preparation.PName == null)
                error.AppendLine("Введите название препарата!");
            else if (KyrsEntities1.GetContext().Preparation.Any(row
            => row.PName == preparation.PName))
                error.AppendLine("Название уже существует!");
            if (TypeComboBox.SelectedItem !=
            null && TypeComboBox.SelectedItem is Type_ selectedTName)
                preparation.TypeID = selectedTName.TypeID;
            else error.AppendLine("Выберите пункт с типом препарата!");
            if (preparation.Description_ == null)
                error.AppendLine("Введите противопоказания!");
            if (ManufacturerComboBox.SelectedItem !=
        null && ManufacturerComboBox.SelectedItem is Manufacturer selectedMName)
                preparation.ManufacturerID = selectedMName.ManufacturerID;
            else error.AppendLine("Выберите производителя!");
            if (ComboRecipe.SelectedItem !=
            null && ComboRecipe.SelectedItem is Recipe selectedRName)
                preparation.RecipeID = selectedRName.RecipeID;
            else error.AppendLine("Выберите пункт с наличием рецепта!");
            if (preparation.Cost == null)
                error.AppendLine("Введите цену!");
            else if (!int.TryParse(preparation.Cost.ToString(),
            out int Cost) || Cost <= 0)
                error.AppendLine("Цена должна иметь положительное и не отрицательное значение!");
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Предупреждение!", MessageBoxButton.OK,
                MessageBoxImage.Information);
                return;
            }
            try
            {
                var context = KyrsEntities1.GetContext();
                context.Type_.Add(type_);
                context.Manufacturer.Add(manufacturer);
                context.SaveChanges();
                var TypeId = type_.TypeID;
                var ManufacturerId = manufacturer.ManufacturerID;
                preparation.TypeID = TypeId;
                preparation.ManufacturerID = ManufacturerId;
                context.Preparation.Add(preparation);
                context.SaveChanges();
                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
