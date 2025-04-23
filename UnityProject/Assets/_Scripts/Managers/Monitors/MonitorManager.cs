using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonitorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _webSite;
    [SerializeField] private SiteTabsManager _siteManager;
    [SerializeField] private ToolBarManager _toolsManager;

    [SerializeField] private Button _browserButton;
    [SerializeField] private GameObject _browserBar;

    void OnEnable()
    {
        _siteManager.Init(this);
        _toolsManager.Init(this);

        EventManager.OnWebsiteLinkerIsOpen += UpdateWebSite;
        _browserButton.onClick.AddListener(OpenSite);
    }

    void OnDisable()
    {
        _toolsManager.CloseToolBar();
        _siteManager.CloseSiteBar();
        EventManager.OnWebsiteLinkerIsOpen -= UpdateWebSite;
        _browserButton.onClick.RemoveListener(OpenSite);
    }

    private void UpdateWebSite(string link)
    {
        _webSite.text = link;
    }

    private void OpenSite()
    {
        _browserBar.SetActive(true);
        _siteManager.OpenLastSite();
        _toolsManager.ClosePrograms();
    }
    public void ClosePrograms()
    {
        _toolsManager.ClosePrograms();
    }

    public void CloseSites()
    {
        _siteManager.CloseSites();
        _browserBar.SetActive(false);
    }

}
