using App.Content.Player;
using System;
using UnityEngine;

[Serializable]
public sealed class SnowSquareData
{
    public const string SNOW_INFLUENCE_KEY = "SnowInfluence";

    [SerializeField] private CustomRenderTexture _snowHightMapExample;
    [SerializeField] private MeshRenderer _planeMeshRenderer;
    [SerializeField] private Material _heightMapUpdateSample;
    [SerializeField] private Material _snowPlaneMaterialSample;
    [SerializeField] private float _searchingGroundDistance;
    [SerializeField] private float _influenceOnSpeed = 0.4f;

    private readonly VirtualHeightMap _virtualHeightMap = new();
    private WalkerData _drawer = null;
    private CustomRenderTexture _snowHeightMap;
    private Material _heightMapUpdate;
    private Material _snowPlaneMaterial;
    private Collider _hitCollider = null;
    private bool _isDrawingStarted;

    public CustomRenderTexture SnowHightMapExample => _snowHightMapExample;
    public MeshRenderer PlaneMeshRenderer => _planeMeshRenderer;
    public Material HeightMapUpdateSample => _heightMapUpdateSample;
    public Material SnowPlaneMaterialSample => _snowPlaneMaterialSample;
    public VirtualHeightMap VirtualHeightMap => _virtualHeightMap;
    public WalkerData Drawer { get => _drawer; set => _drawer = value; }
    public CustomRenderTexture SnowHeightMap { get => _snowHeightMap; set => _snowHeightMap = value; }
    public Material HeightMapUpdate { get => _heightMapUpdate; set => _heightMapUpdate = value; }
    public Material SnowPlaneMaterial { get => _snowPlaneMaterial; set => _snowPlaneMaterial = value; }
    public Collider HitCollider { get => _hitCollider; set => _hitCollider = value; }
    public bool IsDrawingStarted { get => _isDrawingStarted; set => _isDrawingStarted = value; }
    public float SearchingGroundDistance => _searchingGroundDistance;
    public float InfluenceOnSpeed => _influenceOnSpeed;

}
