using ScalerCore;
using ScalerCore.AprilFools;
using UnityEngine;

namespace ShrinkerGun
{
    // Lives on the plugin object so it ticks in every scene. Two key polls a
    // frame; the avatar lookup only happens on the frame a key goes down.
    class DebugKeys : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9)) ToggleSelf();
            if (Input.GetKeyDown(KeyCode.End) && PluginConfig.LevelCollapseOn) Collapse();
        }

        static void ToggleSelf()
        {
            var player = PlayerAvatar.instance;
            if (player == null) return;
            var ctrl = player.GetComponent<ScaleController>();
            if (ctrl == null) return;

            if (ctrl.IsScaled)
                ctrl.RequestManualExpand();
            else
                ctrl.RequestManualShrink();
        }

        static void Collapse()
        {
            if (LevelGenerator.Instance == null || !LevelGenerator.Instance.Generated) return;
            if (!SemiFunc.IsMasterClientOrSingleplayer()) return;
            MapCollapse.OnMapHit();
        }
    }
}
