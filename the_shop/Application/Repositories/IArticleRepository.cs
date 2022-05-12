using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Repositories.Base;

namespace Application.Repositories
{
    public interface IArticleRepository : IBaseRepository<Article>
    {
        public Article GetByCode(string code);
    }
}
