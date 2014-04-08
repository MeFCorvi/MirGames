namespace MirGames.Domain.Forum.Entities
{
    /// <summary>
    /// Link between topics and tags.
    /// </summary>
    internal sealed class ForumTag
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the topic id.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the tag text.
        /// </summary>
        public string TagText { get; set; }
    }
}
