using App.Architecture;
using App.Architecture.AppData;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Content.Scarecrow
{
    public sealed class ScarecrowEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private ScarecrowData _scarecrowData;

        private VillageTrustSystem _villageTrustSystem;
        private LevelsController _levelsController;

        [Inject]
        public void Construct(VillageTrustSystem villageTrustSystem,
            LevelsController levelsController)
        {
            _levelsController = levelsController;
            levelsController.OnLevelStarted.AddListener(OnLevelStarted);
            _villageTrustSystem = villageTrustSystem;
        }
        public T Get<T>() where T : class
        {
            return null;
        }

        public void Destruct() { }


        private void OnLevelStarted()
        {
            if (_levelsController.CurrentLevel == 0)
            {
                _scarecrowData.FirstLevelModel.SetActive(false);
                _scarecrowData.SecondLevelModel.SetActive(false);
                _scarecrowData.ThirdLevelModel.SetActive(false);
            }
            if (_villageTrustSystem.Trust >= _scarecrowData.FirstLevelTrust)
                _scarecrowData.FirstLevelModel.SetActive(true);
            if (_villageTrustSystem.Trust >= _scarecrowData.SecondLevelTrust)
                _scarecrowData.SecondLevelModel.SetActive(true);
            if (_villageTrustSystem.Trust >= _scarecrowData.ThirdLevelTrust)
                _scarecrowData.ThirdLevelModel.SetActive(true);
        }
    }
}