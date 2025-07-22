using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarManager : MonoBehaviour
{
    [SerializeField] List<SO_Software> _softwares;
    [SerializeField] List<Button> _softwareButtons;
    [SerializeField] GameObject _toolBarScreen;

    private Dictionary<Button, GameObject> _softwareHandler = new Dictionary<Button, GameObject>();
    private MonitorManager _monitorReference;

    public void Init(MonitorManager monitor)
    {
        EventManager.OnNotifyNeeded += UpdateBarNotification;
        EventManager.OnUserEnterRemotopia += HideToolBar;
        EventManager.OnUserQuitRemotopia += ShowToolBar;

        _monitorReference = monitor;

        for (int i = 0; i < _softwareButtons.Count; i++)
        {
            int index = i;
            _softwareButtons[index].onClick.AddListener(() => OpenScreen(_softwareButtons[index]));
            GameObject newScreen = Instantiate(_softwares[index].Prefab, Vector3.zero, Quaternion.identity);
            _softwareHandler.Add(_softwareButtons[index], newScreen);

            newScreen.TryGetComponent(out INeedOpenCanvas closecanvas);
            closecanvas?.CloseCanvas();
        }
    }

    private void ShowToolBar()
    {
        _toolBarScreen.SetActive(true);
    }

    private void HideToolBar()
    {
        _toolBarScreen.SetActive(false);
    }

    private void OpenScreen(Button button)
    {
        _softwareHandler.TryGetValue(button, out GameObject newScreen);
        newScreen.TryGetComponent(out INeedOpenCanvas openCanvas);
        
        foreach(GameObject screen in _softwareHandler.Values)
        {
            screen.TryGetComponent(out INeedOpenCanvas closecanvas);
            closecanvas?.CloseCanvas();
        }
        
        if(newScreen)
        {
            openCanvas.OpenCanvas();
            _monitorReference.CloseSites();
        } 
    }

    public void CloseToolBar()
    {
        foreach (Button button in _softwareHandler.Keys)
            button.onClick.RemoveAllListeners();

        EventManager.OnNotifyNeeded -= UpdateBarNotification;
        EventManager.OnUserEnterRemotopia -= HideToolBar;
        EventManager.OnUserQuitRemotopia -= ShowToolBar;
    }

    private void UpdateBarNotification(GameObject obj)
    {
        foreach (var pair in _softwareHandler)
        {
            if (pair.Value == obj)
            {
                GameObject correspondingButton = pair.Key.gameObject;
                correspondingButton.TryGetComponent(out NotifyButtonFeedback feedback);
                feedback.ShowFeedback(correspondingButton);
                break;
            }
        }
    }

    public void ClosePrograms()
    {
        foreach(GameObject screen in _softwareHandler.Values)
        {
            screen.TryGetComponent(out INeedOpenCanvas closecanvas);
            closecanvas?.CloseCanvas();
        }
    }
}


public enum SoftwareState
{
    Blocked, FirstTimeOpened, Opened, Empty, FullAccess
}
