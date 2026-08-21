using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class TantrumTrapEffect : ICoinEffect
    {
        public string Name
        {
            get { return "TantrumTrap"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .TantrumTrapEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            return player is not null &&
                   player.IsConnected &&
                   player.IsAlive;
        }

        public void Execute(Player player)
        {
            player.PlaceTantrum(true);
        }
    }
}
