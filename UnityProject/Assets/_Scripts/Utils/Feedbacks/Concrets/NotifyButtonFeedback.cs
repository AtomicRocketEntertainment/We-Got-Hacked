using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotifyButtonFeedback : MonoBehaviour, IFeedback, IPointerClickHandler
{
    [SerializeField] private Image notificationObj;
    [SerializeField] private Image _clickedObj;

    public void ShowFeedback(GameObject obj)
    {
        notificationObj.enabled = true;
    }

    public void HideFeedback(GameObject obj)
    {
        notificationObj.enabled = false;
    }

    public void HideFeedback()
    {
        notificationObj.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clickedObj.enabled = true;
    }

    public void HideClickFeedback()
    {
        _clickedObj.enabled = false;
    }
}
