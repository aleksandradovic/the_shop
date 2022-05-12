using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        public List<T> GetAll();
        public T GetById(int id);
        public void Add(T entity);
        public void Remove(int id);
    }
}
