using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _createParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _loginParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _loginBtnParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _playBtnParent;

    [BoxGroup("General Feedback"), SerializeField] private TextMeshProUGUI _feedbackText;

    [BoxGroup("Create Inputs"), SerializeField] private TMP_InputField _nameCreateInputField;
    [BoxGroup("Create Inputs"), SerializeField] private TMP_InputField _emailCreateInputField;
    [BoxGroup("Create Inputs"), SerializeField] private TMP_InputField _passwordCreateInputField;

    [BoxGroup("Login Inputs"), SerializeField] private TMP_InputField _emailLoginInputField;
    [BoxGroup("Login Inputs"), SerializeField] private TMP_InputField _passwordLoginInputField;

    [BoxGroup("Login related Buttons"), SerializeField] private Button _createBtn;
    [BoxGroup("Login related Buttons"), SerializeField] private Button _confirmCreateBtn;
    [BoxGroup("Login related Buttons"), SerializeField] private Button _confirmLoginBtn;
    [BoxGroup("Login related Buttons"), SerializeField] private Button _loginBtn;

    [BoxGroup("Gameplay Buttons"), SerializeField] private Button _startBtn;

    void OnEnable()
    {
        _startBtn.onClick.AddListener(StartGame);
        _createBtn.onClick.AddListener(ShowCreateMenu);
        _loginBtn.onClick.AddListener(ShowLoginMenu);
        _confirmCreateBtn.onClick.AddListener(TryCreatePlayer);
        _confirmLoginBtn.onClick.AddListener(TryLoginPlayer);
        EventManager.OnPlayerCreated += FeedbackPlayerCreated;
        EventManager.OnPlayerLoggedIn += FeedbackPlayerLoggedIn;
        EventManager.OnAuthError += GenericFeedback;
        EventManager.OnDatabaseEror += GenericFeedback;

    }

    void OnDisable()
    {
        _startBtn.onClick.RemoveListener(StartGame);
        _createBtn.onClick.RemoveListener(ShowCreateMenu);
        _loginBtn.onClick.RemoveListener(ShowLoginMenu);
        _confirmCreateBtn.onClick.RemoveListener(TryCreatePlayer);
        _confirmLoginBtn.onClick.RemoveListener(TryLoginPlayer);
        EventManager.OnPlayerCreated -= FeedbackPlayerCreated;
        EventManager.OnPlayerLoggedIn -= FeedbackPlayerLoggedIn;
        EventManager.OnAuthError -= GenericFeedback;
        EventManager.OnDatabaseEror -= GenericFeedback;
    }

    private void TryLoginPlayer()
    {
        string email = _emailLoginInputField.text;
        string password = _passwordLoginInputField.text;

        bool informationsAreSet = !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(password);

        if (informationsAreSet)
            FirebaseAuthHandler.Instance.SignInWithEmailAndPassword(email, password);
        else
            GenericFeedback("Todos os campos devem ser preenchidos.");
    }

    private void TryCreatePlayer()
    {
        string email = _emailCreateInputField.text;
        string password = _passwordCreateInputField.text;
        string playerName = _nameCreateInputField.text;

        bool informationsAreSet = !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(password) || !string.IsNullOrEmpty(playerName);

        if (informationsAreSet)
            FirebaseAuthHandler.Instance.CreateUserWithEmailAndPassword(email, password, playerName);
        else
            GenericFeedback("Todos os campos devem ser preenchidos.");
    }

    private void ShowCreateMenu()
    {
        _loginBtnParent.SetActive(false);
        _createParent.SetActive(true);
    }

    private void ShowLoginMenu()
    {
        _loginBtnParent.SetActive(false);
        _loginParent.SetActive(true);
    }

    private void FeedbackPlayerCreated(string feedbackMessage)
    {
        _feedbackText.text = feedbackMessage;
        _createParent.SetActive(false);
        _playBtnParent.SetActive(true);
    }

    private void FeedbackPlayerLoggedIn(string playerName)
    {
        _feedbackText.text = $"Seja bem vindo {playerName}";
        _loginParent.SetActive(false);
        _playBtnParent.SetActive(true);
    }

    private void GenericFeedback(string feedback)
    {
        _feedbackText.text = feedback;
    }

    public void ClearText()
    {
        _feedbackText.text = "";
    }

    private void StartGame()
    {
        SceneHandler.Instance.ChangeScene();
    }
}
