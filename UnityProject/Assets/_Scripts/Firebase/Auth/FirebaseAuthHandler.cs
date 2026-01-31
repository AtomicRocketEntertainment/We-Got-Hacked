using UnityEngine;
using _Scripts.Firebase.Auth;

public class FirebaseAuthHandler : MonoBehaviour
{
    private FirebaseUser _currentUser;
    private string _userNameHandler;
    public string CurrentUserId => _currentUser.uid;
    public bool PlayerIsVerfied => _currentUser.emailVerified;

    public static FirebaseAuthHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _currentUser = null;
        _userNameHandler = "";
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            EventManager.AuthError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            return;
        }
    }

    public void CreateUserWithEmailAndPassword(string email, string password, string playerName)
    {
        _userNameHandler = playerName;
        FirebaseAuth.CreateUserWithEmailAndPassword(email, password, gameObject.name, nameof(CreateAuthPlayer), nameof(DisplayErrorObject));
    }

    public void SignInWithEmailAndPassword(string email, string password)
    {
        FirebaseAuth.SignInWithEmailAndPassword(email, password, gameObject.name, nameof(UpdateUserStatus), nameof(DisplayErrorObject));
    }

    public void SignOut()
    {
        _currentUser = null;
        FirebaseAuth.OnUserSignOut(gameObject.name, nameof(NotifyError));
    }

    public void LogUser(string user)
    {
        _currentUser = JsonUtility.FromJson<FirebaseUser>(user);

        if (!_currentUser.emailVerified)
        {
            EventManager.AuthError("<color=red>Você precisa verificar seu e-mail antes de jogar.</color>");
            return;
        }

        FirebaseDatabaseHandler.Instance.FetchPlayer(_currentUser.uid);
    }

    public void CreateAuthPlayer(string user)
    {
        _currentUser = JsonUtility.FromJson<FirebaseUser>(user);
        FirebaseDatabaseHandler.Instance.CreateDatabaseInfo(_currentUser.uid, _userNameHandler);
    }

    private void UpdateUserStatus(string user)
    {
        FirebaseAuth.OnAuthStateChanged(gameObject.name, nameof(LogUser), nameof(NotifyError));
    }

    public void NotifyError(string info)
    {
        EventManager.AuthError(info);
    }

    public void DisplayErrorObject(string error)
    {
        FirebaseError parsedError = JsonUtility.FromJson<FirebaseError>(error);

        if (parsedError != null)
        {
            EventManager.AuthError(parsedError.message);
        }

    }

    public void CheckEmailVerification()
    {
        FirebaseAuth.ReloadCurrentUser(gameObject.name, nameof(OnUserReloaded), nameof(NotifyError));
    }

    public void ResendEmailVerification()
    {
        FirebaseAuth.SendEmailVerification(gameObject.name, nameof(OnVerificationEmailSent), nameof(NotifyError));
    }

    private void OnVerificationEmailSent(string feedback)
    {
        EventManager.AuthError("E-mail reenviado, o processo pode levar cerca de 30 minutos.");
    }

    private void OnUserReloaded(string user)
    {
        _currentUser = JsonUtility.FromJson<FirebaseUser>(user);

        if (_currentUser.emailVerified)
        {
            FirebaseDatabaseHandler.Instance.FetchPlayer(_currentUser.uid);
        }
        else
        {
            EventManager.AuthError("Email ainda não foi verificado.");
        }
    }

    public void SendPasswordReset(string email)
    {
        FirebaseAuth.SendPasswordResetEmail(
            email,
            gameObject.name,
            nameof(OnPasswordResetSent),
            nameof(DisplayErrorObject)
        );
    }

    private void OnPasswordResetSent(string message)
    {
        EventManager.AuthError(message);
    }


}

[System.Serializable]
public class FirebaseUser
{
    public string email;
    public string uid;
    public bool emailVerified;
}

[System.Serializable]
public class FirebaseError
{
    public string message;
}


