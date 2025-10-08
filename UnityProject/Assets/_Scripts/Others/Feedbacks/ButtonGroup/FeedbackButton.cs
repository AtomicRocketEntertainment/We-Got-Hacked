using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackButton : MonoBehaviour, IFeedbackGroupButton
{
    [SerializeField] private GameObject _spriteToShow;

    [BoxGroup("Text Feature"), SerializeField] private bool _hasTextFeature = false;
    [BoxGroup("Text Feature"), ShowIf(nameof(_hasTextFeature)), SerializeField] private FontStyles _styleToUpdate;
    [BoxGroup("Text Feature"), ShowIf(nameof(_hasTextFeature)), SerializeField] private FontStyles _normalStyle;
    [BoxGroup("Text Feature"), ShowIf(nameof(_hasTextFeature)), SerializeField] private TextMeshProUGUI _text;

    [BoxGroup("Color Feature"), SerializeField] private bool _hasColorFeature = false;
    [BoxGroup("Color Feature"), ShowIf(nameof(_hasColorFeature)), SerializeField] private Color _defaultColor = Color.white;
    [BoxGroup("Color Feature"), ShowIf(nameof(_hasColorFeature)), SerializeField] private Color _updatedColor;
    [BoxGroup("Color Feature"), ShowIf(nameof(_hasColorFeature)), SerializeField] private Image _spriteToPaint;


    public void ActiveFeedback()
    {
        _spriteToShow.SetActive(true);

        if (!_hasTextFeature) return;

        _text.fontStyle = _styleToUpdate;

        if (!_hasColorFeature) return;

        _spriteToPaint.color = _updatedColor;
    }
    public void DesactiveFeedback()
    {
        _spriteToShow.SetActive(false);

        if (!_hasTextFeature) return;

        _text.fontStyle = _normalStyle;

        if (!_hasColorFeature) return;

        _spriteToPaint.color = _defaultColor;
    }
}
