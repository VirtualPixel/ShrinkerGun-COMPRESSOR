using HarmonyLib;

namespace ShrinkerGun
{
    // Default leaves the prefab's own setting untouched.
    public enum BatteryCharge { Default, Rechargeable, SingleUse }

    // The shrink gun's battery values live on the ItemBattery component baked
    // into the prefab, so they ship as defaults but aren't locked. These two
    // patches overwrite them from config at spawn, gated to our gun by the
    // ItemGunShrink marker so no other battery in the game is touched. Every
    // option has a "leave it alone" default, so the gun behaves exactly as the
    // bundle shipped until someone changes a value.

    // batteryBars (meter granularity) and isUnchargable (rechargeable) live on
    // ItemBattery. Awake runs before BatteryInit reads batteryBars, so set it
    // here and the init picks it up.
    [HarmonyPatch(typeof(ItemBattery), "Awake")]
    static class BatteryConfigPatch
    {
        static void Postfix(ItemBattery __instance)
        {
            if (__instance.GetComponent<ItemGunShrink>() == null) return;

            int bars = Plugin.BatteryBars.Value;
            if (bars > 0) __instance.batteryBars = bars;

            switch (Plugin.BatteryRechargeable.Value)
            {
                case BatteryCharge.Rechargeable: __instance.isUnchargable = false; break;
                case BatteryCharge.SingleUse:    __instance.isUnchargable = true;  break;
            }
        }
    }

    // ItemGun owns the per-shot drain. batteryLife runs 0..100 and a normal shot
    // subtracts batteryDrain, so 100 / drain is the shots a full battery gives.
    // Force fractional-drain mode and solve for the configured shot count.
    [HarmonyPatch(typeof(ItemGun), "Start")]
    static class GunBatteryDrainPatch
    {
        static void Postfix(ItemGun __instance)
        {
            if (__instance.GetComponent<ItemGunShrink>() == null) return;

            int uses = Plugin.BatteryShotsPerCharge.Value;
            if (uses <= 0) return;
            __instance.batteryDrainFullBar = false;
            __instance.batteryDrain = 100f / uses;
        }
    }
}
