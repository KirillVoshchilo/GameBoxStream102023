using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using App.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.Content.UI
{
    public sealed class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _closeAppButton;
        [SerializeField] private Button _descriptionButton;
        [SerializeField] private Button _closeDescriptionButton;
        [SerializeField] private GameObject _descriptionPanel;

        private UIController _uiController;
        private LevelsController _levelsController;
        private AudioController _audioController;

        public UIController UIController { set => _uiController = value; }

        [Inject]
        public void Construct(LevelsController levelsController,
            AudioController audioController)
        {
            _audioController = audioController;
            _levelsController = levelsController;
            _closeAppButton.onClick.AddListener(OnCloseAppClicked);
            _startGameButton.onClick.AddListener(OnStartNewGameButton);
            _closeDescriptionButton.onClick.AddListener(OnCloseDescriptionClicked);
            _descriptionButton.onClick.AddListener(OnDescriptionClicked);
        }

        private void OnDescriptionClicked()
        {
            _descriptionPanel.SetActive(true);
            _audioController.AudioData.SoundTracks.Button.Play();
        }
        private void OnCloseDescriptionClicked()
        {
            _audioController.AudioData.SoundTracks.Button.Play();
            _descriptionPanel.SetActive(false);
        }
        private void OnStartNewGameButton()
        {
            _audioController.AudioData.SoundTracks.Button.Play();
            _uiController.CloseMainMenu();
            _levelsController.StartFirstLevel();
        }

        private void OnCloseAppClicked()
        {
            _audioController.AudioData.SoundTracks.Button.Play();
            Application.Quit();
        }
    }
}