using System;
using System.Collections.Generic;
using Exiled.API.Features;
using UnityEngine;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class PositionSwapEffect : ICoinEffect
    {
        private readonly List<Player> candidates = new List<Player>(32);

        private string partnerName;

        public PositionSwapEffect()
        {
            partnerName = string.Empty;
        }

        public string Name
        {
            get { return "PositionSwap"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .PositionSwapEffectMessage
                    .Replace(
                        "{player}",
                        partnerName);
            }
        }

        public bool CanApply(Player player)
        {
            if (player is null ||
                !player.IsConnected ||
                !player.IsAlive)
            {
                return false;
            }

            return CountCandidates(player) > 0;
        }

        public void Execute(Player player)
        {
            Collect(player);

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException(
                    "No player is available to swap with.");
            }

            Player partner =
                candidates[API.Rng.Next(0, candidates.Count)];

            candidates.Clear();

            if (partner is null)
            {
                throw new InvalidOperationException(
                    "The selected swap partner is no longer valid.");
            }

            Vector3 origin = player.Position;

            player.Position = partner.Position + (Vector3.up * 0.2f);
            partner.Position = origin + (Vector3.up * 0.2f);

            partnerName = partner.Nickname;

            API.HintBridge.Show(
                partner,
                Plugin.Instance.Translation
                    .PositionSwapPartnerMessage
                    .Replace(
                        "{player}",
                        player.Nickname),
                Plugin.Instance.Config.EffectMessageDuration);
        }

        private int CountCandidates(Player player)
        {
            int count = 0;

            foreach (Player other in Player.List)
            {
                if (!IsValidPartner(player, other))
                    continue;

                count++;
            }

            return count;
        }

        private void Collect(Player player)
        {
            candidates.Clear();

            foreach (Player other in Player.List)
            {
                if (!IsValidPartner(player, other))
                    continue;

                candidates.Add(other);
            }
        }

        private bool IsValidPartner(Player player, Player other)
        {
            return other is not null &&
                   other.IsConnected &&
                   other.IsAlive &&
                   !other.IsScp &&
                   other != player;
        }
    }
}
