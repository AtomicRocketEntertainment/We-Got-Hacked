using UnityEngine;
using UnityEngine.UI;

public class MonitorManager : MonoBehaviour
{
    [SerializeField] private SiteTabsManager _siteManager;
    [SerializeField] private ToolBarManager _toolsManager;

    [SerializeField] private Button _browserButton;
    [SerializeField] private GameObject _browserBar;

    void OnEnable()
    {
        _siteManager.Init(this);
        _toolsManager.Init(this);

        _browserButton.onClick.AddListener(OpenSite);
    }

    void OnDisable()
    {
        _toolsManager.CloseToolBar();
        _siteManager.CloseSiteBar();
        _browserButton.onClick.RemoveListener(OpenSite);
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
