using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositorioGitHub.Business;
using RepositorioGitHub.Dominio;
using RepositorioGitHub.Dominio.Interfaces;
using RepositorioGitHub.Infra.ApiGitHub;
using RepositorioGitHub.Infra.Contract;
using System;

namespace RepositorioGitHub.Tests
{
    [TestClass]
    public class GitHubApiBusinessTest
    {
        [TestMethod]
        public void TestSaveFavoriteRepository()
        {
            var context = new Mock<IContextRepository>();
            var api = new Mock<IGitHubApi>();

            context.Setup(contextMethod => contextMethod.ExistsByCheckAlready(new Favorite())).Returns(false);
            context.Setup(contextMethod => contextMethod.Insert(new Favorite())).Returns(true);
            var business = new GitHubApiBusiness(context.Object, api.Object);

            ActionResult<FavoriteViewModel> result = business.SaveFavoriteRepository(new FavoriteViewModel());


            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual("Algo deu errado.", result.Message);
            Assert.AreEqual(null, result.Result);
        }
    }
}
