using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SiteTabsManager : MonoBehaviour
{
    [SerializeField] List<SO_Software> _softwares;
    [SerializeField] List<Button> _buttons;
    [SerializeField] private GridObjectActiveHandler _petroButton;
    

    private Dictionary<Button, SiteInfoHolder> _softwareHandler = new Dictionary<Button, SiteInfoHolder>();
    private MonitorManager _monitor;
    private GameObject _lastSite;

    private readonly string _siteUrl = "petrocais.com";

    public void Init(MonitorManager monitor)
    {
        _monitor = monitor;
        EventManager.OnLinkIsClicked += ActiveSite;

        for(int i = 0; i < _buttons.Count; i++)
        {
            int index = i;
            _buttons[index].onClick.AddListener(() => OpenSite(_buttons[index]));
            GameObject newScreen = Instantiate(_softwares[i].Prefab, Vector3.zero, Quaternion.identity);
            _softwareHandler.Add(_buttons[i], new SiteInfoHolder(_softwares[i], newScreen));
            newScreen.TryGetComponent(out INeedOpenCanvas closecanvas);
            closecanvas?.CloseCanvas();
            

            if(newScreen.TryGetComponent(out EmailHandler emailHandler))
            {
                emailHandler.OpenCanvas();
                _lastSite = newScreen;
            }
        }
    }

    private void OpenSite(Button button)
    {
        _softwareHandler.TryGetValue(button, out SiteInfoHolder newScreen);
        newScreen.InstanciedScreen.TryGetComponent(out INeedOpenCanvas openCanvas);

        
        foreach(SiteInfoHolder screen in _softwareHandler.Values)
        {
            screen.InstanciedScreen.TryGetComponent(out INeedOpenCanvas closecanvas);
            closecanvas?.CloseCanvas();
        }
        
        if(newScreen != null)
        {
            openCanvas.OpenCanvas();
            _monitor.ClosePrograms();
        } 
    }

    public void OpenLastSite()
    {
        if(_lastSite == null) return;

        _lastSite.TryGetComponent(out INeedOpenCanvas openCanvas);
        openCanvas.OpenCanvas();
    }

    public void ActiveSite(string siteName)
    {
        if(siteName != _siteUrl) return;
        
        _petroButton.Active();
    }

    public void CloseSites()
    {
        foreach(SiteInfoHolder screen in _softwareHandler.Values)
        {
            if(screen.InstanciedScreen.transform.GetChild(0).gameObject.activeSelf)
                _lastSite = screen.InstanciedScreen;
                
            screen.InstanciedScreen.TryGetComponent(out INeedOpenCanvas closecanvas);
            closecanvas?.CloseCanvas();
        }
    }

    public void CloseSiteBar()
    {
        EventManager.OnLinkIsClicked -= ActiveSite;
        foreach(Button button in _softwareHandler.Keys)
            button.onClick.RemoveAllListeners();
    }
}

public class SiteInfoHolder
{
    public SiteInfoHolder(SO_Software infos, GameObject screen)
    {
        InstanciedScreen = screen;
        Type = infos.Type;
        Website = infos.Website;
        Icon = infos.Icon;
    }

    public GameObject InstanciedScreen;
    public SoftwareType Type;
    public string Website;
    public Sprite Icon;
}
