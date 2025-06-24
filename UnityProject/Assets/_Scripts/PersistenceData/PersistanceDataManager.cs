using System;
using System.Collections.Generic;
using UnityEngine;

public class PersistanceDataManager : MonoBehaviour
{
    [SerializeField] private List<SO_Stock> _companys;
    private List<PlayerDataAnswer> _playerDataAnswerList;

    private PlayerDatabaseData _currentPlayerData;
    private Dictionary<string, List<int>> _stockDataByCompany = new Dictionary<string, List<int>>();

    public List<PlayerDataAnswer> AnswersToSave => _playerDataAnswerList;
    public PlayerDatabaseData PlayerData => _currentPlayerData;
    public Dictionary<string, List<int>> StocksInfos => _stockDataByCompany;
    private Dictionary<string, SO_Stock> _stockMetaByCompany = new Dictionary<string, SO_Stock>();



    public static PersistanceDataManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        foreach (var company in _companys)
            _stockMetaByCompany[company.CompanyName] = company;

        _playerDataAnswerList = new List<PlayerDataAnswer>();
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable()
    {
        EventManager.OnAnswerToSaveIsMaded += AddOnList;
    }

    void OnDisable()
    {
        EventManager.OnAnswerToSaveIsMaded -= AddOnList;
    }

    private void AddOnList(PlayerDataAnswer answer)
    {
        _playerDataAnswerList.Add(answer);
    }

    public void CreatePlayerData(string playerName)
    {
        _currentPlayerData = new PlayerDatabaseData(playerName);

        foreach (var company in _companys)
            _stockDataByCompany[company.CompanyName] = new List<int>(company.Values);

        PrepareDataToSave();
    }

    public void PrepareDataToSave()
    {
        _currentPlayerData.PopulateAnswers(_playerDataAnswerList);
        _currentPlayerData.ClearCompaniesInData();

        foreach (var pair in _stockDataByCompany)
            _currentPlayerData.PopulateCompanyStatus(pair.Value, pair.Key);
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

        _stockDataByCompany.Clear();

        foreach (var company in _companys)
        {
            var savedCompanyData = _currentPlayerData.Companies.Find(c => c.CompanyName == company.CompanyName);

            if (savedCompanyData != null)
                _stockDataByCompany[company.CompanyName] = new List<int>(savedCompanyData.StockValues);
            else
                _stockDataByCompany[company.CompanyName] = new List<int>(company.Values);
        }
    }

    public void SaveGame()
    {
        PrepareDataToSave();
        FirebaseDatabaseHandler.Instance?.UpdatePlayer();
    }
    
    public bool TryGetCompanyMeta(string companyName, out SO_Stock companyMeta)
    {
        return _stockMetaByCompany.TryGetValue(companyName, out companyMeta);
    }
}

[Serializable]
public class PlayerDatabaseData
{
    public string Name;
    public List<PlayerDataAnswer> Answers = new();
    public List<CompanyStockData> Companies = new();


    public PlayerDatabaseData(string name)
    {
        Name = name;
        Answers = new List<PlayerDataAnswer>();
        Companies = new List<CompanyStockData>();
    }

    public void PopulateAnswers(List<PlayerDataAnswer> list)
    {
        Answers = list;
    }

    public void PopulateCompanyStatus(List<int> list, string companyName)
    {
        Companies.Add(new CompanyStockData { CompanyName = companyName, StockValues = list.ToArray() });
    }

    public void ClearCompaniesInData()
    {
        Companies.Clear();
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