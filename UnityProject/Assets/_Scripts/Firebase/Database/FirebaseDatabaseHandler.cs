using _Scripts.Firebase.Database;
using TMPro;
using UnityEngine;

public class FirebaseDatabaseHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _callbackText;

    private readonly string _userPath = "users_dev/";
    private PlayerDatabaseData _currentPlayerData;

    public static FirebaseDatabaseHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _currentPlayerData = null;
        DontDestroyOnLoad(this.gameObject);
    }

    public void CallJSON()
    {
        //FirebaseDatabase.GetJSON(_userPath, gameObject.name, nameof(OnRequestSuccess), nameof(OnRequestFailed));
    }

    public void FetchPlayer(string uid)
    {
        string pathWithId = _userPath + uid;
        FirebaseDatabase.FetchPlayerData(pathWithId, gameObject.name, nameof(PlayerFethed), nameof(OnError));
    }


    public void CreateDatabaseInfo(string uid, string playerName)
    {
        _currentPlayerData = new PlayerDatabaseData(playerName);
        _currentPlayerData.Name = playerName;
        string pathWithId = _userPath + uid;

        string json = JsonUtility.ToJson(_currentPlayerData);
        FirebaseDatabase.CreatePlayerDataIfNotExists(pathWithId, json, gameObject.name, nameof(OnPlayerCreated), nameof(OnError));
    }

    public void UpdatePlayer()
    {
        string pathWithId = _userPath + FirebaseAuthHandler.Instance.CurrentUserId;
        string json = JsonUtility.ToJson(_currentPlayerData);
        FirebaseDatabase.UpdatePlayerData(pathWithId, json, gameObject.name, nameof(PlayerUpdated), nameof(OnError));
    }

    private void PlayerUpdated(string data)
    {
        Debug.Log("Dados do jogador foram atualizados no banco de dados." + _currentPlayerData.Name);
    }


    private void PlayerFethed(string data)
    {
        _currentPlayerData = JsonUtility.FromJson<PlayerDatabaseData>(data);
        Debug.Log("Dados do jogador estão no bd e foram armazenados localmente." + _currentPlayerData.Name);
    }

    private void OnPlayerCreated(string message)
    {
        _callbackText.color = Color.green;
        _callbackText.text = "Create callback: " + message;
    }

    private void OnError(string error)
    {
        _callbackText.color = Color.red;
        _callbackText.text = "Firebase error: " + error;
    }

    public void GenerateRandomInfo()
    {
        _currentPlayerData.Randomize();
    }
}

[System.Serializable]
public class PlayerDatabaseData
{
    public string Name;
    public int int1;
    public int int2;
    public int int3;
    public int int4;

    public PlayerDatabaseData(string name)
    {
        Name = name;
        int1 = Random.Range(0, 10);
        int2 = Random.Range(0, 10);
        int3 = Random.Range(0, 10);
        int4 = Random.Range(0, 10);
    }

    public void Randomize()
    {
        int1 = Random.Range(0, 10);
        int2 = Random.Range(0, 10);
        int3 = Random.Range(0, 10);
        int4 = Random.Range(0, 10);
    }
}
