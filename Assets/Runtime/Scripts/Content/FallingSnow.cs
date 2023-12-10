using App.Content.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace App.Content
{
    public sealed class FallingSnow : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fallingSnow;
        [SerializeField] private ParticleSystem _fallingSimpleSnow;

        private WalkerData _playerWalkerData;

        [Inject]
        public void Construct(PlayerEntity playerEntity)
        {
            _playerWalkerData = playerEntity.Get<WalkerData>();
            _playerWalkerData.OnMovingStarted.AddListener(StartUpdatingPosition);
        }
        public void StartSnowing()
        {
            _fallingSnow.Play();
            _fallingSimpleSnow.Play();
            UpdataSpawnerPosition();
        }
        public void StopSnowing()
        {
            _fallingSimpleSnow.Stop();
            _fallingSnow.Stop();
        }

        private void StartUpdatingPosition()
        {
            UpdateProces()
                .Forget();
        }
        private async UniTask UpdateProces()
        {
            while (_playerWalkerData.IsMoving)
            {
                UpdataSpawnerPosition();
                await UniTask.Delay(100);
            }
        }
        private void UpdataSpawnerPosition()
        {
            Vector3 pos = _fallingSnow.transform.position;
            pos.x = _playerWalkerData.Position.x;
            pos.z = _playerWalkerData.Position.z;
            _fallingSnow.transform.position = pos;
        }
    }
}