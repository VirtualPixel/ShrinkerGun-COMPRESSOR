using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using ScalerCore;
using UnityEngine;

namespace ShrinkerGun
{
    [BepInPlugin("Vippy.ShrinkerGun", "ShrinkerGun", "0.1.0")]
    [BepInDependency("Vippy.ScalerCore", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log = null!;
        static ConfigEntry<bool> _enableDebugKeys = null!;

        void Awake()
        {
            Log = Logger;

            // Scaling settings — written to ScalerCore's defaults so the library
            // picks them up without needing its own config file.
            ShrinkConfig.Factor = Config.Bind("Scaling", "ShrinkFactor", 0.4f,
                "Scale multiplier applied when shrunk (0.4 = 40% of original).").Value;
            ShrinkConfig.Speed = Config.Bind("Scaling", "ShrinkSpeed", 2.0f,
                "Animation speed. Scaled by the object's original size.").Value;
            ShrinkConfig.EnemyDamageMult = Config.Bind("Scaling", "EnemyDamageMultiplier", 0.1f,
                "Damage multiplier for shrunken enemies (0.1 = 10%).").Value;
            ShrinkConfig.EnemyShrinkDuration = Config.Bind("Scaling", "EnemyShrinkDuration", 120f,
                "Seconds until a shrunken enemy auto-restores. 0 = never.").Value;
            ShrinkConfig.ItemShrinkDuration = Config.Bind("Scaling", "ItemShrinkDuration", 300f,
                "Seconds until a shrunken item auto-restores. 0 = never.").Value;
            ShrinkConfig.EnemyShrinkSpeedFactor = Config.Bind("Scaling", "EnemySpeedFactor", 0.65f,
                "Movement speed multiplier for shrunken enemies.").Value;

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
