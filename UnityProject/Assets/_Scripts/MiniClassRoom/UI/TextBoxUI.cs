using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MiniClassRoom
{
    public class TextBoxUI : MonoBehaviour
    {
        [SerializeField] private MiniClassroomUI _managerUI;
        [SerializeField] private RectTransform _titlePanel;
        [SerializeField] private Image _titleActorColor;
        [SerializeField] private TextMeshProUGUI _titleName;
        [SerializeField] private TextMeshProUGUI _dialogue;
        [SerializeField] private Image _arrowFinishImg;

        [SerializeField] private Color _defaultTitlePanelColor = new Color(1f, 1f, 1f, 1);
        [SerializeField] private Color _defaultTitleTextColor = Color.white;
        [SerializeField] private float _typingSpeed = 0.02f;

        private Coroutine _typingCoroutine;
        private string _currentText = "";

        public void ClearDialogue()
        {
            _currentText = "";
            _titleName.text = "";
            _titleActorColor.color = _defaultTitlePanelColor;
            _arrowFinishImg.gameObject.SetActive(false);
            _dialogue.text = "";
        }

        public void SetActorName(ActorSO titleActor = null)
        {
           
            SetTitleName(titleActor);
        }

        public void SetDialogue(string dialogueText)
        {
            if (dialogueText == null) return;
            if (_dialogue.text == dialogueText) return;

            _arrowFinishImg.gameObject.SetActive(false);

            _currentText = dialogueText;
            _typingCoroutine = StartCoroutine(TypeText());
        }

        private void SetTitleName(ActorSO titleActor = null)
        {
            if (titleActor == null || titleActor.name == "")
            {
                _titleName.text = "";
                _titleName.color = _defaultTitleTextColor;
                _titleActorColor.color = _defaultTitlePanelColor;
                _titlePanel.gameObject.SetActive(false);
            }
            else
            {
                if (_titleName.text == titleActor.actorName) return;
                _titleName.text = titleActor.actorName;
                _titleName.color = titleActor.colorTitleAtor;
                _titleActorColor.color = titleActor.colorBKGAtor;
                _titlePanel.gameObject.SetActive(true);
            }
        }

        private IEnumerator TypeText()
        {
            _dialogue.text = "";

            foreach (char c in _currentText)
            {
                _dialogue.text += c;
                yield return new WaitForSeconds(_typingSpeed);
            }

            TextFinish();
        }

        private void TextFinish()
        {
            _arrowFinishImg.gameObject.SetActive(true);
            _managerUI.DialogueTextReady();
        }

        public void SkipText()
        {
            if(_typingCoroutine != null) StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;

            _dialogue.text = _currentText;
            _arrowFinishImg.gameObject.SetActive(true);
        }
    }
}
