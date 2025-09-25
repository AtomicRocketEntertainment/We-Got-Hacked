using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstanceAuctionHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lastBid;
    [SerializeField] private TextMeshProUGUI _bidQuantity;
    [SerializeField] private TextMeshProUGUI _bidViews;
    [SerializeField] private Button _auctionBtn;

    public void UpdateInfos(SO_AuctionSample sampleInfos)
    {
        _lastBid.SetText($"BrC {sampleInfos.LastBid}");
        _bidQuantity.SetText($"{sampleInfos.BidMadeQuantity}");
        _bidViews.SetText($"{sampleInfos.BidViews}");
    }

    public Button AuctionButton => _auctionBtn;
}
