using Exiled.API.Enums;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class AmnesiaEffect : ICoinEffect
    {
        private readonly float duration;

        public AmnesiaEffect(float duration)
        {
            this.duration = duration;
        }

        public string Name
        {
            get { return "Amnesia"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .AmnesiaEffectMessage
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
            player.EnableEffect(EffectType.AmnesiaVision, duration, false);
            player.EnableEffect(EffectType.AmnesiaItems, duration, false);
            player.EnableEffect(EffectType.Concussed, duration, false);
        }
    }
}
