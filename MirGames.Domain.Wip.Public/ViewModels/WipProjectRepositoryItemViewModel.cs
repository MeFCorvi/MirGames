namespace MirGames.Domain.Wip.ViewModels
{
    using System;

    public class WipProjectRepositoryItemViewModel
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the commit identifier.
        /// </summary>
        public string CommitId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public WipProjectRepositoryItemType ItemType { get; set; }
    }
}