using System;
using System.Linq;
using System.Windows.Input;
using HierarchicalData;
using System.Globalization;

namespace VK_UnkleVasya
{
    public static class Utils
    {
        private static Random _randomizer;
        public static int GetNextRandom(int from, int to)
        {
            if (_randomizer == null)
                _randomizer = new Random();
            return _randomizer.Next(from, to);
        }

        public static string[] LoadStringData(string file)
        {
            var hData = HierarchicalObject.FromFile(file);
            return hData.Values.Cast<string>().ToArray();
        }

        public static decimal? ConvertToDecimal(string target)
        {
            if (target.Trim().Length != target.Length) return null;
            decimal value;
            if (decimal.TryParse(target.Replace(",","."), NumberStyles.Number, NumberFormatInfo.InvariantInfo, out value))
                return value;
            return null;
        }

        public static string GetValueBetween(string targetString, string startStr, string endStr)
        {
            var indexOfStart = targetString.IndexOf(startStr) + startStr.Length;
            var indexOfEnd = targetString.IndexOf(endStr);
            if (indexOfStart > 0 && indexOfEnd > 0 && indexOfStart < indexOfEnd)
                return targetString.Substring(indexOfStart, indexOfEnd - indexOfStart);
            return string.Empty;
        }
    }
}
