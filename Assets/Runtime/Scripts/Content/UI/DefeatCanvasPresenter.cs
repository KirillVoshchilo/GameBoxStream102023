using App.Architecture.AppInput;
using App.Architecture;
using App.Content.Player;
using App.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using UnityEngine.UI;

public class DefeatCanvasPresenter : MonoBehaviour
{
    [SerializeField] private Button _endGameButton;

    private UIController _uiController;
    private Inventory _playerInventory;
    private PlayerEntity _playerEntity;
    private LevelLoaderSystem _levelLoader;
    private BonfireFactory _bonusFactory;

    [Inject]
    public void Construct(LevelLoaderSystem levelLoader,
        PlayerEntity playerEntity,
        UIController uiController,
        BonfireFactory bonfireFactory)
    {
        _bonusFactory = bonfireFactory;
        _uiController = uiController;
        _playerInventory = playerEntity.Get<Inventory>();
        _playerEntity = playerEntity;
        _levelLoader = levelLoader;
        _endGameButton.onClick.AddListener(OnCloseAppClicked);
    }

    private void OnCloseAppClicked()
    {
        _bonusFactory.ClearAll();
        _playerInventory.Clear();
        _playerEntity.Get<HeatData>().IsFreezing = false;
        _playerEntity.GetComponent<Rigidbody>().useGravity = false;
        _levelLoader.UnloadScene(LevelLoaderSystem.FIRST_LEVEL);
        _uiController.OpenMainMenu();
    }
}
