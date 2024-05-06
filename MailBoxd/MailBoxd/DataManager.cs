using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;

namespace MailBoxd
{
    public class DataManager
    {
        public List<Film> Films { get; set; } = new List<Film>(); // Initialisez par défaut
        public List<Serie> Series { get; set; } = new List<Serie>();

        private string dataFilePath;

        public DataManager()
        {
            // Construit le chemin vers le dossier Data dans le répertoire de l'application
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(appDirectory, "Data");
            dataFilePath = Path.Combine(dataDirectory, "data.json");

            // Assurez-vous que le répertoire existe
            Directory.CreateDirectory(dataDirectory);

            LoadData();
        }
        public void SaveData()
        {
            var data = new { Films = this.Films, Series = this.Series };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(dataFilePath, json);
        }
        public void LoadData()
        {
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                try
                {
                    var data = JsonConvert.DeserializeObject<DataContainer>(json);
                    Films = data?.Films ?? new List<Film>();
                    Series = data?.Series ?? new List<Serie>();
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error deserializing JSON: " + ex.Message);
                }
            }
        }

        public class DataContainer
        {
            public List<Film> Films { get; set; }
            public List<Serie> Series { get; set; }
        }
    }
}
