using UnityEngine;

namespace ShrinkerGun
{
    // On the gun prefab. Doubles as the marker the battery patches use to tell
    // our gun from every other battery item in the game, so it stays on the
    // prefab even if the material pass below ever goes away.
    public class ItemGunShrink : MonoBehaviour
    {
        void Start()
        {
            // Shader.Find only sees shaders the game has already loaded. If the
            // name ever changes, leave the bundle's own material alone rather
            // than assigning null and turning the gun into a magenta brick.
            var shader = Shader.Find("Fresnel Opaque");
            if (shader == null)
            {
                Plugin.Log.LogWarning("Fresnel Opaque shader missing, leaving the gun's materials as they shipped");
                return;
            }

            foreach (var r in GetComponentsInChildren<MeshRenderer>())
                r.material.shader = shader;
        }
    }
}
