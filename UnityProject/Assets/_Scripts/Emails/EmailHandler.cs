using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections;


public class EmailHandler : MonoBehaviour, INeedOpenCanvas
{
    [BoxGroup("Player Initial Status")] [SerializeField] private WriteEmailState _writeEmailState;
    [BoxGroup("Player Initial Status")] [SerializeField] private List<SO_Email> _firstEmails = new List<SO_Email>();
    [SerializeField] private List<SO_Email> _spamToSend = new List<SO_Email>();
    [SerializeField] private List<SO_Email> _loreToSend = new List<SO_Email>();
    [SerializeField] private List<SO_Email> _hackingToSend = new List<SO_Email>();
    [SerializeField] private List<SO_Email> _newsToSend = new List<SO_Email>();


    [HorizontalLine(color: EColor.Red)]
    [BoxGroup("Canvases")] [SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Canvases")] [SerializeField] private Transform _homeEmailCanvas;
    [BoxGroup("Canvases")] [SerializeField] private Transform _readingEmailCanvas;
    [BoxGroup("Canvases")] [SerializeField] private Transform _writingEmailCanvas;

    [BoxGroup("Email to Read Content"), HorizontalLine(color: EColor.Green)] [SerializeField] private TextMeshProUGUI _emailTitle;
    [BoxGroup("Email to Read Content")] [SerializeField] private TextMeshProUGUI _senderName;
    [BoxGroup("Email to Read Content")] [SerializeField] private TextMeshProUGUI _senderEmail;
    [BoxGroup("Email to Read Content")] [SerializeField] private TextMeshProUGUI _emailContent;
    [BoxGroup("Email to Read Content")] [SerializeField] private Image _senderProfilePicture;

    [BoxGroup("Email to Write Content"), HorizontalLine(color: EColor.Green)] [SerializeField] private TextMeshProUGUI _emailWriteTitle;
    [BoxGroup("Email to Write Content")] [SerializeField] private TextMeshProUGUI _receiverEmail;
    [BoxGroup("Email to Write Content")] [SerializeField] private TextMeshProUGUI _emailWriteContent;

    [BoxGroup("Prefabs"), HorizontalLine(color: EColor.Yellow)] [SerializeField] private GameObject _emailPrefab;
    [SerializeField] private Button _writeEmailBtn;

    private Dictionary<GameObject, Email> _emailsInstanciados = new Dictionary<GameObject, Email>();
    private Email _currentEmailOpen;
    private int _currentSpamSended;
    private int _currentNewsSended;
    private int _currentLoreSended;
    private int _currentHackingSended;

    private readonly string _lore9DayOne = "Lore 9";
    private readonly string _lore12DayOne = "Lore 12";
    private readonly string _lore4DayTwo = "Lore 4 Day 2";
    private readonly string _lore8DayTwo = "Lore 8 Day 2";
    private readonly string _loreA04 = "Lore A04";
    
    private const int LORE_TO_OPEN_WRITE_EMAIL = 5;

    private void OnEnable()
    {
        _currentSpamSended = _currentNewsSended = _currentLoreSended = _currentHackingSended = 0;
        _writeEmailBtn.onClick.AddListener(TryWriteEmail);
        EventManager.OnDisablePlayerWriteEmail += DisableEmailWrite;
        EventManager.OnEnablePlayerWriteEmail += EnableEmailWrite;
        EventManager.OnEventEmailHandlerIsOpen += UpdateState;
        EventManager.OnSpawnEmail += CreateEmail;
        EventManager.OnSpawnSpecificEmail += SpawnSpecificEmail;
        EventManager.OnOpenEmail += OpenEmail;
        EventManager.OnWriteEmail += TryWriteEmail;
        EventManager.OnChangeEmailContentText += ChangeContentEmail;
        EventManager.OnChangeEmailReceiver += ChangeWritingReceiver;
        EventManager.OnEmailIsAnswered += EmailIsAnswered;
        EventManager.OnReturnEmailContent += ReturnEmailContent;
        EventManager.OnTimerIsComplete += CheckToSpawn;

        foreach (SO_Email email in _firstEmails)
            CreateSpecificEmail(email, false, true);

    }
    void OnDisable()
    {
        _writeEmailBtn.onClick.RemoveListener(TryWriteEmail);
        EventManager.OnDisablePlayerWriteEmail -= DisableEmailWrite;
        EventManager.OnEnablePlayerWriteEmail -= EnableEmailWrite;
        EventManager.OnWriteEmail -= TryWriteEmail;
        EventManager.OnEventEmailHandlerIsOpen -= UpdateState;
        EventManager.OnSpawnEmail -= CreateEmail;
        EventManager.OnSpawnSpecificEmail -= SpawnSpecificEmail;
        EventManager.OnOpenEmail -= OpenEmail;
        EventManager.OnChangeEmailContentText -= ChangeContentEmail;
        EventManager.OnChangeEmailReceiver -= ChangeWritingReceiver;
        EventManager.OnEmailIsAnswered -= EmailIsAnswered;
        EventManager.OnReturnEmailContent -= ReturnEmailContent;
        EventManager.OnTimerIsComplete -= CheckToSpawn;
    }

    private void UpdateState(string emailIndex)
    {
        if(emailIndex == _lore8DayTwo)
            return;

        if (emailIndex == _lore9DayOne || emailIndex == _lore12DayOne || emailIndex == _lore4DayTwo || emailIndex == _loreA04)
                _writeEmailState = WriteEmailState.CanWrite;
            else
                _writeEmailState = WriteEmailState.CantWrite;
    }

    private void DisableEmailWrite() => _writeEmailState = WriteEmailState.CantWrite;
    private void EnableEmailWrite() => _writeEmailState = WriteEmailState.CanWrite;


    private void CreateEmail(EmailType emailType)
    {
        Email email = null;
        switch (emailType)
        {
            case EmailType.SPAM:
                if (_currentSpamSended < _spamToSend.Count)
                {
                    email = new Email(_spamToSend[_currentSpamSended]);
                    _currentSpamSended++;
                }
                break;
            case EmailType.LORE:
                if (_currentLoreSended < _loreToSend.Count)
                {
                    email = new Email(_loreToSend[_currentLoreSended]);
                    _currentLoreSended++;
                    UpdateLoreMechanics();
                }
                break;
            case EmailType.HACKING:
                if (_currentHackingSended < _hackingToSend.Count)
                {
                    email = new Email(_hackingToSend[_currentHackingSended]);
                    _currentHackingSended++;
                }
                break;
            case EmailType.NEWS:
                if (_currentNewsSended < _newsToSend.Count)
                {
                    email = new Email(_newsToSend[_currentNewsSended]);
                    _currentNewsSended++;
                }
                break;
        }

        if (email == null) return;

        StartCoroutine(SpawnEmail(email, Random.Range(2, 6)));
    }

    private void CreateSpecificEmail(SO_Email emailToCreate, bool shouldAdvanceHistory, bool spawnOnTime)
    {
        if(shouldAdvanceHistory)
        {
            _currentLoreSended++;
            UpdateLoreMechanics();
        }
        
        int seconds = spawnOnTime ? 0 : Random.Range(2, 6);
        Email email = new Email(emailToCreate);
        StartCoroutine(SpawnEmail(email, seconds));
    }

    private void SpawnSpecificEmail(PointEmailEntry emailToCreate)
    {
        if (emailToCreate.ShouldAdvanceHistory)
        {
            _currentLoreSended++;
            UpdateLoreMechanics();
        }
        
        int seconds = emailToCreate.SpawnOnTime ? 0 : Random.Range(2, 6);
        Email email = new Email(emailToCreate.email);
        StartCoroutine(SpawnEmail(email, seconds));
    }

    IEnumerator SpawnEmail(Email email, int seconds)
    {
        yield return new WaitForSeconds(seconds);

        GameObject instanceEmail = Instantiate(_emailPrefab, Vector3.zero, Quaternion.identity);
        instanceEmail.transform.SetParent(_homeEmailCanvas);
        instanceEmail.name = email.Title;
        instanceEmail.transform.localScale = Vector3.one;

        if (instanceEmail.TryGetComponent(out EmailInstance instance))
            instance.UpdateInfos(sender: email.Sender, title: email.Title, contentSmall: email.Content, startOpen: email.StartOpen);

        if (!_emailsInstanciados.ContainsKey(instanceEmail))
            _emailsInstanciados.Add(instanceEmail, email);

        if (!_mainCanvas.activeSelf)
            EventManager.NotifyBrowser();
    }

    private void UpdateLoreMechanics()
    {
        if(_currentLoreSended == LORE_TO_OPEN_WRITE_EMAIL)
            _writeEmailState = WriteEmailState.CanWrite;
    }

    public void OpenEmail(GameObject email)
    {
        _homeEmailCanvas.gameObject.SetActive(false);
        _readingEmailCanvas.gameObject.SetActive(true);

        var scroll = _readingEmailCanvas.gameObject.GetComponentInChildren<ScrollRect>();
        scroll.verticalNormalizedPosition = 1f;

        if(_emailsInstanciados.TryGetValue(email, out Email instance))
        {
            _currentEmailOpen = instance;
            _emailTitle.text = instance.Title;
            _emailContent.text = instance.Content;
            _senderName.text = instance.Sender.Name;
            _senderEmail.text = instance.Sender.Email;
            _senderProfilePicture.sprite = instance.Sender.Profile;
        }

        CheckEmailEvents();
    }

    public void TryWriteEmail(Email email)
    {
        _homeEmailCanvas.gameObject.SetActive(false);
        _writingEmailCanvas.gameObject.SetActive(true);

        _currentEmailOpen = email;
        _emailWriteTitle.text = email.Title;
        _emailWriteContent.text = email.Content;
        _receiverEmail.text = email.Receiver.Email;
    }

    public void CloseEmail()
    {
        EventManager.CloseResponseScreen();
        _homeEmailCanvas.gameObject.SetActive(true);
        _readingEmailCanvas.gameObject.SetActive(false);
        _writingEmailCanvas.gameObject.SetActive(false);
        _currentEmailOpen = null;
    }

    private void EmailIsAnswered(string emailIndex)
    {
        _currentEmailOpen.AnswerEmail();
        CloseEmail();
    }

    private void ChangeContentEmail(string newEmailText)
    {
        _emailContent.text = newEmailText;
        _emailWriteContent.text = newEmailText;
    }

    private void ReturnEmailContent()
    {
        _emailContent.text = _currentEmailOpen.Content;
        _emailWriteContent.text = _currentEmailOpen.Content;
        _receiverEmail.text = _currentEmailOpen.Receiver.Email;
    }

    private void ChangeWritingReceiver(string newReceiver)
    {
        _receiverEmail.text = newReceiver;
    }

    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        CloseEmail();
        _mainCanvas.SetActive(false);
    }

