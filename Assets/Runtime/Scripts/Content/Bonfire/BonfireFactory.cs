using App.Architecture.AppInput;
using App.Simples.CellsInventory;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace App.Content.Bonfire
{
    public sealed class BonfireFactory : MonoBehaviour
    {
        [SerializeField] private BonfireEntity _bonfireEntity;

        private readonly List<BonfireEntity> _bonfires = new();
        private WorldCanvasStorage _worldCanvasStorage;
        private IAppInputSystem _aspInputSystem;
        private Inventory _playerInventory;

        public Inventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }

        [Inject]
        public void Construct(WorldCanvasStorage worldCanvasStorage,
            IAppInputSystem appInputSystem)
        {
            _worldCanvasStorage = worldCanvasStorage;
            _aspInputSystem = appInputSystem;
        }
        public void ClearAll()
        {
            BonfireEntity[] bonfires = _bonfires.ToArray();
            int count = bonfires.Length;
            for (int i = 0; i < count; i++)
            {
                _bonfires[i].Destruct();
            }
        }
        public BonfireEntity BuildBonfire(Vector3 position)
        {
            BonfireEntity bonfireEntity = Instantiate(_bonfireEntity, position, Quaternion.identity);
            bonfireEntity.Construct(_worldCanvasStorage, _aspInputSystem, _playerInventory, this);
            _bonfires.Add(bonfireEntity);
            return bonfireEntity;
        }
        public void RemoveBonfire(BonfireEntity bonfireEntity)
        {
            bonfireEntity.Destruct();
            _bonfires.Remove(bonfireEntity);
            Destroy(bonfireEntity.gameObject);
        }
    }
}