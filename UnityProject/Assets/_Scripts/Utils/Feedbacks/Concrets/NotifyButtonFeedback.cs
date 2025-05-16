using UnityEngine;
using UnityEngine.EventSystems;

public class NotifyButtonFeedback : MonoBehaviour, IFeedback, IPointerClickHandler
{
    [SerializeField] private GameObject notificationObj;
    [SerializeField] private GameObject _clickedObj;

    public void ShowFeedback(GameObject obj)
    {
        notificationObj.SetActive(true);
    }

    public void HideFeedback(GameObject obj)
    {
        notificationObj.SetActive(false);
    }

    public void HideFeedback()
    {
        notificationObj.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clickedObj.SetActive(true);
    }

    public void HideClickFeedback()
    {
        _clickedObj.SetActive(false);
    }
}
