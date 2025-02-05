using _Scripts.Firebase.Database;
using TMPro;
using UnityEngine;

public class FirebaseDatabaseHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private readonly string _userPath = "users_dev";

    public void CallJSON() => FirebaseDatabase.GetJSON(_userPath, gameObject.name, "OnRequestSuccess", "OnRequestFailed");

    private void OnRequestSuccess(string data)
    {
        _text.color = Color.green;
        _text.text = data; 
    }

    private void OnRequestFailed(string error)
    {
        _text.color = Color.red;
        _text.text = error; 
    }
}
