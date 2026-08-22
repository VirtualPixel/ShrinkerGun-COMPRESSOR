using HarmonyLib;
using ScalerCore;

namespace ShrinkerGun
{
    // Set PendingSourceCtrl before ShootBulletRPC instantiates the bullet so
    // ItemGunShrinkBullet.Awake (which fires synchronously inside Instantiate)
    // can capture which gun fired it and skip self-shrinking. This runs for
    // every gun in the game, so clear it for the ones that aren't ours instead
    // of parking another gun's controller in a static for the rest of the run.
    [HarmonyPatch(typeof(ItemGun), nameof(ItemGun.ShootBulletRPC))]
    static class GunSourcePatch
    {
        static void Prefix(ItemGun __instance)
        {
            ItemGunShrinkBullet.PendingSourceCtrl = __instance.GetComponent<ItemGunShrink>() == null
                ? null
                : __instance.GetComponent<ScaleController>();
        }
    }
}
