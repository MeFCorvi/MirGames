﻿namespace MirGames.Services.Git.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;
    using MirGames.Infrastructure.Transactions;
    using MirGames.Services.Git.Entities;
    using MirGames.Services.Git.Public.Commands;
    using MirGames.Services.Git.Public.Exceptions;
    using MirGames.Services.Git.Services;

    using Repository = LibGit2Sharp.Repository;

    internal sealed class InitRepositoryCommandHandler : CommandHandler<InitRepositoryCommand, int>
    {
        /// <summary>
        /// The repository path provider.
        /// </summary>
        private readonly IRepositoryPathProvider repositoryPathProvider;

        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The transaction executor.
        /// </summary>
        private readonly ITransactionExecutor transactionExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitRepositoryCommandHandler" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="transactionExecutor">The transaction executor.</param>
        public InitRepositoryCommandHandler(
            IRepositoryPathProvider repositoryPathProvider,
            IWriteContextFactory writeContextFactory,
            ITransactionExecutor transactionExecutor)
        {
            Contract.Requires(repositoryPathProvider != null);
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(transactionExecutor != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.writeContextFactory = writeContextFactory;
            this.transactionExecutor = transactionExecutor;
        }

        /// <inheritdoc />
        public override int Execute(InitRepositoryCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            int userId = principal.GetUserId().GetValueOrDefault();
            string repositoryName = command.RepositoryName.ToLowerInvariant();
            string path = this.repositoryPathProvider.GetPath(repositoryName);

            if (Directory.Exists(path))
            {
                throw new RepositoryAlreadyExistException(repositoryName);
            }

            var newRepository = new Entities.Repository
            {
                Name = repositoryName,
                Title = command.Title
            };

            authorizationManager.EnsureAccess(principal, "Create", newRepository);

            using (var writeContext = this.writeContextFactory.Create())
            {
                var repository =
                    writeContext.Set<Entities.Repository>().FirstOrDefault(r => r.Name == repositoryName);

                if (repository != null)
                {
                    throw new RepositoryAlreadyExistException(repositoryName);
                }

                writeContext.Set<Entities.Repository>().Add(newRepository);
                writeContext.SaveChanges();

                var repositoryAccess = new RepositoryAccess
                {
                    AccessLevel = RepositoryAccessLevel.Owner,
                    RepositoryId = newRepository.Id,
                    UserId = userId
                };

                writeContext.Set<RepositoryAccess>().Add(repositoryAccess);
                writeContext.SaveChanges();

                this.transactionExecutor.Execute(
                    () => Repository.Init(path, true),
                    () =>
                    {
                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                        }
                    });

                writeContext.SaveChanges();

                return newRepository.Id;
            }
        }
    }
}