namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetMainTagsQuery.
    /// </summary>
    internal sealed class GetForumTagsQueryHandler : QueryHandler<GetForumTagsQuery, string>
    {
        /// <inheritdoc />
        protected override IEnumerable<string> Execute(IReadContext readContext, GetForumTagsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            return this.GetTagsQuery(readContext).Take(20).OrderBy(t => t);
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetForumTagsQuery query, ClaimsPrincipal principal)
        {
            return this.GetTagsQuery(readContext).Count();
        }

        /// <summary>
        /// Gets the tags query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <returns>
        /// The tags query.
        /// </returns>
        private IQueryable<string> GetTagsQuery(IReadContext readContext)
        {
            return readContext
                .Query<ForumTag>()
                .GroupBy(t => t.TagText, (tag, rows) => new { Tag = tag, Count = rows.Count() })
                .OrderByDescending(t => t.Count)
                .Select(t => t.Tag);
        }
    }
}