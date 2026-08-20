# BetterCoinflipsRewritten 2.0


> Portage EXILED 9.14.2 d'un plugin de **Mikihero, Matisiowy**. Depot non affilie a
> l'auteur d'origine. Voir [NOTICE.md](NOTICE.md) pour l'attribution.

Pile ou face : la piece declenche un effet benefique ou nefaste.

**EXILED 9.14.2** — `dotnet build -c Release BetterCoinflipsRewritten.csproj`

## Spawn des pieces

`spawn_coins_on_map` est desormais a **`false`** par defaut. Le remplacement du
loot appartient a SCP500s, qui place pieces et pilules avec un ratio unique
(10 % piece, 90 % pilule).

Ne repasser cette option a `true` que si SCP500s est absent ou si son mode de
spawn est `SpawnPoints`. Sinon les deux plugins se disputent les memes pickups
et aucun des deux ratios ne tient.

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
