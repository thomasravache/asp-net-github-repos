using RepositorioGitHub.Business.Contract;
using RepositorioGitHub.Dominio;
using RepositorioGitHub.Dominio.Interfaces;
using RepositorioGitHub.Infra.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace RepositorioGitHub.Business
{
    public class GitHubApiBusiness: IGitHubApiBusiness
    {
        private readonly IContextRepository _context;
        private readonly IGitHubApi _gitHubApi;
        public GitHubApiBusiness(IContextRepository context, IGitHubApi gitHubApi)
        {
            _context = context;
            _gitHubApi = gitHubApi;
        }

        public async Task<ActionResult<GitHubRepositoryViewModel>> Get()
        {
            ActionResult<GitHubRepositoryViewModel> action = new ActionResult<GitHubRepositoryViewModel>();

            var api = await _gitHubApi.GetRepository("thomasravache");

            if (api.IsValid)
            {
                var apiResults = api.Results;

                List<GitHubRepositoryViewModel> results = Helper.GithubViewModelSerialize(apiResults);

                action.IsValid = true;
                action.Message = api.Message;
                action.Result = results[0];
                action.Results = results;
            }
            else
            {
                action.IsValid = false;
                action.Message = api.Message;
            }

            return action;
        }

        public async Task<ActionResult<GitHubRepositoryViewModel>> GetById(long id)
        {
            ActionResult<GitHubRepositoryViewModel> action = new ActionResult<GitHubRepositoryViewModel>();

            var api = await _gitHubApi.GetRepository("thomasravache");

            if (api.IsValid)
            {
                var apiResults = api.Results;

                List<GitHubRepositoryViewModel> results = Helper.GithubViewModelSerialize(apiResults);

                var findedRepo = results.Find(element => element.Id == id);

                action.IsValid = true;
                action.Message = api.Message;
                action.Result = findedRepo;
                action.Results = results;
            }
            else
            {
                action.IsValid = false;
                action.Message = api.Message;
            }

            return action;
        }

        public async Task<ActionResult<RepositoryViewModel>> GetByName(string name)
        {
            ActionResult<RepositoryViewModel> action = new ActionResult<RepositoryViewModel>();

            var api = await _gitHubApi.GetRepositoryByName(name);

            if (api.IsValid)
            {
                RepositoryViewModel result = new RepositoryViewModel
                {
                    Repositories = api.Result.Repositories,
                    TotalCount = api.Result.TotalCount,
                };

                action.IsValid = true;
                action.Message = api.Message;
                action.Result = result;
            }
            else
            {
                action.IsValid = false;
                action.Message = api.Message;
            }

            return action;
        }

        public ActionResult<FavoriteViewModel> GetFavoriteRepository()
        {
            ActionResult<FavoriteViewModel> action = new ActionResult<FavoriteViewModel>();
            List<Favorite> favorites = _context.GetAll();

            if (favorites == null)
            {
                action.IsValid = false;
                action.Message = "Nenhum repositório foi favoritado.";
            }
            else
            {
                List<FavoriteViewModel> results = Helper.FavoriteSerialize(favorites);

                action.IsValid = true;
                action.Message = "Carregamento OK!";
                action.Results = results;
            }

            return action;
        }

        public async Task<ActionResult<GitHubRepositoryViewModel>> GetRepository(string owner, string name)
        {
            ActionResult<GitHubRepositoryViewModel> action = new ActionResult<GitHubRepositoryViewModel>();

            var api = await _gitHubApi.GetRepositoryByOwnerAndName(owner, name);

            if (api.IsValid)
            {
                GitHubRepositoryViewModel result = new GitHubRepositoryViewModel
                {
                    Id = api.Result.Id,
                    Name = api.Result.Name,
                    FullName = api.Result.FullName,
                    Owner = api.Result.Owner,
                    Description = api.Result.Description,
                    Url = api.Result.Url,
                    UpdatedAt = api.Result.UpdatedAt,
                    Homepage = api.Result.Homepage,
                    Language = api.Result.Language,
                };
                
                action.IsValid = true;
                action.Message = api.Message;
                action.Result = result;
            }
            else
            {
                action.IsValid = false;
                action.Message = api.Message;
            }

            return action;
        }

        public ActionResult<FavoriteViewModel> SaveFavoriteRepository(FavoriteViewModel view)
        {
            ActionResult<FavoriteViewModel> action = new ActionResult<FavoriteViewModel>();

            Favorite favorite = new Favorite()
            {
                Id = view.Id,
                Description = view.Description,
                Language = view.Language,
                UpdateLast = view.UpdateLast,
                Owner = view.Owner,
                Name = view.Name,
            };

            bool alreadyExists = _context.ExistsByCheckAlready(favorite);

            if (alreadyExists)
            {
                action.IsValid = false;
                action.Message = "Este item já foi favoritado.";
            } 
            else
            {
                bool isInserted = _context.Insert(favorite);

                if (isInserted)
                {
                    action.IsValid = true;
                    action.Message = "Salvo com sucesso!";
                    action.Result = view;
                }
                else
                {
                    action.IsValid = false;
                    action.Message = "Algo deu errado.";
                }

            }

            return action;
        }
    }
}
