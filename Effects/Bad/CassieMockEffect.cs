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

            if (announcements is null)
                return;

            foreach (string line in announcements)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if (!ExiledCassie.IsValid(line))
                {
                    Log.Warn(
                        "[BetterCoinflipsRewritten] C.A.S.S.I.E. announcement " +
                        "refused by the game, it will never be played: \"" +
                        line +
                        "\".");

                    continue;
                }

                lines.Add(line);
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
                   lines.Count > 0;
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
    }
}
