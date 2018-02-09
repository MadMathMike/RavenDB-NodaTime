using NodaTime;
using NodaTime.Text;
namespace Raven.Client.NodaTime
{
    internal class CustomQueryValueConverters
    {
        public static bool InstantConverter(string name, Instant value, bool forRange, out string strValue)
        {
            NodaUtil.Instant.Validate(value);

            strValue = value.ToString(NodaUtil.Instant.FullIsoPattern.PatternText, null);

            return true;
        }

        public static bool LocalDateTimeConverter(string name, LocalDateTime value, bool forRange, out string strValue)
        {
            strValue = value.ToString(NodaUtil.LocalDateTime.FullIsoPattern.PatternText, null);

            return true;
        }

        public static bool LocalDateConverter(string name, LocalDate value, bool forRange, out string strValue)
        {
            strValue = value.ToString(LocalDatePattern.Iso.PatternText, null);

            return true;
        }


        public static bool LocalTimeConverter(string name, LocalTime value, bool forRange, out string strValue)
        {
            if (forRange)
            {
                strValue = NumberUtil.NumberToString(value.TickOfDay);

                return true;
            }

            var timeSpan = value.ToTimeSpan();

            strValue = "\"" + timeSpan.ToString("c") + "\"";

            return true;
        }

        public static bool OffsetConverter(string name, Offset value, bool forRange, out string strValue)
        {
            if (forRange)
            {
                strValue = NumberUtil.NumberToString(value.Ticks);

                return true;
            }

            var timeSpan = value.ToTimeSpan();

            strValue = "\"" + timeSpan.ToString("c") + "\"";

            return true;
        }

        public static bool DurationConverter(string name, Duration value, bool forRange, out string strValue)
        {
            if (forRange)
            {
                strValue = NumberUtil.NumberToString(value.BclCompatibleTicks);

                return true;
            }

            var timeSpan = value.ToTimeSpan();

            strValue = "\"" + timeSpan.ToString("c") + "\"";

            return true;
        }

        public static bool OffsetDateTimeConverter(string name, OffsetDateTime value, bool forRange, out string strValue)
        {
            var instant = value.ToInstant();
            NodaUtil.Instant.Validate(instant);

            strValue = instant.ToString(NodaUtil.Instant.FullIsoPattern.PatternText, null);

            return true;
        }

        public static bool ZonedDateTimeConverter(string fieldname, ZonedDateTime value, bool forRange, out string strValue)
        {
            var instant = value.ToInstant();
            NodaUtil.Instant.Validate(instant);

            strValue = instant.ToString(NodaUtil.Instant.FullIsoPattern.PatternText, null);

            return true;
        }

        public static bool PeriodConverter(string name, Period value, bool forRange, out string strValue)
        {
            strValue = value.ToString();

            return true;
        }
    }
}
