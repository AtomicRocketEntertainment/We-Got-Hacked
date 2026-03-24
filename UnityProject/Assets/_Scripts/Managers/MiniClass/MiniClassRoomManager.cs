using System.Collections.Generic;
using UnityEngine;

namespace MiniClassRoom
{
    public class MiniClassRoomManager : MonoBehaviour
    {
        public Conversation sequence;

        private int currentIndex = 0;
        private List<DialogueLine> history = new();

        public void StartDialogue(Conversation seq)
        {
            sequence = seq;
            currentIndex = 0;
            history.Clear();
            ShowLine();
        }

        public void Next()
        {
            if (currentIndex >= sequence.lines.Count - 1) return;

            currentIndex++;
            ShowLine();
        }

        public void Previous()
        {
            if (currentIndex <= 0) return;

            currentIndex--;
            ShowLine();
        }

        void ShowLine()
        {
            var line = sequence.lines[currentIndex];

            history.Add(line);

        }
    }
}
