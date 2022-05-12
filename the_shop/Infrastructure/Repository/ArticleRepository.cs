using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;
using Repository.Context;

namespace Repository.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly InMemoryDbContext _context;

        public ArticleRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public Article Add(Article article)
        {
            article.Id = Guid.NewGuid().ToString();
            _context.Articles.Add(article);
            return article;
        }

        public List<Article> GetAll()
        {
            return _context.Articles.ToList();
        }

        public Article GetByCode(string code)
        {
            return _context.Articles.FirstOrDefault(a => a.Code == code);
        }

        public Article GetById(string id)
        {
            return _context.Articles.FirstOrDefault(a => a.Id == id);
        }

        public void Remove(string id)
        {
            var article = _context.Articles.Where(a => a.Id == id).FirstOrDefault();

            if (article != null)
            {
                _context.Articles.Remove(article);
            }
        }
    }
}
