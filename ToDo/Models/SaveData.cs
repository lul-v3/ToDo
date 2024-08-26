using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class SaveData
    {
        public ObservableCollection<ToDoItem> ToDoItems { get; set; } = new ObservableCollection<ToDoItem>();
    }
}
