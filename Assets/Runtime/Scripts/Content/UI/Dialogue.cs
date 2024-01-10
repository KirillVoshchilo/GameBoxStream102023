using App.Simples;
using UnityEngine;

namespace App.Content.UI
{
    public sealed class Dialogue : MonoBehaviour
    {
        [SerializeField] private GameObject[] _messages;
        [SerializeField] private SData<string, string>[] _interlocutors;

        private SEvent _onSlidShowEnded = new();
        private int _currentMessage;
        private bool _isLoop;

        public SEvent OnSlidShowEnded => _onSlidShowEnded;
        public bool IsLoop { get => _isLoop; set => _isLoop = value; }
        public SData<string, string>[] Interlocators => _interlocutors;
        public int CurrentMessage => _currentMessage;
        public int MessagesCount => _messages.Length;

        public void ShowFirst()
        {
            CloseAll();
            _currentMessage = 0;
            _messages[0].SetActive(true);
        }
        public void ShowNext()
        {
            _messages[_currentMessage].SetActive(false);
            _currentMessage++;
            if (_currentMessage >= _messages.Length)
            {
                if (_isLoop)
                {
                    _currentMessage = 0;
                    _messages[_currentMessage].SetActive(true);
                }
                else _onSlidShowEnded.Invoke();
            }
            else _messages[_currentMessage].SetActive(true);
        }

        private void CloseAll()
        {
            int count = _messages.Length;
            for (int i = 0; i < count - 1; i++)
                _messages[i].SetActive(false);
        }
    }
}