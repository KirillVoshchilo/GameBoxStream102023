using App.Logic;
using Cysharp.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;
using VContainer;

public class SnowBrush : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture _snowHightMapExample;
    [SerializeField] private MeshRenderer _planeMeshRenderer;
    [SerializeField] private Material _heightMapUpdateSample;
    [SerializeField] private Material _snowPlaneMaterialSample;
    [SerializeField] private float _secondsToRestore = 100;

    private GameObject _drawer = null;
    private float _timeToRestoreOneTick;
    private CustomRenderTexture _snowHeightMap;
    private Material _heightMapUpdate;
    private Material _snowPlaneMaterial;
    private int DrawPosition = Shader.PropertyToID("_DrawPosition");
    private int DrawAngle = Shader.PropertyToID("_DrawAngle");
    private int RestoreAmount = Shader.PropertyToID("_RestoreAmount");
    private int HeightMapInShader = Shader.PropertyToID("_HeightMap");
    private Collider _hitCollider = null;

    private void Start()
    {
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
        // _snowHeightMap.Initialize();
    }
    private void OnCollisionStay(Collision collision)
    {
        _drawer = collision.gameObject;
    }

    private void Update()
    {
        // Считаем таймер до восстановления каждого пикселя текстуры на единичку 
        //_timeToRestoreOneTick -= Time.deltaTime;
        //if (_timeToRestoreOneTick < 0)
        //{
        //    _heightMapUpdate.SetFloat(RestoreAmount, 1 / 250f);
        //    _timeToRestoreOneTick = _secondsToRestore / 250f;
        //}
        //else _heightMapUpdate.SetFloat(RestoreAmount, 0);
        if (_drawer == null)
            return;
        DrawWithTires();
    }

    private void DrawWithTires()
    {
        Ray ray = new(_drawer.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (_hitCollider == null)
            {
                _hitCollider = hit.collider;
            }
            if (_hitCollider != hit.collider)
            {
                _drawer = null;
                return;
            }
            Vector2 hitTextureCoord = hit.textureCoord;
            float angle = _drawer.transform.rotation.eulerAngles.y;
            _heightMapUpdate.SetVector(DrawPosition, hitTextureCoord);
            _heightMapUpdate.SetFloat(DrawAngle, angle * Mathf.Deg2Rad);
            _snowHeightMap.Update();
        }
    }
}