    private void CheckToSpawn(int timerEventNumber)
    {
        switch(timerEventNumber)
        {
            case (int)TimerEventSpawner.WelcomeEmail:
                CreateEmail(EmailType.LORE);
                break;
        }
    }

    private void CheckEmailEvents()
    {
        if(_currentEmailOpen.HasResponse && !_currentEmailOpen.IsAnswered)
            EventManager.OpenEmailResponse(_currentEmailOpen, isResponse: true);

        if(_currentEmailOpen.DisptacherInfo.HasEmailEvent && !_currentEmailOpen.DisptacherInfo.EmailEventSended)
        {
            _currentEmailOpen.DispatchEmailEvent();
            EventManager.SpawnEmail(_currentEmailOpen.DisptacherInfo.EmailTypeToCreate);
        }

        if(_currentEmailOpen.DisptacherInfo.HasNormalEvent && !_currentEmailOpen.DisptacherInfo.NormalEventSended)
        {
            _currentEmailOpen.DispatchNormalEvent();
            EventManager.EventEmailIsOpen(_currentEmailOpen.Index);
        }
    }

    private void TryWriteEmail()
    {
        if(_writeEmailState == WriteEmailState.CanWrite)
            EventManager.TryWriteEmail();
        else
            EventManager.MakePlayerThink(ThoughtKey.WrongTimeToWriteEmail);
    }
}

public enum WriteEmailState
{
    CantWrite, CanWrite
}

public enum TimerEventSpawner
{
    WelcomeEmail
}
