using System.Globalization;

namespace _game.Scripts.Utility
{
    public static class NumberConverter
    {
        private static string _textAmount;

        public static string ConvertToFormat(long amount)
        {
            if (amount == 0)
            {
                return "0";
            }
            _textAmount = amount.ToString(CultureInfo.InvariantCulture);
            if (amount >= 1000000000000) _textAmount = (amount / 1000000000000).ToString("#") + "T";
            if (amount >= 1000000000) _textAmount = (amount / 1000000000).ToString("#") + "B";
            else if (amount >= 1000000) _textAmount = (amount / 1000000).ToString("#") + "M";
            else if (amount >= 1000) _textAmount = (amount / 1000).ToString("#") + "K";
            else _textAmount = amount.ToString();

            return _textAmount;
        }
        public static string ConvertToFloatFormat(long amount)
        {
            if (amount == 0)
            {
                return "0";
            }
            _textAmount = amount.ToString(CultureInfo.InvariantCulture);
            if (amount >= 1000000000000) _textAmount = (amount / 1000000000000).ToString("#,##") + "T";
            if (amount >= 1000000000) _textAmount = (amount / 1000000000).ToString("#,##") + "B";
            else if (amount >= 1000000) _textAmount = (amount / 1000000).ToString("#,##") + "M";
            else if (amount >= 1000) _textAmount = (amount / 1000).ToString("#,##") + "K";
            else _textAmount = amount.ToString();

            return _textAmount;
        }
    }
}