using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace JetstreamSkiserviceAPI.Data
{
    public static class DateTimeHelper
    {
        public static DateTime ConvertFromIsoDate(string isoDateString)
        {
            if (DateTime.TryParse(isoDateString, out DateTime dateTime))
            {
                // Konvertiere das Datum in UTC, falls erforderlich
                return dateTime.ToUniversalTime();
            }
            // Standardwert oder Fehlerbehandlung, falls das Parsing fehlschlägt
            return DateTime.UtcNow;
        }

        public static string ConvertToIsoDate(DateTime dateTime)
        {
            // Konvertiere das DateTime-Objekt in einen ISO 8601 String in UTC
            return dateTime.ToUniversalTime().ToString("o");
        }
    }
}
