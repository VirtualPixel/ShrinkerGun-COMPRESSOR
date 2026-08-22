namespace ShrinkerGun
{
    // Per-shot battery drain for a configured shot count. No game types, so the
    // test project can compile this file on its own.
    static class BatteryMath
    {
        // The meter decides when a gun is out of ammo, not the raw battery value.
        // ItemBattery.Update rounds batteryLife (0..100) to whole bars and
        // ItemGun.Shoot refuses to fire at zero bars, so the bottom half bar is
        // dead weight. Draining a flat 100 / uses per shot spends that dead half
        // bar as if it were ammo and comes up short: 92 shots on a six-bar gun
        // asked for 100, 46 asked for 50.
        //
        // Aim the last shot at the middle of the dead band instead. The shot
        // before it still rounds up to one bar so the gun fires; the shot after
        // it rounds down to none so the gun stops, and half a bar of slack on
        // either side keeps float drift from costing or gifting a shot.
        public static float ShotDrain(int uses, int bars)
        {
            if (uses < 1) uses = 1;
            if (bars < 1) bars = 1;
            float deadBand = 100f / (2f * bars);
            return (100f - deadBand) / (uses - 0.5f);
        }
    }
}
