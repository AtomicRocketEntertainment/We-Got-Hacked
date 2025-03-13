using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ChoicesManager : MonoBehaviour
{
    [BoxGroup("Screens")] [SerializeField] private GameObject _emailScreen;

    void OnEnable()
    {
        _emailScreen.SetActive(false);
        EventManager.OnEmailResponseNeeded += OpenEmailResponse;
    }

    void OnDisable()
    {
        EventManager.OnEmailResponseNeeded -= OpenEmailResponse;
    }

    private void OpenEmailResponse(Email email)
    {
        _emailScreen.SetActive(true);
        _emailScreen.TryGetComponent(out EmailChoices emailManager);
        emailManager.OpenResponse(email);
    }
}
