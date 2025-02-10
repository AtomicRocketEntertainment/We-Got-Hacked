using System.Collections.Generic;
using UnityEngine;

public class EmailHandler : MonoBehaviour
{
    [SerializeField] private List<SO_Email> _emailsToSend = new List<SO_Email>();
    [SerializeField] private GameObject _emailPrefab;
    [SerializeField] private Transform _emailParent;
    private List<Email> _currentEmails = new List<Email>();
    private int _currentEmailSended = 0;

    private void OnEnable() 
    {
        _currentEmailSended = 0;
    }

    [ContextMenu("Spawn Email")]
    public void SpawnEmail()
    {
        Email email = new Email(_emailsToSend[_currentEmailSended]);

        GameObject instanceEmail = Instantiate(_emailPrefab, Vector3.down * 50f, Quaternion.identity);
        instanceEmail.transform.SetParent(_emailParent);

        if(instanceEmail.TryGetComponent(out EmailInstance instance))
            instance.UpdateInfos(sender: email.Sender, title: email.Title, contentSmall: email.Content);
        
        _currentEmailSended++;
    }
}
