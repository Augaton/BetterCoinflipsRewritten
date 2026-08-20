namespace BetterCoinflipsRewritten.API
{
    public static class Rng
    {
        public static int Next(int minInclusive, int maxExclusive)
        {
            return UnityEngine.Random.Range(minInclusive, maxExclusive);
        }

        public static int NextWeight(int totalWeight)
        {
            return UnityEngine.Random.Range(0, totalWeight) + 1;
        }
    }
}
