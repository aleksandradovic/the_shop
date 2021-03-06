using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;
using Repository.Context;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly InMemoryDbContext _context;
        private readonly ILogger<ArticleRepository> _logger;

        public ArticleRepository(InMemoryDbContext context, ILogger<ArticleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Article Add(Article article)
        {
            _context.Articles.Add(article);
            return article;
        }

        public List<Article> GetAll()
        {
            return _context.Articles.ToList();
        }

        // Id for article is article code
        public Article GetById(string code)
        {
            return _context.Articles.FirstOrDefault(a => a.Code == code);
        }

        public void Remove(string code)
        {
            var article = _context.Articles.Where(a => a.Code == code).FirstOrDefault();

            if (article != null)
            {
                _context.Articles.Remove(article);
            }
            else
            {
                _logger.LogInformation($"Failed deleting article. Article with code {code} does not exist.");
            }
        }
    }
}
