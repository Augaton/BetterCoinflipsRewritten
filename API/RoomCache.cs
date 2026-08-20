using ZoneShilari.Common.Caching;

namespace BetterCoinflipsRewritten.API
{
    public static class RoomCache
    {
        private static readonly ZoneShilari.Common.Caching.RoomCache Cache = new ZoneShilari.Common.Caching.RoomCache();

        public static int Count => Cache.Count;

        public static void Rebuild() => Cache.Rebuild();

        public static void Clear() => Cache.Clear();

        public static Exiled.API.Features.Room PickRandom() => Cache.PickRandom();
    }
}
