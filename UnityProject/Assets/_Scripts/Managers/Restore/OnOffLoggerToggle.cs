using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NaughtyAttributes;

public class OnOffLoggerToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool _correctSite;
    [BoxGroup("Colors of Components"), SerializeField] private Color _circleOnColor;
    [BoxGroup("Colors of Components"), SerializeField] private Color _circleOffColor;
    [BoxGroup("Colors of Components"), SerializeField] private Color _headerOnColor;
    [BoxGroup("Colors of Components"), SerializeField] private Color _headerOffColor;
    [BoxGroup("Colors of Components"), SerializeField] private Color _backgroundOnColor;
    [BoxGroup("Colors of Components"), SerializeField] private Color _backgroundOffColor;
    [BoxGroup("Objects"), SerializeField] private GameObject _circle;
    [BoxGroup("Objects"), SerializeField] private Image _circleBackground;
    [BoxGroup("Objects"), SerializeField] private Image _headerBackground;
    [BoxGroup("Objects"), SerializeField] private Image _componentBackground;


    private bool _active = true;
    private bool _canInteractive = false;
    private const float Xon = -2f;
    private const float Xoff = -28f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_correctSite && _canInteractive)
        {
            EventManager.CorrectChoice();
            EventManager.TicketObjectiveCompleted();
            VisualFeedbackWithUpdateState();
            return;
        }

        if(_canInteractive)
            EventManager.MakePlayerThink(ThoughtKey.ShutdownWrongSite);
        else
            EventManager.MakePlayerThink(ThoughtKey.WrongTimeShutdownSite);

        EventManager.WrongChoice();
    }

    private void VisualFeedbackWithUpdateState()
    {
        EventManager.SiteIsOff(_active ? RestoreState.Logger : RestoreState.None);
        
        LeanTween.cancel(_circle);
        _active = !_active;
        float goTo = _active ? Xon : Xoff;
        Color color = _active ? _circleOnColor : _circleOffColor;
        Color headerColor = _active ? _headerOnColor : _headerOffColor;
        Color componentColor = _active ? _backgroundOnColor : _backgroundOffColor;

        _circleBackground.color = color;
        _headerBackground.color = headerColor;
        _componentBackground.color = componentColor;
        LeanTween.moveLocalX(_circle, goTo, 0.2f).setEase(LeanTweenType.easeOutQuad);
    }

    public void ChangeInteractable(bool status)
    {
        _canInteractive = status;
    }
}
