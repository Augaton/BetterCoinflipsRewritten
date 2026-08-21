using Exiled.API.Enums;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class PetrifiedEffect : ICoinEffect
    {
        private readonly float duration;

        public PetrifiedEffect(float duration)
        {
            this.duration = duration;
        }

        public string Name
        {
            get { return "Petrified"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .PetrifiedEffectMessage
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
                   !player.IsScp &&
                   duration > 0f;
        }

        public void Execute(Player player)
        {
            player.EnableEffect(EffectType.Ensnared, duration, false);
            player.EnableEffect(EffectType.Deafened, duration, false);
        }
    }
}
