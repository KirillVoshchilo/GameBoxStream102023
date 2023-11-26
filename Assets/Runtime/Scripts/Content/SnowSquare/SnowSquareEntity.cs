using App.Content.Entities;
using App.Content.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

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

    }


    private void OnCollisionStay(Collision collision)
    {
        if (!_snowSquareData.HasHeightMap)
        {
            Debug.Log("СОздал карту высот");
            CreateHeightMap();
            _snowSquareData.HasHeightMap = true;
        }
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
        SlowDownUnitsProcess()
            .Forget();
    }
    private void OnCollisionExit(Collision collision)
    {
        _snowSquareData.IsDrawingStarted = false;
        _snowSquareData.Drawer = null;
    }
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

    private void CreateHeightMap()
    {
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
            if (!_snowSquareData.Drawer.IsMoving)
            {
                await UniTask.NextFrame();
                continue;
            }
            if (!Raycast(out RaycastHit hit))
                break;
            Draw(hit.textureCoord);
            await UniTask.NextFrame();
        }
        _snowSquareData.IsDrawingStarted = false;
        _snowSquareData.Drawer = null;
    }
    private bool CheckForSnow(Vector3 point, out RaycastHit hit, out VirtualHeightMap heightMap)
    {
        Ray ray = new(point, Vector3.down);
        RaycastHit[] raycasts = Physics.RaycastAll(ray);
        foreach (RaycastHit raycast in raycasts)
        {
            if (raycast.collider.gameObject.TryGetComponent(out IEntity entity))
            {
                heightMap = entity.Get<VirtualHeightMap>();
                if (heightMap != null)
                {
                    hit = raycast;
                    return true;
                }
            }
        }
        hit = default;
        heightMap = null;
        return false;
    }
    private async UniTask SlowDownUnitsProcess()
    {
        while (_snowSquareData.Drawer != null)
        {
            if (!_snowSquareData.Drawer.IsMoving)
            {
                await UniTask.NextFrame();
                continue;
            }
            Vector3 movingDirection = _snowSquareData.Drawer.MovingDirection;
            if (movingDirection == Vector3.zero)
            {
                await UniTask.NextFrame();
                continue;
            }
            Vector3 nextPoint = _snowSquareData.Drawer.Position + movingDirection * _snowSquareData.SearchingGroundDistance;
            if (!CheckForSnow(nextPoint, out RaycastHit hit, out VirtualHeightMap heightMap))
            {
                SetSpeedMultiplierTo(1);
                await UniTask.NextFrame();
                continue;
            }
            float heightValue = heightMap.GetHeightByCoordinates(hit.textureCoord);
            if (heightValue > 0)
                SetSpeedMultiplierTo(_snowSquareData.InfluenceOnSpeed);
            else SetSpeedMultiplierTo(1);
            await UniTask.NextFrame();
        }
        _snowSquareData.IsDrawingStarted = false;
        _snowSquareData.Drawer = null;
    }
    private void SetSpeedMultiplierTo(float value)
    {
        if (_snowSquareData.Drawer.SpeedMultipliers.ContainsKey(SnowSquareData.SNOW_INFLUENCE_KEY))
            _snowSquareData.Drawer.SpeedMultipliers[SnowSquareData.SNOW_INFLUENCE_KEY] = value;
        else _snowSquareData.Drawer.SpeedMultipliers.Add(SnowSquareData.SNOW_INFLUENCE_KEY, value);
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
