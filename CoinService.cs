using System;
using System.Collections.Generic;
using BetterCoinflipsRewritten.Effects;
using BetterCoinflipsRewritten.Effects.Bad;
using BetterCoinflipsRewritten.Effects.Good;
using Exiled.API.Features;

namespace BetterCoinflipsRewritten
{
    public sealed class CoinService
    {
        private readonly Plugin plugin;

        private readonly Dictionary<string, CoinState> playerStates;
        private readonly List<WeightedEffect> goodEffects;
        private readonly List<WeightedEffect> badEffects;
        private readonly List<WeightedEffect> candidateBuffer;

        public CoinService(Plugin plugin)
        {
            this.plugin = plugin;

            playerStates = new Dictionary<string, CoinState>();
            goodEffects = new List<WeightedEffect>();
            badEffects = new List<WeightedEffect>();
            candidateBuffer = new List<WeightedEffect>(24);

            RegisterEffects();
        }

        private void RegisterEffects()
        {
            goodEffects.Clear();
            badEffects.Clear();

            goodEffects.Add(
                new WeightedEffect(
                    new RandomKeycardEffect(plugin.Config),
                    plugin.Config.KeycardChance));

            goodEffects.Add(
                new WeightedEffect(
                    new MedicalKitEffect(),
                    plugin.Config.MedicalKitChance));

            goodEffects.Add(
                new WeightedEffect(
                    new TeleportToEscapeEffect(),
                    plugin.Config.TpToEscapeChance));

            goodEffects.Add(
                new WeightedEffect(
                    new HealEffect(plugin.Config.HealAmount),
                    plugin.Config.HealChance));

            goodEffects.Add(
                new WeightedEffect(
                    new MoreHealthEffect(plugin.Config.MoreHpPercent),
                    plugin.Config.MoreHpChance));

            goodEffects.Add(
                new WeightedEffect(
                    new HatEffect(),
                    plugin.Config.HatChance));

            goodEffects.Add(
                new WeightedEffect(
                    new OneAmmoLogicerEffect(),
                    plugin.Config.OneAmmoLogicerChance));

            goodEffects.Add(
                new WeightedEffect(
                    new LightbulbEffect(),
                    plugin.Config.LightbulbChance));

            goodEffects.Add(
                new WeightedEffect(
                    new PinkCandyEffect(),
                    plugin.Config.PinkCandyChance));

            goodEffects.Add(
                new WeightedEffect(
                    new SizeChangeEffect(),
                    plugin.Config.SizeChangeChance));

            badEffects.Add(
    new WeightedEffect(
        new HpReductionEffect(
            plugin.Config.HpReductionPercent),
        plugin.Config.HpReductionChance));

            badEffects.Add(
                new WeightedEffect(
                    new TeleportToClassDCellsEffect(),
                    plugin.Config.TpToClassDCellsChance));

            badEffects.Add(
                new WeightedEffect(
                    new LightsOutEffect(
                        plugin.Config.LightsOutDuration),
                    plugin.Config.LightsOutChance));

            badEffects.Add(
                new WeightedEffect(
                    new TrollFlashEffect(
                        plugin.Config.TrollFlashFuseTime),
                    plugin.Config.TrollFlashChance));

            badEffects.Add(
                new WeightedEffect(
                    new OneHpLeftEffect(),
                    plugin.Config.OneHpLeftChance));

            badEffects.Add(
                new WeightedEffect(
                    new InventoryResetEffect(),
                    plugin.Config.InventoryResetChance));

            badEffects.Add(
                new WeightedEffect(
                    new InstantExplosionEffect(
                        plugin.Config.InstantExplosionFuseTime),
                    plugin.Config.InstantExplosionChance));

            badEffects.Add(
                new WeightedEffect(
                    new RandomTeleportEffect(),
                    plugin.Config.RandomTeleportChance));

            Log.Info(
                "[BetterCoinflipsRewritten] Registered " +
                goodEffects.Count +
                " positive effects and " +
                badEffects.Count +
                " negative effects.");
        }

