using BepInEx.Configuration;
using ScalerCore;
using UnityEngine;

namespace ShrinkerGun
{
    enum LevelCollapseMode { Auto, On, Off }
    enum GunMode { Shrink, Grow }

    // Default leaves the prefab's own setting untouched.
    public enum BatteryCharge { Default, Rechargeable, SingleUse }

    static class PluginConfig
    {
        internal static ConfigEntry<int> BatteryShotsPerCharge = null!;
        internal static ConfigEntry<BatteryCharge> BatteryRechargeable = null!;
        internal static ConfigEntry<bool> EnableDebugKeys = null!;
        internal static ConfigEntry<KeyCode> ShrinkKey = null!;
        internal static ConfigEntry<KeyCode> CollapseKey = null!;
        static ConfigEntry<GunMode> _gunMode = null!;
        static ConfigEntry<bool> _challengeMode = null!;
        static ConfigEntry<bool> _shrinkDeadHeads = null!;
        static ConfigEntry<LevelCollapseMode> _levelCollapse = null!;

        // What every shot applies. Follows Gun / Mode live.
        internal static ScaleOptions ShrinkOptions;

        internal static bool LevelCollapseEnabled => _levelCollapse.Value switch
        {
            LevelCollapseMode.On => true,
            LevelCollapseMode.Off => false,
            _ => System.DateTime.Now is { Month: 4, Day: 1 },
        };

        internal static void Init(ConfigFile config)
        {
            BatteryShotsPerCharge = config.Bind("Battery", "ShotsPerCharge", 0,
                "Requires a game restart to apply. The host's setting rules in multiplayer.\n" +
                "How many shots a full battery gives the shrink gun. The meter keeps its normal bars. " +
                "0 leaves the gun's built-in amount alone.");
            BatteryRechargeable = config.Bind("Battery", "Rechargeable", BatteryCharge.Default,
                "Requires a game restart to apply. The host's setting rules in multiplayer.\n" +
                "Whether the shrink gun recharges at a charging station. Default leaves the built-in setting.");

            _gunMode = config.Bind("Gun", "Mode", GunMode.Shrink,
                "What the gun does to whatever it hits. Shrink (default) makes things small; "
                + "Grow makes them big (twice size, heavier, with the matching audio and reach). "
                + "The host's setting rules in multiplayer.");
            ApplyGunMode();
            _gunMode.SettingChanged += (_, _) => ApplyGunMode();

            EnableDebugKeys = config.Bind("Debug", "EnableDebugKeys", false,
                "Turn on the debug keys below. Off by default.");
            ScaleController.AllowManualScale = EnableDebugKeys.Value;
            EnableDebugKeys.SettingChanged += (_, _) => ScaleController.AllowManualScale = EnableDebugKeys.Value;
            ShrinkKey = config.Bind("Debug", "ShrinkKey", KeyCode.F9,
                "Shrink or unshrink yourself.");
            CollapseKey = config.Bind("Debug", "CollapseKey", KeyCode.End,
                "Start the level collapse by hand. Host only, and only while LevelCollapse is active.");

            _challengeMode = config.Bind("Challenge", "ShrinkChallengeMode", false,
                "All players start shrunken. Shrink guns temporarily grow you back to full size. " +
                "Taking damage while full size shrinks you back down. Enemies behave normally.");
            ScaleController.ChallengeMode = _challengeMode.Value;
            _challengeMode.SettingChanged += (_, _) => ApplyChallengeMode();

            _shrinkDeadHeads = config.Bind("Targets", "ShrinkDeadHeads", false,
                "Let the shrink ray hit dead Semibot heads. Off by default: a shrunk head is easy to "
                + "lose and reviving from a pea is its own problem.");
            ScaleManager.AllowDeadHeads = _shrinkDeadHeads.Value;
            _shrinkDeadHeads.SettingChanged += (_, _) => ScaleManager.AllowDeadHeads = _shrinkDeadHeads.Value;

            _levelCollapse = config.Bind("Chaos", "LevelCollapse", LevelCollapseMode.Auto,
                "Shooting the map with the shrink gun triggers a 90-second collapse event. " +
                "Auto = April 1st only. On = always. Off = never.");
        }

        static void ApplyGunMode()
        {
            ShrinkOptions = _gunMode.Value == GunMode.Grow ? ScaleOptions.Growth : ScaleOptions.Default;
            Plugin.Log.LogInfo($"Gun mode: {_gunMode.Value}");
        }

        // Flipping the toggle in the lobby menu re-pitches everyone right away
        // instead of waiting for the next ScaleController to spawn.
        static void ApplyChallengeMode()
        {
            ScaleController.ChallengeMode = _challengeMode.Value;
            if (!SemiFunc.RunIsLobbyMenu()) return;
            foreach (var vc in Object.FindObjectsOfType<PlayerVoiceChat>())
            {
                if (_challengeMode.Value)
                    vc.OverridePitch(1.3f, 0.2f, 0.5f, 9999f);
                else
                    vc.OverridePitchCancel();
            }
        }
    }
}
