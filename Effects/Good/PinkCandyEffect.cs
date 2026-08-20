using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.Usables.Scp330;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class PinkCandyEffect : ICoinEffect
    {
        public string Name
        {
            get { return "PinkCandy"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .PinkCandyEffectMessage;
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
            Scp330 candy =
                Item.Create(ItemType.SCP330) as Scp330;

            if (candy == null)
                return;

            candy.AddCandy(CandyKindID.Pink);
            candy.CreatePickup(player.Position);
        }
    }
}