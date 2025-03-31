using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float _timerCount = 15f;
    private float _currentTimer;
    private bool _isRunning = false;
    private bool _firstTimer = false;
    private int _currentTimerEvent;


    void OnEnable()
    {
        _currentTimerEvent = 0;
        EventManager.OnFirstTimeSoftwareOpen += SetFirstTimer;
    }

    void OnDisable()
    {
        EventManager.OnFirstTimeSoftwareOpen -= SetFirstTimer;    
    }

    private void Update()
    {
        if (!_isRunning) return; 

        _currentTimer -= Time.deltaTime;

        if(_currentTimer <= 0)
        {
            EventManager.TimerCompleted(_currentTimerEvent);
            _currentTimerEvent++;
            _isRunning = false;
            _currentTimer = 0;
        }
    }

    private void SetFirstTimer()
    {
        if(_firstTimer) return;

        _firstTimer = true;
        _isRunning = true;
        _currentTimer = _timerCount;
    }
}
