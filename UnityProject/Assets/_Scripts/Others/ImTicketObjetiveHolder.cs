using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImTicketObjetiveHolder : MonoBehaviour
{
    public Toggle ObjetiveToggle;
    public TextMeshProUGUI ObjectiveText;

    public void SetInfos(bool isCompleted, string text)
    {
        ObjectiveText.text = text;
        ObjetiveToggle.isOn = isCompleted;
    }
}
