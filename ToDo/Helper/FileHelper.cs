using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ToDo.Models;

namespace ToDo.Helper
{
    public class FileHelper
    {

        public void SaveToDoItems(string filePath, SaveData data)
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        public SaveData LoadToDoItems(string filePath)
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            using (var reader = new StreamReader(filePath))
            {
                return (SaveData)serializer.Deserialize(reader);
            }
        }

    }
}
