using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace BetterCoinflipsRewritten
{
    public sealed class EventHandlers
    {
        private readonly Plugin plugin;
        private readonly CoinService coinService;
        private readonly CoinSpawnService coinSpawnService;

        private CoroutineHandle spawnHandle;
        private bool spawnScheduled;

        public EventHandlers(
            Plugin plugin,
            CoinService coinService,
            CoinSpawnService coinSpawnService)
        {
            this.plugin = plugin;
            this.coinService = coinService;
            this.coinSpawnService = coinSpawnService;
        }

        public void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            if (ev == null ||
                ev.Player == null ||
                !ev.Player.IsConnected ||
                !ev.Player.IsAlive)
            {
                return;
            }

            double secondsRemaining;

            if (coinService.IsOnCooldown(
                ev.Player,
                out secondsRemaining))
            {
                ev.IsAllowed = false;

                string message =
                    plugin.Translation.CooldownMessage.Replace(
                        "{seconds}",
                        secondsRemaining.ToString("0.0"));

                API.HintBridge.Show(
                    ev.Player,
                    message,
                    plugin.Config.CooldownMessageDuration);

                if (plugin.Config.Debug)
                {
                    Log.Debug(
                        "[BetterCoinflipsRewritten] " +
                        ev.Player.Nickname +
                        " attempted to flip a coin during cooldown. " +
                        "Remaining: " +
                        secondsRemaining.ToString("0.0") +
                        " seconds.");
                }

                return;
            }

            coinService.SetCooldown(ev.Player);

            if (plugin.Config.Debug)
            {
                Log.Debug(
                    "[BetterCoinflipsRewritten] Coin flip detected for " +
                    ev.Player.Nickname +
                    ". IsTails: " +
                    ev.IsTails +
                    ".");
            }

            coinService.ExecuteFlip(
                ev.Player,
                ev.IsTails);
        }

        public void OnLeft(LeftEventArgs ev)
        {
            if (ev == null || ev.Player == null)
                return;

            coinService.RemovePlayer(ev.Player);
            API.HintBridge.Remove(ev.Player);
        }

        public void OnRoundStarted()
        {
            coinService.Clear();
            API.HintBridge.Clear();
            API.RoomCache.Rebuild();
            API.GlobalCooldown.Clear();

            if (plugin.Config.Debug)
            {
                Log.Debug(
                    "[BetterCoinflipsRewritten] Round started. " +
                    "Player data has been cleared.");
            }

            KillPendingSpawn();

            spawnScheduled = true;
            spawnHandle = Timing.CallDelayed(
                plugin.Config.CoinSpawnDelay,
                delegate
                {
                    spawnScheduled = false;
                    coinSpawnService.SpawnMapCoins();
                });
        }

        public void OnRestartingRound()
        {
            KillPendingSpawn();
            coinService.Clear();
            API.HintBridge.Clear();
            API.RoomCache.Clear();
            API.GlobalCooldown.Clear();

            if (plugin.Config.Debug)
            {
                Log.Debug(
                    "[BetterCoinflipsRewritten] Round is restarting.");
            }
        }
        public void KillPendingSpawn()
        {
            if (!spawnScheduled)
                return;

            Timing.KillCoroutines(spawnHandle);
            spawnScheduled = false;
        }
    }

}
