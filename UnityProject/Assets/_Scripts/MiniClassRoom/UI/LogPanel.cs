using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniClassRoom
{
    public class LogPanel : MonoBehaviour
    {
        [SerializeField] private LogElement _logTextActorPrefab;
        [SerializeField] private LogElement _logTextNarratorPrefab;
        [SerializeField] private RectTransform _container;
        [SerializeField] private Color _textColorStandard;

        private List<DialogueLine> _lines;
        private List<LogElement> _linesText;

        bool _updateCanvas = false;

        private void Update()
        {
            if (!_updateCanvas) return;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_container);
            Canvas.ForceUpdateCanvases();
            _updateCanvas = false;
        }

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
                LogElement log;
                ActorSO actorSpeaker = line.GetHighlightedActor();
                if (actorSpeaker != null && actorSpeaker.actorName != "")
                {
                    log = Instantiate(_logTextActorPrefab, _container.transform);
                    log.Init(line);
                }
                else
                {
                    log = Instantiate(_logTextNarratorPrefab, _container.transform);
                    log.Init(line);
                }
                    
                _linesText.Add(log);
            }
            gameObject.SetActive(true);

            _updateCanvas = true;
        }
    }
}
