using App.Architecture.AppInput;
using App.Content.Buildings;
using App.Logic;
using App.Simples;
using UnityEngine;
using VContainer;

namespace App.Architecture.AppData
{
    [CreateAssetMenu]
    public sealed class ShopFactory : AScriptableFactory
    {
        [SerializeField] private ShopEntity _shop;

        private readonly SEvent<ShopEntity> _onCreated = new();
        private UIController _uiController;
        private WorldCanvasStorage _worldCanvasStorage;
        private IAppInputSystem _appInputSystem;

        public SEvent<ShopEntity> OnCreated
            => _onCreated;

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            UIController uiController,
            WorldCanvasStorage worldCanvasStorage)
        {
            _uiController = uiController;
            _worldCanvasStorage = worldCanvasStorage;
            _appInputSystem = appInputSystem;
        }
        public override void Create()
        {
            ShopEntity instance = Instantiate(_shop, Parent.position, Parent.rotation, Parent);
            instance.gameObject.name = _shop.name;
            instance.Construct(_uiController, _worldCanvasStorage, _appInputSystem);
            instance.transform.parent = null;
            _onCreated.Invoke(instance);
        }
        public override void Remove(GameObject obj)
        {
            ShopEntity shopEntity = obj.GetComponent<ShopEntity>();
            shopEntity.Destruct();
            Destroy(obj);
        }
    }
}