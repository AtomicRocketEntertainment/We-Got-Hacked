using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ChoicesManager : MonoBehaviour
{
    [BoxGroup("Screens")] [SerializeField] private GameObject _emailScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _thinkScreen;
    [BoxGroup("Email")] [SerializeField] private List<SO_Email> _emailsToWrite;

    private const string PLAYER_DONT_HAVE_EMAIL_TO_WRITE = "Não tenho nada para escrever";
    private int _currentEmailToWrite;
    private Coroutine _closeCrt;

    void OnEnable()
    {
        _currentEmailToWrite = 0;
        _emailScreen.SetActive(false);
        EventManager.OnEmailIsWriten += UpdateEmailToWrite;
        EventManager.OnEmailResponseNeeded += OpenEmailResponse;
        EventManager.OnTryWriteEmail += OpenWriteEmail;
        EventManager.OnPlayerNeedToThink += ShowThink;
        EventManager.OnCloseResponseScreen += CloseResponse;
    }

    void OnDisable()
    {
        EventManager.OnEmailIsWriten -= UpdateEmailToWrite;
        EventManager.OnEmailResponseNeeded -= OpenEmailResponse;
        EventManager.OnTryWriteEmail -= OpenWriteEmail;
        EventManager.OnPlayerNeedToThink -= ShowThink;
        EventManager.OnCloseResponseScreen -= CloseResponse;
    }

    private void UpdateEmailToWrite()
    {
        _currentEmailToWrite++;
    }

    private void OpenWriteEmail()
    {
        if(_currentEmailToWrite == _emailsToWrite.Count)
        {
            ShowThink(PLAYER_DONT_HAVE_EMAIL_TO_WRITE);
            return;
        }

        Email email = new Email(_emailsToWrite[_currentEmailToWrite]);
        EventManager.WriteEmail(email);
        OpenEmailResponse(email, false);//Fake response, do the same thing.
    }

    private void OpenEmailResponse(Email email, bool isResponse)
    {
        _emailScreen.SetActive(true);
        _emailScreen.TryGetComponent(out EmailChoices emailManager);
        emailManager.OpenResponse(email, isResponse);
    }

    private void ShowThink(string obj)
    {
        _thinkScreen.SetActive(true);
        _thinkScreen.TryGetComponent(out Thinking thinking);
        thinking.UpdateThinking(obj);

        if(_closeCrt != null)
            StopCoroutine(_closeCrt);
            
        _closeCrt = StartCoroutine(CloseScreen(_thinkScreen));
    }

    private IEnumerator CloseScreen(GameObject screen)
    {
        yield return new WaitForSeconds(1.5f);
        screen.SetActive(false);
    }

    private void CloseResponse()
    {
        _emailScreen.TryGetComponent(out EmailChoices emailManager);
        emailManager.CloseResponse();
        _emailScreen.SetActive(false);
        _thinkScreen.SetActive(false);
        //include others when we'll have
    }
}
