using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupNotification : MonoBehaviour
{
    public static PopupNotification Instance;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _closeBtn;

    [Header("Animation")]
    [SerializeField] private float _fadeInTime = 0.3f;
    [SerializeField] private float _fadeOutTime = 0.3f;
    [SerializeField] private float _showTime = 4f;
    [SerializeField] private float _moveDistance = 40f;

    private Vector2 _defaultPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _defaultPosition = _rectTransform.anchoredPosition;

        _canvasGroup.alpha = 0;

        _closeBtn.onClick.AddListener(PopupClose);
    }

    private void OnEnable()
    {
        _closeBtn.onClick.AddListener(PopupClose);
    }

    private void OnDisable()
    {
        _closeBtn.onClick.AddListener(PopupClose);
    }

    private void ShowPopup(string message)
    {
        DOTween.Kill(_rectTransform);
        DOTween.Kill(_canvasGroup);

        _messageText.text = message;

        _canvasGroup.alpha = 0;

        _rectTransform.anchoredPosition = _defaultPosition + Vector2.down * _moveDistance;

        Sequence seq = DOTween.Sequence();

        seq.Append(_canvasGroup.DOFade(1f, _fadeInTime));

        seq.Join(
            _rectTransform.DOAnchorPos(
                _defaultPosition,
                _fadeInTime)
            .SetEase(Ease.OutBack)
        );

        seq.AppendInterval(_showTime);

        seq.Append(_canvasGroup.DOFade(0f, _fadeOutTime));

        seq.Join(
            _rectTransform.DOAnchorPos(
                _defaultPosition + Vector2.up * (_moveDistance * 0.5f),
                _fadeOutTime)
            .SetEase(Ease.InQuad)
        );

        seq.OnComplete(() =>
        {
            _canvasGroup.alpha = 0;
        });
    }

    private void PopupClose()
    {
        DOTween.Kill(_rectTransform);
        DOTween.Kill(_canvasGroup);

        _canvasGroup.alpha = 0;
    }

    public void OnStockDown(string playerCompany,int valueDay, int minValue)
    {
        if (PersistanceDataManager.Instance.StocksInfos.TryGetValue(playerCompany, out List<int> values))
        {
            int lastPetroCaisStock = values[values.Count - 1];

            int remainingErrors = Mathf.Max(0, ((lastPetroCaisStock - minValue) / valueDay) - 1);
            string remainingMsg = (remainingErrors > 0) ? $"Agora você só pode cometer mais {remainingErrors} erros" : $"Cuidado essa é sua ultima chance";
            string message = $"Decisão incorreta! Perda de {valueDay} pontos, {remainingMsg}";
            ShowPopup(message);
        }
    }

    public void OnStockUp(string playerCompany, int valueDay)
    {
        if (PersistanceDataManager.Instance.StocksInfos.TryGetValue(playerCompany, out List<int> values))
        {
            int lastPetroCaisStock = values[values.Count - 1];
            string message = $"Decisão correta! Ganho de {valueDay} pontos.";
            ShowPopup(message);
        }
    }

}
