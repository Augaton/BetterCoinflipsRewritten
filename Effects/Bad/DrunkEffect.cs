using Exiled.API.Enums;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class DrunkEffect : ICoinEffect
    {
        private readonly float duration;

        public DrunkEffect(float duration)
        {
            this.duration = duration;
        }

        public string Name
        {
            get { return "Drunk"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .DrunkEffectMessage
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
            player.EnableEffect(EffectType.Concussed, duration, false);
            player.EnableEffect(EffectType.Blurred, 8, duration, false);
            player.EnableEffect(EffectType.SinkHole, 1, duration, false);
            player.EnableEffect(EffectType.Exhausted, duration, false);
        }
    }
}
