# ShrinkerGun: COMPRESSOR

A shrink ray you can buy in the shop. Shoot anything to shrink it — shoot it again to restore it.

Enemies, valuables, items, players -- if you can hit it, you can shrink it.

<img src="PLACEHOLDER_URL/shop_chaos.gif" width="800">

## What happens when you shrink stuff

**Enemies** get tiny, squeaky, and mostly harmless. They still chase you, but their damage and speed drop with their size. Their grab force goes to zero — pick them up and throw them.

<img src="PLACEHOLDER_URL/shrinking_enemy_picking_up.gif" width="800">

**Valuables** shrink for easy transport. No more struggling with oversized items through doorways — shrink it and toss it in the cart. They stay small until they take damage, so carry them carefully.

<img src="PLACEHOLDER_URL/small_player_carring_valuable_to_cart.gif" width="800">

Drop it too hard and it pops back to full size:

<img src="PLACEHOLDER_URL/small_player_shrinking_valuable_carrying_gets_damaged_reverts.gif" width="800">

**Players** can shrink each other. Tiny players get squeaky voices, big anime eyes, and adjusted camera so it actually feels right. Walk under doors you'd normally crouch through. Take damage to pop back.

<img src="PLACEHOLDER_URL/small_player_tumbling_through_doors.gif" width="800">

**Carts, cart cannons, and cart lasers** become pocketable when shrunken. Shrink one, press an inventory key, stash it. Pull it out later — still tiny. Shoot it again and it's full size on the ground.

<img src="PLACEHOLDER_URL/small_player_pushing_cart.gif" width="800">

**Items** shrink too. Grenades get smaller explosions. Orbs get smaller radius. Even doors. Why not.

## Works great with cart mods

Already using [ScaleInCart](https://thunderstore.io/c/repo/p/BULLETBOT/ScaleInCart/) or [SylhShrinkerCartPlus](https://thunderstore.io/c/repo/p/Sylhaance/SylhShrinkerCartPlus/)? The COMPRESSOR handles everything those don't — enemies, players, doors, stuff that hasn't reached the cart yet. They work fine together.

- Shrink a valuable before you pick it up — carry it through tight corridors
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
| Carts/items | Shrunken players can't pocket shrunken items — the cart drops if you get shrunk too. |

## Configuration

Hardcoded defaults:

| Behavior | Value |
|----------|-------|
| Scale factor | 40% of original size |
| Enemy restore | 2 minutes |
| Valuable restore | Permanent (until damaged) |
| Item restore | Permanent (until toggled) |
| Player restore | Permanent (until damaged) |
| Enemy damage | Scales with size (40%) |
| Enemy speed | 75% |

ScalerCore has one extra setting in `ScalerCore.cfg`:

| Setting | Default | What it does |
|---------|---------|-------------|
| ShrinkChallengeMode | false | Everyone starts shrunken. Guns temporarily grow you. Damage shrinks you back. |

<img src="PLACEHOLDER_URL/map_chaos.gif" width="800">

## Multiplayer

All players need ShrinkerGun and ScalerCore installed. Everything syncs over Photon RPCs, including late-join.

## Known Issues

- Loom (Shadow) arms may look off while shrunken (cosmetic — attack distance still scales)
- Some untested enemy types may float or clip slightly while shrunken

Report bugs on the [GitHub issues page](https://github.com/VirtualPixel/ShrinkGun-COMPRESSOR/issues).

## Dependencies

- [BepInEx 5](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/) (5.4.2100+)
- [REPOLib](https://thunderstore.io/c/repo/p/Zehs/REPOLib/) (3.0.3+)
- [ScalerCore](https://thunderstore.io/c/repo/p/Vippy/ScalerCore/) (0.4.1+)

## Credits

Made by Vippy. [ScalerCore](https://thunderstore.io/c/repo/p/Vippy/ScalerCore/) is the scaling engine under the hood — open source if you want to build your own shrink/grow mod.
