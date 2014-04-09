namespace MirGames.Domain.Topics.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the post new comment command.
    /// </summary>
    internal sealed class PostNewCommentCommandHandler : CommandHandler<PostNewCommentCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The text transform.
        /// </summary>
        private readonly ITextTransform textTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostNewCommentCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="textTransform">The text transform.</param>
        public PostNewCommentCommandHandler(IWriteContextFactory writeContextFactory, ICommandProcessor commandProcessor, ITextTransform textTransform)
        {
            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
            this.textTransform = textTransform;
        }

        /// <inheritdoc />
        public override int Execute(PostNewCommentCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            var comment = new Comment
            {
                UserId = principal.GetUserId().GetValueOrDefault(),
                UserLogin = principal.GetUserLogin(),
                UserIP = principal.GetHostAddress(),
                Text = this.textTransform.Transform(command.Text),
                SourceText = command.Text,
                Rating = 0,
                Date = DateTime.UtcNow
            };

            using (var writeContext = this.writeContextFactory.Create())
            {
                var topic = writeContext.Set<Topic>().SingleOrDefault(t => t.Id == command.TopicId);
                authorizationManager.EnsureAccess(principal, "Comment", topic);

                if (topic == null)
                {
                    throw new ItemNotFoundException("Topic", command.TopicId);
                }

                comment.TopicId = topic.Id;

                topic.CountComment = writeContext.Set<Comment>().Count(c => c.TopicId == topic.Id) + 1;
                writeContext.Set<Comment>().Add(comment);

                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new PublishAttachmentsCommand
                        {
                            EntityId = comment.CommentId,
                            Identifiers = command.Attachments
                        });
            }

            return comment.CommentId;
        }
    }
}