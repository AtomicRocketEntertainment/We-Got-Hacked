using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotopiaManager : MonoBehaviour, INeedOpenCanvas, ISoftwareContext
{
    [SerializeField] private GameObject _mainCanvas;
    private SoftwareState _currentState = SoftwareState.Blocked;

    public void ChangeSoftwareState(SoftwareState state)
    {
        _currentState = state;
    }

    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }


    public void ChangeBlockedCanvasStatus(bool status)
    {
        //dont needed
    }
}
