using Exiled.API.Features;
using Exiled.API.Features.Items;
using UnityEngine;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class TrollFlashEffect : ICoinEffect
    {
        private readonly float fuseTime;

        public TrollFlashEffect(float fuseTime)
        {
            this.fuseTime = fuseTime;
        }

        public string Name
        {
            get { return "TrollFlash"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .TrollFlashEffectMessage;
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
            FlashGrenade grenade =
                Item.Create(ItemType.GrenadeFlash)
                    as FlashGrenade;

            if (grenade == null)
                return;

            grenade.FuseTime = fuseTime;

            Vector3 position =
                player.Position + (Vector3.up * 0.2f);

            grenade.SpawnActive(
                position,
                player);
        }
    }
}