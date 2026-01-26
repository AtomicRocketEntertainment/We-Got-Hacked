using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    [Header("Consent UI")]
    [SerializeField] private Button consentButton;

    private bool _consentGranted = false;
    private bool _servicesInitialized = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void GrantConsent()
    {
        if (_consentGranted)
            return;

        if (!_servicesInitialized)
        {
            await UnityServices.InitializeAsync();
            _servicesInitialized = true;
        }

        AnalyticsService.Instance.StartDataCollection();
        _consentGranted = true;

        Debug.Log("Analytics: Consentimento concedido.");
    }

    public void SendEvent(Unity.Services.Analytics.Event myEvent)
    {
        if (!_consentGranted)
        {
            Debug.LogWarning($"Analytics: Evento '{myEvent}' ignorado (sem consentimento).");
            return;
        }

        if (!_servicesInitialized)
        {
            Debug.LogWarning("Analytics: Serviços não inicializados.");
            return;
        }

        AnalyticsService.Instance.RecordEvent(myEvent);
    }
}


public class PlayerChoiceEvent : Unity.Services.Analytics.Event
{
    public PlayerChoiceEvent(PlayerDataAnswer answer) : base("player_choice")
    {
        question = answer.Question;
        response = answer.Response;
        isCorrectAnswer = answer.IsCorrectAnswer;
    }

    public string question;
    public string response;
    public bool isCorrectAnswer;
}

