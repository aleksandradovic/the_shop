using Autofac.Extras.Moq;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Context;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Repository
{
    public class ArticleRepositoryTests
    {
        [Fact]
        public void Add()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var article = new Article { Code = "111", Name = "Article 1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<ArticleRepository>>();

            var articleRepository = mock.Create<ArticleRepository>();

            // Act
            articleRepository.Add(article);

            // Assert
            Assert.Single(context.Articles);
        }

        [Fact]
        public void GetAll()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var articles = new List<Article> { new Article() { Code = "111", Name = "Article 1" },
                                               new Article() { Code = "222", Name = "Article 2" },
                                               new Article() { Code = "333", Name = "Article 3" }};

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Articles.AddRange(articles);

            mock.Mock<ILogger<ArticleRepository>>();

            var articleRepository = mock.Create<ArticleRepository>();

            // Act
            var actual = articleRepository.GetAll();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(articles, actual);
        }

        [Fact]
        public void GetById()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var article = new Article { Code = "111", Name = "Article 1" };

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Articles.Add(article);

            mock.Mock<ILogger<ArticleRepository>>();

            var articleRepository = mock.Create<ArticleRepository>();

            // Act
            var actual = articleRepository.GetById(article.Code);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(article.Code, actual.Code);
        }

        [Fact]
        public void GetById_NonExisting()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var article = new Article { Code = "111", Name = "Article 1" };

            var context = mock.Mock<InMemoryDbContext>().Object;
            context.Articles.Add(article);

            mock.Mock<ILogger<ArticleRepository>>();

            var articleRepository = mock.Create<ArticleRepository>();

            // Act
            var actual = articleRepository.GetById("222");

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Remove()
        {
            using var mock = AutoMock.GetLoose();

            // Arrange
            var article = new Article { Code = "111", Name = "Article 1" };

            var context = mock.Mock<InMemoryDbContext>().Object;

            mock.Mock<ILogger<ArticleRepository>>();

            var articleRepository = mock.Create<ArticleRepository>();
            articleRepository.Add(article);

            // Act
            articleRepository.Remove(article.Code);

            // Assert
            Assert.True(!context.Articles.Any());
        }
    }
}
