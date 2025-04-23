using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SiteContentManager : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField] private GameObject _siteHeaderTitle;
    [SerializeField] private GameObject _siteHeaderBg;
    [SerializeField] private RectTransform _buttonsTransform;
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _petrolinhoLeft;
    [SerializeField] private GameObject _petrolinhoRight;
    [SerializeField] private Button[] _siteButtons;
    [SerializeField] private GameObject[] _siteContentScreens;

    private Dictionary<Button, GameObject> _screenHandler = new Dictionary<Button, GameObject>();

    void OnEnable()
    {
        if (_siteButtons.Length != _siteContentScreens.Length)
        {
            Debug.LogWarning("Listas das telas dos sites e botões devem ser do mesmo tamanho");
            return;
        }

        for (int i = 0; i < _siteButtons.Length; i++)
        {
            if (!_screenHandler.ContainsKey(_siteButtons[i]))
                _screenHandler.Add(_siteButtons[i], _siteContentScreens[i]);

            Button buttonRef = _siteButtons[i]; 
            buttonRef.onClick.AddListener(() => ActiveButtonScreen(buttonRef));
        }
    }

    void OnDisable()
    {
        foreach (var button in _siteButtons)
            button.onClick.RemoveAllListeners();
    }

    private void ActiveButtonScreen(Button btn)
    {
        foreach (var screen in _siteContentScreens)
            screen.SetActive(false);

        if (_screenHandler.TryGetValue(btn, out GameObject screenToActivate))
        {
            screenToActivate.SetActive(true);

            if(btn != _siteButtons[0]) //not home screen
            {
                _petrolinhoLeft.gameObject.SetActive(false);
                _petrolinhoRight.gameObject.SetActive(false);
                _siteHeaderBg.SetActive(false);
                _siteHeaderTitle.SetActive(false);
                _buttonsTransform.anchoredPosition = new Vector2(_buttonsTransform.anchoredPosition.x, 290f);
            }
            else //home screen
            {
                _petrolinhoLeft.gameObject.SetActive(true);
                _petrolinhoRight.gameObject.SetActive(true);
                _siteHeaderBg.SetActive(true);
                _siteHeaderTitle.SetActive(true);
                _buttonsTransform.anchoredPosition = new Vector2(_buttonsTransform.anchoredPosition.x, 5f);
            }
        }
    }


    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }
}
