using Exiled.API.Enums;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class TeleportToClassDCellsEffect : ICoinEffect
    {
        public string Name
        {
            get { return "TeleportToClassDCells"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .TeleportToClassDCellsEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            return player != null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   Room.Get(RoomType.LczClassDSpawn) != null;
        }

        public void Execute(Player player)
        {
            Room classDSpawn =
                Room.Get(RoomType.LczClassDSpawn);

            if (classDSpawn == null)
                return;

            player.Teleport(classDSpawn);
        }
    }
}