using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace MiniClassRoom
{
    public class LogElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public virtual void Init(DialogueLine line)
        {
            _text.text = line.dialogueText;
        }
    }
}
