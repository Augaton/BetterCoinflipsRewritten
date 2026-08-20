using Exiled.API.Features;
using Exiled.API.Features.Items;
using UnityEngine;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class InstantExplosionEffect : ICoinEffect
    {
        private readonly float fuseTime;

        public InstantExplosionEffect(float fuseTime)
        {
            this.fuseTime = fuseTime;
        }

        public string Name
        {
            get { return "InstantExplosion"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .InstantExplosionEffectMessage;
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
            ExplosiveGrenade grenade =
                Item.Create(ItemType.GrenadeHE)
                    as ExplosiveGrenade;

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