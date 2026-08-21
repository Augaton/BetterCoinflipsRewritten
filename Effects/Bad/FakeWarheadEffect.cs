using Exiled.API.Features;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class FakeWarheadEffect : ICoinEffect
    {
        private readonly float dimDuration;

        public FakeWarheadEffect(float dimDuration)
        {
            this.dimDuration = dimDuration;
        }

        public string Name
        {
            get { return "FakeWarhead"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .FakeWarheadEffectMessage;
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
            player.SendWarheadExplosionEffect(false);

            if (dimDuration <= 0f)
                return;

            player.DimScreen();

            API.Scheduler.Delay(
                dimDuration,
                delegate
                {
                    if (player is null || !player.IsConnected)
                        return;

                    player.UndimScreen();
                });
        }
    }
}
