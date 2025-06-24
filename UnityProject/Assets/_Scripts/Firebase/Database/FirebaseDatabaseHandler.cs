using _Scripts.Firebase.Database;
using UnityEngine;

public class FirebaseDatabaseHandler : MonoBehaviour
{
    private readonly string _userPath = "users_dev/";
    
    public static FirebaseDatabaseHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

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
        PersistanceDataManager.Instance?.CreatePlayerData(playerName);
        string pathWithId = _userPath + uid;

        string json = JsonUtility.ToJson(PersistanceDataManager.Instance?.PlayerData);
        FirebaseDatabase.CreatePlayerDataIfNotExists(pathWithId, json, gameObject.name, nameof(OnPlayerCreated), nameof(OnError));
    }

    public void UpdatePlayer()
    {
        string pathWithId = _userPath + FirebaseAuthHandler.Instance.CurrentUserId;
        string json = JsonUtility.ToJson(PersistanceDataManager.Instance?.PlayerData);
        FirebaseDatabase.UpdatePlayerData(pathWithId, json, gameObject.name, nameof(PlayerUpdated), nameof(OnError));
    }

    private void PlayerUpdated(string data)
    {
        Debug.Log("Dados do jogador foram atualizados no banco de dados." + PersistanceDataManager.Instance?.PlayerData.Name);
    }


    private void PlayerFethed(string data)
    {
        PersistanceDataManager.Instance?.FetchPlayerData(JsonUtility.FromJson<PlayerDatabaseData>(data));
        string playerName = PersistanceDataManager.Instance?.PlayerData.Name;
        EventManager.PlayerLoggedIn(playerName);
        Debug.Log("Dados do jogador estão no bd e foram armazenados localmente." + PersistanceDataManager.Instance?.PlayerData.Name);
    }

    private void OnPlayerCreated(string message)
    {
        EventManager.PlayerAreCreated(message);
    }

    private void OnError(string error)
    {
        EventManager.DatabaseError(error);
    }
}
