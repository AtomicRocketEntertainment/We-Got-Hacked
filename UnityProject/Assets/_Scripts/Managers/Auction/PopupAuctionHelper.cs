using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupAuctionHelper : MonoBehaviour
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private TextMeshProUGUI[] _headers;
    [SerializeField] private TextMeshProUGUI[] _lineOne;
    [SerializeField] private TextMeshProUGUI[] _lineTwo;
    [SerializeField] private TextMeshProUGUI[] _lineThree;
    [SerializeField] private TextMeshProUGUI[] _lineFour;

    private void OnEnable()
    {
        _closeBtn.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _closeBtn.onClick.RemoveListener(Close);
    }

    public void UpdateAuctionPopUp(SO_AuctionSample sample)
    {
        for (int i = 0; i < sample.LineHeader.Length; i++)
        {
            _headers[i].SetText(sample.LineHeader[i]);
            _lineOne[i].SetText(sample.LineOne[i]);
            _lineTwo[i].SetText(sample.LineTwo[i]);
            _lineThree[i].SetText(sample.LineThree[i]);
            _lineFour[i].SetText(sample.LineFour[i]);
        }
    }

    private void Close() => this.gameObject.SetActive(false);

}
