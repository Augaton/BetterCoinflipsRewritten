using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using UnityEngine;

namespace BetterCoinflipsRewritten.Effects.Good
{
    public sealed class MedicalKitEffect : ICoinEffect
    {
        private bool droppedOnGround;

        public string Name
        {
            get { return "MedicalKit"; }
        }

        public string Message
        {
            get
            {
                if (droppedOnGround)
                {
                    return Plugin.Instance.Translation
                        .MedicalKitDroppedMessage;
                }

                return Plugin.Instance.Translation
                    .MedicalKitEffectMessage;
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
            droppedOnGround = false;

            bool alreadyHasMedkit = false;

            foreach (Exiled.API.Features.Items.Item item in player.Items)
            {
                if (item == null || item.Type != ItemType.Medkit)
                    continue;

                alreadyHasMedkit = true;
                break;
            }

            if (!alreadyHasMedkit)
            {
                player.AddItem(ItemType.Medkit);
                return;
            }

            Vector3 dropPosition =
                player.Position + (Vector3.up * 0.2f);

            Pickup.CreateAndSpawn(
                ItemType.Medkit,
                dropPosition,
                Quaternion.identity,
                player);

            droppedOnGround = true;
        }
    }
}