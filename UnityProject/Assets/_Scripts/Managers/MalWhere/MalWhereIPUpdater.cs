using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MalWhereIPUpdater : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _ipField;
    [SerializeField] private TextMeshProUGUI _isInBankField;
    [SerializeField] private TextMeshProUGUI _relatedTimesField;
    [SerializeField] private TextMeshProUGUI _maliciousField;
    [SerializeField] private TextMeshProUGUI _domainNameField;
    [SerializeField] private TextMeshProUGUI _countryField;
    [SerializeField] private TextMeshProUGUI _stateField;
    [SerializeField] private TextMeshProUGUI _cityField;

    public void UpdateIpInfos(string ip, RansomwareInformations infos, Sprite icon)
    {
        _icon.sprite = icon;
        _ipField.SetText(ip);
        _isInBankField.SetText(infos.IsInTheBank);
        _relatedTimesField.SetText($"{infos.RelatedTimes}");
        _maliciousField.SetText(infos.MaliciousPercentage + "%");
        _domainNameField.SetText(infos.DomainName);
        _countryField.SetText(infos.Country);
        _stateField.SetText(infos.State);
        _cityField.SetText(infos.City);
    }
}
