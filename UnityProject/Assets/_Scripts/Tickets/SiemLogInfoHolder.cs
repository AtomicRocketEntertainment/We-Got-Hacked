using TMPro;
using UnityEngine;

public class SiemLogInfoHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;
    
    public void UpdateLog(string logText)
    {
        _logText.text = logText;
    }
}
