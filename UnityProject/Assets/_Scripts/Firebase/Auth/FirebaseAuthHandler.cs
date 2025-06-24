using UnityEngine;
using _Scripts.Firebase.Auth;

public class FirebaseAuthHandler : MonoBehaviour
{
    private FirebaseUser _currentUser;
    private string _userNameHandler;
    public string CurrentUserId => _currentUser.uid;

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
}

[System.Serializable]
public class FirebaseUser
{
    public string email;
    public string uid;
}

[System.Serializable]
public class FirebaseError
{
    public string message;
}


