using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using ScalerCore;
using ScalerCore.AprilFools;
using UnityEngine;

namespace ShrinkerGun
{
    enum LevelCollapseMode { Auto, On, Off }

    [BepInPlugin("Vippy.ShrinkerGun", "ShrinkerGun", BuildInfo.Version)]
    [BepInDependency("Vippy.ScalerCore", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log = null!;
        internal static ScaleOptions ShrinkOptions;
        internal static float _enemyDuration = 120f;
        internal static float _itemDuration = 0f; // permanent until toggled
        static ConfigEntry<bool> _enableDebugKeys = null!;
        static ConfigEntry<bool> _growGunMode = null!;
        static ConfigEntry<bool> _challengeMode = null!;
        static ConfigEntry<bool> _shrinkDeadHeads = null!;
        static ConfigEntry<LevelCollapseMode> _levelCollapse = null!;

        internal static bool LevelCollapseEnabled => _levelCollapse.Value switch
        {
            LevelCollapseMode.On => true,
            LevelCollapseMode.Off => false,
            _ => System.DateTime.Now is { Month: 4, Day: 1 },
        };

        void Awake()
        {
            Log = Logger;

            _growGunMode = Config.Bind("Debug", "GrowGunMode", false,
                "TEMP testing toggle: the gun grows targets (ScaleOptions.Growth, factor 2) instead of "
                + "shrinking them, to prove ScalerCore handles growth end to end. Goes away once verified.");
            ApplyGunMode();
            _growGunMode.SettingChanged += (_, _) => ApplyGunMode();

            _enableDebugKeys = Config.Bind("Debug", "EnableDebugKeys", false,
                "Enable F9 to shrink/unshrink yourself. Off by default.");
            ScaleController.AllowManualScale = _enableDebugKeys.Value;
            _enableDebugKeys.SettingChanged += (_, _) => ScaleController.AllowManualScale = _enableDebugKeys.Value;

            _challengeMode = Config.Bind("Challenge", "ShrinkChallengeMode", false,
                "All players start shrunken. Shrink guns temporarily grow you back to full size. " +
                "Taking damage while full size shrinks you back down. Enemies behave normally.");
            ScaleController.ChallengeMode = _challengeMode.Value;
            _challengeMode.SettingChanged += (_, _) =>
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
            };

            _shrinkDeadHeads = Config.Bind("Targets", "ShrinkDeadHeads", false,
                "Let the shrink ray hit dead Semibot heads. Off by default: a shrunk head is easy to "
                + "lose and reviving from a pea is its own problem.");
            ScaleManager.AllowDeadHeads = _shrinkDeadHeads.Value;
            _shrinkDeadHeads.SettingChanged += (_, _) => ScaleManager.AllowDeadHeads = _shrinkDeadHeads.Value;

            _levelCollapse = Config.Bind("Chaos", "LevelCollapse", LevelCollapseMode.Auto,
                "Shooting the map with the shrink gun triggers a 90-second collapse event. " +
                "Auto = April 1st only. On = always. Off = never.");

            new Harmony("Vippy.ShrinkerGun").PatchAll();
        }

        static void ApplyGunMode()
        {
            ShrinkOptions = _growGunMode.Value ? ScaleOptions.Growth : ScaleOptions.Default;
            Log.LogInfo($"Gun mode: {(_growGunMode.Value ? "GROW (testing)" : "shrink")}");
        }

        void Update()
        {
            var player = PlayerAvatar.instance;
            if (player == null) return;
            var ctrl = player.GetComponent<ScaleController>();
            if (ctrl == null) return;

            if (Input.GetKeyDown(KeyCode.F9))
            {
                if (ctrl.IsScaled)
                    ctrl.RequestManualExpand();
                else
                    ctrl.RequestManualShrink();
            }

            // Debug trigger for level collapse (End key, only when set to On)
            if (Input.GetKeyDown(KeyCode.End) && _levelCollapse.Value == LevelCollapseMode.On)
            {
                if (LevelGenerator.Instance != null && LevelGenerator.Instance.Generated
                    && SemiFunc.IsMasterClientOrSingleplayer())
                    MapCollapse.OnMapHit();
            }
        }

        // Set PendingSourceCtrl before ShootBulletRPC instantiates the bullet so
        // ItemGunShrinkBullet.Awake (which fires synchronously inside Instantiate)
        // can capture which gun fired it and skip self-shrinking.
        [HarmonyPatch(typeof(ItemGun), nameof(ItemGun.ShootBulletRPC))]
        class GunSourcePatch
        {
            static void Prefix(ItemGun __instance)
            {
                ItemGunShrinkBullet.PendingSourceCtrl = __instance.GetComponent<ScaleController>();
            }
        }

        // Trigger level collapse when a shrink bullet hits the map (not a player/enemy/valuable).
        [HarmonyPatch(typeof(ItemGunBullet), nameof(ItemGunBullet.ActivateAll))]
        class LevelCollapseHitPatch
        {
            static void Postfix(ItemGunBullet __instance)
            {
                if (!LevelCollapseEnabled) return;
                if (!__instance.bulletHit) return;
                if (__instance.GetComponent<ItemGunShrinkBullet>() == null) return;

                int mask = (int)SemiFunc.LayerMaskGetPhysGrabObject() | LayerMask.GetMask("Enemy", "Player");
                foreach (var c in Physics.OverlapSphere(__instance.hitPosition, 0.3f, mask, QueryTriggerInteraction.Collide))
                    if (c.GetComponent<PlayerShrinkLink>()?.Controller != null || c.GetComponentInParent<ScaleController>() != null)
                        return;

                MapCollapse.OnMapHit();
            }
        }
    }
}
