using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlertInstance : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _idText;
    [SerializeField] private TextMeshProUGUI _riskLevel;
    [SerializeField] private Image _riskBg;
    [SerializeField] private List<Color> _riskColors;

    private Ticket _currentHolder; 

    public void Init(Ticket infos)
    {
        _currentHolder = infos;
        _idText.text = _currentHolder.ID;
        _riskLevel.text = $"<line-height=130%><size=120m>{_currentHolder.RiskLevel}</size>\nrisco</line-height>";

        _riskBg.color = _riskColors[_currentHolder.RiskLevel - 1];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.OpenAlert(_currentHolder, _riskBg.color);
    }
}
