using UnityEngine;

namespace ShrinkerGun
{
    public class ItemGunShrink : MonoBehaviour
    {
        void Start()
        {
            foreach (var r in GetComponentsInChildren<MeshRenderer>())
                r.material.shader = Shader.Find("Fresnel Opaque");
        }
    }
}
