# Changelog

## 0.6.0

- **Updated:** Requires ScalerCore 0.6.0. Loom's arms finally track a shrunken body. Held forceGrabPoint items (guns, melee, batteries) sit at eye height when shrunken instead of waist. Big API expansion that lets other shrinker cart and persistent-shrink mods drop their reflection workarounds.
- **Updated:** Requires REPOLib 4.0.2 (customize menu fix, meta save backup).

## 0.5.2

- **Updated:** Requires ScalerCore 0.5.2 (fixes a KeyNotFoundException in `GetBaseGrabStats` that was breaking voice cancel on unshrink and level transitions, plus a non-host cleanup hole that was leaving voices stuck pitched).
- **Updated:** Description refreshed to mention v0.4 content (cosmetic boxes, vehicles).

## 0.5.1

- **New:** Cosmetic boxes shrink now. They sat outside the previous handler predicates (no `ValuableObject`, no `ItemAttributes`) so the gun just bounced off them.
- **Fixed:** Shrinking the new `ItemVehicle` while a player was sitting in it shrunk the colliders and grab point but left the visible mesh full-size. The game deparents the mesh from the vehicle root when occupied, so it no longer inherits the root scale. Then expanding made the mesh balloon to about 2.5× original because the next reparent had inflated its localScale to compensate for the shrunken parent. Vehicles shrink and grow normally now whether occupied or not.
- **Fixed:** Shrunken vehicles wouldn't turn. Throttle worked, so they'd accelerate in whatever direction they were facing and there was no way to steer. The game's mount check uses a hardcoded distance that doesn't scale with the vehicle, so on a small scooter the seated player could never finish "mounting" in the game's eyes and steering input stayed clamped to zero.
- **Fixed:** Shrunken `Semiscooter` (the regular one) had no inventory icon when pocketed (the small variant did). The icon renderer was looking for visuals under the vehicle root, but the regular scooter deparents its mesh and the bounds calc missed it. Renders correctly now.
- **Fixed:** You could pocket a shrunken vehicle while you were also shrunken. That was supposed to be blocked the same way shrunken carts are: fits in your pocket only if you're full-size.
- **Fixed:** Carts could vanish on the second shrink. The icon-generation path had an order-of-operations bug that NRE'd inside the game's icon coroutine, which itself moves the item to `(-1000, -1000, -1000)` for rendering and was leaving it there. Carts stay alive across as many shrink/unshrink cycles as you want.
- **Improved:** Shrunken vehicles (`ItemVehicle`, `ValuableArcticSnowBike`) now drive at proportionally lower top speed instead of full speed on a tiny chassis. Vehicles stay pocketable when small (and only when you're full-size).
- **Updated:** Requires ScalerCore 0.5.1.
- **Updated:** Requires REPOLib 4.0.1.

## 0.5.0

- **Updated for R.E.P.O. v0.4:** Rebuilt against the latest game release. 0.4.4 will not load on v0.4.
- **Updated:** Requires ScalerCore 0.5.0 (multiplayer fix: map collapse messages, sirens, and the truck cascade were running way too fast)
- **Updated:** Requires REPOLib 4.0.0.

## 0.4.4

- **Fixed:** Non-host clients could trigger scale/restore logic directly, causing desyncs
- **Updated:** Requires ScalerCore 0.4.4

## 0.4.3

- **New:** `LevelCollapse` setting (Chaos section). Shoot the map with the shrink gun. Auto = April 1st only, On = always, Off = never.
- **Moved:** `ShrinkChallengeMode` and all user-facing settings now live here
- **Fixed:** Map collapse audio respects master volume
- **Fixed:** Pocketed item icons no longer disappear after level transitions
- **Fixed:** Camera glitch effect fills the full screen while shrunken
- **Improved:** Pocketed items generate icons at runtime (no more embedded PNGs)
- **Improved:** Map collapse crush feels heavier (FOV slam, shake, and a brief hold before death)
- **Improved:** Map collapse enemies are slightly faster instead of comically fast
- **Updated:** Requires ScalerCore 0.4.3

## 0.4.2

- Bumped ScalerCore dependency to 0.4.2

## 0.4.1

- **New:** Shrunken items that aren't normally pocketable (carts, cart cannon, cart laser, tracker) can now be pocketed
- **New:** Shrunken players can't pocket shrunken items. Pocketed ones drop if you get shrunk
- **New:** Shrunken items show as smaller dots on the map
- **Fixed:** Shrunken players can no longer wall-jump infinitely
- **Fixed:** Shrink Challenge mode works properly on clients and in singleplayer
- **Fixed:** Shrink Challenge mode no longer fires in the lobby
- **Fixed:** Shrink Challenge mode config toggles instantly in the lobby
- **Fixed:** Camera clipping through walls at shrunken size reduced
- **Fixed:** Items stay shrunken permanently until shot again (was 5 minutes)
- **Improved:** Enemy speed while shrunken is 75% (was 65%)
- **Improved:** F9 toggles shrink/unshrink (debug key, single key instead of two)
- **Improved:** Inventory icons for cart, cart cannon, and cart laser
- **Improved:** Cleaned up logging
- **Updated:** Requires ScalerCore 0.4.1

## 0.4.0

- **Fixed:** Shrunken objects now appear correctly on non-host clients (was showing full size due to missing RPC data)
- **Fixed:** Shrunken player mesh no longer freezes in place on the host's screen
- **Fixed:** Late-joining players see correct shrunken sizes
- **Fixed:** Players no longer revert when someone jumps or tumbles into them. Only real damage triggers bonk
- **Fixed:** Debug key toggle now reads live from config, no restart needed
- **Fixed:** Configuration section in README now shows correct values
- **Updated:** Requires ScalerCore 0.4.0

## 0.3.0

- **Updated:** Requires ScalerCore 0.3.0 with major enemy scaling improvements
- **Fixed:** HeartHugger visual alignment and gas pull distance
- **Fixed:** Birthday Boy balloons now shrink with the enemy
- **Fixed:** Enemies no longer respawn shrunken after being killed while scaled
- **Fixed:** Single doors break off cleanly instead of floating
- **Fixed:** Loom attack distance now scales properly
- **Improved:** Replaced most internal reflection with publicizer for better performance

## 0.2.0

- **Updated:** Dependency list
- **Updated:** Version number to be more dynamic

## 0.1.1

- **Fixed:** Missing the correct repobundle 

## 0.1.0

Initial early access release.
