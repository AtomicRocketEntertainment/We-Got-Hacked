using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockInformationHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _content;

    public void UpdateNewsInfo(string title, string description)
    {
        _header.text = title;
        _content.text = description;
    }
}
