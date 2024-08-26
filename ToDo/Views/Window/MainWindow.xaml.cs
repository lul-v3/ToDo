using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using ToDo.Helper;
using ToDo.Models;
using Wpf.Ui.Controls;

namespace ToDo.Views.Window
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public ObservableCollection<ToDoItem> ToDoItems { get; set; }
        FileHelper fileHelper;

        public MainWindow()
        {
            InitializeComponent();
            fileHelper = new FileHelper();

            ToDoItems = new ObservableCollection<ToDoItem>();
            ToDoItemsControl.ItemsSource = ToDoItems;

            LoadDataFromFile();
            Update();
        }

        #region Save&Load Data

        private void SaveDataToFile()
        {
            var saveData = new SaveData { ToDoItems = ToDoItems };
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todoitems.xml");
            fileHelper.SaveToDoItems(filePath, saveData);
        }
        private void LoadDataFromFile()
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todoitems.xml");
            if(File.Exists(filePath))
            {
                var loadedData = fileHelper.LoadToDoItems(filePath);
                this.ToDoItems = loadedData.ToDoItems;
                ToDoItemsControl.ItemsSource = this.ToDoItems;
            }
            else
            {
                // Create an empty save data file if it doesn't exist'
                File.Create(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todoitems.xml"));
            }
        }

        #endregion

        #region Events

        #region Button Click Events
        private void AddToDo_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditWindow();
            if (editWindow.ShowDialog() == true)
            {
                ToDoItems.Add(editWindow._ToDoItem);
            }
            Update();
        }

        private void EditToDo_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Wpf.Ui.Controls.Button;
            var selectedItem = button?.Tag as ToDoItem;

            if (selectedItem != null)
            {
                var editorWindow = new EditWindow(selectedItem);
                if (editorWindow.ShowDialog() == true)
                {
                    // Update the selected item
                    var index = ToDoItems.IndexOf(selectedItem);
                    ToDoItems[index] = editorWindow._ToDoItem;
                    ToDoItemsControl.Items.Refresh();
                }
            }
            else
            {
                ShowMessage("Error", "Edit failed: No item found");
            }
            Update();
        }

        private void DeleteToDo_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Wpf.Ui.Controls.Button;
            var selectedItem = button?.Tag as ToDoItem;

            if (selectedItem != null)
            {
                // Delete the selected item
                ToDoItems.Remove(selectedItem);
                ToDoItemsControl.Items.Refresh();
            }
            else
            {
                ShowMessage("Error", "Deletion failed: No To Do Item found.");
            }
            Update();
        }

        private void DeleteCompletedToDos_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Wpf.Ui.Controls.Button;
            var selectedItem = button?.Tag as ToDoItem;

            int delCount = 0;

            // Liste für die zu entfernenden Elemente
            var itemsToRemove = new List<ToDoItem>();

            foreach (var item in ToDoItems)
            {
                if (item.IsCompleted)
                {
                    itemsToRemove.Add(item);
                }
            }

            // Entfernen der Elemente nach der Iteration
            foreach (var item in itemsToRemove)
            {
                ToDoItems.Remove(item);
                delCount++;
            }

            ToDoItemsControl.Items.Refresh();

            if (delCount > 0) { ShowMessage("Information", $"{delCount} ToDo Items were deleted."); }
            else { ShowMessage("Information", "No completed ToDo items found."); }
            Update();
        }


        #endregion

        #region CheckBox Events

        private void IsCompletedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void IsCompletedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        #endregion

        #region Window Events

        private void FluentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveDataToFile();
            // ShowMessage("Information", "The To Do data was saved successfully!");
        }

        #endregion

        #endregion

        #region Methods

        private void Update()
        {
            #region Update DeleteCompletedToDos Button
            int i = 0;
            foreach (var item in ToDoItems)
            {
                if (item.IsCompleted)
                {
                    i++;
                }
            }

            if (i > 0) { DeleteCompletedToDosButton.IsEnabled = true; }
            else { DeleteCompletedToDosButton.IsEnabled = false; }
            #endregion
        }

        private void ShowMessage(string Title, string Message)
        {
            Wpf.Ui.Controls.MessageBox msgb = new Wpf.Ui.Controls.MessageBox();
            msgb.Title = Title;
            msgb.Content = Message;
            msgb.ShowDialogAsync();
        }

        #endregion
    }
}
