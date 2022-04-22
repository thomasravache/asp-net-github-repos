using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioGitHub.Dominio.Interfaces
{
    public interface IGitHubApi
    {
        Task<ActionResult<GitHubRepository>> GetRepository(string owner);
        Task<ActionResult<RepositoryModel>> GetRepositoryByName(string name);
        Task<ActionResult<GitHubRepository>> GetRepositoryByOwnerAndName(string owner, string name);
    }
}
