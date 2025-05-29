using TMPro;
using UnityEngine;
using _Scripts.Firebase.Auth;

public class FirebaseAuthHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI _callbackText;
    private FirebaseUser _currentUser;
    public string CurrentUserId => _currentUser.uid;

    public static FirebaseAuthHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _currentUser = null;
        DontDestroyOnLoad(this.gameObject);
    } 

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            return;
        }
    }

    public void CreateUserWithEmailAndPassword()
    {

        FirebaseAuth.CreateUserWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, nameof(CreateAuthPlayer), nameof(DisplayErrorObject));
    }

    public void SignInWithEmailAndPassword()
    {

        FirebaseAuth.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, nameof(UpdateUserStatus), nameof(DisplayErrorObject));
    }

    public void SignOut()
    {

        FirebaseAuth.OnUserSignOut(gameObject.name, nameof(DisplayInfo));
    }

    public void LogUser(string user)
    {
        _currentUser = JsonUtility.FromJson<FirebaseUser>(user);
        FirebaseDatabaseHandler.Instance.FetchPlayer(_currentUser.uid);
    }

    public void CreateAuthPlayer(string user)
    {
        _currentUser = JsonUtility.FromJson<FirebaseUser>(user);
        FirebaseDatabaseHandler.Instance.CreateDatabaseInfo(_currentUser.uid, nameInputField.text);
    }

    private void UpdateUserStatus(string user)
    {
        FirebaseAuth.OnAuthStateChanged(gameObject.name, nameof(LogUser), nameof(DisplayInfo));
    }

    public void DisplayInfo(string info)
    {
        _callbackText.color = Color.white;
        _callbackText.text = info;
    }

    public void DisplayErrorObject(string error)
    {
        FirebaseError parsedError = JsonUtility.FromJson<FirebaseError>(error);
        if (parsedError != null)
        {
            DisplayError(parsedError.message);
        }
        else
        {
            DisplayError("Failed to parse error data.");
        }
    }

    public void DisplayError(string error)
    {
        _callbackText.color = Color.red;
        _callbackText.text = error;
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


