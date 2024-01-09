using App.Content;
using App.Content.UI;
using UnityEngine;
using VContainer;

namespace App.Architecture.Factories.UI
{
    public sealed class InteractionIconFactory : ASceneObjectFactory<InteractIcon>
    {
        private const string PREFAB = "Prefabs/UI/World/InteractionIcon";

        private readonly CamerasStorage _camerasStorage;

        [Inject]
        public InteractionIconFactory(CamerasStorage camerasStorage)
        {
            _camerasStorage = camerasStorage;
        }

        public override InteractIcon Create()
        {
            InteractIcon prefab = Resources.Load<InteractIcon>(PREFAB);
            InteractIcon instance = Object.Instantiate(prefab);
            instance.Construct(_camerasStorage);
            instance.IsEnable = true;
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(InteractIcon obj)
        {
            OnRemoved.Invoke(obj);
            obj.IsEnable = false;
            Object.Destroy(obj.gameObject);
        }
    }
}