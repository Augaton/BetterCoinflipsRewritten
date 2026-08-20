using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class OneHpLeftEffect : ICoinEffect
    {
        public string Name
        {
            get { return "OneHpLeft"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .OneHpLeftEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            return player != null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   player.Health > 1f;
        }

        public void Execute(Player player)
        {
            player.Health = 1f;
        }
    }
}