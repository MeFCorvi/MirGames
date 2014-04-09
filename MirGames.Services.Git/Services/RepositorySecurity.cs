namespace MirGames.Services.Git.Services
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Services.Git.Entities;

    internal sealed class RepositorySecurity : IRepositorySecurity
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> principalProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositorySecurity"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="principalProvider">The principal provider.</param>
        public RepositorySecurity(IReadContextFactory readContextFactory, Func<ClaimsPrincipal> principalProvider)
        {
            Contract.Requires(principalProvider != null);
            Contract.Requires(readContextFactory != null);

            this.readContextFactory = readContextFactory;
            this.principalProvider = principalProvider;
        }

        /// <inheritdoc />
        public bool CanWrite(string repositoryName)
        {
            var repositoryAccess = this.GetRepositoryAccess(repositoryName);
            
            return repositoryAccess != null &&
                   (repositoryAccess.AccessLevel == RepositoryAccessLevel.Contributor
                    || repositoryAccess.AccessLevel == RepositoryAccessLevel.Owner);
        }

        /// <inheritdoc />
        public bool CanRead(string repositoryName)
        {
            var repositoryAccess = this.GetRepositoryAccess(repositoryName);

            return repositoryAccess != null &&
                   (repositoryAccess.AccessLevel == RepositoryAccessLevel.Contributor
                    || repositoryAccess.AccessLevel == RepositoryAccessLevel.Owner
                    || repositoryAccess.AccessLevel == RepositoryAccessLevel.Reader);
        }

        /// <inheritdoc />
        public bool CanDelete(string repositoryName)
        {
            var repositoryAccess = this.GetRepositoryAccess(repositoryName);

            return repositoryAccess != null && repositoryAccess.AccessLevel == RepositoryAccessLevel.Owner;
        }

        /// <summary>
        /// Gets the repository access.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>The repository access.</returns>
        /// <exception cref="System.Exception">Repository have not been found</exception>
        private RepositoryAccess GetRepositoryAccess(string repositoryName)
        {
            var principal = this.principalProvider.Invoke();

            using (var readContext = this.readContextFactory.Create())
            {
                var repository = readContext.Query<Repository>().FirstOrDefault(r => r.Name == repositoryName);

                if (repository == null)
                {
                    throw new Exception("Repository have not been found");
                }

                int? userId = principal != null ? principal.GetUserId() : null;

                return readContext
                    .Query<RepositoryAccess>()
                    .Where(
                        a =>
                        a.RepositoryId == repository.Id && (a.UserId == userId || a.UserId == null))
                    .OrderByDescending(a => a.UserId)
                    .FirstOrDefault();
            }
        }
    }
}