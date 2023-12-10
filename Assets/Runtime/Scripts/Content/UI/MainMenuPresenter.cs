using App.Content.Audio;
using App.Logic;
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
        private AudioStorage _audioController;

        public UIController UIController { set => _uiController = value; }

        [Inject]
        public void Construct(LevelsController levelsController,
            AudioStorage audioController)
        {
            _audioController = audioController;
            _levelsController = levelsController;
            _closeAppButton.onClick.AddListener(OnCloseAppClicked);
            _startGameButton.onClick.AddListener(OnStartNewGameButton);
            _closeDescriptionButton.onClick.AddListener(OnCloseDescriptionClicked);
            _descriptionButton.onClick.AddListener(OnDescriptionClicked);
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.MainMenuMusic);
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