using System.Collections;
using TMPro;
using UnityEngine;

namespace MiniClassRoom
{
    public class TextBoxUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _titlePanel;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _dialogue;

        public void ClearDialogue()
        {
            _title.text = "";
            _titlePanel.gameObject.SetActive(false);
            _dialogue.text = "";
        }

        public void SetActorName(string titleName = "")
        {
            SetTitleName(titleName);
        }

        public void SetDialogue(string dialogueText)
        {
            if (dialogueText == null) return;
            if (_dialogue.text == dialogueText) return;
            StartCoroutine(TypeText(dialogueText));
        }

        private void SetTitleName(string titleName = "")
        {
            if (titleName == null || titleName == "")
            {
                _title.text = "";
                _titlePanel.gameObject.SetActive(false);
            }
            else
            {
                if (_title.text == titleName) return;
                _title.text = titleName;
                _titlePanel.gameObject.SetActive(true);
            }
        }

        private IEnumerator TypeText(string text)
        {
            _dialogue.text = "";

            foreach (char c in text)
            {
                _dialogue.text += c;
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
