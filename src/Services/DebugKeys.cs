using ScalerCore;
using ScalerCore.AprilFools;
using UnityEngine;

namespace ShrinkerGun
{
    // Lives on the plugin object so it ticks in every scene. One bool and two
    // key polls a frame; the avatar lookup only happens when a key goes down.
    class DebugKeys : MonoBehaviour
    {
        void Update()
        {
            if (!PluginConfig.EnableDebugKeys.Value) return;
            if (Input.GetKeyDown(PluginConfig.ShrinkKey.Value)) ToggleSelf();
            if (Input.GetKeyDown(PluginConfig.CollapseKey.Value) && PluginConfig.LevelCollapseEnabled) Collapse();
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
