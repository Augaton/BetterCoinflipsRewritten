using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.API
{
    public static class RoomCache
    {
        private static readonly List<Room> Rooms = new List<Room>(64);

        public static int Count => Rooms.Count;

        public static void Rebuild()
        {
            Rooms.Clear();

            foreach (Room room in Room.List)
            {
                if (room is null)
                    continue;

                if (room.Type == RoomType.Unknown || room.Type == RoomType.Surface || room.Type == RoomType.Pocket)
                    continue;

                Rooms.Add(room);
            }
        }

        public static void Clear() => Rooms.Clear();

        public static Room PickRandom()
        {
            if (Rooms.Count == 0)
                Rebuild();

            return Rooms.Count == 0 ? null : Rooms[UnityEngine.Random.Range(0, Rooms.Count)];
        }
    }
}
