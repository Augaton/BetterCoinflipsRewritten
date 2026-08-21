using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using UnityEngine;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class DoubleCoinEffect : ICoinEffect
    {
        private readonly int maxInventorySlots;

        private bool droppedOnGround;

        public DoubleCoinEffect(int maxInventorySlots)
        {
            this.maxInventorySlots = maxInventorySlots;
        }

        public string Name
        {
            get { return "DoubleCoin"; }
        }

        public string Message
        {
            get
            {
                if (droppedOnGround)
                {
                    return Plugin.Instance.Translation
                        .DoubleCoinDroppedMessage;
                }

                return Plugin.Instance.Translation
                    .DoubleCoinEffectMessage;
            }
        }

        public bool CanApply(Player player)
        {
            return player is not null &&
                   player.IsConnected &&
                   player.IsAlive;
        }

        public void Execute(Player player)
        {
            droppedOnGround = false;

            Vector3 dropPosition =
                player.Position + (Vector3.up * 0.2f);

            for (int i = 0; i < 2; i++)
            {
                if (player.Items.Count < maxInventorySlots)
                {
                    player.AddItem(ItemType.Coin);
                    continue;
                }

                Pickup.CreateAndSpawn(
                    ItemType.Coin,
                    dropPosition,
                    Quaternion.identity,
                    player);

                droppedOnGround = true;
            }
        }
    }
}
