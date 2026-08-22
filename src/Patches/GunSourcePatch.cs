using HarmonyLib;
using ScalerCore;

namespace ShrinkerGun
{
    // Set PendingSourceCtrl before ShootBulletRPC instantiates the bullet so
    // ItemGunShrinkBullet.Awake (which fires synchronously inside Instantiate)
    // can capture which gun fired it and skip self-shrinking.
    [HarmonyPatch(typeof(ItemGun), nameof(ItemGun.ShootBulletRPC))]
    static class GunSourcePatch
    {
        static void Prefix(ItemGun __instance)
        {
            ItemGunShrinkBullet.PendingSourceCtrl = __instance.GetComponent<ScaleController>();
        }
    }
}
