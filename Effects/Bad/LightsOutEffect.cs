using BetterCoinflipsRewritten.API;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class LightsOutEffect : ICoinEffect
    {
        private const string CooldownKey = "lightsout";

        private readonly float duration;

        public LightsOutEffect(float duration)
        {
            this.duration = duration;
        }

        public string Name
        {
            get { return "LightsOut"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .LightsOutEffectMessage
                    .Replace(
                        "{duration}",
                        duration.ToString("0.#"));
            }
        }

        public bool CanApply(Player player)
        {
            return player != null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   duration > 0f;
        }

        public void Execute(Player player)
        {
            float cooldown =
                Plugin.Instance.Config.FacilityEffectCooldown;

            if (!GlobalCooldown.TryConsume(CooldownKey, cooldown))
                return;

            Map.TurnOffAllLights(duration);
        }
    }
}
