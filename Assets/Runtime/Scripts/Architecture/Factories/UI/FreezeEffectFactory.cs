using App.Content;
using App.Content.Player;
using App.Content.UI;
using UnityEngine;
using VContainer;

namespace App.Architecture.Factories.UI
{
    public sealed class FreezeEffectFactory : ASceneObjectFactory<FreezeScreenEffect>
    {
        private const string PREFAB = "Prefabs/UI/FreezeEffect";

        private readonly CamerasStorage _camerasStorage;
        private readonly PlayerEntity _playerEntity;

        [Inject]
        public FreezeEffectFactory(PlayerEntity playerEntity, CamerasStorage camerasStorage)
        {
            _camerasStorage = camerasStorage;
            _playerEntity = playerEntity;
        }

        public override FreezeScreenEffect Create()
        {
            FreezeScreenEffect prefab = Resources.Load<FreezeScreenEffect>(PREFAB);
            FreezeScreenEffect instance = Object.Instantiate(prefab);
            instance.Construct(_playerEntity);
            Canvas canvas = instance.GetComponent<Canvas>();
            canvas.worldCamera = _camerasStorage.MainCamera;
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(FreezeScreenEffect obj)
        {
            OnRemoved.Invoke(obj);
            obj.Destruct();
            Object.Destroy(obj.gameObject);
        }
    }
}