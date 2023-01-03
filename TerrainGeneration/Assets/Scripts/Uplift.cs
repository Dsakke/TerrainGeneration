using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Uplift : TerrainAlgorithm
{
    ComputeShader _shader = null;
    [SerializeField, Tooltip("Represents how much terrain will rise each step. Uses the red channel")]
    private Texture2D _upliftMap = null;
    [SerializeField, Tooltip("The maximum amount of uplift that will occur in one step. (meters)")]
    private float _upliftStrength = 0.1f;

    public override void Step(RenderTexture input, ref RenderTexture output)
    {
        ComputeShader _shader = Resources.Load<ComputeShader>("Shaders/Uplift");
        int kernel = _shader.FindKernel("CSMain");

        _shader.SetTexture(kernel, "_input", input);
        _shader.SetTexture(kernel, "_upliftMap", _upliftMap);
        _shader.SetTexture(kernel, "_output", output);
        _shader.SetFloat("_upliftStrength", _upliftStrength);

        _shader.Dispatch(kernel, 2048, 2048, 1);
    }
}
