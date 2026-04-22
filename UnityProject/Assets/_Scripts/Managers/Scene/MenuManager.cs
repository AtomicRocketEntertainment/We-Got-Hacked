using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _createParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _loginParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _loginBtnParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _playBtnParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _disclaimerParent;
    [BoxGroup("Element's Parent"), SerializeField] private CanvasGroup _allScreensParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _emailConfirmationParent;
    [BoxGroup("Element's Parent"), SerializeField] private GameObject _resetPasswordParent;

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
    [BoxGroup("Login related Buttons"), SerializeField] private Button _playerConfirmedVerifiedBtn;
    [BoxGroup("Login related Buttons"), SerializeField] private Button _reSendConfirmationEmailBtn;
    [BoxGroup("Login related Buttons"), SerializeField] private Button _forgotPasswordBtn;

    [BoxGroup("Reset password related"), SerializeField] private Button _sendForgotPassword;
    [BoxGroup("Reset password related"), SerializeField] private Button _backFromResetPassword;
    [BoxGroup("Reset password related"), SerializeField] private TMP_InputField _emailToSendForgotPassword;

    [BoxGroup("Disclaimer"), SerializeField] private Button _closeDisclaimer;

    [BoxGroup("Credits"), SerializeField] private Button _openCreditBtn;
    [BoxGroup("Credits"), SerializeField] private CreditManager _creditManager;

    [BoxGroup("Intro Scene"), SerializeField] private CanvasGroup _introPanel;
    [BoxGroup("Intro Scene"), SerializeField] private Button _introBtn;

    [BoxGroup("Gameplay Buttons"), SerializeField] private Button _startBtn;
    [BoxGroup("Gameplay Buttons"), SerializeField] private Button _newGameBtn;
    [BoxGroup("Gameplay Buttons"), SerializeField] private Button _continueBtn;

    private readonly int _firstSceneIndex = 1;

    void OnEnable()
    {
        _closeDisclaimer.onClick.AddListener(CloseDisclaimer);
        _startBtn.onClick.AddListener(StartGame);
        _newGameBtn.onClick.AddListener(StartNewGame);
        _continueBtn.onClick.AddListener(ContinueGame);
        _createBtn.onClick.AddListener(ShowCreateMenu);
        _loginBtn.onClick.AddListener(ShowLoginMenu);
        _confirmCreateBtn.onClick.AddListener(TryCreatePlayer);
        _confirmLoginBtn.onClick.AddListener(TryLoginPlayer);
        _openCreditBtn.onClick.AddListener(OpenCredit);
        _playerConfirmedVerifiedBtn.onClick.AddListener(PlayerConfirmedVerification);
        _reSendConfirmationEmailBtn.onClick.AddListener(ReSendConfirmationEmail);
        _forgotPasswordBtn.onClick.AddListener(OpenResetPassword);
        _sendForgotPassword.onClick.AddListener(ForgotPasswordClicked);
        _backFromResetPassword.onClick.AddListener(CloseForgotPassword);
        _introBtn.onClick.AddListener(ShowMenu);
        EventManager.OnPlayerCreated += FeedbackPlayerCreated;
        EventManager.OnPlayerLoggedIn += FeedbackPlayerLoggedIn;
        EventManager.OnAuthError += GenericFeedback;
        EventManager.OnDatabaseEror += GenericFeedback;
        EventManager.OnMusicBanksLoaded += MenuReady;

        _continueBtn.gameObject.SetActive(false); //only available if player login in.
        _newGameBtn.gameObject.SetActive(false); //only available if player login in.
    }

    void OnDisable()
    {
        _closeDisclaimer.onClick.RemoveListener(CloseDisclaimer);
        _startBtn.onClick.RemoveListener(StartGame);
        _newGameBtn.onClick.RemoveListener(StartNewGame);
        _continueBtn.onClick.RemoveListener(ContinueGame);
        _createBtn.onClick.RemoveListener(ShowCreateMenu);
        _loginBtn.onClick.RemoveListener(ShowLoginMenu);
        _confirmCreateBtn.onClick.RemoveListener(TryCreatePlayer);
        _confirmLoginBtn.onClick.RemoveListener(TryLoginPlayer);
        _openCreditBtn.onClick.RemoveListener(OpenCredit);
        _playerConfirmedVerifiedBtn.onClick.RemoveListener(PlayerConfirmedVerification);
        _reSendConfirmationEmailBtn.onClick.RemoveListener(ReSendConfirmationEmail);
        _forgotPasswordBtn.onClick.RemoveListener(OpenResetPassword);
        _sendForgotPassword.onClick.RemoveListener(ForgotPasswordClicked);
        _backFromResetPassword.onClick.RemoveListener(CloseForgotPassword);
        _introBtn.onClick.RemoveListener(ShowMenu);
        EventManager.OnPlayerCreated -= FeedbackPlayerCreated;
        EventManager.OnPlayerLoggedIn -= FeedbackPlayerLoggedIn;
        EventManager.OnAuthError -= GenericFeedback;
        EventManager.OnDatabaseEror -= GenericFeedback;
        EventManager.OnMusicBanksLoaded -= MenuReady;
    }

    private void ReSendConfirmationEmail()
    {
        FirebaseAuthHandler.Instance.ResendEmailVerification();
        _reSendConfirmationEmailBtn.interactable = false;
    }

    private void MenuReady()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_introPanel.DOFade(1, 0.5f));
        _introPanel.interactable = true;
        VolumeManager.instance.LoadVolume();
    }

    private void ShowMenu()
    {
        Sequence seq = DOTween.Sequence();
        _introPanel.gameObject.SetActive(false);
        seq.Join(_allScreensParent.DOFade(1, 0.5f));

        MusicManager.instance.StartTrack();
    }

    private void OpenCredit()
    {
        _creditManager.StartCredits();
    }

    private void CloseDisclaimer()
    {
        _disclaimerParent.SetActive(false);
        _allScreensParent.gameObject.SetActive(true);
    }

    private void TryLoginPlayer()
    {
        string email = _emailLoginInputField.text;
        string password = _passwordLoginInputField.text;

        bool informationsAreSet = !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(password);

        if (informationsAreSet)
        {
            FirebaseAuthHandler.Instance.SignInWithEmailAndPassword(email, password);
        }
        else
            GenericFeedback("Todos os campos devem ser preenchidos.");
    }

    private void TryCreatePlayer()
    {
        string email = _emailCreateInputField.text;
        string password = _passwordCreateInputField.text;
        string playerName = _nameCreateInputField.text;

        bool informationsAreSet = !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(password) || !string.IsNullOrEmpty(playerName);
        bool isEmail = IsValidEmail(email);

        if (informationsAreSet && isEmail)
            FirebaseAuthHandler.Instance.CreateUserWithEmailAndPassword(email, password, playerName);
        else if(!isEmail)
            GenericFeedback("Esse email não é válido.");
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
        _emailLoginInputField.SetTextWithoutNotify("");
        _passwordLoginInputField.SetTextWithoutNotify("");
        _loginBtnParent.SetActive(false);
        _loginParent.SetActive(true);
    }

    private void FeedbackPlayerCreated(string feedbackMessage)
    {
        _feedbackText.text = "";
        _createParent.SetActive(false);
        _emailConfirmationParent.SetActive(true);
    }

    private void FeedbackPlayerLoggedIn(string playerName)
    {
        if (FirebaseAuthHandler.Instance.PlayerIsVerfied)
        {
            _feedbackText.text = $"Seja bem vindo {playerName}";

            HandleButtonsToShow();
            _loginParent.SetActive(false);
            _emailConfirmationParent.SetActive(false);
            _playBtnParent.SetActive(true);
        }
        else
        {
            _feedbackText.text = $"Por favor, valide seu e-mail.";

            _loginParent.SetActive(false);
            _emailConfirmationParent.SetActive(true);
        }
    }

    private void GenericFeedback(string feedback)
    {
        switch (feedback)
        {
            case "Firebase: Error (auth/invalid-credential).": _feedbackText.text = $"<color=red>Email ou senha não existem.</color>"; break;
            case "Firebase: Error (auth/missing-password).": _feedbackText.text = $"<color=red>Senha incorreta.</color>"; break;
            case "Firebase: Error (auth/invalid-email).": _feedbackText.text = $"<color=red>Este email não é válido.</color>"; break;
            case "Firebase: Error (auth/email-already-in-use).": _feedbackText.text = $"<color=red>Este usuário já está cadastrado.</color>"; break;
            case "Firebase: Password should be at least 6 characters (auth/weak-password).": _feedbackText.text = $"<color=red>A senha precisa ter pelo menos 6 characteres.</color>"; break;
            default: _feedbackText.text = $"<color=red>{feedback}</color>"; break;
        }

    }
    
    private void ForgotPasswordClicked()
    {
        string email = _emailToSendForgotPassword.text;

        if (string.IsNullOrEmpty(email) && IsValidEmail(email))
        {
            EventManager.AuthError("Informe o e-mail.");
            return;
        }

        _sendForgotPassword.interactable = false;
        FirebaseAuthHandler.Instance.SendPasswordReset(email);
    }

    private void OpenResetPassword()
    {
        _emailToSendForgotPassword.SetTextWithoutNotify("");
        _loginParent.SetActive(false);
        _resetPasswordParent.SetActive(true);
    }

    private void CloseForgotPassword()
    {
        _resetPasswordParent.SetActive(false);
        _loginBtnParent.SetActive(true);
    }

    private void HandleButtonsToShow()
    {
        _newGameBtn.gameObject.SetActive(true);
        _startBtn.gameObject.SetActive(false);

        //botão de continue só faz sentido se o jogador estiver depois da primeira cena. 
        bool isPlayerInFirstScene = PersistanceDataManager.Instance.PlayerData.CurrentScene == _firstSceneIndex; 
        _continueBtn.gameObject.SetActive(!isPlayerInFirstScene);
    }

    private void StartNewGame()
    {
        PersistanceDataManager.Instance.PlayerWantsNewRun();
        StartGame();
    }
    private void PlayerConfirmedVerification() => FirebaseAuthHandler.Instance.CheckEmailVerification();
    private void StartGame() => SceneHandler.Instance.StartGame();
    private void ContinueGame() => SceneHandler.Instance.ContinueGame();
    
    bool IsValidEmail(string email)
    {
        try
        {
            MailAddress m = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
