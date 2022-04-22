
using RepositorioGitHub.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioGitHub.Business.Contract
{
   public interface IGitHubApiBusiness
   {
        Task<ActionResult<GitHubRepositoryViewModel>> Get();
        Task<ActionResult<RepositoryViewModel>> GetByName(string name);
        Task<ActionResult<GitHubRepositoryViewModel>> GetById(long id);
        Task<ActionResult<GitHubRepositoryViewModel>> GetRepository(string owner, string name);
        ActionResult<FavoriteViewModel> GetFavoriteRepository();
        ActionResult<FavoriteViewModel> SaveFavoriteRepository(FavoriteViewModel view);
   }
}
