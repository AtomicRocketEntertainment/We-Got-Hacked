using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreamingCardInstance : MonoBehaviour, IMeetingPersonInstance
{
    [SerializeField] private GameObject _micIcon;
    [SerializeField] private GameObject _talkingBaloon;
    [SerializeField] private RectTransform _baloonBackground;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _profile;

    public void UpdateMyCard(Sprite profile)
    {
        _profile.sprite = profile;
        CloseMic();
    }

    public void OpenMicAndTalk(string text)
    {
        _micIcon.SetActive(false);
        _talkingBaloon.SetActive(true);

        _talkingBaloon.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        _talkingBaloon.LeanScaleX(1.0f, 0.25f).setOnComplete(() =>
        {
            _text.text = text;
            float textHeight;
            _text.TryGetComponent(out RectTransform rect);
            textHeight = rect.rect.height;
            _baloonBackground.sizeDelta = new Vector2(_baloonBackground.sizeDelta.x, textHeight + 50f);
        });

    }

    public void CloseMic()
    {
        _micIcon.SetActive(true);
        _talkingBaloon.SetActive(false);
    }
}