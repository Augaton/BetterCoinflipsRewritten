using Exiled.API.Enums;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class SugarRushEffect : ICoinEffect
    {
        private readonly float duration;

        public SugarRushEffect(float duration)
        {
            this.duration = duration;
        }

        public string Name
        {
            get { return "SugarRush"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .SugarRushEffectMessage
                    .Replace(
                        "{duration}",
                        duration.ToString("0.#"));
            }
        }

        public bool CanApply(Player player)
        {
            return player is not null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   duration > 0f;
        }

        public void Execute(Player player)
        {
            player.EnableEffect(EffectType.MovementBoost, 50, duration, false);
            player.EnableEffect(EffectType.Invigorated, duration, false);
            player.EnableEffect(EffectType.Vitality, duration, false);
            player.ResetStamina();
        }
    }
}
