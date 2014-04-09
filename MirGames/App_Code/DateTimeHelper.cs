namespace System.Web.Mvc
{
    using System.Security.Claims;

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

            if (delta < 1 * Minute)
            {
                return ts.Seconds.Pluralize("�������", "�������", "������", format);
            }

            if (delta < 2 * Minute)
            {
                return "������ �����";
            }

            if (delta < 45 * Minute)
            {
                return ts.Minutes.Pluralize("������", "������", "�����", format);
            }

            if (delta < 90 * Minute)
            {
                return "��� �����";
            }

            if (delta < 24 * Hour)
            {
                return ts.Hours.Pluralize("���", "����", "�����", format);
            }

            if (delta < 48 * Hour)
            {
                return "�����";
            }

            if (delta < 30 * Day)
            {
                return ts.Days.Pluralize("����", "���", "����", format);
            }

            if (delta < 12 * Month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months.Pluralize("�����", "������", "�������", format);
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years.Pluralize("���", "����", "���", format);
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