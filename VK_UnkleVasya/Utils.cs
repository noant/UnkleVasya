using System;
using System.Linq;
using System.Windows.Input;
using HierarchicalData;

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
    }
}
