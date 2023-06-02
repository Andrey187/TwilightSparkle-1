using UnityEngine;

public class GpuInctancingEnabler : MonoCache
{
    private void Awake()
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        MeshRenderer meshRenderer = Get<MeshRenderer>();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
