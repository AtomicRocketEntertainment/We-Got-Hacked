using NaughtyAttributes;
using UnityEngine;

public class ChoicesManager : MonoBehaviour
{
    [BoxGroup("Screens")] [SerializeField] private GameObject _emailScreen;

    void OnEnable()
    {
        _emailScreen.SetActive(false);
        EventManager.OnEmailResponseNeeded += OpenEmailResponse;
        EventManager.OnCloseResponseScreen += CloseResponse;
    }

    void OnDisable()
    {
        EventManager.OnEmailResponseNeeded -= OpenEmailResponse;
        EventManager.OnCloseResponseScreen -= CloseResponse;
    }

    private void OpenEmailResponse(Email email)
    {
        _emailScreen.SetActive(true);
        _emailScreen.TryGetComponent(out EmailChoices emailManager);
        emailManager.OpenResponse(email);
    }

    private void CloseResponse()
    {
        _emailScreen.SetActive(false);
        //include others when we'll have
    }
}
