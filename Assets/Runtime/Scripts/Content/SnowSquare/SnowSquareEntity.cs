using App.Content.Entities;
using App.Content.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class SnowSquareEntity : MonoBehaviour, IEntity
{
    [SerializeField] private SnowSquareData _snowSquareData;

    private void Start()
    {
        _snowSquareData.HitCollider = GetComponent<Collider>();
        _snowSquareData.HeightMapUpdate = new Material(_snowSquareData.HeightMapUpdateSample)
        {
            name = $"{gameObject.name} Height Map Update"
        };
        _snowSquareData.SnowPlaneMaterial = new Material(_snowSquareData.SnowPlaneMaterialSample)
        {
            name = $"{gameObject.name} Snow Plane"
        };
        _snowSquareData.PlaneMeshRenderer.material = _snowSquareData.SnowPlaneMaterial;
        _snowSquareData.SnowHeightMap = new(_snowSquareData.SnowHightMapExample.width, _snowSquareData.SnowHightMapExample.height, _snowSquareData.SnowHightMapExample.format, RenderTextureReadWrite.sRGB)
        {
            updateMode = CustomRenderTextureUpdateMode.OnDemand,
            doubleBuffered = true,
            material = _snowSquareData.HeightMapUpdate,
            initializationMode = CustomRenderTextureUpdateMode.OnLoad,
            name = $"{gameObject.name} Snow Height Map"
        };
        _snowSquareData.SnowPlaneMaterial.SetTexture(ShaderKeys.HeightMapInShader, _snowSquareData.SnowHeightMap);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (_snowSquareData.IsDrawingStarted)
            return;
        if (!collision.gameObject.TryGetComponent(out IEntity entity))
            return;
        _snowSquareData.Drawer = entity.Get<WalkerData>();
        if (_snowSquareData.Drawer == null)
            return;
        _snowSquareData.IsDrawingStarted = true;
        DrawProcess()
            .Forget();
    }
    private void OnCollisionExit(Collision collision)
    {
        _snowSquareData.IsDrawingStarted = false;
        _snowSquareData.Drawer = null;
    }

    [ContextMenu("ResetHeight")]
    public void ResetHeight()
    {
        ResetProcess()
            .Forget();
    }
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(VirtualHeightMap))
            return _snowSquareData.VirtualHeightMap as T;
        return null;
    }

    private async UniTask ResetProcess()
    {
        _snowSquareData.HeightMapUpdate.SetFloat(ShaderKeys.RestoreAmount, 1);
        _snowSquareData.SnowHeightMap.Update();
        await UniTask.DelayFrame(2);
        _snowSquareData.HeightMapUpdate.SetFloat(ShaderKeys.RestoreAmount, 0);
        _snowSquareData.SnowHeightMap.Update();
    }
    private async UniTask DrawProcess()
    {
        while (_snowSquareData.Drawer != null)
        {
            if (!Raycast(out RaycastHit hit))
                break;
            Draw(hit.textureCoord);
            await UniTask.NextFrame();
        }
        _snowSquareData.IsDrawingStarted = false;
        _snowSquareData.Drawer = null;
    }
    private bool Raycast(out RaycastHit hit)
    {
        Ray ray = new(_snowSquareData.Drawer.Position, Vector3.down);
        RaycastHit[] raycasts = Physics.RaycastAll(ray);
        foreach (RaycastHit raycast in raycasts)
        {
            if (raycast.collider == _snowSquareData.HitCollider)
            {
                hit = raycast;
                return true;
            }
        }
        hit = default;
        return false;
    }
    private void Draw(Vector2 hitTextureCoord)
    {
        float angle = _snowSquareData.Drawer.EulerRotation.y;
        _snowSquareData.HeightMapUpdate.SetVector(ShaderKeys.DrawPosition, hitTextureCoord);
        _snowSquareData.VirtualHeightMap.SetHeightByCoordinates(hitTextureCoord, 0);
        _snowSquareData.HeightMapUpdate.SetFloat(ShaderKeys.DrawAngle, angle * Mathf.Deg2Rad);
        _snowSquareData.SnowHeightMap.Update();
    }
}
