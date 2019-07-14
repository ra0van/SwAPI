using SW.Entities;
using SW.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SW.Application
{
    public class FilmService<T> : IExecutor<T> where T : Film
    {
        IRepository<T> filmRepository;
        ICollection<T> resultSet;
        public FilmService()
        {
            filmRepository = new Repository<T>();
            resultSet = filmRepository.GetAllEntities();
        }

        public ICollection<T> Execute()
        {
            if (resultSet == null)
                resultSet = filmRepository.GetAllEntities();

            return resultSet;
        }

        public T GetByName(string name)
        {
            if (resultSet == null)
                resultSet = filmRepository.GetAllEntities();

            return resultSet.Where(x => x.Title == name).FirstOrDefault();
        }
    }
}
