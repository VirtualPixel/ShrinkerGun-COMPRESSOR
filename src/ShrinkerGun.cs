using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ScalerCore;

namespace ShrinkerGun
{
    [BepInPlugin("Vippy.ShrinkerGun", "ShrinkerGun", "0.1.0")]
    [BepInDependency("Vippy.ScalerCore", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log = null!;

        void Awake()
        {
            Log = Logger;
            new Harmony("Vippy.ShrinkerGun").PatchAll();
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
