using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class InventoryResetEffect : ICoinEffect
    {
        public string Name
        {
            get { return "InventoryReset"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .InventoryResetEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            return player != null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   player.Items != null &&
                   player.Items.Count > 0;
        }

        public void Execute(Player player)
        {
            player.ClearInventory();
        }
    }
}