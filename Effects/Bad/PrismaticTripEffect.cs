using Exiled.API.Enums;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class PrismaticTripEffect : ICoinEffect
    {
        private readonly float duration;

        public PrismaticTripEffect(float duration)
        {
            this.duration = duration;
        }

        public string Name
        {
            get { return "PrismaticTrip"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .PrismaticTripEffectMessage
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
            player.EnableEffect(EffectType.RainbowTaste, 5, duration, false);
            player.EnableEffect(EffectType.Traumatized, duration, false);
            player.EnableEffect(EffectType.Blurred, 5, duration, false);
        }
    }
}
