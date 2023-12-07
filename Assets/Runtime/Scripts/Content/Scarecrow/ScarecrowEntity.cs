using App.Architecture.AppData;
using App.Content.Entities;
using System;
using UnityEngine;
using VContainer;

public class ScarecrowEntity : MonoBehaviour, IEntity, IDestructable
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
    public T Get<T>() where T : class => throw new NotImplementedException();
    public void Destruct() => throw new NotImplementedException();


    private void OnLevelStarted()
    {
        if (_levelsController.CurrentLevel == 0)
        {
            _scarecrowData.SecondLevelModel.SetActive(false);
            _scarecrowData.ThirdLevelModel.SetActive(false);
        }
        if (_villageTrustSystem.Trust > _scarecrowData.SecondLevelTrust)
            _scarecrowData.SecondLevelModel.SetActive(true);
        if (_villageTrustSystem.Trust > _scarecrowData.ThirdLevelTrust)
            _scarecrowData.ThirdLevelModel.SetActive(true);
    }
}
