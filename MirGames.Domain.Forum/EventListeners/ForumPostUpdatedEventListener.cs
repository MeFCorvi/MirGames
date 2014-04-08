namespace MirGames.Domain.Forum.EventListeners
{
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic replied.
    /// </summary>
    internal sealed class ForumPostUpdatedEventListener : EventListenerBase<ForumPostUpdatedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostUpdatedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumPostUpdatedEventListener(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumPostUpdatedEvent @event)
        {
            this.commandProcessor.Execute(new ReindexForumTopicCommand { TopicId = @event.TopicId });
        }
    }
}