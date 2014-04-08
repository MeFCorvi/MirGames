﻿namespace MirGames.Domain.Attachments.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Entities;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to view event log.
    /// </summary>
    internal sealed class AttachmentPublishAccessRule : AccessRule<Attachment>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Publish"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, Attachment resource)
        {
            return principal.IsInRole("User") && resource.UserId == principal.GetUserId() && !resource.IsPublished;
        }
    }
}
