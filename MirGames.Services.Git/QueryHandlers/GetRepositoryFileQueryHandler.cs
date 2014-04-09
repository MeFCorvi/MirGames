namespace MirGames.Services.Git.QueryHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using LibGit2Sharp;

    using MirGames.Domain.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Exceptions;
    using MirGames.Services.Git.Public.Queries;
    using MirGames.Services.Git.Public.ViewModels;
    using MirGames.Services.Git.Services;

    internal sealed class GetRepositoryFileQueryHandler : SingleItemQueryHandler<GetRepositoryFileQuery, GitRepositoryFileViewModel>
    {
        /// <summary>
        /// The repository path provider.
        /// </summary>
        private readonly IRepositoryPathProvider repositoryPathProvider;

        /// <summary>
        /// The repository security.
        /// </summary>
        private readonly IRepositorySecurity repositorySecurity;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRepositoryFileQueryHandler" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="repositorySecurity">The repository security.</param>
        public GetRepositoryFileQueryHandler(IRepositoryPathProvider repositoryPathProvider, IRepositorySecurity repositorySecurity)
        {
            Contract.Requires(repositoryPathProvider != null);
            Contract.Requires(repositorySecurity != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.repositorySecurity = repositorySecurity;
        }

        /// <inheritdoc />
        public override GitRepositoryFileViewModel Execute(IReadContext readContext, GetRepositoryFileQuery query, ClaimsPrincipal principal)
        {
            Contract.Requires(query != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(query.FilePath));
            
            var repository = GetRepository(readContext, query);

            if (!this.repositorySecurity.CanRead(repository.Name))
            {
                throw new UnauthorizedAccessException(string.Format("User have no rights to read repository {0}", repository.Name));
            }

            var repositoryPath = this.repositoryPathProvider.GetPath(repository.Name);
            var gitRepository = new Repository(repositoryPath);

            var treeEntry = gitRepository.Head.Tip[query.FilePath];

            if (treeEntry == null)
            {
                throw new RepositoryPathNotFoundException(query.FilePath);
            }

            if (treeEntry.TargetType == TreeEntryTargetType.Blob)
            {
                var blob = (Blob)treeEntry.Target;

                return new GitRepositoryFileViewModel
                {
                    FileName = treeEntry.Name,
                    Content = blob.GetContentText(),
                    UpdatedDate = DateTime.Now
                };
            }

            return null;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The repository.</returns>
        private static Entities.Repository GetRepository(IReadContext readContext, GetRepositoryFileQuery query)
        {
            var repository = readContext.Query<Entities.Repository>().SingleOrDefault(r => r.Id == query.RepositoryId);

            if (repository == null)
            {
                throw new ItemNotFoundException("GitRepository", query.RepositoryId);
            }

            return repository;
        }
    }
}