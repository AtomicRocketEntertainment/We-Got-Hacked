using TMPro;
using UnityEngine;

public class EmailInstance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sender;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _contentSmall;

    public void UpdateInfos(string sender, string title, string contentSmall)
    {
        _sender.text = sender;
        _title.text = title;
        _contentSmall.text = contentSmall;
    }
}
