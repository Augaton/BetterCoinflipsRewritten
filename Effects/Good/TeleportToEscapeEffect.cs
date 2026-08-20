using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class TeleportToEscapeEffect : ICoinEffect
    {
        public string Name
        {
            get { return "TeleportToEscape"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .TeleportToEscapeEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            if (player == null ||
                !player.IsConnected ||
                !player.IsAlive)
            {
                return false;
            }

            return Door.Get(
                DoorType.EscapeSecondary) != null;
        }

        public void Execute(Player player)
        {
            Door escapeDoor =
                Door.Get(DoorType.EscapeSecondary);

            if (escapeDoor == null)
                return;

            player.Teleport(escapeDoor);
        }
    }
}