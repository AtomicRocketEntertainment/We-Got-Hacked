using UnityEngine;
using UnityEngine.EventSystems;

public class HooverFeedback : MonoBehaviour, IFeedback, IPointerEnterHandler, IPointerExitHandler
{

   public void OnPointerEnter(PointerEventData eventData)
   {
      ShowFeedback(this.gameObject);
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      HideFeedback(this.gameObject);
   }

   public void ShowFeedback(GameObject obj)
   {
      LeanTween.scale(obj, new Vector3(1.2f, 1.2f, 1.2f), 0.1f); 
   }

   public void HideFeedback(GameObject obj)
   {
      LeanTween.scale(obj, new Vector3(1.0f, 1.0f, 1.0f), 0.1f); 
   }
}
