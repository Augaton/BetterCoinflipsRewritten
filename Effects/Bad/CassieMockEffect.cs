using System.Collections.Generic;
using BetterCoinflipsRewritten.API;
using Exiled.API.Features;
using ExiledCassie = Exiled.API.Features.Cassie;

namespace BetterCoinflipsRewritten.Effects.Bad
{
    public sealed class CassieMockEffect : ICoinEffect
    {
        private const string CooldownKey = "cassie";

        private readonly List<string> lines;
        private readonly List<string> pendingLines;
        private readonly float glitchChance;
        private readonly float jamChance;

        public CassieMockEffect(
            IEnumerable<string> announcements,
            float glitchChance,
            float jamChance)
        {
            this.glitchChance = glitchChance;
            this.jamChance = jamChance;

            lines = new List<string>(4);
            pendingLines = new List<string>(4);

            if (announcements is null)
                return;

            foreach (string line in announcements)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                pendingLines.Add(line);
            }
        }

        public string Name
        {
            get { return "CassieMock"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .CassieMockEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            return player is not null &&
                   player.IsConnected &&
                   player.IsAlive &&
                   GlobalCooldown.IsReady(
                       CooldownKey,
                       Plugin.Instance.Config.FacilityEffectCooldown) &&
                   HasPlayableLine();
        }

        public void Execute(Player player)
        {
            float cooldown =
                Plugin.Instance.Config.FacilityEffectCooldown;

            if (!GlobalCooldown.TryConsume(CooldownKey, cooldown))
                return;

            string line = lines[Rng.Next(0, lines.Count)];

            ExiledCassie.GlitchyMessage(line, glitchChance, jamChance);
        }

        private bool HasPlayableLine()
        {
            if (pendingLines.Count > 0 &&
                global::Cassie.CassieTtsAnnouncer.TryGetDatabase(out _))
            {
                ValidatePendingLines();
            }

            return lines.Count > 0;
        }

        private void ValidatePendingLines()
        {
            foreach (string line in pendingLines)
            {
                if (ExiledCassie.CalculateDuration(line) > 0f)
                {
                    lines.Add(line);
                    continue;
                }

                Log.Warn(
                    "[BetterCoinflipsRewritten] C.A.S.S.I.E. announcement " +
                    "refused by the game, it will never be played: \"" +
                    line +
                    "\".");
            }

            pendingLines.Clear();
        }
    }
}
