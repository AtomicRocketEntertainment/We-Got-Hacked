using TMPro;
using UnityEngine;
using _Scripts.Firebase.Auth;

public class FirebaseAuthHandler : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI outputText;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            return;
        }
        
        FirebaseAuth.OnAuthStateChanged(gameObject.name, "DisplayUserInfo", "DisplayInfo");
    }

    public void CreateUserWithEmailAndPassword() => 
        FirebaseAuth.CreateUserWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void SignInWithEmailAndPassword() => 
        FirebaseAuth.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void SignInWithGoogle() => 
        FirebaseAuth.SignInWithGoogle(gameObject.name, "DisplayInfo", "DisplayErrorObject");


    public void DisplayUserInfo(string user)
    {
        FirebaseUser parsedUser = JsonUtility.FromJson<FirebaseUser>(user);
        DisplayData($"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");
    }

    public void DisplayData(string data)
    {
        outputText.color = outputText.color == Color.green ? Color.blue : Color.green;
        outputText.text = data;
        Debug.Log(data);
    }

    public void DisplayInfo(string info)
    {
        outputText.color = Color.white;
        outputText.text = info;
        Debug.Log(info);
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
        outputText.color = Color.red;
        outputText.text = error;
        Debug.LogError(error);
    }
}

[System.Serializable]
public class FirebaseUser
{
    public string email;
    public string uid;
    public bool isEmailVerified;
}

[System.Serializable]
public class FirebaseError
{
    public string message;
}


