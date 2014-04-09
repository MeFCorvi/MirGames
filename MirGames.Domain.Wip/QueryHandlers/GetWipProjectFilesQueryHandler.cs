namespace MirGames.Domain.Wip.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Queries;
    using MirGames.Services.Git.Public.ViewModels;

    internal sealed class GetWipProjectFilesQueryHandler : QueryHandler<GetWipProjectFilesQuery, WipProjectRepositoryItemViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectCommitsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetWipProjectFilesQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);

            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override IEnumerable<WipProjectRepositoryItemViewModel> Execute(
            IReadContext readContext,
            GetWipProjectFilesQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var project = GetProject(readContext, query);

            if (!project.RepositoryId.HasValue || string.IsNullOrEmpty(project.RepositoryType))
            {
                return Enumerable.Empty<WipProjectRepositoryItemViewModel>();
            }

            switch (project.RepositoryType)
            {
                case "git":
                    return this.queryProcessor
                               .Process(new GetRepositoryFilesQuery
                               {
                                   RepositoryId = project.RepositoryId.GetValueOrDefault(),
                                   RelativePath = query.RelativePath
                               })
                               .Select(h => new WipProjectRepositoryItemViewModel
                               {
                                   Name = h.Name,
                                   Path = h.Path,
                                   ItemType = GetGitItemType(h.ItemType)
                               });
                default:
                    throw new IndexOutOfRangeException(string.Format("{0} is not supported type of repositories.", project.RepositoryType));
            }
        }

        /// <inheritdoc />
        protected override int GetItemsCount(
            IReadContext readContext,
            GetWipProjectFilesQuery query,
            ClaimsPrincipal principal)
        {
            var project = GetProject(readContext, query);

            if (!project.RepositoryId.HasValue || string.IsNullOrEmpty(project.RepositoryType))
            {
                return 0;
            }

            switch (project.RepositoryType)
            {
                case "git":
                    return this.queryProcessor
                               .GetItemsCount(new GetRepositoryFilesQuery
                               {
                                   RepositoryId = project.RepositoryId.GetValueOrDefault(),
                                   RelativePath = query.RelativePath
                               });
                default:
                    throw new IndexOutOfRangeException(string.Format("{0} is not supported type of repositories.", project.RepositoryType));
            }
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The project.</returns>
        private static Project GetProject(IReadContext readContext, GetWipProjectFilesQuery query)
        {
            var project = readContext.Query<Project>().SingleOrDefault(p => p.Alias == query.Alias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", query.Alias);
            }

            return project;
        }

        private static WipProjectRepositoryItemType GetGitItemType(GitRepositoryFileItemType itemType)
        {
            switch (itemType)
            {
                case GitRepositoryFileItemType.File:
                    return WipProjectRepositoryItemType.File;
                case GitRepositoryFileItemType.Directory:
                    return WipProjectRepositoryItemType.Directory;
                case GitRepositoryFileItemType.Link:
                    return WipProjectRepositoryItemType.Other;
                default:
                    throw new ArgumentOutOfRangeException("itemType");
            }
        }
    }
}