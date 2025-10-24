using UnityEngine;

public class PopUpEmailHandler : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;

    [SerializeField] private string _textLinkToCheck; 

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
        if (link == _textLinkToCheck)
        {
            _canvas.SetActive(true);
            EventManager.MakePlayerThink(ThoughtKey.OpenKellyMessage);
            EventManager.EnablePlayerWriteEmail();
        }
    }
}
