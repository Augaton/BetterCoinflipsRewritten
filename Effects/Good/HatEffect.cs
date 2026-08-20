using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class HatEffect : ICoinEffect
    {
        public string Name
        {
            get { return "SCP268"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .HatEffectMessage;
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
            player.AddItem(ItemType.SCP268);
        }
    }
}