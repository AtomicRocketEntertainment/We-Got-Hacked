using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MalWhereHashUpdater : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _hashMainNameField;
    [SerializeField] private TextMeshProUGUI _hashExeNameField;
    [SerializeField] private TextMeshProUGUI _lastAnalysisField;
    [SerializeField] private TextMeshProUGUI _sizeField;
    [SerializeField] private TextMeshProUGUI _aboutRansomwareField;
    [SerializeField] private TextMeshProUGUI _manufacturerOneField;
    [SerializeField] private TextMeshProUGUI _manufacturerTwoField;

    public void UpdateHashInfos(RansomwareInformations infos, Sprite icon)
    {
        _icon.sprite = icon;
        _hashMainNameField.SetText(infos.Hash);
        _hashExeNameField.SetText(infos.Exe);
        _lastAnalysisField.SetText(infos.LastAnalysis + " meses atrás");
        _sizeField.SetText(infos.Size + " KB");
        _aboutRansomwareField.SetText(infos.Description);
        _manufacturerOneField.SetText("AntiVirótico: " + infos.ManufacterNameOne);
        _manufacturerTwoField.SetText("Lava Drive: " + infos.ManufacterNameTwo);
    }
}
