using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarManager : MonoBehaviour
{
    [SerializeField] List<SO_Software> _softwares;
    [SerializeField] List<Button> _softwareButtons;

    private Dictionary<Button, GameObject> _softwareHandler = new Dictionary<Button, GameObject>();
    private MonitorManager _monitorReference;

    public void Init(MonitorManager monitor)
    {
        _monitorReference = monitor;
        for(int i = 0; i < _softwareButtons.Count; i++)
        {
            int index = i;
            _softwareButtons[index].onClick.AddListener(() => OpenScreen(_softwareButtons[index]));
            GameObject newScreen = Instantiate(_softwares[index].Prefab, Vector3.zero, Quaternion.identity);
            _softwareHandler.Add(_softwareButtons[index], newScreen);
            newScreen.SetActive(false);
        }
    }

    private void OpenScreen(Button button)
    {
        _softwareHandler.TryGetValue(button, out GameObject newScreen);
        
        foreach(GameObject screen in _softwareHandler.Values)
            screen.SetActive(false);
        
        if(newScreen)
        {
            newScreen.SetActive(true);
            _monitorReference.CloseSites();
        } 
    }

    public void CloseToolBar()
    {
        foreach(Button button in _softwareHandler.Keys)
            button.onClick.RemoveAllListeners();
    }

    public void ClosePrograms()
    {
        foreach(GameObject screen in _softwareHandler.Values)
            screen.SetActive(false);
    }
}
