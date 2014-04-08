namespace MirGames.Domain.Topics.Queries
{
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns topic by identifier.
    /// </summary>
    public sealed class GetTopicQuery : SingleItemQuery<TopicViewModel>
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }
    }
}