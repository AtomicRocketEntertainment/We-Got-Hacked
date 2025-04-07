using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ChoicesManager : MonoBehaviour
{
    [BoxGroup("Screens")] [SerializeField] private GameObject _emailScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _thinkScreen;
    [BoxGroup("Email")] [SerializeField] private List<SO_Email> _emailsToWrite;

    private int _currentEmailToWrite;
    private Coroutine _closeCrt;

    void OnEnable()
    {
        _currentEmailToWrite = 0;
        _emailScreen.SetActive(false);
        EventManager.OnEmailResponseNeeded += OpenEmailResponse;
        EventManager.OnPlayerNeedToThink += ShowThink;
        EventManager.OnCloseResponseScreen += CloseResponse;
    }

    void OnDisable()
    {
        EventManager.OnEmailResponseNeeded -= OpenEmailResponse;
        EventManager.OnPlayerNeedToThink -= ShowThink;
        EventManager.OnCloseResponseScreen -= CloseResponse;
    }

    private void OpenEmailResponse(Email email)
    {
        _emailScreen.SetActive(true);
        _emailScreen.TryGetComponent(out EmailChoices emailManager);
        emailManager.OpenResponse(email);
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
        _emailScreen.SetActive(false);
        _thinkScreen.SetActive(false);
        //include others when we'll have
    }
}
