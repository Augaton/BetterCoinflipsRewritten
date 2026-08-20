using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class OneAmmoLogicerEffect : ICoinEffect
    {
        public string Name
        {
            get { return "OneAmmoLogicer"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .OneAmmoLogicerEffectMessage;
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
            Firearm firearm =
                Item.Create(ItemType.GunLogicer) as Firearm;

            if (firearm == null)
                return;

            firearm.BarrelAmmo = 1;
            firearm.CreatePickup(player.Position);
        }
    }
}