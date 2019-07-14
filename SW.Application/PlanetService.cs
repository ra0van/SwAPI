using SW.Entities;
using SW.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Application
{
    public class PlanetService<T> : IExecutor<T> where T : Planet
    {
        IRepository<T> planetRepository;
        ICollection<T> resultSet;
        public PlanetService()
        {
            planetRepository = new Repository<T>();
            resultSet = planetRepository.GetAllEntities();
        }

        public ICollection<T> Execute()
        {
            if(resultSet == null)
                resultSet = planetRepository.GetAllEntities();

            return resultSet;
        }

        public T GetByName(string name)
        {
            if (resultSet == null)
                resultSet = planetRepository.GetAllEntities();

            return resultSet.Where(x => x.Name == name).FirstOrDefault();
        }
    }
}
