using System;
using Xunit;

namespace ShrinkerGun.Tests
{
    public class BatteryMathTests
    {
        // Mirror of the two bits of game code that decide when a gun is out of ammo:
        //   ItemBattery.Update  batteryLifeInt = (int)Mathf.Round(batteryLife / (100f / batteryBars))
        //   ItemGun.Shoot       refuses to fire while batteryLifeInt <= 0
        //   ItemGun.ShootRPC    batteryLife -= batteryDrain   (fractional-drain mode)
        // Mathf.Round is Math.Round(double), so half values go to even, same as here.
        static int ShotsFromFullBattery(float drain, int bars)
        {
            float life = 100f;
            int shots = 0;
            while (shots < 10000)
            {
                int bar = (int)(float)Math.Round((double)(life / (100f / bars)));
                if (bar <= 0) break;
                life -= drain;
                shots++;
            }
            return shots;
        }

        public static TheoryData<int, int> BarsAndUses()
        {
            var data = new TheoryData<int, int>();
            foreach (int bars in new[] { 2, 4, 6, 8, 10, 12 })
                foreach (int uses in new[] { 1, 2, 3, 5, 10, 12, 20, 24, 37, 50, 60, 99, 100 })
                    data.Add(bars, uses);
            return data;
        }

        [Theory]
        [MemberData(nameof(BarsAndUses))]
        public void Drain_gives_exactly_the_configured_shots(int bars, int uses)
        {
            float drain = BatteryMath.ShotDrain(uses, bars);
            Assert.Equal(uses, ShotsFromFullBattery(drain, bars));
        }

        [Theory]
        [InlineData(6, 100, 92)]
        [InlineData(6, 50, 46)]
        [InlineData(6, 24, 23)]
        // The obvious 100 / uses spends the meter's dead half bar as if it were
        // ammo, so the gun runs dry early. This is what ShotDrain exists to avoid.
        public void Flat_drain_comes_up_short(int bars, int uses, int expected)
        {
            Assert.Equal(expected, ShotsFromFullBattery(100f / uses, bars));
        }

        [Theory]
        [InlineData(0, 6)]
        [InlineData(-5, 6)]
        [InlineData(10, 0)]
        public void Nonsense_input_still_returns_a_usable_drain(int uses, int bars)
        {
            float drain = BatteryMath.ShotDrain(uses, bars);
            Assert.True(drain > 0f && !float.IsInfinity(drain));
        }
    }
}
