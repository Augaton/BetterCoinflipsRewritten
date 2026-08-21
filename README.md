# BetterCoinflipsRewritten 2.1


> Portage EXILED 9.14.2 d'un plugin de **Mikihero, Matisiowy**. Depot non affilie a
> l'auteur d'origine. Voir [NOTICE.md](NOTICE.md) pour l'attribution.

Pile ou face : la piece declenche un effet benefique ou nefaste.

**EXILED 9.14.2** — `dotnet build -c Release BetterCoinflipsRewritten.csproj`

## Consommation de la piece

`consume_coin_on_flip` est a **`true`** par defaut : la piece est detruite apres
le lancer. Un joueur ne peut donc plus rester assis sur une piece a enchainer
les tirages jusqu'a decrocher le bon effet, ce que le cooldown par joueur ne
faisait que ralentir.

La destruction est **differee** de `coin_consume_delay` (1,5 s par defaut).
Retirer l'objet pendant l'event lui-meme casse l'animation client et laisse le
jeu manipuler un item detruit. Ne pas descendre sous la seconde.

Lacher la piece pendant ce delai ne la sauve pas : si elle n'est plus dans
l'inventaire, le pickup correspondant est detruit au sol par son numero de
serie.

Le seul moyen de finir un lancer avec plus de pieces qu'au depart est l'effet
`double_coin_chance`, volontairement rare.

## Spawn des pieces

Deux mecanismes distincts, qui ne font pas la meme chose.

### `add_bonus_coins_to_loot` (nouveau, `true` par defaut)

Ajoute `bonus_coin_amount` pieces **a cote** du loot existant, sans rien
detruire. C'est la contrepartie de la consommation : les pieces brulent a
l'usage, il en faut donc davantage sur la carte.

`bonus_coin_source_items` liste les pools de loot qui recoivent une piece en
plus. `bonus_coins_per_source_type` plafonne le nombre de pieces posees par type
d'objet, pour ne pas concentrer les dix pieces sur les seuls medkits.

Comme rien n'est detruit, ce mode **cohabite avec SCP500s** : les deux plugins
ne se disputent aucun pickup.

### `spawn_coins_on_map` (`false` par defaut)

Remplacement pur et simple du loot par des pieces. Le remplacement appartient a
SCP500s, qui place pieces et pilules avec un ratio unique (10 % piece, 90 %
pilule).

Ne repasser cette option a `true` que si SCP500s est absent ou si son mode de
spawn est `SpawnPoints`. Sinon les deux plugins se disputent les memes pickups
et aucun des deux ratios ne tient.

Les largages de **SupplyDrop** embarquent egalement deux pieces par caisse, MTF
comme Chaos.

## Effets

Les effets saisonniers d'EXILED (`BecomingFlamingo`, `Prismatic`, `Metal`,
`SugarRush`) sont **inutilisables** : le jeu ne les applique que pendant
Halloween, Noel ou le 1er avril. Aucun effet du plugin ne s'appuie dessus,
sinon un tirage sur trois ne ferait rien onze mois par an.

### Face

Carte d'acces, medkit, teleportation a l'evasion, soin, vie maximale, SCP-268,
Logicer a une balle, SCP-2176, bonbon rose, changement de taille, bouclier
temporaire, fantome, montee d'adrenaline, piece dedoublee.

### Pile

Perte de vie, cellules Classe-D, coupure de lumiere, grenade aveuglante, 1 PV,
inventaire vide, grenade instantanee, teleportation aleatoire, ivresse, trip
arc-en-ciel, amnesie, projection en l'air, echange de position avec un autre
joueur, fausse detonation d'ogive, tantrum SCP-173, petrification, lumieres en
mode discotheque, invasion de souris, annonce C.A.S.S.I.E. moqueuse, fausse
explosion.

`disco_lights`, `mice_invasion` et `cassie_mock` touchent tout le serveur : ils
passent par `facility_effect_cooldown`, comme `lights_out`.

`position_swap` deplace un second joueur, qui recoit son propre message. Son
poids reste bas pour cette raison.

Les annonces de `cassie_mock_lines` sont validees au chargement : une ligne que
le jeu refuse est signalee dans les logs et n'est jamais jouee.

## Affichage

Les messages passent par **HintServiceMeow** sur la ligne
`hint_y_coordinate` (750 par defaut), pour ne pas ecraser ceux des autres
plugins. Repli automatique sur les hints natifs si le service est absent.

Les textes joueur sont integralement en francais dans le fichier de traduction.

## Configuration notable

| Cle | Defaut | Role |
|---|---|---|
| `coin_cooldown` | `5` | Cooldown par joueur entre deux lancers |
| `log_flips` | `false` | Journalise chaque lancer. Une ecriture disque par lancer |
| `facility_effect_cooldown` | `60` | Cooldown **serveur** des effets a portee globale |
| `spawn_coins_on_map` | `false` | Voir ci-dessus |
| `consume_coin_on_flip` | `true` | Detruit la piece apres le lancer |
| `coin_consume_delay` | `1.5` | Delai avant destruction, laisse l'animation finir |
| `add_bonus_coins_to_loot` | `true` | Ajoute des pieces sans detruire de loot |
| `bonus_coin_amount` | `10` | Nombre de pieces ajoutees |

