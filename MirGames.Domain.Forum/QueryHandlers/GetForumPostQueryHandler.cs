namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the get forum topic query.
    /// </summary>
    public class GetForumPostQueryHandler : SingleItemQueryHandler<GetForumPostQuery, ForumPostsListItemViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumPostQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetForumPostQueryHandler(IQueryProcessor queryProcessor, IAuthorizationManager authorizationManager)
        {
            this.queryProcessor = queryProcessor;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public override ForumPostsListItemViewModel Execute(IReadContext readContext, GetForumPostQuery query, ClaimsPrincipal principal)
        {
            var post = readContext.Query<ForumPost>()
                .SingleOrDefault(t => t.PostId == query.PostId);

            if (post == null)
            {
                return null;
            }

            var author = this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = new[] { new AuthorViewModel { Login = post.AuthorLogin, Id = post.AuthorId } }
                    }).Single();

            var postIndex = readContext
                .Query<ForumPost>()
                .Count(p => p.TopicId == post.TopicId && p.PostId < post.PostId) + 1;

            return new ForumPostsListItemViewModel
            {
                Author = author,
                AuthorIP = post.AuthorIP,
                CreatedDate = post.CreatedDate,
                TopicId = post.TopicId,
                UpdatedDate = post.UpdatedDate,
                IsHidden = post.IsHidden,
                PostId = post.PostId,
                Text = post.Text,
                Index = postIndex,
                IsRead = true,
                FirstUnread = false,
                CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", post) && postIndex > 1,
                CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", post)
            };
        }
    }
}