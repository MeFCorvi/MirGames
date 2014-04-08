namespace MirGames.Domain.Forum.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class PostNewForumTopicCommandHandler : CommandHandler<PostNewForumTopicCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The topics text transform.
        /// </summary>
        private readonly ITextTransform textTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostNewForumTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="textTransform">The text transform.</param>
        public PostNewForumTopicCommandHandler(
            IWriteContextFactory writeContextFactory,
            IEventBus eventBus,
            IQueryProcessor queryProcessor,
            ICommandProcessor commandProcessor,
            ITextTransform textTransform)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventBus != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(commandProcessor != null);
            Contract.Requires(textTransform != null);

            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
            this.textTransform = textTransform;
        }

        /// <inheritdoc />
        public override int Execute(PostNewForumTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            int authorId = principal.GetUserId().GetValueOrDefault();
            var author = this.queryProcessor.Process(new GetAuthorQuery { UserId = authorId });

            var topic = new ForumTopic
            {
                AuthorId = authorId,
                AuthorLogin = author.Login,
                AuthorIp = principal.GetHostAddress(),
                TagsList = command.Tags,
                CreatedDate = DateTime.UtcNow,
                LastPostAuthorId = authorId,
                LastPostAuthorLogin = author.Login,
                Title = command.Title,
                UpdatedDate = DateTime.UtcNow,
                PostsCount = 1
            };

            authorizationManager.EnsureAccess(principal, "Create", topic);

            var post = new ForumPost
                {
                    AuthorId = author.Id,
                    AuthorLogin = author.Login,
                    AuthorIP = principal.GetHostAddress(),
                    CreatedDate = DateTime.UtcNow,
                    SourceText = command.Text,
                    Text = this.textTransform.Transform(command.Text),
                    Topic = topic,
                    IsStartPost = true,
                    UpdatedDate = DateTime.UtcNow
                };

            using (var writeContext = this.writeContextFactory.Create())
            {
                writeContext.Set<ForumTopic>().Add(topic);
                writeContext.SaveChanges();

                writeContext.Set<ForumPost>().Add(post);
                writeContext.SaveChanges();

                foreach (var tag in command.Tags.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    writeContext.Set<ForumTag>().Add(new ForumTag { TagText = tag.Trim(), TopicId = topic.TopicId });
                }

                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new PublishAttachmentsCommand
                        {
                            EntityId = post.PostId,
                            Identifiers = command.Attachments
                        });
            }

            this.eventBus.Raise(new ForumTopicCreatedEvent { TopicId = topic.TopicId, AuthorId = author.Id, CreationDate = topic.CreatedDate });

            return topic.TopicId;
        }
    }
}