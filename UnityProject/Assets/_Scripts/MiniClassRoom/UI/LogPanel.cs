using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MiniClassRoom
{
    public class LogPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _logTextPrefab;
        [SerializeField] private RectTransform _container;

        private List<DialogueLine> _lines;
        private List<TextMeshProUGUI> _linesText;

        public void ClearLog() {            
            _lines = null;
            if (_linesText != null)
            {
                foreach (var log in _linesText)
                {
                    Destroy(log.gameObject);
                }
                _linesText.Clear();
            }
            gameObject.SetActive(false);
        }

        public void OpenLog(List<DialogueLine> lines)
        {
            if (lines == null) return;

            _lines = lines;
            _linesText = new();
            foreach (var line in _lines)
            {
                TextMeshProUGUI log = Instantiate(_logTextPrefab, _container.transform);
                log.text = (line.actorHighlightedID != null && line.actorHighlightedID != "") ? $"{line.actorHighlightedID}: {line.dialogueText}" : $"{line.dialogueText}";
                _linesText.Add(log);
            }
            gameObject.SetActive(true);
        }
    }
}
