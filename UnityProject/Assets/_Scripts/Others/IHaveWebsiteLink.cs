using UnityEngine;

public class IHaveWebsiteLink : MonoBehaviour
{
    [SerializeField] private string _link;

    void OnEnable()
    {
        EventManager.WebsiteIsOpen(_link);
    }

}
