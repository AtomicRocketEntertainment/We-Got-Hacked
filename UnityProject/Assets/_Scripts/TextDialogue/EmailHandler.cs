using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmailHandler : MonoBehaviour
{
    [SerializeField] private List<SO_Email> _emailsToSend = new List<SO_Email>();
    [SerializeField] private GameObject _emailPrefab;
    [SerializeField] private Transform _homeEmailCanvas;
    [SerializeField] private Transform _emailCanvas;
    [SerializeField] private TextMeshProUGUI _emailTitle;
    [SerializeField] private TextMeshProUGUI _emailSender;
    [SerializeField] private TextMeshProUGUI _emailContent;
    private List<Email> _currentEmails = new List<Email>();
    private int _currentEmailSended = 0;
    private Dictionary<GameObject, Email> _emailsInstanciados = new Dictionary<GameObject, Email>();

    private void OnEnable() 
    {
        _currentEmailSended = 0;
    }

    [ContextMenu("Spawn Email")]
    public void SpawnEmail()
    {
        Email email = new Email(_emailsToSend[_currentEmailSended]);

        GameObject instanceEmail = Instantiate(_emailPrefab, Vector3.zero, Quaternion.identity);
        instanceEmail.transform.SetParent(_homeEmailCanvas);
        instanceEmail.name = email.Title;
        instanceEmail.transform.localScale = new Vector3(1, 1, 1);
        instanceEmail.transform.localPosition = new Vector3(-960f, 350f + (_currentEmailSended * -50f), 0);

        if(instanceEmail.TryGetComponent(out EmailInstance instance))
        {
            instance.UpdateInfos(sender: email.Sender, title: email.Title, contentSmall: email.Content);
            instance.OnClickEmail.AddListener(OpenEmail);
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
            _emailSender.text = instance.Sender;
        }
    }

    public void CloseEmail()
    {
        _homeEmailCanvas.gameObject.SetActive(true);
        _emailCanvas.gameObject.SetActive(false);
    }
}