Les poids de chaque effet et les tunables associes restent configurables
individuellement. Les textes joueur sont dans le fichier de traduction.

## Passe d'optimisation

- `SelectEffect` n'alloue plus. Il utilisait `.Where().ToList()` puis `.Sum()`
  a chaque lancer ; il remplit maintenant un tampon preexistant.
- `RandomTeleportEffect` s'appuie sur un cache de salles reconstruit au debut du
  round. Il faisait auparavant un `Room.List.Where(...).ToList()` complet dans
  `CanApply`, donc a chaque lancer, puis un second dans `Execute`.
- `MedicalKitEffect` et `CoinSpawnService` n'utilisent plus LINQ.
- Le `Log.Info` par lancer est passe derriere `log_flips`.
- Les quatre `System.Random` instancies a quelques microsecondes d'intervalle
  sont remplaces par une instance partagee. Sur .NET Framework, la graine derive
  du compteur de ticks : deux instances proches peuvent produire la meme suite.
- La coroutine de spawn est tuee au redemarrage de round et a la desactivation.

## Cycle de vie

Les effets differes (destruction de la piece, retour des lumieres, fin de
l'ecran noir) passent par `API.Scheduler`, qui garde les handles MEC et les tue
au `RoundStarted`, au `RestartingRound` et dans `OnDisabled`. Aucune coroutine
ne survit a un `reload`.

## Passe de securite

- `LightsOutEffect` coupait toute la facility depuis la piece d'un seul joueur,
  sans cooldown global : plusieurs joueurs pouvaient enchainer les extinctions
  en continu. Il passe maintenant par `facility_effect_cooldown`, un verrou
  serveur et non par joueur.
- Suppression du code mort : `Commands/CoinUsesCommand.cs` (classe vide
  compilee dans la DLL), `ForceRespawnChance` (declare, jamais consomme) et
  l'abonnement `Verified` qui ne faisait rien.
- Ajout de `Prefix`, pour que la section de config ne depende plus du `Name`.

## Point restant

`InstantExplosionEffect` attribue la grenade au lanceur via
`SpawnActive(position, player)`. Les coequipiers tues dans le rayon comptent
donc comme des teamkills de sa part, ce qui peut declencher un autoban
anti-TK selon les plugins installes. Non corrige : le comportement souhaite
depend de votre moderation.

## Dependances

Ce plugin depend de **AugatonLib**, la bibliotheque partagee de la
collection.

| Fichier | Destination |
|---|---|
| `BetterCoinflipsRewritten.dll` | `Plugins/7777/` |
| `AugatonLib.dll` | `Plugins/dependencies/` |
| HintServiceMeow | `Plugins/7777/` |

`AugatonLib.dll` ne va **jamais** dans `Plugins/7777/` : EXILED
tenterait de le charger comme plugin. Il doit etre deploye avant ce plugin et
mis a jour en meme temps.

Pour compiler ce depot isolement, cloner
[AugatonLib](https://github.com/Augaton/AugatonLib) a cote,
ou passer `-p:CommonProject=chemin/vers/AugatonLib.csproj`.

## Installation depuis une release

Chaque tag `v*` declenche une release qui publie une archive **contenant deja
AugatonLib**. Extraire `BetterCoinflipsRewritten.zip` dans `.config/EXILED/` :

```
Plugins/7777/BetterCoinflipsRewritten.dll
Plugins/dependencies/AugatonLib.dll
```

Les DLL sont aussi publiees separement pour une mise a jour ciblee.

Si plusieurs plugins de la collection sont installes, garder la version
d'AugatonLib la plus recente : elle est partagee par tous.

## Integration continue

`build` et `release` sont deux **workflows distincts**.

| Workflow | Declencheur | Produit |
|---|---|---|
| `build` | push sur `main`, pull request | Compile, cree le tag si besoin, declenche la release |
| `release` | tag `v*`, ou lancement manuel | Une **release** avec la DLL et l'archive |

### Publication automatique

Le job `tag` de `build` lit la balise `<Version>` du `.csproj`. Si le tag
`v<version>` n'existe pas encore, il le cree et declenche `release`.

Publier revient donc a **incrementer `<Version>` dans le `.csproj`** puis
pousser sur `main`. Sans changement de version, aucun tag n'est cree et aucune
release n'est publiee : les commits de correction ne generent pas de bruit.

La version affichee par `status` en jeu est lue dans l'assembly, elle-meme
issue de cette meme balise. Le tag, la DLL et l'affichage en jeu ne peuvent
donc pas diverger.

### Publication manuelle

```bash
git tag v1.0.0 && git push origin v1.0.0
```

Ou onglet Actions, workflow `release`, bouton **Run workflow** : le tag est cree
s'il n'existe pas.

La CI recupere AugatonLib par `actions/checkout` sur le depot
[Augaton/AugatonLib](https://github.com/Augaton/AugatonLib), branche `main` par
defaut. Le declenchement manuel de `release` permet de fixer une autre version
via l'entree `augatonlib_ref`.

Gitleaks scanne l'historique complet a chaque push et bloque en cas de secret
detecte. Il est invoque en binaire plutot que via son action GitHub : l'action
calcule une plage de commits `<precedent>^..<actuel>` qui echoue sur le commit
initial d'un depot.
