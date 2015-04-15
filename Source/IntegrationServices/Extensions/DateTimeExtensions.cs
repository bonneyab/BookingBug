using System;

namespace IntegrationServices.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToApiFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
