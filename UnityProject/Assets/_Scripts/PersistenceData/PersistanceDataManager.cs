using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistanceDataManager : MonoBehaviour
{
    [SerializeField] private List<SO_Stock> _companys;
    public static PersistanceDataManager Instance { get; private set; }

    private List<PlayerDataAnswer> _playerDataAnswerList;
    private PlayerDatabaseData _currentPlayerData;
    private Dictionary<string, List<int>> _stockDataByCompany = new Dictionary<string, List<int>>();
    public PlayerDatabaseData PlayerData => _currentPlayerData;
    public Dictionary<string, List<int>> StocksInfos => _stockDataByCompany;
    private Dictionary<string, SO_Stock> _stockMetaByCompany = new Dictionary<string, SO_Stock>();
    private readonly int _firstSceneIndex = 1;



    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        foreach (SO_Stock company in _companys)
            _stockMetaByCompany[company.CompanyName] = company;

        _playerDataAnswerList = new List<PlayerDataAnswer>();
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable()
    {
        EventManager.OnAnswerToSaveIsMaded += AddOnList;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneHandler.Instance.IsGameplayScene()) //Player save the game at loading gameplay scenes.
        {
            PrepareDataToSave(playerNeedRestart: false);
            SaveGame();
        }
    }

    void OnDisable()
    {
        EventManager.OnAnswerToSaveIsMaded -= AddOnList;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void AddOnList(PlayerDataAnswer answer)
    {
        _playerDataAnswerList.Add(answer);
    }

    public void CreatePlayerData(string playerName)
    {
        _currentPlayerData = new PlayerDatabaseData(playerName);
        StartNewRun();
    }

    public void StartNewRun()
    {
        var newRun = new RunData();
        _stockDataByCompany.Clear();

        foreach (SO_Stock company in _companys)
        {
            _stockDataByCompany[company.CompanyName] = new List<int>(company.Values);
            newRun.Companies.Add(new CompanyStockData
            {
                CompanyName = company.CompanyName,
                StockValues = company.Values.ToArray()
            });
        }

        _currentPlayerData.Runs.Add(newRun);
        _playerDataAnswerList = new List<PlayerDataAnswer>();

        SaveGame();
    }

    public void PlayerWantsNewRun()
    {
        _currentPlayerData.CreateNewRun();
        StartNewRun();
    }

    public void PrepareDataToSave(bool playerNeedRestart)
    {
        //player should be on the gameplay scene 1 if have lost the game, or wants to restart game at main menu.
        bool isPlayerInMenu = SceneHandler.Instance.CurrentSceneIndex == 0;
        bool playerShouldGoToFirstScene = isPlayerInMenu || playerNeedRestart;

        _currentPlayerData.CurrentScene = playerShouldGoToFirstScene ? _firstSceneIndex : SceneHandler.Instance.CurrentSceneIndex; 
        RunData run = _currentPlayerData.Runs[_currentPlayerData.CurrentRun];

        foreach (PlayerDataAnswer answer in _playerDataAnswerList)
            run.Answers.Add(answer);

        run.Companies.Clear();
        foreach (var pair in _stockDataByCompany)
        {
            run.Companies.Add(new CompanyStockData
            {
                CompanyName = pair.Key,
                StockValues = pair.Value.ToArray()
            });
        }

        _playerDataAnswerList.Clear();
    }

    public void AddStockValue(string companyName, int valueToAdd)
    {
        if (_stockDataByCompany.TryGetValue(companyName, out var list))
        {
            list.Add(valueToAdd);
        }
        else
            Debug.LogWarning($"Empresa '{companyName}' não encontrada.");
    }

    public void FetchPlayerData(PlayerDatabaseData data)
    {
        _currentPlayerData = data;

        RunData run = _currentPlayerData.Runs[_currentPlayerData.CurrentRun];
        SceneHandler.Instance.SetSceneByIndex(_currentPlayerData.CurrentScene);

        _stockDataByCompany.Clear();
        foreach (var company in _companys)
        {
            var saved = run.Companies.Find(c => c.CompanyName == company.CompanyName);
            if (saved != null)
                _stockDataByCompany[company.CompanyName] = new List<int>(saved.StockValues);
            else
                _stockDataByCompany[company.CompanyName] = new List<int>(company.Values);
        }

    }

    public void SaveGame() => FirebaseDatabaseHandler.Instance?.UpdatePlayer();
    
    public bool TryGetCompanyMeta(string companyName, out SO_Stock companyMeta)
    {
        return _stockMetaByCompany.TryGetValue(companyName, out companyMeta);
    }

    public void HandleEndGame()
    {
        PrepareDataToSave(playerNeedRestart: true);
        _currentPlayerData.CurrentRun++;
        SaveGame();
    }
}

[Serializable]
public class PlayerDatabaseData
{
    public string Name;
    public int CurrentRun;
    public int CurrentScene;
    public List<RunData> Runs = new();

    public PlayerDatabaseData(string name)
    {
        Name = name;
        Runs = new List<RunData>();
        CurrentRun = 0;
        CurrentScene = 1;
    }

    public void CreateNewRun()
    {
        CurrentScene = 1;
        CurrentRun++;
    }
}

[Serializable]
public class PlayerDataAnswer
{
    public PlayerDataAnswer(string question, string response, bool correctStatus)
    {
        Question = question;
        Response = response;
        IsCorrectAnswer = correctStatus;
    }

    public string Question;
    public string Response;
    public bool IsCorrectAnswer;
}

[Serializable]
public class CompanyStockData
{
    public string CompanyName;
    public int[] StockValues;
}

[Serializable]
public class RunData
{
    public List<PlayerDataAnswer> Answers = new();
    public List<CompanyStockData> Companies = new();

    public RunData()
    {
        Answers = new List<PlayerDataAnswer>();
        Companies = new List<CompanyStockData>();
    }
}
