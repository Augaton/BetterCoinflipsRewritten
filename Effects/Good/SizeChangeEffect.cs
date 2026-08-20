using Exiled.API.Features;
using UnityEngine;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class SizeChangeEffect : ICoinEffect
    {
        private string selectedSize;

        public SizeChangeEffect()
        {

            selectedSize =
                Plugin.Instance.Translation
                    .ChangedSizeName;
        }

        public string Name
        {
            get { return "SizeChange"; }
        }

        public string Message
        {
            get
            {
                return Plugin.Instance.Translation
                    .SizeChangeEffectMessage
                    .Replace(
                        "{size}",
                        selectedSize);
            }
        }

        public bool CanApply(Player player)
        {
            return player != null &&
                   player.IsConnected &&
                   player.IsAlive;
        }

        public void Execute(Player player)
        {
            bool makeSmall =
                API.Rng.Next(0, 2) == 0;

            if (makeSmall)
            {
                player.Scale = new Vector3(
                    0.7f,
                    0.7f,
                    0.7f);

                selectedSize =
                    Plugin.Instance.Translation
                        .SmallSizeName;
            }
            else
            {
                player.Scale = new Vector3(
                    1.1f,
                    1.1f,
                    1.1f);

                selectedSize =
                    Plugin.Instance.Translation
                        .LargeSizeName;
            }
        }
    }
}