using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RepositorioGitHub.App.Controllers;
using RepositorioGitHub.Dominio;
using System.Web.Mvc;
using System.Threading.Tasks;
using Moq;
using RepositorioGitHub.Business.Contract;
using System.Collections.Generic;

namespace RepositorioGitHub.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public async Task TestIndexView()
        {
            var business = new Mock<IGitHubApiBusiness>();

            var mockResult = new GitHubRepositoryViewModel
            {
                Id = 1,
                Name = "repo-do-thomas",
                FullName = "thomasravache/repo-do-thomas",
                Owner = new Owner
                {
                    Login = "thomasravache",
                },
                Url = new Uri("https://api.github.com/repos/thomasravache/teste"),
            };

            var mockData = new ActionResult<GitHubRepositoryViewModel>
            {
                IsValid = true,
                Message = "Carregamento OK!",
                Results = new List<GitHubRepositoryViewModel> { mockResult },
            };

            business.Setup(businessMethod => businessMethod.Get()).ReturnsAsync(mockData);

            var controller = new HomeController(business.Object);

            var result = await controller.Index() as ViewResult;

            var model = result.Model as ActionResult<GitHubRepositoryViewModel>;
;
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(ActionResult<GitHubRepositoryViewModel>));
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");
            Assert.AreEqual("repo-do-thomas", model.Results[0].Name);
        }

        [TestMethod]
        public async Task TestDetailsView()
        {
            long testId = 1;

            var business = new Mock<IGitHubApiBusiness>();

            var mockData = new ActionResult<GitHubRepositoryViewModel>
            {
                IsValid = true,
                Message = "Carregamento OK!",
                Result = new GitHubRepositoryViewModel
                {
                    Name = "repo-do-thomas",
                    Description = "Um projeto de teste",
                    Language = "C#",
                    UpdatedAt = DateTime.Now,
                }
            };

            business.Setup(businessMethod => businessMethod.GetById(testId)).ReturnsAsync(mockData);

            var controller = new HomeController(business.Object);

            var result = await controller.Details(testId) as PartialViewResult;
            var model = result.Model as ActionResult<GitHubRepositoryViewModel>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(ActionResult<GitHubRepositoryViewModel>));
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.AreEqual("repo-do-thomas", model.Result.Name);
        }

        [TestMethod]
        public async Task TestGetRepositorieView()
        {
            string testSearchRepo = "repositorio tal";
            var business = new Mock<IGitHubApiBusiness>();

            var mockRepo1 = new GitHubRepository
            {
                Name = testSearchRepo,
                Owner = new Owner { Login = "thomasravache" },
                FullName = "thomasravache/nome_de_Teste",
                Url = new Uri("https://api.github.com/repos/thomasravache/teste"),
            };

            var mockRepo2 = new GitHubRepository
            {
                Name = "teste",
                Owner = new Owner { Login = "thomasravache" },
                FullName = "thomasravache/teste",
                Url = new Uri("https://api.github.com/repos/thomasravache/teste2"),
            };

            GitHubRepository[] mockRepos = { mockRepo1, mockRepo2 };

            var mockData = new ActionResult<RepositoryViewModel>
            {
                IsValid = true,
                Message = "Carregamento OK!",
                Result = new RepositoryViewModel { Repositories = mockRepos, Name = testSearchRepo }, // Name será utilizado no InputText para buscar os repos
            };

            business.Setup(businessMethod => businessMethod.GetByName(testSearchRepo)).ReturnsAsync(mockData);

            var controller = new HomeController(business.Object);

            var result = await controller.GetRepositorie(mockData) as ViewResult;
            var model = result.Model as ActionResult<RepositoryViewModel>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(ActionResult<RepositoryViewModel>));
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "GetRepositorie");
            Assert.AreEqual("repositorio tal", model.Result.Repositories[0].Name);
        }

        [TestMethod]
        public async Task TestDetailsRepositoryView()
        {
            string testOwner = "alguem";
            string testName = "repoTeste";
            var business = new Mock<IGitHubApiBusiness>();

            var mockData = new ActionResult<GitHubRepositoryViewModel>
            {
                IsValid = true,
                Message = "Successo!",
                Result = new GitHubRepositoryViewModel
                {
                    Name = testName,
                    Description = "Um repositório teste",
                    Language = "C#",
                    UpdatedAt = DateTime.Now,
                    Owner = new Owner { Login = testOwner },
                },
            };

            business.Setup(businessMethod => businessMethod.GetRepository(testOwner, testName)).ReturnsAsync(mockData);

            var controller = new HomeController(business.Object);

            var result = await controller.DetailsRepository(testName, testOwner) as PartialViewResult;
            var model = result.Model as ActionResult<GitHubRepositoryViewModel>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(ActionResult<GitHubRepositoryViewModel>));
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "DetailsRepository");
            Assert.AreEqual("repoTeste", model.Result.Name);
        }

        [TestMethod]
        public void TestFavoriteView()
        {
            var business = new Mock<IGitHubApiBusiness>();

            var mockData = new ActionResult<FavoriteViewModel>
            {
                IsValid = true,
                Message = "Carregamento OK!",
                Results = new List<FavoriteViewModel>
                {
                    new FavoriteViewModel
                    {
                        Id = 0,
                        Description = "Repositório maneiro",
                        Language = "C#",
                        UpdateLast = DateTime.Now,
                        Owner = "thomasravache",
                        Name = "repo-de-teste",
                    },
                },
            };

            business.Setup(businessMethod => businessMethod.GetFavoriteRepository()).Returns(mockData);

            var controller = new HomeController(business.Object);

            var result = controller.Favorite() as ViewResult;
            var model = result.Model as ActionResult<FavoriteViewModel>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(ActionResult<FavoriteViewModel>));
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Favorite");
            Assert.AreEqual("repo-de-teste", model.Results[0].Name);
        }
    }
}
