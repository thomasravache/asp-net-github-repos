using Newtonsoft.Json;
using RepositorioGitHub.Dominio;
using RepositorioGitHub.Dominio.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioGitHub.Infra.ApiGitHub
{
    public class GitHubApi : IGitHubApi
    {
        private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("https://api.github.com") };

        public async Task<ActionResult<GitHubRepository>> GetRepositoryByOwnerAndName(string owner, string name)
        {
            _client.DefaultRequestHeaders.Add("User-Agent", "request");

            string endpoint = "repos/" + owner + "/" + name;
            var response = await _client.GetAsync(endpoint);
            ActionResult<GitHubRepository> action = new ActionResult<GitHubRepository>();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var repo = JsonConvert.DeserializeObject<GitHubRepository>(content);

                action.IsValid = true;
                action.Message = "Carregamento OK!";
                action.Result = repo;
            } 
            else
            {
                action.IsValid = false;
                action.Message = "Algo deu errado.";
            }

            return action;
        }
        public async Task<ActionResult<GitHubRepository>> GetRepository(string owner)
        {
            _client.DefaultRequestHeaders.Add("User-Agent", "request");

            string endpoint = "users/" + owner + "/repos";
            var response = await _client.GetAsync(endpoint);
            ActionResult<GitHubRepository> action = new ActionResult<GitHubRepository>();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var repos = JsonConvert.DeserializeObject<GitHubRepository[]>(content);

                action.IsValid = true;
                action.Message = "Carregamento OK!";
                action.Result = repos[0];
                action.Results = repos;
            } 
            else
            {
                action.IsValid = false;
                action.Message = "Algo deu errado.";
            }

            return action;
        }

        public async Task<ActionResult<RepositoryModel>> GetRepositoryByName(string name)
        {
            _client.DefaultRequestHeaders.Add("User-Agent", "request");

            string endpoint = "search/repositories?q=" + name;
            var response = await _client.GetAsync(endpoint);
            ActionResult<RepositoryModel> action = new ActionResult<RepositoryModel>();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var reposResult = JsonConvert.DeserializeObject<RepositoryModel>(content);
                // RepositoryModel[] results = { reposResult };
                

                action.IsValid = true;
                action.Message = "Carregamento OK!";
                action.Result = reposResult;
                // action.Results = results;
            }
            else
            {
                action.IsValid = false;
                action.Message = "Algo deu errado.";
            }

            return action;
        }
    }
}
