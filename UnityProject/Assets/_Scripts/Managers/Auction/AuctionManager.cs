using NaughtyAttributes;
using UnityEngine;

public class AuctionManager : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Dependencies"), SerializeField] private PopupAuctionHelper _popupAuction;
    [BoxGroup("Dependencies"), SerializeField] private SO_AuctionSample[] _samples;
    [BoxGroup("Dependencies"), SerializeField] private InstanceAuctionHelper[] _auctions;

    private bool _alreadyNotify;

    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }

    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
    }

    private void Awake()
    {
        if (_samples.Length != _auctions.Length)
        {
            Debug.LogWarning("Tamanho de samples e de leilões precisa ser o mesmo.");
            return;
        }

        for (int i = 0; i < _samples.Length; i++)
        {
            int index = i;
            SO_AuctionSample sample = _samples[index];
            _auctions[index].AuctionButton.onClick.AddListener(() => OpenPopUp(sample));
            _auctions[index].UpdateInfos(sample);
        }

        _alreadyNotify = false;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _samples.Length; i++)
            _auctions[i].AuctionButton.onClick.RemoveAllListeners();

    }

    private void OpenPopUp(SO_AuctionSample sampleInfo)
    {
        _popupAuction.gameObject.SetActive(true);
        _popupAuction.UpdateAuctionPopUp(sampleInfo);

        if (!_alreadyNotify && sampleInfo.IsCorrect)
        {
            _alreadyNotify = true;
            EventManager.SpawnEmail(EmailType.NEWS);
            EventManager.EnablePlayerWriteEmail();
        }
    }
}
