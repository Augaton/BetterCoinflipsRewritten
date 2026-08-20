using System.ComponentModel;
using Exiled.API.Interfaces;

namespace BetterCoinflipsRewritten
{
    public sealed class Translation : ITranslation
    {
        [Description("Titre affiche au-dessus du resultat du lancer.")]
        public string CoinFlipTitle { get; set; } =
            "<b><color=yellow>PILE OU FACE</color></b>";

        [Description("Message affiche quand aucun effet valide ne peut etre selectionne.")]
        public string NoAvailableEffect { get; set; } =
            "<color=white>Aucun effet disponible n'a pu etre selectionne.</color>";

        [Description("Message affiche quand l'execution d'un effet echoue.")]
        public string EffectError { get; set; } =
            "<b><color=red>Erreur d'effet de piece</color></b>\n" +
            "<color=white>Merci de signaler ce probleme a l'administration du serveur.</color>";

        [Description("Message de cooldown. {seconds} est remplace par le temps restant.")]
        public string CooldownMessage { get; set; } =
            "<b><color=red>La piece est en cooldown</color></b>\n" +
            "<color=white>Temps restant : </color>" +
            "<color=yellow>{seconds} secondes</color>";

        [Description("Ligne de resultat. {result} est remplace par pile ou face.")]
        public string ResultFormat { get; set; } =
            "<color=white>Resultat :</color> {result}";

        [Description("Ligne d'effet. {effect} est remplace par le message de l'effet selectionne.")]
        public string EffectFormat { get; set; } =
            "<color=white>Effet :</color> {effect}";

        [Description("Nom affiche pour face.")]
        public string HeadsName { get; set; } =
            "<color=green>Face</color>";

        [Description("Nom affiche pour pile.")]
        public string TailsName { get; set; } =
            "<color=red>Pile</color>";

        [Description("Message de soin. {amount} est remplace par les points de vie rendus.")]
        public string HealEffectMessage { get; set; } =
            "La piece vous a rendu {amount} PV.";

        [Description("Message de vie maximale. {percentage} est remplace par le pourcentage d'augmentation.")]
        public string MoreHealthEffectMessage { get; set; } =
            "Votre vie maximale a augmente de {percentage} %.";

        [Description("Message de repli pour l'effet de carte aleatoire.")]
        public string RandomKeycardEffectMessage { get; set; } =
            "Vous avez recu une carte d'acces aleatoire.";

        [Description("Message de carte aleatoire. {keycard} est remplace par le nom de la carte tiree.")]
        public string RandomKeycardSelectedMessage { get; set; } =
            "Vous avez recu une {keycard}.";

        [Description("Nom affiche de la carte d'agent d'entretien.")]
        public string JanitorKeycardName { get; set; } =
            "Carte d'Agent d'Entretien";

        [Description("Nom affiche de la carte de scientifique.")]
        public string ScientistKeycardName { get; set; } =
            "Carte de Scientifique";

        [Description("Nom affiche de la carte de responsable de zone.")]
        public string ZoneManagerKeycardName { get; set; } =
            "Carte de Responsable de Zone";

        [Description("Nom affiche de la carte de garde.")]
        public string GuardKeycardName { get; set; } =
            "Carte de Garde";

        [Description("Nom affiche de la carte de coordinateur de recherche.")]
        public string ResearchCoordinatorKeycardName { get; set; } =
            "Carte de Coordinateur de Recherche";

        [Description("Nom affiche de la carte d'ingenieur de confinement.")]
        public string ContainmentEngineerKeycardName { get; set; } =
            "Carte d'Ingenieur de Confinement";

        [Description("Message quand un medkit est ajoute a l'inventaire.")]
        public string MedicalKitEffectMessage { get; set; } =
            "Vous avez recu un medkit.";

        [Description("Message quand un medkit est lache au sol parce que le joueur en avait deja un.")]
        public string MedicalKitDroppedMessage { get; set; } =
            "Vous aviez deja un medkit, un second a ete lache a vos pieds.";

        [Description("Message de recompense SCP-268.")]
        public string HatEffectMessage { get; set; } =
            "Vous avez recu le SCP-268.";

        [Description("Message de recompense SCP-2176.")]
        public string LightbulbEffectMessage { get; set; } =
            "Vous avez recu le SCP-2176.";

        [Description("Message de recompense du Logicer a une balle.")]
        public string OneAmmoLogicerEffectMessage { get; set; } =
            "Vous avez recu un Logicer avec une seule balle.";

        [Description("Message de recompense du bonbon rose.")]
        public string PinkCandyEffectMessage { get; set; } =
            "Vous avez recu un bonbon rose.";

        [Description("Message de teleportation vers la zone d'evasion.")]
        public string TeleportToEscapeEffectMessage { get; set; } =
            "Vous avez ete teleporte vers la zone d'evasion.";

        [Description("Message de changement de taille. {size} est remplace par la taille tiree.")]
        public string SizeChangeEffectMessage { get; set; } =
            "Votre taille est passee a {size}.";

        [Description("Nom affiche de la petite taille.")]
        public string SmallSizeName { get; set; } =
            "petite";

        [Description("Nom affiche de la grande taille.")]
        public string LargeSizeName { get; set; } =
            "grande";

        [Description("Nom de taille par defaut.")]
        public string ChangedSizeName { get; set; } =
            "une taille differente";

        [Description("Message de perte de vie. {amount} est remplace par les points de vie retires.")]
        public string HpReductionEffectMessage { get; set; } =
            "Vous avez perdu {amount} PV.";

        [Description("Message de l'effet de grenade instantanee.")]
        public string InstantExplosionEffectMessage { get; set; } =
            "Une grenade a explose a vos pieds.";

        [Description("Message de reinitialisation de l'inventaire.")]
        public string InventoryResetEffectMessage { get; set; } =
            "Tout votre inventaire a ete supprime.";

        [Description("Message d'extinction des lumieres. {duration} est remplace par la duree.")]
        public string LightsOutEffectMessage { get; set; } =
            "Les lumieres du complexe ont ete coupees pendant {duration} secondes.";

        [Description("Message de l'effet laissant 1 PV.")]
        public string OneHpLeftEffectMessage { get; set; } =
            "La piece ne vous a laisse qu'un seul PV.";

        [Description("Message de teleportation aleatoire. {room} est remplace par le nom de la salle.")]
        public string RandomTeleportEffectMessage { get; set; } =
            "Vous avez ete teleporte dans une salle au hasard : {room}.";

        [Description("Nom de salle par defaut.")]
        public string UnknownRoomName { get; set; } =
            "salle inconnue";

        [Description("Message de teleportation vers les cellules des Classe-D.")]
        public string TeleportToClassDCellsEffectMessage { get; set; } =
            "Vous avez ete teleporte dans les cellules des Classe-D.";

        [Description("Message de l'effet de grenade aveuglante.")]
        public string TrollFlashEffectMessage { get; set; } =
            "Une grenade aveuglante est apparue a vos pieds.";
    }
}
