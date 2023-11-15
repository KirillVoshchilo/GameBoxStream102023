using App.Content.Entities;
using App.Content.Player;
using App.Logic;
using Cysharp.Threading.Tasks;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class Snow : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture _snowHightMapExample;
    [SerializeField] private MeshRenderer _planeMeshRenderer;
    [SerializeField] private Material _heightMapUpdateSample;
    [SerializeField] private Material _snowPlaneMaterialSample;
    [SerializeField] private float _secondsToRestore = 100;

    private SnowHeightData _snowMap;
    private WalkerData _drawer = null;
    private Vector3 _previousDrawing;
    private float _timeToRestoreOneTick;
    private CustomRenderTexture _snowHeightMap;
    private Material _heightMapUpdate;
    private Material _snowPlaneMaterial;
    private int DrawPosition = Shader.PropertyToID("_DrawPosition");
    private int DrawAngle = Shader.PropertyToID("_DrawAngle");
    private int RestoreAmount = Shader.PropertyToID("_RestoreAmount");
    private int HeightMapInShader = Shader.PropertyToID("_HeightMap");
    private Collider _hitCollider = null;
    private Vector2 _previousDraw;
    private bool _isDrawingStarted;
    private bool _drawFirstPosition;

    public SnowHeightData SnowHeightData => _snowMap;

    private void Start()
    {
        _snowMap = new SnowHeightData();
        _heightMapUpdate = new Material(_heightMapUpdateSample);
        _heightMapUpdate.name = $"{gameObject.name} Height Map Update";
        _snowPlaneMaterial = new Material(_snowPlaneMaterialSample);
        _snowPlaneMaterial.name = $"{gameObject.name} Snow Plane";
        _planeMeshRenderer.materials = new Material[] { _snowPlaneMaterial };
        _snowHeightMap = new(_snowHightMapExample.width, _snowHightMapExample.height, _snowHightMapExample.format, RenderTextureReadWrite.sRGB)
        {
            updateMode = CustomRenderTextureUpdateMode.OnDemand,
            doubleBuffered = true,
            material = _heightMapUpdate,
            initializationMode = CustomRenderTextureUpdateMode.OnLoad,
        };
        _snowHeightMap.name = $"{gameObject.name} Snow Height Map";
        _snowPlaneMaterial.SetTexture(HeightMapInShader, _snowHeightMap);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_isDrawingStarted)
            return;
        if (!collision.gameObject.TryGetComponent<IEntity>(out IEntity entity))
            return;
        _drawer = entity.Get<WalkerData>();
        if (_drawer == null)
            return;
        _drawFirstPosition = false;
        _isDrawingStarted = true;
        DrawProcess().Forget();
    }
    private void OnCollisionExit(Collision collision)
    {
        _isDrawingStarted = false;
        _hitCollider = null;
        _drawer = null;
    }

    public void ResetHeight()
    {
        //Тут логика для зброса высоты
    }
    private async UniTask DrawProcess()
    {
        while (_drawer != null)
        {
            if (!Raycast(out RaycastHit hit))
            {
                await UniTask.NextFrame();
                continue;
            }
            if (_hitCollider == null)
                _hitCollider = hit.collider;
            if (_hitCollider != hit.collider)
                break;
            if (_drawer.Position != _previousDrawing)
                _previousDraw = _drawer.Position;
            else continue;
            Draw(hit.textureCoord);
            await UniTask.NextFrame();
        }
        _isDrawingStarted = false;
        _hitCollider = null;
        _drawer = null;
    }
    private bool Raycast(out RaycastHit hit)
    {
        Ray ray = new(_drawer.Position, Vector3.down);
        if (Physics.Raycast(ray, out hit))
            return true;
        return false;
    }


    private void Draw(Vector2 hitTextureCoord)
    {
        float angle = _drawer.EulerRotation.y;
        _heightMapUpdate.SetVector(DrawPosition, hitTextureCoord);
        _snowMap.SetHeightByCoordinates(hitTextureCoord, 0);
        _heightMapUpdate.SetFloat(DrawAngle, angle * Mathf.Deg2Rad);
        _snowHeightMap.Update();
    }
}
