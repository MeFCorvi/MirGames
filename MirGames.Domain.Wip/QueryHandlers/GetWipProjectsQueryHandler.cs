﻿namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Queries;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the WIP projects.
    /// </summary>
    internal sealed class GetWipProjectsQueryHandler : QueryHandler<GetWipProjectsQuery, WipProjectViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectsQueryHandler"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetWipProjectsQueryHandler(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetWipProjectsQuery query, ClaimsPrincipal principal)
        {
            return this.GetQuery(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<WipProjectViewModel> Execute(IReadContext readContext, GetWipProjectsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var projects =
                this.ApplyPagination(this.GetQuery(readContext, query).OrderByDescending(p => p.UpdatedDate), pagination)
                    .ToList()
                    .Select(
                        p => new WipProjectViewModel
                        {
                            CreationDate = p.CreationDate,
                            Author = new AuthorViewModel
                            {
                                Id = p.AuthorId
                            },
                            Alias = p.Alias,
                            Description = p.Description,
                            FollowersCount = p.FollowersCount,
                            ProjectId = p.ProjectId,
                            Title = p.Title,
                            UpdatedDate = p.UpdatedDate,
                            Version = p.Version,
                            Votes = p.Votes,
                            VotesCount = p.VotesCount,
                            Tags = p.TagsList.Split(',')
                        })
                    .ToList();

            projects.ForEach(project =>
            {
                var attachment = this.queryProcessor.Process(new GetAttachmentsQuery
                {
                    EntityId = project.ProjectId,
                    EntityType = "project-logo",
                    IsImage = true
                }).FirstOrDefault();

                if (attachment != null)
                {
                    project.LogoUrl = attachment.AttachmentUrl;
                }
            });

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = projects.Select(p => p.Author)
                    });

            return projects;
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The queryable results.</returns>
        private IQueryable<Project> GetQuery(IReadContext readContext, GetWipProjectsQuery query)
        {
            IQueryable<Project> projects = readContext.Query<Project>();

            if (!string.IsNullOrWhiteSpace(query.Tag))
            {
                var tags = readContext.Query<ProjectTag>().Where(t => t.TagText == query.Tag);
                projects = projects.Join(
                    tags,
                    project => project.ProjectId,
                    tag => tag.ProjectId,
                    (topic, tag) => topic);
            }

            return projects;
        }
    }
}