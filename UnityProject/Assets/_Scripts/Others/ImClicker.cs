using UnityEngine;
using UnityEngine.EventSystems;

public class ImClicker : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(this.gameObject);
    }
}
