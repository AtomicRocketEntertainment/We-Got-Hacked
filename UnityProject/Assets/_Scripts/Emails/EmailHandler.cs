using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;


public class EmailHandler : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField] private List<SO_Email> _spamToSend = new List<SO_Email>();
    [SerializeField] private List<SO_Email> _loreToSend = new List<SO_Email>();
    [SerializeField] private List<SO_Email> _hackingToSend = new List<SO_Email>();
    [SerializeField] private List<SO_Email> _newsToSend = new List<SO_Email>();

    [SerializeField] private Button _writeEmailBtn;

    [HorizontalLine(color: EColor.Red)]
    [BoxGroup("Canvases")] [SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Canvases")] [SerializeField] private Transform _homeEmailCanvas;
    [BoxGroup("Canvases")] [SerializeField] private Transform _emailCanvas;
    [BoxGroup("Opened Content"), HorizontalLine(color: EColor.Green)] [SerializeField] private TextMeshProUGUI _emailTitle;
    [BoxGroup("Opened Content")] [SerializeField] private TextMeshProUGUI _senderName;
    [BoxGroup("Opened Content")] [SerializeField] private TextMeshProUGUI _senderEmail;
    [BoxGroup("Opened Content")] [SerializeField] private TextMeshProUGUI _emailContent;
    [BoxGroup("Opened Content")] [SerializeField] private Image _senderProfilePicture;

    [BoxGroup("Prefabs"), HorizontalLine(color: EColor.Yellow)] [SerializeField] private GameObject _emailPrefab;

    private Dictionary<GameObject, Email> _emailsInstanciados = new Dictionary<GameObject, Email>();
    private Email _currentEmailOpen;
    private int _currentSpamSended;
    private int _currentNewsSended;
    private int _currentLoreSended;
    private int _currentHackingSended;


    private void OnEnable() 
    {
        _currentSpamSended = _currentNewsSended = _currentLoreSended = _currentHackingSended = 0;
        EventManager.OnSpawnEmail += CreateEmail;
        EventManager.OnOpenEmail += OpenEmail;
        EventManager.OnChangeEmailContentText += ChangeContentEmail;
        EventManager.OnEmailIsAnswered += EmailIsAnswered;
        EventManager.OnReturnEmailContent += ReturnEmailContent;
        EventManager.OnTimerIsComplete += CheckToSpawn;

        CreateEmail(EmailType.SPAM);
        CreateEmail(EmailType.SPAM);
        CreateEmail(EmailType.NEWS);
    }
    void OnDisable()
    {
        EventManager.OnSpawnEmail -= CreateEmail;
        EventManager.OnOpenEmail -= OpenEmail;
        EventManager.OnChangeEmailContentText -= ChangeContentEmail;
        EventManager.OnEmailIsAnswered -= EmailIsAnswered;
        EventManager.OnReturnEmailContent -= ReturnEmailContent;
        EventManager.OnTimerIsComplete -= CheckToSpawn;
    }

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

        GameObject instanceEmail = Instantiate(_emailPrefab, Vector3.zero, Quaternion.identity);
        instanceEmail.transform.SetParent(_homeEmailCanvas);
        instanceEmail.name = email.Title;
        instanceEmail.transform.localScale = Vector3.one;

        if (instanceEmail.TryGetComponent(out EmailInstance instance))
            instance.UpdateInfos(sender: email.Sender, title: email.Title, contentSmall: email.Content);

        if (!_emailsInstanciados.ContainsKey(instanceEmail))
            _emailsInstanciados.Add(instanceEmail, email);
    }

    public void OpenEmail(GameObject email)
    {
        _homeEmailCanvas.gameObject.SetActive(false);
        _emailCanvas.gameObject.SetActive(true);

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

    public void CloseEmail()
    {
        EventManager.CloseResponseScreen();
        _homeEmailCanvas.gameObject.SetActive(true);
        _emailCanvas.gameObject.SetActive(false);
        _currentEmailOpen = null;
    }

    private void EmailIsAnswered()
    {
        _currentEmailOpen.AnswerEmail();
        CloseEmail();
    }

    private void ChangeContentEmail(string newEmailText)
    {
        _emailContent.text = newEmailText;
    }

    private void ReturnEmailContent()
    {
        _emailContent.text = _currentEmailOpen.Content;
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
            EventManager.OpenEmailResponse(_currentEmailOpen);

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
}

public enum TimerEventSpawner
{
    WelcomeEmail
}
