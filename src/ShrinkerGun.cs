using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using ScalerCore;
using UnityEngine;

namespace ShrinkerGun
{
    [BepInPlugin("Vippy.ShrinkerGun", "ShrinkerGun", BuildInfo.Version)]
    [BepInDependency("Vippy.ScalerCore", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log = null!;
        internal static ScaleOptions ShrinkOptions;
        internal static float _enemyDuration;
        internal static float _itemDuration;
        static ConfigEntry<bool> _enableDebugKeys = null!;

        void Awake()
        {
            Log = Logger;

            ShrinkOptions = new ScaleOptions
            {
                Factor = Config.Bind("Scaling", "ShrinkFactor", 0.4f,
                    new ConfigDescription("Scale multiplier applied when shrunk (0.4 = 40% of original).",
                        new AcceptableValueRange<float>(0.1f, 1f))).Value,
                Speed = Config.Bind("Scaling", "ShrinkSpeed", 2.0f,
                    new ConfigDescription("Animation speed. Scaled by the object's original size.",
                        new AcceptableValueRange<float>(0.1f, 10f))).Value,
                DamageMultiplier = Config.Bind("Scaling", "EnemyDamageMultiplier", 0.1f,
                    new ConfigDescription("Damage multiplier for shrunken enemies (0.1 = 10%).",
                        new AcceptableValueRange<float>(0f, 1f))).Value,
                SpeedFactor = Config.Bind("Scaling", "EnemySpeedFactor", 0.65f,
                    new ConfigDescription("Movement speed multiplier for shrunken enemies.",
                        new AcceptableValueRange<float>(0.1f, 2f))).Value,
                Duration = 0f,
                BonkImmuneDuration = 5f,
                MassCap = 5f,
                AnimSpeedMultiplier = 1.5f,
                FootstepPitchMultiplier = 1.5f,
                AllowedTargets = ScaleTargets.All,
                InvertedMode = false,
            };

            _enemyDuration = Config.Bind("Scaling", "EnemyShrinkDuration", 120f,
                new ConfigDescription("Seconds until a shrunken enemy auto-restores. 0 = never.",
                    new AcceptableValueRange<float>(0f, 600f))).Value;
            _itemDuration = Config.Bind("Scaling", "ItemShrinkDuration", 300f,
                new ConfigDescription("Seconds until a shrunken item auto-restores. 0 = never.",
                    new AcceptableValueRange<float>(0f, 600f))).Value;

            _enableDebugKeys = Config.Bind("Debug", "EnableDebugKeys", false,
                "Enable F9/F10 keys to shrink/unshrink yourself. Off by default.");

            new Harmony("Vippy.ShrinkerGun").PatchAll();
        }

        void Update()
        {
            if (!_enableDebugKeys.Value) return;

            var player = PlayerAvatar.instance;
            if (player == null) return;
            var ctrl = player.GetComponent<ScaleController>();
            if (ctrl == null) return;

            bool isLocal = !PhotonNetwork.InRoom || (player.photonView != null && player.photonView.IsMine);
            if (!isLocal) return;

            if (!ctrl.IsScaled && Input.GetKeyDown(KeyCode.F9))
                ctrl.RequestManualShrink();
            if (ctrl.IsScaled && Input.GetKeyDown(KeyCode.F10))
                ctrl.RequestManualExpand();
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
    }
}
