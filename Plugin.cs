using System;
using Exiled.API.Features;

using PlayerEvents = Exiled.Events.Handlers.Player;
using ServerEvents = Exiled.Events.Handlers.Server;

namespace BetterCoinflipsRewritten
{
    public sealed class Plugin : Plugin<Config, Translation>
    {
        public override string Name
        {
            get { return "BetterCoinflipsRewritten"; }
        }

        public override string Author
        {
            get
            {
                return "Matisiowy, original plugin written by Mikihero";
            }
        }

        public override Version Version
        {
            get { return new Version(2, 1, 0); }
        }

        public override Version RequiredExiledVersion
        {
            get { return new Version(9, 14, 2); }
        }

        public override string Prefix
        {
            get { return "bettercoinflips"; }
        }

        public static Plugin Instance { get; private set; }

        private CoinService coinService;
        private EventHandlers eventHandlers;
        private CoinSpawnService coinSpawnService;
        private CoinConsumeService coinConsumeService;

        public override void OnEnabled()
        {
            Instance = this;

            API.HintBridge.YCoordinate = Config.HintYCoordinate;
            API.HintBridge.FontSize = Config.HintFontSize;

            ValidateConfig();

            coinService = new CoinService(this);
            coinSpawnService = new CoinSpawnService(this);
            coinConsumeService = new CoinConsumeService(this);

            eventHandlers = new EventHandlers(
                this,
                coinService,
                coinSpawnService,
                coinConsumeService);

            PlayerEvents.FlippingCoin += eventHandlers.OnFlippingCoin;
            PlayerEvents.Left += eventHandlers.OnLeft;

            ServerEvents.RoundStarted += eventHandlers.OnRoundStarted;
            ServerEvents.RestartingRound += eventHandlers.OnRestartingRound;

            Log.Info("==============================================");
            Log.Info("[BetterCoinflipsRewritten] Plugin loaded.");
            Log.Info(
                "[BetterCoinflipsRewritten] Coins on map: " +
                Config.MapCoinAmount +
                ", extra coins in loot pools: " +
                (Config.AddBonusCoinsToLoot ? Config.BonusCoinAmount : 0));
            Log.Info(
                "[BetterCoinflipsRewritten] Coin consumed on flip: " +
                Config.ConsumeCoinOnFlip);
            Log.Info("==============================================");

            base.OnEnabled();
        }

        private void ValidateConfig()
        {
            Config.CoinConsumeDelay = Clamp(
                "coin_consume_delay",
                Config.CoinConsumeDelay,
                0.5f,
                10f);

            Config.PetrifiedDuration = Clamp(
                "petrified_duration",
                Config.PetrifiedDuration,
                0f,
                15f);

            Config.SkyLaunchHeight = Clamp(
                "sky_launch_height",
                Config.SkyLaunchHeight,
                0f,
                100f);

            Config.DiscoLightsInterval = Clamp(
                "disco_lights_interval",
                Config.DiscoLightsInterval,
                0.2f,
                10f);

            Config.CassieMockGlitchChance = Clamp(
                "cassie_mock_glitch_chance",
                Config.CassieMockGlitchChance,
                0f,
                1f);

            Config.CassieMockJamChance = Clamp(
                "cassie_mock_jam_chance",
                Config.CassieMockJamChance,
                0f,
                1f);

            if (Config.AddBonusCoinsToLoot &&
                (Config.BonusCoinSourceItems is null ||
                 Config.BonusCoinSourceItems.Count == 0))
            {
                Log.Warn(
                    "[BetterCoinflipsRewritten] bonus_coin_source_items is " +
                    "empty, no extra coin will reach the loot pools.");
            }

            if (!Config.ConsumeCoinOnFlip)
                return;

            if (Config.AddBonusCoinsToLoot || Config.SpawnCoinsOnMap)
                return;

            Log.Warn(
                "[BetterCoinflipsRewritten] Coins are consumed on flip but " +
                "no coin is placed on the map. Only the vanilla loot table " +
                "will provide them.");
        }

        private static float Clamp(
            string name,
            float value,
            float minimum,
            float maximum)
        {
            if (value >= minimum && value <= maximum)
                return value;

            float clamped = value < minimum ? minimum : maximum;

            Log.Warn(
                "[BetterCoinflipsRewritten] " +
                name +
                " (" +
                value.ToString("0.##") +
                ") is out of the " +
                minimum.ToString("0.##") +
                "-" +
                maximum.ToString("0.##") +
                " range, clamped to " +
                clamped.ToString("0.##") +
                ".");

            return clamped;
        }

        public override void OnDisabled()
        {
            if (eventHandlers != null)
            {
                PlayerEvents.FlippingCoin -= eventHandlers.OnFlippingCoin;
                PlayerEvents.Left -= eventHandlers.OnLeft;

                ServerEvents.RoundStarted -= eventHandlers.OnRoundStarted;
                ServerEvents.RestartingRound -=
                    eventHandlers.OnRestartingRound;
            }

            if (eventHandlers != null)
                eventHandlers.KillPendingSpawn();

            if (coinService != null)
                coinService.Clear();

            API.HintBridge.Clear();
            API.Scheduler.Clear();
            API.GlobalCooldown.Clear();
            API.RoomCache.Clear();

            eventHandlers = null;
            coinService = null;
            coinSpawnService = null;
            coinConsumeService = null;
            Instance = null;

            Log.Info(
                "[BetterCoinflipsRewritten] Plugin disabled.");

            base.OnDisabled();
        }
    }
}