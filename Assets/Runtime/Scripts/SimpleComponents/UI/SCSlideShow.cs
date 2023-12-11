using App.Simples;
using UnityEngine;

namespace SimpleComponents.UI
{
    public sealed class SCSlideShow : MonoBehaviour
    {
        [SerializeField] private GameObject[] _slides;

        private SEvent _onSlidShowEnded = new();
        private int _currentSlide;
        private bool _isLoop;

        public SEvent OnSlidShowEnded => _onSlidShowEnded;
        public bool IsLoop { get => _isLoop; set => _isLoop = value; }

        public void ShowFirst()
        {
            CloseAll();
            _currentSlide = 0;
            _slides[0].SetActive(true);
        }
        public void ShowNext()
        {
            _slides[_currentSlide].SetActive(false);
            _currentSlide++;
            if (_currentSlide >= _slides.Length)
            {
                if (_isLoop)
                {
                    _currentSlide = 0;
                    _slides[_currentSlide].SetActive(true);
                }
                else _onSlidShowEnded.Invoke();
            }
            else _slides[_currentSlide].SetActive(true);
        }

        private void CloseAll()
        {
            int count = _slides.Length;
            for (int i = 0; i < count - 1; i++)
                _slides[i].SetActive(false);
        }
    }
}