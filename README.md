# ShrinkerGun: COMPRESSOR

A shrink ray you can buy in the shop. Shoot anything to shrink it. Shoot it again to restore it.

> **v0.7.0: now a grow gun too.** Flip `Gun / Mode` to Grow and the ray makes things big instead of small: enemies, valuables, even you, with the deeper audio and longer reach to match. Grown enemies still fit the level. Shrink is the default. Needs ScalerCore 1.0.4 and REPOLib 4.2.0.

Enemies, valuables, items, players -- if you can hit it, you can shrink it. Or, in Grow mode, supersize it.

<img src="https://raw.githubusercontent.com/VirtualPixel/ShrinkerGun-COMPRESSOR/main/media/shop_chaos.gif" width="800">

## What happens when you shrink stuff

**Enemies** get tiny, squeaky, and mostly harmless. They still chase you, but their damage and speed drop with their size. Their grab force goes to zero, so you can pick them up and throw them.

<img src="https://raw.githubusercontent.com/VirtualPixel/ShrinkerGun-COMPRESSOR/main/media/shrinking_enemy_picking_up.gif" width="800">

**Valuables** shrink for easy transport. No more struggling with oversized items through doorways. Shrink it and toss it in the cart. They stay small until they take damage, so carry them carefully.

<img src="https://raw.githubusercontent.com/VirtualPixel/ShrinkerGun-COMPRESSOR/main/media/small_player_carring_valuable_to_cart.gif" width="800">

Drop it too hard and it pops back to full size:

<img src="https://raw.githubusercontent.com/VirtualPixel/ShrinkerGun-COMPRESSOR/main/media/small_player_shrinking_valuable_carrying_gets_damaged_reverts.gif" width="800">

**Players** can shrink each other. Tiny players get squeaky voices, big anime eyes, and adjusted camera so it actually feels right. Walk under doors you'd normally crouch through. Take damage to pop back.

<img src="https://raw.githubusercontent.com/VirtualPixel/ShrinkerGun-COMPRESSOR/main/media/small_player_tumbling_through_doors.gif" width="800">

**Carts, cart cannons, and cart lasers** become pocketable when shrunken. Shrink one, press an inventory key, stash it. Pull it out later, still tiny. Shoot it again and it's full size on the ground.

<img src="https://raw.githubusercontent.com/VirtualPixel/ShrinkerGun-COMPRESSOR/main/media/small_player_pushing_cart.gif" width="800">

**Items** shrink too. Grenades get smaller explosions. Orbs get smaller radius. Even doors. Why not.

## Works great with cart mods

Already using [ScaleInCart](https://thunderstore.io/c/repo/p/BULLETBOT/ScaleInCart/) or [SylhShrinkerCartPlus](https://thunderstore.io/c/repo/p/Sylhaance/SylhShrinkerCartPlus/)? The COMPRESSOR handles everything those don't: enemies, players, doors, stuff that hasn't reached the cart yet. They work fine together.

- Shrink a valuable before you pick it up, then carry it through tight corridors
- Shrink an enemy mid-chase and punt it across the room
- Shrink your friend for laughs (squeaky voice and everything)
- Shrink the extraction haul early so you're not fumbling at the truck
- Pocket a shrunken cart for later

## Balance

It costs a weapon slot and uses ammo. Everything has counterplay:

| What you shrink | What keeps it fair |
|-----------------|-------------------|
| Enemies | Auto-restore after 2 minutes. Still fight back at reduced damage. |
| Valuables | Pop back to full size if you drop them too hard. |
| Players | Take any damage to restore. |
| Carts/items | Shrunken players can't pocket shrunken items. The cart drops if you get shrunk too. |

## Configuration

Settings (in-game config / `BepInEx/config`, host's settings rule in multiplayer):

| Setting | Default | What it does |
|---------|---------|--------------|
| `Gun / Mode` | Shrink | Shrink or Grow. Grow makes targets big instead of small. |
| `Targets / ShrinkDeadHeads` | Off | Let the ray hit dead Semibot heads. |
| `Battery / ShotsPerCharge` | 0 | Shots a full battery gives, up to 100. 0 leaves the built-in amount. Needs a restart. |
| `Battery / Rechargeable` | Default | Whether the gun tops up at a charging station. Default leaves the built-in setting. Needs a restart. |
| `Challenge / ShrinkChallengeMode` | Off | Everyone starts shrunken. The gun grows you back for a while, damage shrinks you again. |
| `Chaos / LevelCollapse` | Auto | Shooting the map starts a 90-second collapse. Auto = April 1st only, On = always, Off = never. |
| `Debug / EnableDebugKeys` | Off | Turns the two keys below on. |
| `Debug / ShrinkKey` | F9 | Shrink or unshrink yourself, no gun needed. |
| `Debug / CollapseKey` | End | Start the collapse by hand. Host only, and only while `LevelCollapse` is active. |

Hardcoded defaults:

| Behavior | Value |
|----------|-------|
| Scale factor | 40% shrink / 200% grow |
| Enemy restore | 2 minutes |
| Valuable restore | Permanent (until damaged) |
| Item restore | Permanent (until toggled) |
| Player restore | Permanent (until damaged) |
| Enemy damage | Scales with size (40%) |
| Enemy speed | 75% |

<img src="https://raw.githubusercontent.com/VirtualPixel/ShrinkerGun-COMPRESSOR/main/media/map_chaos.gif" width="800">

## Multiplayer

All players need ShrinkerGun and ScalerCore installed. Everything syncs over Photon RPCs, including late-join.

## Known Issues

- Loom (Shadow) arms may look off while shrunken (cosmetic, attack distance still scales)
- Some untested enemy types may float or clip slightly while shrunken

## Dependencies

- [BepInEx 5](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/) (5.4.2305+)
- [REPOLib](https://thunderstore.io/c/repo/p/Zehs/REPOLib/) (4.2.0+)
- [ScalerCore](https://thunderstore.io/c/repo/p/Vippy/ScalerCore/) (1.0.4+)

## Credits

Made by Vippy. [ScalerCore](https://thunderstore.io/c/repo/p/Vippy/ScalerCore/) is the scaling engine under the hood, open source if you want to build your own shrink/grow mod.

## Contact

| Purpose | Where |
|---|---|
| Bug reports and suggestions | [GitHub Issues](https://github.com/VirtualPixel/ShrinkerGun-COMPRESSOR/issues) |
| Questions, test builds, or just hanging out | [Vippy's Discord](https://discord.gg/kKqhck2NrP) |
| R.E.P.O. modding in general | [R.E.P.O. Modding Server](https://discord.gg/9fDzZ9sk95) |

Everything I make stays free. If one of these mods made your runs better and you feel like
saying thanks, there is a [Ko-fi](https://ko-fi.com/vippydev).
