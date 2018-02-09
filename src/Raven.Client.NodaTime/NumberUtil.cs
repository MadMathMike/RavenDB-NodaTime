using System.Globalization;

namespace Raven.Client.NodaTime
{
    internal class NumberUtil
    {

        internal static string NumberToString(long number)
        {
            // Lifted from: https://searchcode.com/codesearch/view/2445187/
            // Not really sure if this is the right thing to do.
            return "Lx" + number.ToString("G", CultureInfo.InvariantCulture);
        }
    }
}