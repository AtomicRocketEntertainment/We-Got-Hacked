using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private EventReference _hoverSFX;
    [SerializeField] private EventReference _clickSFX;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverBehaviour();
    }

    private void Awake()
    {
        _button.onClick.AddListener(ClickBehaviour);
    }

    private void ClickBehaviour()
    {
        PlaySFX(_clickSFX);
    }

    private void HoverBehaviour()
    {
        PlaySFX(_hoverSFX);
    }

    private void PlaySFX(EventReference sfx)
    {
        var instance = RuntimeManager.CreateInstance(sfx);

        instance.start();
        instance.release();
    }
}
