using System.Collections.Generic;
using RepositorioGitHub.Dominio;

namespace RepositorioGitHub.Business
{
    internal class Helper
    {
		public static List<GitHubRepositoryViewModel> GithubViewModelSerialize(IList<GitHubRepository> apiResults)
        {
            List<GitHubRepositoryViewModel> results = new List<GitHubRepositoryViewModel>();
            foreach (var item in apiResults)
            {
                GitHubRepositoryViewModel repoView = new GitHubRepositoryViewModel
                {
                    // repoView.<...> = item.<...>
                    Id = item.Id,
                    Name = item.Name,
                    FullName = item.FullName,
                    Owner = item.Owner,
                    Description = item.Description,
                    Url = item.Url,
                    UpdatedAt = item.UpdatedAt,
                    Homepage = item.Homepage,
                    Language = item.Language
                };

                results.Add(repoView);
            }

            return results;
        }

        public static List<FavoriteViewModel> FavoriteSerialize(List<Favorite> favorites)
        {
            List<FavoriteViewModel> results = new List<FavoriteViewModel>();

            foreach (Favorite favorite in favorites)
            {
                FavoriteViewModel fav = new FavoriteViewModel
                {
                    Id = favorite.Id,
                    Description = favorite.Description,
                    Language = favorite.Language,
                    UpdateLast = favorite.UpdateLast,
                    Owner = favorite.Owner,
                    Name = favorite.Name,
                };

                results.Add(fav);
            }

            return results;
        }
	}
}
