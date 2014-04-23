﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SaveWipProjectCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    internal sealed class SaveWipProjectCommandHandler : CommandHandler<SaveWipProjectCommand>
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
        /// Initializes a new instance of the <see cref="SaveWipProjectCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public SaveWipProjectCommandHandler(
            IWriteContextFactory writeContextFactory,
            ICommandProcessor commandProcessor)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(commandProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Execute(
            SaveWipProjectCommand command,
            ClaimsPrincipal principal,
            IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            Contract.Requires(command.Alias != null);

            command.Alias = command.Alias.ToLowerInvariant();
            Project project;

            using (var writeContext = this.writeContextFactory.Create())
            {
                project = writeContext.Set<Project>().FirstOrDefault(p => p.Alias == command.Alias);

                if (project == null)
                {
                    throw new ItemNotFoundException("Project", command.Alias);
                }

                authorizationManager.EnsureAccess(principal, "Edit", project);

                project.Title = command.Title;
                project.Description = command.Description;
                project.TagsList = command.Tags;
                project.UpdatedDate = DateTime.UtcNow;

                writeContext.SaveChanges();

                var oldTags = writeContext.Set<ProjectTag>().Where(p => p.ProjectId == project.ProjectId);
                writeContext.Set<ProjectTag>().RemoveRange(oldTags);

                foreach (var tag in command.Tags.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    writeContext.Set<ProjectTag>().Add(new ProjectTag { TagText = tag.Trim(), ProjectId = project.ProjectId });
                }

                writeContext.SaveChanges();
            }

            if (command.LogoAttachmentId.HasValue)
            {
                this.commandProcessor.Execute(new RemoveAttachmentsCommand
                {
                    EntityId = project.ProjectId,
                    EntityType = "project-logo"
                });

                this.commandProcessor.Execute(new PublishAttachmentsCommand
                {
                    EntityId = project.ProjectId,
                    Identifiers = new[] { command.LogoAttachmentId.Value }
                });
            }

            this.commandProcessor.Execute(new PublishAttachmentsCommand
            {
                EntityId = project.ProjectId,
                Identifiers = command.Attachments
            });
        }
    }
}