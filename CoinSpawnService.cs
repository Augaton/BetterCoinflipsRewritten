using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using UnityEngine;

namespace BetterCoinflipsRewritten
{
    public sealed class CoinSpawnService
    {
        private readonly Plugin plugin;

        public CoinSpawnService(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void SpawnMapCoins()
        {
            if (!plugin.Config.SpawnCoinsOnMap)
                return;

            int requestedAmount = plugin.Config.MapCoinAmount;

            if (requestedAmount <= 0)
                return;

            List<Pickup> candidates = new List<Pickup>(64);

            foreach (Pickup pickup in Pickup.List)
            {
                if (!IsValidReplacement(pickup))
                    continue;

                candidates.Add(pickup);
            }

            Shuffle(candidates);

            int amountToReplace =
                Math.Min(requestedAmount, candidates.Count);

            int spawnedCoins = 0;

            for (int i = 0; i < amountToReplace; i++)
            {
                Pickup oldPickup = candidates[i];

                if (oldPickup == null ||
                    !oldPickup.IsSpawned)
                {
                    continue;
                }

                Vector3 position =
                    oldPickup.Position + (Vector3.up * 0.03f);

                Quaternion rotation = oldPickup.Rotation;

                try
                {
                    Pickup.CreateAndSpawn(
                        ItemType.Coin,
                        position,
                        rotation);

                    oldPickup.Destroy();
                    spawnedCoins++;

                    if (plugin.Config.Debug)
                    {
                        Log.Debug(
                            "[BetterCoinflipsRewritten] Replaced " +
                            oldPickup.Type +
                            " with a coin in room " +
                            GetRoomName(oldPickup) +
                            ".");
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(
                        "[BetterCoinflipsRewritten] Failed to spawn a coin: " +
                        exception);
                }
            }

            Log.Info(
                "[BetterCoinflipsRewritten] Spawned " +
                spawnedCoins +
                "/" +
                requestedAmount +
                " coins across the map.");
        }

        private bool IsValidReplacement(Pickup pickup)
        {
            if (pickup == null ||
                !pickup.IsSpawned ||
                pickup.Type == ItemType.Coin)
            {
                return false;
            }

            switch (pickup.Type)
            {
                case ItemType.Medkit:
                    return plugin.Config.ReplaceMedkitsWithCoins;

                case ItemType.Painkillers:
                    return plugin.Config.ReplacePainkillersWithCoins;

                case ItemType.Flashlight:
                    return plugin.Config.ReplaceFlashlightsWithCoins;

                case ItemType.KeycardJanitor:
                case ItemType.KeycardScientist:
                case ItemType.KeycardZoneManager:
                    return plugin.Config.ReplaceWeakKeycardsWithCoins;

                default:
                    return false;
            }
        }

        private void Shuffle(List<Pickup> pickups)
        {
            for (int i = pickups.Count - 1; i > 0; i--)
            {
                int selectedIndex = API.Rng.Next(0, i + 1);

                Pickup temporary = pickups[i];
                pickups[i] = pickups[selectedIndex];
                pickups[selectedIndex] = temporary;
            }
        }

        private string GetRoomName(Pickup pickup)
        {
            if (pickup == null || pickup.Room == null)
                return "Unknown";

            return pickup.Room.Type.ToString();
        }
    }
}