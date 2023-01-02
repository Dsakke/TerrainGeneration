using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TerrainAlgorithm
{
    public abstract void Step(RenderTexture input, ref RenderTexture output);
}
