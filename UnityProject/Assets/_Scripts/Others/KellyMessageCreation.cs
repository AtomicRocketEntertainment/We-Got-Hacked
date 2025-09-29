using UnityEngine;

public class KellyMessageCreation : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;

    private readonly string _kellyLInk = "Caveira"; 

    void OnEnable()
    {
        EventManager.OnLinkIsClicked += OpenCanvas;
    }

    void OnDisable()
    {
        EventManager.OnLinkIsClicked -= OpenCanvas;
    }

    private void OpenCanvas(string link)
    {
        if (link == _kellyLInk)
        {
            _canvas.SetActive(true);
            EventManager.EnablePlayerWriteEmail();
        }
    }
}
