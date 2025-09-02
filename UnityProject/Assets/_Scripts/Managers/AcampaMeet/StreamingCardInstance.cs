using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreamingCardInstance : MonoBehaviour, IMeetingPersonInstance
{
    [SerializeField] private GameObject _micIcon;
    [SerializeField] private GameObject _talkingBaloon;
    [SerializeField] private GameObject _talkingBackground;
    [SerializeField] private RectTransform _baloonBackground;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _profile;

    private void FixRects()
    {
        float textHeight;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_text.rectTransform);
        _text.TryGetComponent(out RectTransform rect);
        textHeight = rect.sizeDelta.y;

        _baloonBackground.sizeDelta = new Vector2(_baloonBackground.sizeDelta.x, textHeight + 30f);

        _talkingBaloon.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        _talkingBaloon.LeanScaleX(1.0f, 0.25f);
    }

    public void UpdateMyCard(Sprite profile)
    {
        _profile.sprite = profile;
        CloseMic();
    }

    public void OpenMicAndTalk(string text)
    {
        _micIcon.SetActive(false);
        _talkingBaloon.SetActive(true);
        _talkingBackground.SetActive(true);

        _text.text = text;
        FixRects();
    }

    public void CloseMic()
    {
        _micIcon.SetActive(true);
        _talkingBaloon.SetActive(false);
        _talkingBackground.SetActive(false);
    }

    public Transform GetTransform() => this.transform;
}