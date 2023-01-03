using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainGeneration
{
    [Header("General settings")]
    [SerializeField]
    private TerrainData _heightMap = null;
    [SerializeField]
    private int _textureWidth = 2048;
    [SerializeField]
    private int _textureHeight = 2048;
    private RenderTexture _renderTo;
    private RenderTexture _renderFrom;

    [Header("Uplift Settings")]
    [SerializeField]
    private Uplift _uplift = null;

    public void Simulate(int nrIterations)
    {
        for(int i = 0; i < nrIterations; ++i)
        {
            SimulateUplift();
            RenderTexture temp = _renderFrom;
            _renderFrom = _renderTo;
            RenderTexture.active = _renderTo;
        }
        ApplyChanges();
    }

    public void ResetTerrain()
    {
        float[,] pixels = new float[_textureWidth,_textureHeight];
        _heightMap.SetHeights(0, 0, pixels);
    }

    private void SimulateUplift()
    {
        _uplift.Step(_renderFrom, ref _renderTo);
    }

    public void Initialize()
    {
        RenderTextureDescriptor textureDescriptor = new RenderTextureDescriptor(_textureWidth, _textureHeight, RenderTextureFormat.RFloat);
        textureDescriptor.enableRandomWrite = true;
        _renderTo = new RenderTexture(textureDescriptor);
        _renderFrom = new RenderTexture(textureDescriptor);
    }

    private void ApplyChanges()
    {
        Texture2D tex = new Texture2D(_textureWidth, _textureHeight, UnityEngine.Experimental.Rendering.GraphicsFormat.R32_SFloat, UnityEngine.Experimental.Rendering.TextureCreationFlags.None);
        Rect sourceRect = new Rect(0, 0, _textureWidth, _textureHeight);
        tex.ReadPixels(sourceRect, 0, 0);
        var pixels = tex.GetPixelData<float>(0);
        float[,] pixels2D = new float[_textureWidth, _textureHeight];
        for(int i = 0; i < pixels.Length; ++i)
        {
            pixels2D[i / _textureWidth, i % _textureWidth] = pixels[i];
        }

        _heightMap.SetHeights(0, 0, pixels2D);
    }
}
