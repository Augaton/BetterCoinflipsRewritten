using Exiled.API.Enums;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class GhostlyEffect : ICoinEffect
    {
        private readonly float duration;

        public GhostlyEffect(float duration)
        {
            this.duration = duration;
        }

        public string Name
        {
            get { return "Ghostly"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .GhostlyEffectMessage
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
            player.EnableEffect(EffectType.Ghostly, duration, false);
            player.EnableEffect(EffectType.Invisible, duration, false);
            player.EnableEffect(EffectType.SilentWalk, 255, duration, false);
        }
    }
}
