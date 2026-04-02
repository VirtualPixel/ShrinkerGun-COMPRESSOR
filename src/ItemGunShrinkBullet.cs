using ScalerCore;
using UnityEngine;

namespace ShrinkerGun
{
    // Sits alongside ItemGunBullet on the bullet prefab.
    // ItemGunBullet owns all visuals; HurtCollider owns stun.
    // This component owns the shrink effect at the hit point.
    //
    // No inspector wiring needed — Start() runs after ItemGun sets hitPosition
    // on the sibling ItemGunBullet, so the hit point is ready immediately.
    public class ItemGunShrinkBullet : MonoBehaviour
    {
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

        void Start()
        {
            Plugin.Log.LogInfo($"[SG] Bullet Start  isMaster={SemiFunc.IsMasterClientOrSingleplayer()}  instanceID={GetInstanceID()}  GO={gameObject.name}");
            if (!SemiFunc.IsMasterClientOrSingleplayer()) return;

            var bullet = GetComponent<ItemGunBullet>();
            if (bullet == null || !bullet.bulletHit)
            {
                Plugin.Log.LogInfo($"[SG] Bullet SKIP  bullet={bullet != null}  hit={bullet?.bulletHit}");
                return;
            }

            Plugin.Log.LogInfo($"[SG] Bullet HIT at {bullet.hitPosition}");
            ShrinkAtPoint(bullet.hitPosition);
        }

        void ShrinkAtPoint(Vector3 point)
        {
            // Include "Player" so the gun's own raycast hit (which already landed on the
            // player capsule) is found here. PlayerShrinkLink bridges CollisionTransform
            // (separate scene GO) back to the ScaleController on PlayerAvatar.
            int layerMask = (int)SemiFunc.LayerMaskGetPhysGrabObject()
                          + LayerMask.GetMask("Enemy", "Player");

            var colliders = Physics.OverlapSphere(
                point, 0.3f, layerMask, QueryTriggerInteraction.Collide);

            Plugin.Log.LogInfo($"[SG] OverlapSphere  center={point}  radius=0.3  hits={colliders.Length}");
            int playerLayer = LayerMask.NameToLayer("Player");
            foreach (var col in colliders)
            {
                var ctrl = col.GetComponent<PlayerShrinkLink>()?.Controller
                        ?? col.GetComponentInParent<ScaleController>();

                // Player layer hit but no ShrinkLink found — the sphere hit the character
                // controller capsule instead of the avatar's CollisionTransform. Both sit at
                // the same world position, so find the nearest player by CollisionTransform.
                if (ctrl == null && col.gameObject.layer == playerLayer)
                    ctrl = FindNearestPlayerCtrl(col.transform.position);

                Plugin.Log.LogInfo($"[SG]   collider={col.gameObject.name}  layer={col.gameObject.layer}  ctrl={ctrl != null}  ctrlID={ctrl?.GetInstanceID()}  IsScaled={ctrl?.IsScaled}");
                if (ctrl == null || ctrl == _sourceCtrl) continue;
                Toggle(ctrl);
                return;
            }
            Plugin.Log.LogInfo("[SG] No target found");
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
            Plugin.Log.LogInfo($"[SG] Toggle  ctrl={ctrl.GetInstanceID()}  IsScaled={ctrl.IsScaled}  currentScale={ctrl.transform.localScale}");
            if (ctrl.IsScaled)
            {
                ScaleManager.Restore(ctrl.gameObject);
            }
            else
            {
                var opts = Plugin.ShrinkOptions;
                // Set duration based on target type
                if (ctrl.Handler is ScalerCore.Handlers.EnemyHandler)
                    opts.Duration = Plugin._enemyDuration;
                else if (ctrl.Handler is ScalerCore.Handlers.ItemHandler)
                    opts.Duration = Plugin._itemDuration;
                // Players and valuables: opts.Duration stays 0 (permanent)
                ScaleManager.Apply(ctrl.gameObject, opts);
            }
        }
    }
}
