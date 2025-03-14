using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicketsManager : MonoBehaviour
{
    [Header("UI Dependencies")]
    [SerializeField] private Button _newTicketBtn;
    [SerializeField] private Button _currentTicketBtn;
    [SerializeField] private Button _doneTicketBtn;
    [SerializeField] private Button _playbookBtn;

    [Header("Screens")]
    [SerializeField] private GameObject _newTicketCanvas;
    [SerializeField] private GameObject _currentTicketCanvas;
    [SerializeField] private GameObject _doneTicketCanvas;
    [SerializeField] private GameObject _playbooksCanvas;

    [Header("System Dependencies")]
    [SerializeField] private SiemManager _siem;

    private Dictionary<Button, GameObject> _screens = new Dictionary<Button, GameObject>();

    void OnEnable()
    {
        if(!_screens.ContainsKey(_newTicketBtn)) _screens.Add(_newTicketBtn, _newTicketCanvas);
        if(!_screens.ContainsKey(_currentTicketBtn)) _screens.Add(_currentTicketBtn, _currentTicketCanvas);
        if(!_screens.ContainsKey(_doneTicketBtn)) _screens.Add(_doneTicketBtn, _doneTicketCanvas);
        if(!_screens.ContainsKey(_playbookBtn)) _screens.Add(_playbookBtn, _playbooksCanvas);

        _newTicketBtn.onClick.AddListener(() => OpenScreen(_newTicketBtn));
        _currentTicketBtn.onClick.AddListener(() => OpenScreen(_currentTicketBtn));
        _doneTicketBtn.onClick.AddListener(() => OpenScreen(_doneTicketBtn));
        _playbookBtn.onClick.AddListener(() => OpenScreen(_playbookBtn));

        OpenScreen(_newTicketBtn);
    }

    void OnDisable()
    {
        _newTicketBtn.onClick.RemoveAllListeners();
        _currentTicketBtn.onClick.RemoveAllListeners();
        _doneTicketBtn.onClick.RemoveAllListeners();
        _playbookBtn.onClick.RemoveAllListeners();
    }

    private void OpenScreen(Button button)
    {
        foreach(var screen in _screens)
        {
            if(screen.Key == button)
            {
                GameObject screenObj = screen.Value;
                screenObj.TryGetComponent(out TicketScreen ticketUpdater);
                ticketUpdater.UpdateInfos(ticketUpdater.CurrentType, _siem);
                screenObj.SetActive(true);

            }
            else
                screen.Value.SetActive(false);
        }
    }

}
