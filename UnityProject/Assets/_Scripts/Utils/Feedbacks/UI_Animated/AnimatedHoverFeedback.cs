using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatedHoverFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [BoxGroup("Animated Seetings"), SerializeField] private float _animTime = 0.2f;
    [BoxGroup("Animated Seetings"), SerializeField] private LeanTweenType _tweenCurve = LeanTweenType.easeOutCubic;
    [BoxGroup("Animated Seetings"), SerializeField] private Vector3 _scaleToGo = new Vector3(1.1f, 1.1f, 1.1f);

    public void OnPointerEnter(PointerEventData eventData) => LeanTween.scale(this.gameObject, _scaleToGo, _animTime).setEase(_tweenCurve);
    public void OnPointerExit(PointerEventData eventData) => LeanTween.scale(this.gameObject, Vector3.one, _animTime).setEase(_tweenCurve);

    void OnDisable() => this.gameObject.transform.localScale = Vector3.one;
}
