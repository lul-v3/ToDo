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
using ToDo.Models;
using Wpf.Ui.Controls;

namespace ToDo.Views.Window
{
    /// <summary>
    /// Interaktionslogik für EditWindow.xaml
    /// </summary>
    public partial class EditWindow : FluentWindow
    {
        public ToDoItem _ToDoItem { get; private set; }

        public EditWindow(ToDoItem item = null)
        {
            InitializeComponent();
            if(item != null)
            {
                _ToDoItem = item;
                TitleTextBox.Text = item.Title;
                DescriptionTextBox.Text = item.Description;
                IsCompletedCheckBox.IsChecked = item.IsCompleted;
            }
            else
            {
                _ToDoItem = new ToDoItem();
            }
        }

        // Save Button Clicked
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _ToDoItem.Title = TitleTextBox.Text;
            _ToDoItem.Description = DescriptionTextBox.Text;
            _ToDoItem.IsCompleted = IsCompletedCheckBox.IsChecked ?? false;
            DialogResult = true;
            Close();
        }

    }
}
