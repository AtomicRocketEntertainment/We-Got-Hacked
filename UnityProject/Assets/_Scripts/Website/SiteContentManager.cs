using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiteContentManager : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField] private GameObject _siteHeaderBg;
    [SerializeField] private RectTransform _buttonsTransform;
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private Button[] _siteButtons;
    [SerializeField] private GameObject[] _siteContentScreens;

    private Dictionary<Button, GameObject> _screenHandler = new Dictionary<Button, GameObject>();
    private bool _firstTimeOpenSustentability;
    private readonly int _sustentabilityIndex = 4;

    private void Awake()
    {
        _firstTimeOpenSustentability = true;
    }

    void OnEnable()
    {
        if (_siteButtons.Length != _siteContentScreens.Length)
        {
            Debug.LogWarning("Listas das telas dos sites e botões devem ser do mesmo tamanho");
            return;
        }

        for (int i = 0; i < _siteButtons.Length; i++)
        {
            int index = i;
            if (!_screenHandler.ContainsKey(_siteButtons[i]))
                _screenHandler.Add(_siteButtons[i], _siteContentScreens[i]);

            Button buttonRef = _siteButtons[i]; 
            buttonRef.onClick.AddListener(() => ActiveButtonScreen(buttonRef, index));
        }
    }

    void OnDisable()
    {
        foreach (var button in _siteButtons)
            button.onClick.RemoveAllListeners();
    }

    private void ActiveButtonScreen(Button btn, int index)
    {
        if (index == _sustentabilityIndex && _firstTimeOpenSustentability) //not the bast way, but works for now.
        {
            _firstTimeOpenSustentability = false;
            EventManager.MakePlayerThink(ThoughtKey.OpenHackedWebsite);
        }
            
        foreach (var screen in _siteContentScreens)
            screen.SetActive(false);

        if (_screenHandler.TryGetValue(btn, out GameObject screenToActivate))
        {
            screenToActivate.SetActive(true);
            bool isHomeScreen = btn == _siteButtons[0];
            ResizeHeader(isHomeScreen);
        }
    }

    private void ResizeHeader(bool isHomeScreen)
    {
        Vector2 newPos = _buttonsTransform.anchoredPosition;
        float newY = isHomeScreen ? 0f : 250f;
        newPos.y = newY;
        _siteHeaderBg.SetActive(!isHomeScreen);
        _siteHeaderBg.SetActive(isHomeScreen);

        _buttonsTransform.anchoredPosition = newPos;
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
