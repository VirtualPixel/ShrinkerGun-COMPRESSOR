# ShrinkerGun: COMPRESSOR

A shrink ray for R.E.P.O. Shoot anything to shrink it. Shoot it again to restore it.

Enemies, valuables, items, players -- if you can hit it, you can shrink it.

## What happens when you shrink stuff

**Enemies** get tiny, squeaky, and mostly harmless. Their damage drops to 10%, they slow down, and their grab force goes to zero. That means you can pick them up. With your bare hands. And throw them at a wall. Or at your friends. A shrunken Huntsman is just a very angry football.

**Valuables** shrink down for easy transport. No more wedging that oversized painting through a doorway -- just zap it and toss it in the cart. Shrunken valuables stay small until they take damage (drop them too hard and they pop back to full size). They're briefly invulnerable after shrinking, so the scale change itself won't break anything.

**Players** can shrink each other. Tiny players get squeaky voices, big anime eyes, faster animations, and adjusted camera/collision so it actually feels right. Walk under doors you'd normally have to crouch through. Take damage to pop back, or press F10 to manually restore.

**Items** shrink too. Grenades get smaller explosions. Orbs get smaller radius. The gun itself can be shrunk by another player's shot (but won't shrink itself).

## Features

- Purchasable shrink ray available in the in-game shop
- Toggle shrink: first shot shrinks, second shot restores
- Smooth scale animation with physics impact effects
- All audio pitch-shifted (shrunken enemies sound ridiculous)
- Shrunken enemies are instantly grabbable at 0 grab force
- Enemy damage and knockback scaled down proportionally
- Enemies auto-restore after 2 minutes (configurable)
- Valuables stay shrunken until damaged or end of round
- Player shrink with full camera/collision/movement adjustment
- Works on doors too, why not
- Multiplayer synced -- all players see the same thing
- Late-join support -- players joining mid-game see correct sizes

## Why a shrink gun?

Shrinker cart mods like ScaleInCart and SylhShrinkerCartPlus are great for automatic loot management. The COMPRESSOR is a different flavor -- an active tool that gives you direct control, and works on more than just valuables. They pair well together.

- Shrink a valuable before you even pick it up -- carry it easily through tight corridors
- Shrink an enemy mid-chase and watch it become a squeaky little menace you can punt across the room
- Shrink your friend for laughs (they'll have a squeaky voice and everything)
- Shrink the extraction haul early so you're not fumbling at the truck

If you use a shrinker cart for fitting loot into the cart, the COMPRESSOR handles the rest: tiny enemies, shrunken players, scale control over anything you can shoot.

## Configuration

Scaling behavior is built into the gun — it uses hardcoded `ScaleOptions` values tuned for the best gameplay feel:

| Behavior | Value | Notes |
|----------|-------|-------|
| Scale factor | 40% | Objects shrink to 40% of original size |
| Enemy duration | 120s | Enemies auto-restore after 2 minutes |
| Valuable duration | Permanent | Stay shrunken until damaged or round ends |
| Item duration | 300s | Items auto-restore after 5 minutes |
| Player duration | Permanent | Stay shrunken until bonked or F10 |
| Enemy damage | 10% | Shrunken enemies deal reduced damage |
| Enemy speed | 65% | Shrunken enemies move slower |

ScalerCore has one user-facing setting in `ScalerCore.cfg`:

| Setting | Default | What it does |
|---------|---------|-------------|
| ShrinkChallengeMode | false | Players start shrunken. Guns temporarily grow you; damage shrinks you back. |

## Multiplayer

Works in multiplayer. All players need both ShrinkerGun and ScalerCore installed. Scaling state is synced via Photon RPCs. Players joining mid-game will see the correct sizes for everything that's already been shrunk.

## Known Issues

- Loom (Shadow) arms may look off while shrunken (cosmetic — attack distance still scales correctly)
- Some untested enemy types may float or clip slightly while shrunken
- The gun uses base gun visuals (no custom model yet)

Report bugs on the [GitHub issues page](https://github.com/VirtualPixel/ShrinkGun-COMPRESSOR/issues).

## Dependencies

- [BepInEx 5](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/) (5.4.2100+)
- [REPOLib](https://thunderstore.io/c/repo/p/Zehs/REPOLib/) (3.0.3+)
- [ScalerCore](https://thunderstore.io/c/repo/p/Vippy/ScalerCore/) (0.3.0+)

## Credits

Made by Vippy. ScalerCore is the shared scaling engine; ShrinkerGun is the weapon that uses it. Both are open source.
