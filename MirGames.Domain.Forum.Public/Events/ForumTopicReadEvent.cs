namespace MirGames.Domain.Forum.Events
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Raised when some of topics has been read.
    /// </summary>
    public class ForumTopicReadEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public IEnumerable<int> UserIdentifiers { get; set; }

        /// <summary>
        /// Gets or sets the excluded users.
        /// </summary>
        public IEnumerable<int> ExcludedUsers { get; set; } 

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        protected override string EventType
        {
            get { return "Forum.ForumTopicRead"; }
        }
    }
}