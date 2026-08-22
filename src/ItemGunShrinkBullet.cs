using ScalerCore;
using ScalerCore.AprilFools;
using UnityEngine;

namespace ShrinkerGun
{
    // Sits alongside ItemGunBullet on the bullet prefab.
    // ItemGunBullet owns all visuals; HurtCollider owns stun.
    // This component owns the shrink effect at the hit point.
    //
    // No inspector wiring needed, Start() runs after ItemGun sets hitPosition
    // on the sibling ItemGunBullet, so the hit point is ready immediately.
    public class ItemGunShrinkBullet : MonoBehaviour
    {
        const float EnemyDuration = 120f;
        const float ItemDuration = 0f; // permanent until toggled

        // Set by GunSourcePatch Prefix immediately before ItemGun.ShootBulletRPC
        // instantiates the bullet. Awake() reads and clears it synchronously
        // (Awake fires inside Instantiate) so rapid fire never bleeds across bullets.
        public static ScaleController? PendingSourceCtrl;
        ScaleController? _sourceCtrl;

        void Awake()
        {
            _sourceCtrl = PendingSourceCtrl;
            PendingSourceCtrl = null;
        }

        // Host only. ShootBulletRPC goes to every client, so the host sees every
        // shot and decides both outcomes here: scale whatever the ray landed on,
        // or, if it landed on bare map, start the collapse. Clients never decide.
        void Start()
        {
            if (!SemiFunc.IsMasterClientOrSingleplayer()) return;

            var bullet = GetComponent<ItemGunBullet>();
            if (bullet == null || !bullet.bulletHit) return;

            if (ShrinkAtPoint(bullet.hitPosition)) return;
            if (PluginConfig.LevelCollapseEnabled) MapCollapse.OnMapHit();
        }

        // Returns true when anything ScalerCore knows about sat at the hit point,
        // including the gun that fired (which is skipped, not scaled).
        bool ShrinkAtPoint(Vector3 point)
        {
            // Include "Player" so the gun's own raycast hit (which already landed on the
            // player capsule) is found here. PlayerShrinkLink bridges CollisionTransform
            // (separate scene GO) back to the ScaleController on PlayerAvatar.
            int layerMask = (int)SemiFunc.LayerMaskGetPhysGrabObject()
                          | LayerMask.GetMask("Enemy", "Player");

            var colliders = Physics.OverlapSphere(
                point, 0.3f, layerMask, QueryTriggerInteraction.Collide);

            int playerLayer = LayerMask.NameToLayer("Player");
            bool foundAny = false;
            foreach (var col in colliders)
            {
                var ctrl = col.GetComponent<PlayerShrinkLink>()?.Controller
                        ?? col.GetComponentInParent<ScaleController>();

                // Player layer hit but no ShrinkLink found, the sphere hit the character
                // controller capsule instead of the avatar's CollisionTransform. Both sit at
                // the same world position, so find the nearest player by CollisionTransform.
                if (ctrl == null && col.gameObject.layer == playerLayer)
                    ctrl = FindNearestPlayerCtrl(col.transform.position);

                if (ctrl == null) continue;
                foundAny = true;
                if (ctrl == _sourceCtrl) continue;
                Toggle(ctrl);
                return true;
            }
            return foundAny;
        }

        static ScaleController? FindNearestPlayerCtrl(Vector3 pos)
        {
            if (GameDirector.instance?.PlayerList == null) return null;
            ScaleController? best = null;
            float bestDist = 2f;
            foreach (var player in GameDirector.instance.PlayerList)
            {
                if (player == null || player.isDisabled) continue;
                var ctrl = player.GetComponent<ScaleController>();
                if (ctrl == null) continue;
                var pac = player.GetComponent<PlayerAvatarCollision>();
                var origin = pac?.CollisionTransform != null ? pac.CollisionTransform.position : player.transform.position;
                float d = Vector3.Distance(origin, pos);
                if (d < bestDist) { bestDist = d; best = ctrl; }
            }
            return best;
        }

        static void Toggle(ScaleController ctrl)
        {
            // ScalerCore handles same-factor toggle automatically, just always Apply.
            var opts = PluginConfig.ShrinkOptions;
            if (ctrl.TargetType == ScaleTargets.Enemies)
                opts.Duration = EnemyDuration;
            else if (ctrl.TargetType == ScaleTargets.Items)
                opts.Duration = ItemDuration;
            ScaleManager.Apply(ctrl.gameObject, opts);
        }
    }
}
