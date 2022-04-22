using Newtonsoft.Json;
using RepositorioGitHub.Dominio;
using RepositorioGitHub.Infra.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RepositorioGitHub.Infra.Repositorio
{
    public class ContextRepository : IContextRepository
    {
        private readonly string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models", "favorites.json");
        
        public bool ExistsByCheckAlready(Favorite favorite)
        {
            List<Favorite> test = GetAll();
            Favorite fav = test.Find(element => element.Name == favorite.Name && element.Owner == favorite.Owner);

            if (fav != null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public List<Favorite> GetAll()
        {
            string jsonString = File.ReadAllText(_dbPath);
            var parsedJson = JsonConvert.DeserializeObject<List<Favorite>>(jsonString);

            return parsedJson;
        }

        public bool Insert(Favorite favorite)
        {
            try
            {
                string jsonString = File.ReadAllText(_dbPath);

                var parsedJson = JsonConvert.DeserializeObject<List<Favorite>>(jsonString);

                List<Favorite> data = new List<Favorite>();

                if (parsedJson != null)
                {
                    long maxId = parsedJson.Max(fav => fav.Id);
                    favorite.Id = maxId + 1;

                    data.AddRange(parsedJson);
                }
                data.Add(favorite);

                string json = JsonConvert.SerializeObject(data.ToArray(), Formatting.Indented);

                File.WriteAllText(_dbPath, json);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
