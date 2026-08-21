using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class AhpShieldEffect : ICoinEffect
    {
        private readonly float amount;

        public AhpShieldEffect(float amount)
        {
            this.amount = amount;
        }

        public string Name
        {
            get { return "AhpShield"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .AhpShieldEffectMessage
                    .Replace(
                        "{amount}",
                        amount.ToString("0.#"));
            }
        }

        public bool CanApply(Player player)
        {
            return player is not null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   amount > 0f;
        }

        public void Execute(Player player)
        {
            player.AddAhp(amount, amount, 0.5f, 0.7f, 10f, false);
        }
    }
}
