using System;
using BetterCoinflipsRewritten.API;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class MiceInvasionEffect : ICoinEffect
    {
        private const string CooldownKey = "mice";

        private readonly byte miceType;

        public MiceInvasionEffect(byte miceType)
        {
            this.miceType = miceType;
        }

        public string Name
        {
            get { return "MiceInvasion"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .MiceInvasionEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            return player is not null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   GlobalCooldown.IsReady(
                       CooldownKey,
                       Plugin.Instance.Config.FacilityEffectCooldown);
        }

        public void Execute(Player player)
        {
            float cooldown =
                Plugin.Instance.Config.FacilityEffectCooldown;

            if (!GlobalCooldown.TryConsume(CooldownKey, cooldown))
                return;

            SqueakSpawner spawner =
                UnityEngine.Object.FindObjectOfType<SqueakSpawner>();

            if (spawner is null ||
                miceType == 0 ||
                miceType > spawner.mice.Length)
            {
                throw new InvalidOperationException(
                    "Mice type " + miceType + " cannot be spawned.");
            }

            spawner.NetworksyncSpawn = miceType;
            spawner.SyncMouseSpawn(0, spawner.NetworksyncSpawn);
        }
    }
}
