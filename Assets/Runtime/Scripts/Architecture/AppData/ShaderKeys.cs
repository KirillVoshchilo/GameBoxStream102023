using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShaderKeys
{
    public static int DrawPosition = Shader.PropertyToID("_DrawPosition");
    public static int DrawAngle = Shader.PropertyToID("_DrawAngle");
    public static int RestoreAmount = Shader.PropertyToID("_RestoreAmount");
    public static int HeightMapInShader = Shader.PropertyToID("_HeightMap");

}
