using UnityEngine;
using UnityEngine.UI;

public class PopUpEmailHandler : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private string _textLinkToCheck;
    [SerializeField] private bool _isTiagoSideStory = true;

    private bool _alertIsDone = false;

    void OnEnable()
    {
        EventManager.OnLinkIsClicked += OpenCanvas;
        _closeBtn.onClick.AddListener(CloseCanvas);
    }

    void OnDisable()
    {
        EventManager.OnLinkIsClicked -= OpenCanvas;
        _closeBtn.onClick.RemoveListener(CloseCanvas);
    }

    private void OpenCanvas(string link)
    {
        if (link == _textLinkToCheck)
        {
            _canvas.SetActive(true);

            if (_isTiagoSideStory && !_alertIsDone)
            {
                _alertIsDone = true;
                EventManager.MakePlayerThink(ThoughtKey.OpenKellyMessage);
                EventManager.EnablePlayerWriteEmail();
            }
        }
    }

    private void CloseCanvas() => _canvas.SetActive(false);

}
