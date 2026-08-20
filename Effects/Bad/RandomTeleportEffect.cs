using BetterCoinflipsRewritten.API;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class RandomTeleportEffect : ICoinEffect
    {
        private string selectedRoomName;

        public RandomTeleportEffect()
        {
            selectedRoomName =
                Plugin.Instance.Translation.UnknownRoomName;
        }

        public string Name
        {
            get { return "RandomTeleport"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .RandomTeleportEffectMessage
                    .Replace(
                        "{room}",
                        selectedRoomName);
            }
        }

        public bool CanApply(Player player)
        {
            return player != null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   RoomCache.Count > 0;
        }

        public void Execute(Player player)
        {
            Room selectedRoom = RoomCache.PickRandom();

            if (selectedRoom == null)
            {
                selectedRoomName =
                    Plugin.Instance.Translation.UnknownRoomName;
                return;
            }

            selectedRoomName = selectedRoom.Type.ToString();
            player.Teleport(selectedRoom);
        }
    }
}
