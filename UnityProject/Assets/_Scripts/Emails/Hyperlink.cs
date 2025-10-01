using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class Hyperlink : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text _textMeshProComponent;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshProComponent, eventData.position, null);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = _textMeshProComponent.textInfo.linkInfo[linkIndex];
            EventManager.ClickLink(linkInfo.GetLinkID());
        }
    }
}
