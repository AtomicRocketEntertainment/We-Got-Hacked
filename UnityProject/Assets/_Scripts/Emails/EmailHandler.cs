using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailHandler : MonoBehaviour
{
    [SerializeField] private List<SO_Email> _emailsToSend = new List<SO_Email>();
    [SerializeField] private GameObject _emailPrefab;
    [SerializeField] private Transform _homeEmailCanvas;
    [SerializeField] private Transform _emailCanvas;
    [SerializeField] private TextMeshProUGUI _emailTitle;
    [SerializeField] private TextMeshProUGUI _senderName;
    [SerializeField] private TextMeshProUGUI _senderEmail;
    [SerializeField] private TextMeshProUGUI _emailContent;
    [SerializeField] private Image _senderProfilePicture;
    private int _currentEmailSended = 0;
    private Dictionary<GameObject, Email> _emailsInstanciados = new Dictionary<GameObject, Email>();

    private void OnEnable() 
    {
        _currentEmailSended = 0;
        EventManager.OnOpenEmail += OpenEmail;
        EventManager.OnLinkIsClicked += OpenSite;

    }

    void OnDisable()
    {
        EventManager.OnOpenEmail -= OpenEmail;
        EventManager.OnLinkIsClicked -= OpenSite;

    }

    [ContextMenu("Spawn Email")]
    public void SpawnEmail()
    {
        Email email = new Email(_emailsToSend[_currentEmailSended]);

        GameObject instanceEmail = Instantiate(_emailPrefab, Vector3.zero, Quaternion.identity);
        instanceEmail.transform.SetParent(_homeEmailCanvas);
        instanceEmail.name = email.Title;
        instanceEmail.transform.localScale = new Vector3(1, 1, 1);
        instanceEmail.transform.localPosition = new Vector3(-730f, 350f + (_currentEmailSended * -50f), 0);

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
            _emailTitle.text = instance.Title;
            _emailContent.text = instance.Content;
            _senderName.text = instance.Sender.Name;
            _senderEmail.text = instance.Sender.Email;
            _senderProfilePicture.sprite = instance.Sender.Profile;
        }
    }

    public void CloseEmail()
    {
        _homeEmailCanvas.gameObject.SetActive(true);
        _emailCanvas.gameObject.SetActive(false);
    }

    private void OpenSite(string siteName)
    {
        print(siteName);
    }
}
