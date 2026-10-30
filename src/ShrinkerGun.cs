using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ShrinkerGun
{
    [BepInPlugin("Vippy.ShrinkerGun", "ShrinkerGun", BuildInfo.Version)]
    [BepInDependency("Vippy.ScalerCore", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log = null!;

        void Awake()
        {
            Log = Logger;
            PluginConfig.Init(Config);
            gameObject.AddComponent<DebugKeys>();
            new Harmony("Vippy.ShrinkerGun").PatchAll();
        }
    }
}
