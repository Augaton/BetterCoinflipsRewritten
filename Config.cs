using System.ComponentModel;
using Exiled.API.Interfaces;

namespace BetterCoinflipsRewritten
{
    public sealed class Config : IConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether additional debug logs should be displayed.")]
        public bool Debug { get; set; } = false;

        [Description("Whether every coin flip is written to the server log. Costs one disk write per flip.")]
        public bool LogFlips { get; set; } = false;

        [Description("Server-wide cooldown in seconds for facility-wide effects such as lights out, so chained flips cannot keep the facility dark.")]
        public float FacilityEffectCooldown { get; set; } = 60f;

        [Description("Cooldown between coin flips in seconds.")]
        public float CoinCooldown { get; set; } = 5f;

        [Description("Duration of the effect message in seconds.")]
        public float EffectMessageDuration { get; set; } = 5f;

        [Description("Duration of the cooldown message in seconds.")]
        public float CooldownMessageDuration { get; set; } = 3f;


        [Description("Vertical position of this plugin hint inside HintServiceMeow. Must differ from other plugins.")]
        public float HintYCoordinate { get; set; } = 750f;

        [Description("Font size of this plugin hint.")]
        public int HintFontSize { get; set; } = 20;

        [Description("Weight of the random keycard effect.")]
        public int KeycardChance { get; set; } = 20;

        [Description("Weight of the medical kit effect.")]
        public int MedicalKitChance { get; set; } = 35;

        [Description("Weight of the teleport-to-escape effect.")]
        public int TpToEscapeChance { get; set; } = 5;

        [Description("Weight of the healing effect.")]
        public int HealChance { get; set; } = 10;

        [Description("Weight of the maximum health increase effect.")]
        public int MoreHpChance { get; set; } = 10;

        [Description("Weight of the SCP-268 reward effect.")]
        public int HatChance { get; set; } = 10;

        [Description("Weight of the one-ammo Logicer reward effect.")]
        public int OneAmmoLogicerChance { get; set; } = 1;

        [Description("Weight of the SCP-2176 reward effect.")]
        public int LightbulbChance { get; set; } = 15;

        [Description("Weight of the pink candy reward effect.")]
        public int PinkCandyChance { get; set; } = 10;

        [Description("Weight of the random player size effect.")]
        public int SizeChangeChance { get; set; } = 20;

        [Description("Amount of health restored by the healing effect.")]
        public float HealAmount { get; set; } = 25f;

        [Description("Percentage by which the player's maximum health is increased.")]
        public float MoreHpPercent { get; set; } = 10f;

        [Description("Weight of the Janitor Keycard in the random keycard pool.")]
        public int JanitorKeycardChance { get; set; } = 40;

        [Description("Weight of the Scientist Keycard in the random keycard pool.")]
        public int ScientistKeycardChance { get; set; } = 25;

        [Description("Weight of the Zone Manager Keycard in the random keycard pool.")]
        public int ZoneManagerKeycardChance { get; set; } = 18;

        [Description("Weight of the Facility Guard Keycard in the random keycard pool.")]
        public int GuardKeycardChance { get; set; } = 10;

        [Description("Weight of the Research Coordinator Keycard in the random keycard pool.")]
        public int ResearchCoordinatorKeycardChance { get; set; } = 6;

        [Description("Weight of the Containment Engineer Keycard in the random keycard pool.")]
        public int ContainmentEngineerKeycardChance { get; set; } = 1;

        [Description("Weight of the current health reduction effect.")]
        public int HpReductionChance { get; set; } = 20;

        [Description("Percentage of the player's current health removed by the effect.")]
        public float HpReductionPercent { get; set; } = 30f;

        [Description("Weight of the teleport-to-Class-D-cells effect.")]
        public int TpToClassDCellsChance { get; set; } = 5;

        [Description("Weight of the facility lights-out effect.")]
        public int LightsOutChance { get; set; } = 20;

        [Description("Duration of the lights-out effect in seconds.")]
        public float LightsOutDuration { get; set; } = 10f;

        [Description("Weight of the active flashbang-at-player effect.")]
        public int TrollFlashChance { get; set; } = 50;

        [Description("Fuse time of the flashbang in seconds.")]
        public float TrollFlashFuseTime { get; set; } = 0.8f;

        [Description("Weight of the effect that leaves the player with 1 HP.")]
        public int OneHpLeftChance { get; set; } = 15;

        [Description("Weight of the inventory reset effect.")]
        public int InventoryResetChance { get; set; } = 20;

        [Description("Weight of the instant grenade effect.")]
        public int InstantExplosionChance { get; set; } = 10;

        [Description("Fuse time of the instant grenade in seconds.")]
        public float InstantExplosionFuseTime { get; set; } = 0.1f;

        [Description("Weight of the random room teleport effect.")]
        public int RandomTeleportChance { get; set; } = 15;

        [Description("Whether this plugin replaces map loot with coins. Leave false when SCP500s owns loot replacement, otherwise both plugins compete for the same pickups and neither ratio holds.")]
        public bool SpawnCoinsOnMap { get; set; } = false;

        [Description("Number of coins to spawn across the map.")]
        public int MapCoinAmount { get; set; } = 6;

        [Description("Delay before spawning coins after the round starts, in seconds.")]
        public float CoinSpawnDelay { get; set; } = 4f;

        [Description("Whether medkits can be replaced with coins.")]
        public bool ReplaceMedkitsWithCoins { get; set; } = true;

        [Description("Whether painkillers can be replaced with coins.")]
        public bool ReplacePainkillersWithCoins { get; set; } = true;

        [Description("Whether low-level keycards can be replaced with coins.")]
        public bool ReplaceWeakKeycardsWithCoins { get; set; } = true;

        [Description("Whether flashlights can be replaced with coins.")]
        public bool ReplaceFlashlightsWithCoins { get; set; } = true;
    }
}