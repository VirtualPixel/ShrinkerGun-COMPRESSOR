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
        internal static float _enemyDuration = 120f;
        internal static float _itemDuration = 300f;
        static ConfigEntry<bool> _enableDebugKeys = null!;

        void Awake()
        {
            Log = Logger;

            ShrinkOptions = ScaleOptions.Default;

            _enableDebugKeys = Config.Bind("Debug", "EnableDebugKeys", false,
                "Enable F9/F10 keys to shrink/unshrink yourself. Off by default. Host controls this for all players.");
            ScaleController.AllowManualScale = _enableDebugKeys.Value;

            new Harmony("Vippy.ShrinkerGun").PatchAll();
        }

        void Update()
        {
            var player = PlayerAvatar.instance;
            if (player == null) return;
            var ctrl = player.GetComponent<ScaleController>();
            if (ctrl == null) return;

            // AllowManualScale is set by the host's config — the host gates
            // the RPC so clients can freely press the keys. If the host has
            // debug keys off, the request is rejected server-side.
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
