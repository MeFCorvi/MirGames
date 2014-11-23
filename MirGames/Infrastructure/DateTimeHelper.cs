// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DateTimeHelper.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames
{
    using System;
    using System.Security.Claims;
    using System.Web.Mvc;

    using MirGames.Domain.Security;

    /// <summary>
    /// Date Time helper.
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Returns the relative date.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="date">The date.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// The string representation of the relative date.
        /// </returns>
        public static string RelativeDate(this HtmlHelper helper, DateTime date, string format = "{0} {1} �����")
        {
            const int Second = 1;
            const int Minute = 60 * Second;
            const int Hour = 60 * Minute;
            const int Day = 24 * Hour;
            const int Month = 30 * Day;

            var ts = DateTime.UtcNow - date;
            var delta = ts.TotalSeconds;

            if (delta < 0)
            {
                return "����� ������";
            }

            if (delta < 45 * Second)
            {
                return string.Format(format, "���������", "������");
            }

            if (delta < 90 * Second)
            {
                return string.Format(format, string.Empty, "������");
            }

            if (delta < 45 * Minute)
            {
                return Math.Max(ts.Minutes, 2).Pluralize("������", "������", "�����", format);
            }

            if (delta < 90 * Minute)
            {
                return "��� �����";
            }

            if (delta < 22 * Hour)
            {
                return Math.Max(ts.Hours, 2).Pluralize("���", "����", "�����", format);
            }

            if (delta < 36 * Hour)
            {
                return "���� �����";
            }

            if (delta < 25 * Day)
            {
                return ts.Days.Pluralize("����", "���", "����", format);
            }

            return date.Format();
        }

        /// <summary>
        /// Converts the UTC to the user's time-zone.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The users date.</returns>
        public static DateTime UserDate(this DateTime date)
        {
            var timeZone = ClaimsPrincipal.Current.GetTimeZone() ?? TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(date, timeZone);
        }

        /// <summary>
        /// Converts the UTC to the user's time-zone.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The users date.</returns>
        public static string Format(this DateTime date)
        {
            return string.Format("{0:dd.MM.yy HH:mm}", date.UserDate());
        }
    }
}