using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class FakeExplosionEffect : ICoinEffect
    {
        private readonly float stunDuration;

        public FakeExplosionEffect(float stunDuration)
        {
            this.stunDuration = stunDuration;
        }

        public string Name
        {
            get { return "FakeExplosion"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .FakeExplosionEffectMessage;
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
            Vector3 position =
                player.Position + (Vector3.up * 0.2f);

            Map.ExplodeEffect(position, ProjectileType.FragGrenade);

            if (stunDuration <= 0f)
                return;

            player.EnableEffect(EffectType.Deafened, stunDuration, false);
            player.EnableEffect(EffectType.Concussed, stunDuration, false);
        }
    }
}
