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

        public void SetDialogue(string dialogueText, string titleName = "")
        {
            if (dialogueText == null) return;

            if(titleName == null || titleName == "")
            {
                _title.text = "";
                _titlePanel.gameObject.SetActive(false);
            }
            else
            {
                _title.text = titleName;
                _titlePanel.gameObject.SetActive(true);
            }

            StartCoroutine(TypeText(dialogueText));
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
