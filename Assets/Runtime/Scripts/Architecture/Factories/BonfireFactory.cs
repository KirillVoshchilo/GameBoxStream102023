using App.Architecture.AppInput;
using App.Architecture.Factories;
using App.Architecture.Factories.UI;
using App.Content.Player;
using App.Simples.CellsInventory;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace App.Content.Bonfire
{
    public sealed class BonfireFactory : ASceneObjectFactory<BonfireEntity>
    {
        private const string PREFAB = "Prefabs/UI/FreezeEffect";

        private readonly List<BonfireEntity> _bonfires = new();
        private readonly InteractionIconFactory _interactionIconFactory;
        private readonly IAppInputSystem _appInputSystem;
        private PlayerEntity _playerEntity;

        public PlayerEntity PlayerEntity { get => _playerEntity; set => _playerEntity = value; }

        [Inject]
        public BonfireFactory(IAppInputSystem appInputSystem,
            InteractionIconFactory interactionIconFactory)
        {
            _appInputSystem = appInputSystem;
            _interactionIconFactory = interactionIconFactory;
        }


        public void ClearAll()
        {
            BonfireEntity[] bonfires = _bonfires.ToArray();
            int count = bonfires.Length;
            for (int i = 0; i < count; i++)
                _bonfires[i].Destruct();
        }
        public override BonfireEntity Create()
        {
            Inventory playerInventory = _playerEntity.Get<Inventory>();
            BonfireEntity prefab = Resources.Load<BonfireEntity>(PREFAB);
            BonfireEntity instance = Object.Instantiate(prefab, _parent.position, Quaternion.identity);
            instance.Construct(_interactionIconFactory,
                _appInputSystem,
                playerInventory, 
                this);
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(BonfireEntity obj)
        {
            OnRemoved.Invoke(obj);
            obj.Destruct();
            Object.Destroy(obj.gameObject);
        }
    }
}