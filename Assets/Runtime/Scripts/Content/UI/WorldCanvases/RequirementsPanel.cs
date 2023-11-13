using App.Architecture.AppData;
using App.Content.UI.Shop;
using App.Logic;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace App.Content.UI.WorldCanvases
{
    public sealed class RequirementsPanel : MonoBehaviour
    {
        [SerializeField] private RequirementItemPresenter _prefab;
        [SerializeField] private Transform _content;

        private readonly List<RequirementItemPresenter> _itemsList = new();
        private IconsConfiguration _iconsConfiguration;
        private bool _isEnable;
        private Transform _mainCameraTransform;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                if (value)
                {
                    OrientProcess()
                            .Forget();
                }
            }
        }

        [Inject]
        public void Construct(Configuration configurations,
            CamerasStorage camerasStorage)
        {
            _mainCameraTransform = camerasStorage.MainCamera.transform;
            _iconsConfiguration = configurations.IconsConfiguration;
        }
        public void SetPosition(Vector3 position)
            => transform.position = position;
        public void FillWithItems(ItemCount[] items)
        {
            foreach (ItemCount item in items)
            {
                RequirementItemPresenter instance = Instantiate(_prefab, _content);
                instance.Count = item.Count;
                instance.Icon = _iconsConfiguration[item.Key];
                _itemsList.Add(instance);
            }
        }
        public void Clear()
        {
            RequirementItemPresenter[] array = _itemsList.ToArray();
            int count = array.Length;
            for (int i = 0; i < count; i++)
                Destroy(array[i].gameObject);
            _itemsList.Clear();
        }

        private async UniTask OrientProcess()
        {
            while (_isEnable)
            {
                transform.LookAt(transform.position + _mainCameraTransform.forward);
                await UniTask.Delay(100);
            }
        }
    }
}