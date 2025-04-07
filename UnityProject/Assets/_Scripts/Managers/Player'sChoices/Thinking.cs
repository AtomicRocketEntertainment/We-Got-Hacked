using TMPro;
using UnityEngine;

public class Thinking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _thinking;

    public void UpdateThinking(string thinking)
    {
        _thinking.text = thinking;
    }
}
