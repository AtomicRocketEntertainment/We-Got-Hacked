using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiteContentManager : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField] private GameObject _mainCanvas;
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
            screenToActivate.SetActive(true);
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
