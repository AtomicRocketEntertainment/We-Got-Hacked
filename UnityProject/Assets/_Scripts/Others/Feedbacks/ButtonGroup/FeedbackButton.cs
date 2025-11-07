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

    [BoxGroup("Animated Seetings"), SerializeField] private bool _shouldGoBackToColor = false;


    public void ActiveFeedback()
    {
        if (_spriteToShow != null) _spriteToShow.SetActive(true);

        if (_hasTextFeature)
        {
            _text.fontStyle = _styleToUpdate;
        }


        if (_hasColorFeature)
        {
            _spriteToPaint.color = _updatedColor;
        }
    }

    public void DesactiveFeedback()
    {
        if (_spriteToShow != null) _spriteToShow.SetActive(false);

        if (_hasTextFeature)
        {
            _text.fontStyle = _normalStyle;
        }


        if (_hasColorFeature)
        {
            _spriteToPaint.color = _defaultColor;
        }

    }

    private void OnDisable()
    {
        if (_shouldGoBackToColor)
            DesactiveFeedback();
    }
}
