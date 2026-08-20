using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class LightbulbEffect : ICoinEffect
    {
        public string Name
        {
            get { return "SCP2176"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .LightbulbEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            return player != null &&
                   player.IsConnected &&
                   player.IsAlive;
        }

        public void Execute(Player player)
        {
            player.AddItem(ItemType.SCP2176);
        }
    }
}