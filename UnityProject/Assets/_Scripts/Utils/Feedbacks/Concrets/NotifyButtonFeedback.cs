using UnityEngine;
using UnityEngine.UI;

public class NotifyButtonFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private GameObject notificationObj;

    public void ShowFeedback(GameObject obj)
    {
        notificationObj.SetActive(true);

        Image img = notificationObj.GetComponent<Image>();

        Color startColor = img.color;
        startColor.a = 1f;
        img.color = startColor;

        LeanTween.cancel(notificationObj);

        LeanTween.alpha(notificationObj.GetComponent<RectTransform>(), 0f, 0.4f)
            .setEaseInOutSine()
            .setLoopPingPong();
    }

    public void HideFeedback(GameObject obj)
    {
        StopBlinking();
    }

    public void HideFeedback()
    {
        StopBlinking();
    }

    private void StopBlinking()
    {
        LeanTween.cancel(notificationObj);
        notificationObj.SetActive(false);
    }
}
