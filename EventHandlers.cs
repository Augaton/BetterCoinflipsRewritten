using System;
﻿using AugatonLib.Arbitration;
using AugatonLib.Bus;
using BetterCoinflipsRewritten.API;
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
        private readonly CoinConsumeService coinConsumeService;

        private CoroutineHandle spawnHandle;
        private bool spawnScheduled;

        public EventHandlers(
            Plugin plugin,
            CoinService coinService,
            CoinSpawnService coinSpawnService,
            CoinConsumeService coinConsumeService)
        {
            this.plugin = plugin;
            this.coinService = coinService;
            this.coinSpawnService = coinSpawnService;
            this.coinConsumeService = coinConsumeService;
        }

        public void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            try
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

                coinConsumeService.Schedule(ev.Player, ev.Item);

                coinService.ExecuteFlip(
                    ev.Player,
                    ev.IsTails);

                PluginBus.Publish(
                    BusTopics.CoinFlipped,
                    Interop.Owner,
                    ev.Player);
            }
            catch (Exception e)
            {
                Log.Error($"OnFlippingCoin: {e}");
            }
        }

        public void OnLeft(LeftEventArgs ev)
        {
            try
            {
                if (ev == null || ev.Player == null)
                    return;

                coinService.RemovePlayer(ev.Player);
                API.HintBridge.Remove(ev.Player);
                ScaleArbiter.Forget(ev.Player);
            }
            catch (Exception e)
            {
                Log.Error($"OnLeft: {e}");
            }
        }

        public void OnRoundStarted()
        {
            try
            {
                coinService.Clear();
                API.HintBridge.Clear();
                API.RoomCache.Rebuild();
                ReleaseArbiters();
                API.GlobalCooldown.Clear();
                API.Scheduler.Clear();

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
            catch (Exception e)
            {
                Log.Error($"OnRoundStarted: {e}");
            }
        }

        public void OnRestartingRound()
        {
            try
            {
                KillPendingSpawn();
                coinService.Clear();
                API.HintBridge.Clear();
                API.RoomCache.Clear();
                ReleaseArbiters();
                API.GlobalCooldown.Clear();
                API.Scheduler.Clear();

                if (plugin.Config.Debug)
                {
                    Log.Debug(
                        "[BetterCoinflipsRewritten] Round is restarting.");
                }
            }
            catch (Exception e)
            {
                Log.Error($"OnRestartingRound: {e}");
            }
        }
        public static void ReleaseArbiters()
        {
            ScaleArbiter.ReleaseOwner(Interop.Owner);
            LightArbiter.ReleaseOwner(Interop.Owner);
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
