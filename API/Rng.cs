using System;

namespace BetterCoinflipsRewritten.API
{
    public static class Rng
    {
        private static readonly Random Shared = new Random();

        public static int Next(int minInclusive, int maxExclusive)
        {
            return Shared.Next(minInclusive, maxExclusive);
        }

        public static int NextWeight(int totalWeight)
        {
            return Shared.Next(1, totalWeight + 1);
        }
    }
}
