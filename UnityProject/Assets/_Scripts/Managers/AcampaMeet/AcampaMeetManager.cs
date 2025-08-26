using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class AcampaMeetManager : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField, BoxGroup("Screens")] private GameObject _mainCanvas;
    [SerializeField, BoxGroup("Screens")] private GameObject _startScreen;
    [SerializeField, BoxGroup("Screens")] private GameObject _awaitingScreen;
    [SerializeField, BoxGroup("Screens")] private GameObject _streamingScreen;

    [SerializeField, BoxGroup("Start Screen")] private Button _startMeetingButton;

    [SerializeField, BoxGroup("Awaiting Screen")] private Transform _awaitingContainer;

    [SerializeField, BoxGroup("Streaming Screen")] private Transform _streamingContainer;
    [SerializeField, BoxGroup("Streaming Screen")] private Image _streamingImage;

    [SerializeField, BoxGroup("Prefabs")] private GameObject _awaitingPrefab;
    [SerializeField, BoxGroup("Prefabs")] private GameObject _streamingPrefab;


    [SerializeField] private List<string> _speakerTimeline = new List<string>();
    [SerializeField] private List<SO_MeetingPerson> _meetingPersons = new List<SO_MeetingPerson>();
    [SerializeField] private List<MeetingPersonLines> _meetingPersonLines = new List<MeetingPersonLines>();

    private Dictionary<string, MeetingPerson> _peopleAtCall;
    private int _currentSpeaker;

    private void Awake()
    {
        if (_meetingPersons.Count != _meetingPersonLines.Count)
        {
            Debug.LogWarning("Lista de personagens e lista de linhas de falas devem ser do mesmo tamanho.");
            return;
        }

        _currentSpeaker = 0;
        _peopleAtCall = new Dictionary<string, MeetingPerson>();

        for (int personIndex = 0; personIndex < _meetingPersons.Count; personIndex++)
        {
            MeetingPerson newPerson = new MeetingPerson(_meetingPersons[personIndex], _meetingPersonLines[personIndex]);

            if (!_peopleAtCall.ContainsKey(newPerson.Name))
                _peopleAtCall.Add(newPerson.Name, newPerson);
        }
    }

    private void OnEnable()
    {
        _startMeetingButton.onClick.AddListener(StartMeeting);
    }

    private void OnDisable()
    {
        _startMeetingButton.onClick.RemoveListener(StartMeeting);
    }

    private void StartMeeting()
    {
        EventManager.DisableToolbar();
        _startScreen.SetActive(false);
        _awaitingScreen.SetActive(false);

        StartCoroutine(LoginPeople());
    }

    private IEnumerator LoginPeople()
    {
        yield return new WaitForSeconds(0);
    }

    public void CloseCanvas()
    {
        throw new System.NotImplementedException();
    }

    public void OpenCanvas()
    {
        throw new System.NotImplementedException();
    }
}
