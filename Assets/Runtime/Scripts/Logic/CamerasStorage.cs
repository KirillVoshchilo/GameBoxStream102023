using UnityEngine;

namespace App.Logic
{
    public sealed class CamerasStorage : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;

        public Camera MainCamera
            => _mainCamera;
    }
}