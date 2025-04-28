using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnOffLoggerToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool _correctSite;
    [SerializeField] private Color _onColor;
    [SerializeField] private Color _offColor;
    [SerializeField] private GameObject _circle;
    [SerializeField] private Image _background;

    private bool _active = true;
    private const float Xon = -28f;
    private const float Xoff = -2f;
    private const string WRONG_SITE_TO_SHUTDOWN = "Pensando melhor, acho que não é este site que devo desligar.";

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_correctSite)
        {
            EventManager.CorrectChoice();
            VisualFeedback();
            return;
        }

        EventManager.MakePlayerThink(WRONG_SITE_TO_SHUTDOWN);
        EventManager.WrongChoice();

    }

    private void VisualFeedback()
    {
        LeanTween.cancel(_circle);
        _active = !_active;
        float goTo = _active ? Xon : Xoff;
        Color color = _active ? _onColor : _offColor;

        _background.color = color;
        LeanTween.moveLocalX(_circle, goTo, 0.2f).setEase(LeanTweenType.easeOutQuad);
    }
}
