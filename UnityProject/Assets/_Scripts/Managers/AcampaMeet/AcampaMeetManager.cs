using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class AcampaMeetManager : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField, BoxGroup("Screens and General")] private GameObject _mainCanvas;
    [SerializeField, BoxGroup("Screens and General")] private GameObject _startScreen;
    [SerializeField, BoxGroup("Screens and General")] private GameObject _awaitingScreen;
    [SerializeField, BoxGroup("Screens and General")] private GameObject _streamingScreen;
    [SerializeField, BoxGroup("Screens and General")] private Button _avanceLinesButton;


    [SerializeField, BoxGroup("Start Screen")] private Button _startMeetingButton;
    [SerializeField, BoxGroup("Start Screen")] private bool _shouldStartAutomatic;

    [SerializeField, BoxGroup("Awaiting Screen")] private Transform _awaitingContainer;

    [SerializeField, BoxGroup("Streaming Screen")] private Transform _streamingContainer;
    [SerializeField, BoxGroup("Streaming Screen")] private Image _streamingImage;

    [SerializeField, BoxGroup("Prefabs")] private GameObject _awaitingPrefab;
    [SerializeField, BoxGroup("Prefabs")] private GameObject _streamingPrefab;


    [SerializeField] private List<SO_MeetingPerson> _meetingPersons = new List<SO_MeetingPerson>();
    [SerializeField] private List<SO_MeetingPerson> _speakerTimelineSO = new List<SO_MeetingPerson>();


    private Dictionary<string, MeetingPerson> _peopleAtCall;
    private int _currentSpeaker;
    private MeetingPerson _lastPerson;

    private void Awake()
    {
        _avanceLinesButton.gameObject.SetActive(false);
        _currentSpeaker = 0;
        _lastPerson = null;
    }

    private void Start()
    {
        if (_shouldStartAutomatic)
        {
            SpawnPeople();
            StartStreaming();
        }
    }

    private void OnEnable()
    {
        EventManager.OnPlayerAnswerWrQuestion += ContinueWr;
        _startMeetingButton.onClick.AddListener(StartMeeting);
        _avanceLinesButton.onClick.AddListener(AvanceLine);
    }

    private void OnDisable()
    {
        EventManager.OnPlayerAnswerWrQuestion += ContinueWr;
        _startMeetingButton.onClick.RemoveListener(StartMeeting);
        _avanceLinesButton.onClick.RemoveListener(AvanceLine);
    }

    private void StartMeeting()
    {
        EventManager.DisableToolbar();
        SpawnPeople();
        _startScreen.SetActive(false);
        _awaitingScreen.SetActive(true);

        StartCoroutine(LoginPeople());
    }

    private void SpawnPeople()
    {
        _peopleAtCall = new Dictionary<string, MeetingPerson>();

        for (int personIndex = 0; personIndex < _meetingPersons.Count; personIndex++)
        {
            MeetingPerson newPerson = new MeetingPerson(_meetingPersons[personIndex]);
            SpawnStreaming(newPerson);

            if (!_peopleAtCall.ContainsKey(newPerson.Name))
                _peopleAtCall.Add(newPerson.Name, newPerson);
        }
    }

    private IEnumerator LoginPeople()
    {
        //First person starts the meeting, already in the call and responsable to answer the questions
        KeyValuePair<string, MeetingPerson> firstPerson = _peopleAtCall.FirstOrDefault();
        SpawnAwainting(firstPerson.Value);
        firstPerson.Value.SubscribeEvents();

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
        MakePeopleTalk();

    }

    private void MakePeopleTalk()
    {

        if (_currentSpeaker == _speakerTimelineSO.Count)
        {
            EventManager.ShowStoryBoard();
            return;
        }

        if (_peopleAtCall.TryGetValue(_speakerTimelineSO[_currentSpeaker].Name, out MeetingPerson person))
        {
            _lastPerson?.StopTalkin();
            _lastPerson = person;

            CheckNeedAnswer(person.IsLineToAnswer());
            CheckStreamUpdate(person.ShouldUpdateStream(), person.GetSpriteToShow());

            person.StartTalking();
            _currentSpeaker++;
        }
    }

    private void CheckStreamUpdate(bool shouldUpdateStream, Sprite spriteToShow)
    {
        if (shouldUpdateStream)
            _streamingImage.sprite = spriteToShow;
    }

    private void CheckNeedAnswer(bool isLineToAnswer)
    {
        if (isLineToAnswer)
        {
            EventManager.OpenGenericResponse();
            _avanceLinesButton.gameObject.SetActive(false);
        }
        else
            _avanceLinesButton.gameObject.SetActive(true);
    }

    private void ContinueWr()
    {
        KeyValuePair<string, MeetingPerson> firstPerson = _peopleAtCall.FirstOrDefault();
        _lastPerson?.StopTalkin();

        _lastPerson = firstPerson.Value; //Character is now the last talker
        _avanceLinesButton.gameObject.SetActive(true);
    }

    private void AvanceLine()
    {
        MakePeopleTalk();
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

    private void OnDestroy()
    {
        KeyValuePair<string, MeetingPerson> firstPerson = _peopleAtCall.FirstOrDefault();
        firstPerson.Value.UnsubscribeEvents();
    }
}
