using System.Collections.Generic;
using UnityEngine;

namespace MiniClassRoom
{
    public class MiniClassRoomManager : MonoBehaviour
    {
        [SerializeField] private ConversationSO _sequence;
        [SerializeField] private MiniClassroomUI _miniClassroomUI;

        private int _currentIndex = 0;
        private List<DialogueLine> _history = new();

        public void StartDialogue()
        {
            _miniClassroomUI.ClearConversation();

            _currentIndex = 0;
            _history.Clear();
            ShowLine();
        }

        public void Next()
        {
            if (_currentIndex >= _sequence.lines.Count - 1) return;

            _currentIndex++;
            ShowLine();
        }

        public void Previous()
        {
            if (_currentIndex <= 0) return;

            _currentIndex--;
            var line = _sequence.lines[_currentIndex];

            ShowLine();
            _history.Remove(line);
        }

        void ShowLine()
        {
            var line = _sequence.lines[_currentIndex];
            _miniClassroomUI.SetConversation(line);

            _history.Add(line);

        }
    }
}
