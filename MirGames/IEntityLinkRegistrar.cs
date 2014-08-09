﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IEntityLinkRegistrar.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames
{
    internal interface IEntityLinkRegistrar
    {
        /// <summary>
        /// Determines whether this instance can process the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>True whether entity of the specified type could be processed.</returns>
        bool CanProcess(string entityType);

        /// <summary>
        /// Gets the link.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>The link.</returns>
        string GetLink(int? entityId, string entityType);
    }
}