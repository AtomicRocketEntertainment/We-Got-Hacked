using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;


public class EmailHandler : MonoBehaviour
{
    [SerializeField] private List<SO_Email> _emailsToSend = new List<SO_Email>();
    [SerializeField] private Button _writeEmailBtn;

    [HorizontalLine(color: EColor.Red)]
    [BoxGroup("Canvases")] [SerializeField] private Transform _homeEmailCanvas;
    [BoxGroup("Canvases")] [SerializeField] private Transform _emailCanvas;
    [BoxGroup("Opened Content"), HorizontalLine(color: EColor.Green)][SerializeField] private TextMeshProUGUI _emailTitle;
    [BoxGroup("Opened Content")] [SerializeField] private TextMeshProUGUI _senderName;
    [BoxGroup("Opened Content")] [SerializeField] private TextMeshProUGUI _senderEmail;
    [BoxGroup("Opened Content")] [SerializeField] private TextMeshProUGUI _emailContent;
    [BoxGroup("Opened Content")] [SerializeField] private Image _senderProfilePicture;

    [BoxGroup("Prefabs"), HorizontalLine(color: EColor.Yellow)] [SerializeField] private GameObject _emailPrefab;

    private int _currentEmailSended = 0;
    private Dictionary<GameObject, Email> _emailsInstanciados = new Dictionary<GameObject, Email>();
    private Email _currentEmailOpen;

    private void OnEnable() 
    {
        _currentEmailSended = 0;
        EventManager.OnOpenEmail += OpenEmail;
        EventManager.OnLinkIsClicked += OpenSite;
        EventManager.OnChangeEmailContentText += ChangeContentEmail;
        EventManager.OnEmailIsAnswered += EmailIsAnswered;
        EventManager.OnReturnEmailContent += ReturnEmailContent;

    }

    void OnDisable()
    {
        EventManager.OnOpenEmail -= OpenEmail;
        EventManager.OnLinkIsClicked -= OpenSite;
        EventManager.OnChangeEmailContentText -= ChangeContentEmail;
        EventManager.OnEmailIsAnswered -= EmailIsAnswered;
        EventManager.OnReturnEmailContent -= ReturnEmailContent;
    }

    [ContextMenu("Spawn Email")]
    public void SpawnEmail()
    {
        Email email = new Email(_emailsToSend[_currentEmailSended]);

        GameObject instanceEmail = Instantiate(_emailPrefab, Vector3.zero, Quaternion.identity);
        instanceEmail.transform.SetParent(_homeEmailCanvas);
        instanceEmail.name = email.Title;
        instanceEmail.transform.localScale = new Vector3(1, 1, 1);

        if(instanceEmail.TryGetComponent(out EmailInstance instance))
        {
            instance.UpdateInfos(sender: email.Sender, title: email.Title, contentSmall: email.Content);
        }

        if(!_emailsInstanciados.ContainsKey(instanceEmail))
            _emailsInstanciados.Add(instanceEmail, email);
        
        _currentEmailSended++;
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

        if(_currentEmailOpen.HasResponse && !_currentEmailOpen.IsAnswered)
            EventManager.OpenEmailResponse(_currentEmailOpen);
    }

    public void CloseEmail()
    {
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

    private void OpenSite(string siteName)
    {
        print(siteName);
    }
}