            public bool IsOnCooldown(
            Player player,
            out double secondsRemaining)
        {
            secondsRemaining = 0;

            if (player == null)
                return true;

            CoinState state;

            if (!playerStates.TryGetValue(player.UserId, out state))
                return false;

            double elapsedSeconds =
                (DateTime.UtcNow - state.LastFlipTime).TotalSeconds;

            double cooldown = plugin.Config.CoinCooldown;

            if (elapsedSeconds >= cooldown)
                return false;

            secondsRemaining = cooldown - elapsedSeconds;
            return true;
        }

        public void SetCooldown(Player player)
        {
            if (player == null)
                return;

            CoinState state;

            if (!playerStates.TryGetValue(player.UserId, out state))
            {
                state = new CoinState();
                playerStates.Add(player.UserId, state);
            }

            state.LastFlipTime = DateTime.UtcNow;
        }

        public ICoinEffect SelectEffect(
            bool isTails,
            Player player)
        {
            List<WeightedEffect> source =
                isTails ? badEffects : goodEffects;

            candidateBuffer.Clear();

            int totalWeight = 0;

            foreach (WeightedEffect weightedEffect in source)
            {
                if (weightedEffect == null ||
                    weightedEffect.Effect == null ||
                    weightedEffect.Weight <= 0)
                {
                    continue;
                }

                if (!weightedEffect.Effect.CanApply(player))
                    continue;

                candidateBuffer.Add(weightedEffect);
                totalWeight += weightedEffect.Weight;
            }

            if (candidateBuffer.Count == 0 || totalWeight <= 0)
                return null;

            int selectedNumber =
                API.Rng.NextWeight(totalWeight);

            ICoinEffect fallback =
                candidateBuffer[candidateBuffer.Count - 1].Effect;

            foreach (WeightedEffect weightedEffect in candidateBuffer)
            {
                selectedNumber -= weightedEffect.Weight;

                if (selectedNumber <= 0)
                {
                    candidateBuffer.Clear();
                    return weightedEffect.Effect;
                }
            }

            candidateBuffer.Clear();
            return fallback;
        }

        public void ExecuteFlip(
    Player player,
    bool isTails)
        {
            if (player == null ||
                !player.IsConnected ||
                !player.IsAlive)
            {
                return;
            }

            ICoinEffect selectedEffect =
                SelectEffect(isTails, player);

            if (selectedEffect == null)
            {
                Log.Warn(
                    "[BetterCoinflipsRewritten] No available effect was " +
                    "found for " +
                    player.Nickname +
                    ".");

                API.HintBridge.Show(
                    player,
                    plugin.Translation.CoinFlipTitle +
                    "\n" +
                    plugin.Translation.NoAvailableEffect,
                    plugin.Config.EffectMessageDuration);

                return;
            }

            try
            {
                selectedEffect.Execute(player);

                string translatedResult =
                    isTails
                        ? plugin.Translation.TailsName
                        : plugin.Translation.HeadsName;

                string resultLine =
                    plugin.Translation.ResultFormat.Replace(
                        "{result}",
                        translatedResult);

                string effectLine =
                    plugin.Translation.EffectFormat.Replace(
                        "{effect}",
                        selectedEffect.Message);

                string resultMessage =
                    plugin.Translation.CoinFlipTitle +
                    "\n" +
                    resultLine +
                    "\n" +
                    effectLine;

                API.HintBridge.Show(
                    player,
                    resultMessage,
                    plugin.Config.EffectMessageDuration);

                if (plugin.Config.LogFlips)
                {
                    Log.Info(
                        "[BetterCoinflipsRewritten] " +
                        player.Nickname +
                        " (" + player.UserId + ") flipped " +
                        (isTails ? "tails" : "heads") +
                        ". Effect: " +
                        selectedEffect.Name +
                        ".");
                }
            }
            catch (Exception exception)
            {
                Log.Error(
                    "[BetterCoinflipsRewritten] Error while executing effect " +
                    selectedEffect.Name +
                    ": " +
                    exception);

                if (player.IsConnected)
                {
                    API.HintBridge.Show(
                        player,
                        plugin.Translation.EffectError,
                        plugin.Config.EffectMessageDuration);
                }
            }
        }

        public void RemovePlayer(Player player)
        {
            if (player == null ||
                string.IsNullOrEmpty(player.UserId))
            {
                return;
            }

            playerStates.Remove(player.UserId);
        }

        public void Clear()
        {
            playerStates.Clear();
            candidateBuffer.Clear();
        }
    }
}