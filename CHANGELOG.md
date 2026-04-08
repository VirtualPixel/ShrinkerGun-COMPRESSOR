# Changelog

## 0.4.3

- **New:** `LevelCollapse` setting (Chaos section) — shoot the map with the shrink gun. Auto = April 1st only, On = always, Off = never.
- **Moved:** `ShrinkChallengeMode` and all user-facing settings now live here
- **Fixed:** Map collapse audio respects master volume
- **Fixed:** Pocketed item icons no longer disappear after level transitions
- **Fixed:** Camera glitch effect fills the full screen while shrunken
- **Improved:** Pocketed items generate icons at runtime (no more embedded PNGs)
- **Improved:** Map collapse crush feels heavier — FOV slam, shake, and a brief hold before death
- **Improved:** Map collapse enemies are slightly faster instead of comically fast
- **Updated:** Requires ScalerCore 0.4.3

## 0.4.1

- **New:** Shrunken items that aren't normally pocketable (carts, cart cannon, cart laser, tracker) can now be pocketed
- **New:** Shrunken players can't pocket shrunken items — pocketed ones drop if you get shrunk
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
- **Fixed:** Players no longer revert when someone jumps or tumbles into them — only real damage triggers bonk
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

---

## 0.1.1

- **Fixed:** Missing the correct repobundle 

---

## 0.1.0

Initial early access release.
