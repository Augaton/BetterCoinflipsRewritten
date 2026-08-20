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
            get { return new Version(2, 0, 0); }
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

        public override void OnEnabled()
        {
            Instance = this;

            API.HintBridge.YCoordinate = Config.HintYCoordinate;
            API.HintBridge.FontSize = Config.HintFontSize;

            coinService = new CoinService(this);
            coinSpawnService = new CoinSpawnService(this);

            eventHandlers = new EventHandlers(
                this,
                coinService,
                coinSpawnService);

            PlayerEvents.FlippingCoin += eventHandlers.OnFlippingCoin;
            PlayerEvents.Left += eventHandlers.OnLeft;

            ServerEvents.RoundStarted += eventHandlers.OnRoundStarted;
            ServerEvents.RestartingRound += eventHandlers.OnRestartingRound;

            Log.Info("==============================================");
            Log.Info("[BetterCoinflipsRewritten] Plugin loaded.");
            Log.Info(
                "[BetterCoinflipsRewritten] Coins on map: " +
                Config.MapCoinAmount);
            Log.Info("==============================================");

            base.OnEnabled();
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

            eventHandlers = null;
            coinService = null;
            coinSpawnService = null;
            Instance = null;

            Log.Info(
                "[BetterCoinflipsRewritten] Plugin disabled.");

            base.OnDisabled();
        }
    }
}