using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SiteTabsManager : MonoBehaviour
{
    [SerializeField] List<SO_Software> _softwares;
    [SerializeField] List<Button> _buttons;

    private Dictionary<Button, SiteInfoHolder> _softwareHandler = new Dictionary<Button, SiteInfoHolder>();
    private MonitorManager _monitor;
    private GameObject _lastSite;

    public void Init(MonitorManager monitor)
    {
        _monitor = monitor;
        for(int i = 0; i < _buttons.Count; i++)
        {
            int index = i;
            _buttons[index].onClick.AddListener(() => OpenSite(_buttons[index]));
            GameObject newScreen = Instantiate(_softwares[i].Prefab, Vector3.zero, Quaternion.identity);
            _softwareHandler.Add(_buttons[i], new SiteInfoHolder(_softwares[i], newScreen));
            newScreen.SetActive(false);

            if(newScreen.TryGetComponent(out EmailHandler emailHandler))
            {
                emailHandler.gameObject.SetActive(true);
                _lastSite = newScreen;
            }
        }
    }

    private void OpenSite(Button button)
    {
        _softwareHandler.TryGetValue(button, out SiteInfoHolder newScreen);
        
        foreach(SiteInfoHolder screen in _softwareHandler.Values)
            screen.InstanciedScreen.SetActive(false);
        
        if(newScreen != null)
        {
            newScreen.InstanciedScreen.SetActive(true);
            _monitor.ClosePrograms();
        } 
    }

    public void OpenLastSite()
    {
        _lastSite?.SetActive(true);
    }

    public void CloseSites()
    {
        foreach(SiteInfoHolder screen in _softwareHandler.Values)
        {
            if(screen.InstanciedScreen.activeSelf)
                _lastSite = screen.InstanciedScreen;
                
            screen.InstanciedScreen.SetActive(false);
        }
    }

    public void CloseSiteBar()
    {
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
