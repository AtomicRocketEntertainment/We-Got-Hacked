using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    [SerializeField] private List<SO_MeetingPerson> _meetingPersons = new List<SO_MeetingPerson>();
    [SerializeField] private List<string> _speakerTimeline = new List<string>();

    private Dictionary<string, MeetingPerson> _peopleAtCall;
    private int _currentSpeaker;

    private void Awake()
    {
        _currentSpeaker = 0;
        _peopleAtCall = new Dictionary<string, MeetingPerson>();

        for (int personIndex = 0; personIndex < _meetingPersons.Count; personIndex++)
        {
            MeetingPerson newPerson = new MeetingPerson(_meetingPersons[personIndex]);
            SpawnStreaming(newPerson);

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
        _awaitingScreen.SetActive(true);

        StartCoroutine(LoginPeople());
    }

    private IEnumerator LoginPeople()
    {
        //First person starts the meeting, already in the call.
        KeyValuePair<string, MeetingPerson> firstPerson = _peopleAtCall.FirstOrDefault();
        SpawnAwainting(firstPerson.Value);

        foreach (var key in _peopleAtCall)
        {
            if (key.Key != firstPerson.Key)
            {
                int randomSec = Random.Range(1, 4);
                yield return new WaitForSeconds(randomSec);

                SpawnAwainting(key.Value);
            }
        }
        yield return new WaitForSeconds(2f);
        EventManager.MakePlayerThink(ThoughtKey.ShouldStartTheMeeting);
        yield return new WaitForSeconds(3f);
        StartStreaming();
    }

    private void StartStreaming()
    {
        _awaitingScreen.SetActive(false);
        _streamingScreen.SetActive(true);

        if (_peopleAtCall.TryGetValue(_speakerTimeline[_currentSpeaker], out MeetingPerson person))
        {
            person.StartTalking();
        }
    }

    private void SpawnAwainting(MeetingPerson person)
    {
        GameObject newCard;
        newCard = Instantiate(_awaitingPrefab, _awaitingContainer);
        newCard.name = person.Name;

        newCard.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        newCard.LeanScale(Vector3.one, 0.15f);

        newCard.TryGetComponent(out AwaitingCardInstance instance);
        instance.UpdateMyCard(person.Idle);
    }

    private void SpawnStreaming(MeetingPerson person)
    {
        GameObject newCard;
        newCard = Instantiate(_streamingPrefab, _streamingContainer);
        newCard.name = person.Name;

        newCard.TryGetComponent(out IMeetingPersonInstance instance);
        person.InjectPersonCard(instance);
        instance.UpdateMyCard(person.Idle);
    }

    public void CloseCanvas() => _mainCanvas.SetActive(false);
    public void OpenCanvas() => _mainCanvas.SetActive(true);
}
