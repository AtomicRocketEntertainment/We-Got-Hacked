using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockManager : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField] private GameObject _mainCanvas;

    [Header("Point Infos")]
    [SerializeField] private Sprite _stockPoint;
    [SerializeField] private float _pointSize = 15f;

    [Header("Company Infos")]
    [SerializeField] private Image[] _companyImages;
    [SerializeField] private Image[] _companyColors;
    [SerializeField] private TextMeshProUGUI[] _companyNames;


    [Header("Graph Infos")]
    [SerializeField] private StockCalculation _stockCalculation;
    [SerializeField] private RectTransform _graphContainer;
    [SerializeField] private RectTransform _yLabelTemplate;
    [SerializeField] private RectTransform _yDashTemplate;
    [SerializeField] private float _xSpaceBetweenDots = 50f;
    [Tooltip("O valor máximo que vai aparecer na altura do gráfico"), SerializeField] private int _yMaximumValue = 100;
    [Tooltip("O valor minimo que vai aparecer na altura do gráfico"), SerializeField] private int _yMininumValue = 1;

    [Header("Companys to Show")]
    [SerializeField] private List<SO_Stock> _companys;

    [Header("New's info")]
    [SerializeField] private StockInformationHolder[] _news;
    [SerializeField] private SO_StockNew[] _newsSos;

    private readonly string _playerCompany = "Petro Cais"; 
    private List<GameObject> _dotsLists;

    private void Awake()
    {
        _dotsLists = new List<GameObject>();
        CalculateGraphBounds(out _yMininumValue, out _yMaximumValue);

        CreateSeparators();

        int companyIndex = 0; 
        foreach (var pair in PersistanceDataManager.Instance.StocksInfos)
        {
            string companyName = pair.Key;
            List<int> stockValues = pair.Value;

            if (PersistanceDataManager.Instance.TryGetCompanyMeta(companyName, out var companyMeta))
            {
                ShowGraph(stockValues, companyMeta.Color);
                _companyImages[companyIndex].sprite = companyMeta.Icon;
                _companyColors[companyIndex].color = companyMeta.Color;
                _companyNames[companyIndex].text = companyName;
                companyIndex++;
            }
            else
            {
                Debug.LogWarning($"SO_Stock não encontrado para '{companyName}'");
            }
        }
    }


    private void OnEnable()
    {
        EventManager.OnCorrectChoice += CorrectChoice;
        EventManager.OnWrongChoice += WrongChoice;

        for(int i = 0; i < _news.Length; i++)
            _news[i].UpdateNewsInfo(_newsSos[i].Header, _newsSos[i].Content);
    }

    private void OnDisable()
    {
        EventManager.OnCorrectChoice -= CorrectChoice;
        EventManager.OnWrongChoice -= WrongChoice;
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, Color companyColor)
    {
        GameObject newPoint = new GameObject("point", typeof(Image));
        newPoint.transform.SetParent(_graphContainer, false);
        
        newPoint.TryGetComponent(out Image image);
        newPoint.TryGetComponent(out RectTransform rect);
        
        image.maskable = false;
        image.sprite = _stockPoint;
        image.color = companyColor;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(_pointSize, _pointSize);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);

        return newPoint;
    }

    private void ShowGraph(List<int> valuesList, Color companyColor)
    {
        float graphHeight = _graphContainer.sizeDelta.y;
        GameObject lastCircleObj = null;

        for(int i = 0; i < valuesList.Count; i++)
        {
            float xPosition = _xSpaceBetweenDots + i * _xSpaceBetweenDots;
            float yNormalized = (float)(valuesList[i] - _yMininumValue) / (_yMaximumValue - _yMininumValue);
            float yPosition = yNormalized * graphHeight;

            GameObject circleObj = CreateCircle(new Vector2(xPosition, yPosition), companyColor);
            _dotsLists.Add(circleObj);

            if (lastCircleObj != null)
            {
                lastCircleObj.TryGetComponent(out RectTransform rectA);
                circleObj.TryGetComponent(out RectTransform rectB);
                GameObject dotConnectionGo = CreateDotConnection(rectA.anchoredPosition, rectB.anchoredPosition, companyColor);
                _dotsLists.Add(dotConnectionGo);
            }
            
            lastCircleObj = circleObj;
        }
    }

    private void CreateSeparators()
    {
        float graphHeight = _graphContainer.sizeDelta.y;
        
        foreach (GameObject go in _dotsLists)
            Destroy(go);

        _dotsLists.Clear();

        int separatorYCount = 10;

        for (int i = 1; i <= separatorYCount; i++)
        {
            RectTransform labelY = Instantiate(_yLabelTemplate);
            labelY.TryGetComponent(out TextMeshProUGUI tmpro);
            float normalizedValue = i * 1.0f / separatorYCount;

            labelY.SetParent(_graphContainer, false);
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(labelY.anchoredPosition.x, normalizedValue * graphHeight);
            tmpro.text = "R$ " + Mathf.RoundToInt(_yMininumValue + (normalizedValue * (_yMaximumValue - _yMininumValue))).ToString();
            _dotsLists.Add(labelY.gameObject);

            RectTransform dashY = Instantiate(_yDashTemplate);

            dashY.SetParent(_graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(dashY.anchoredPosition.x, 2.0f + (normalizedValue * graphHeight - 5f));
            _dotsLists.Add(dashY.gameObject);
        }
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, Color companyColor)
    {
        GameObject dotLine = new GameObject("dotConnection", typeof(Image));
        dotLine.transform.SetParent(_graphContainer, false);

        dotLine.TryGetComponent(out RectTransform rect);
        dotLine.TryGetComponent(out Image image);

        Vector2 direction = (dotPositionA - dotPositionB).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        image.maskable = false;
        image.color = companyColor;
        rect.sizeDelta = new Vector2(distance, 2.5f);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.anchoredPosition = (dotPositionA + dotPositionB) * 0.5f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.localEulerAngles = new Vector3(0, 0, angle);

        return dotLine;
    }

    private void CorrectChoice()
    {
        _stockCalculation.Calculate(_playerCompany, isCorrectChoice: true);
        CalculateGraphBounds(out _yMininumValue, out _yMaximumValue);
        CreateSeparators();
        UpdateStockGraph();
        EventManager.NotifyBrowser();
        PersistanceDataManager.Instance?.SaveGame();
    }

    private void WrongChoice()
    {
        _stockCalculation.Calculate(_playerCompany, isCorrectChoice: false);
        CalculateGraphBounds(out _yMininumValue, out _yMaximumValue);
        CreateSeparators();
        UpdateStockGraph();
        EventManager.NotifyBrowser();
        PersistanceDataManager.Instance?.SaveGame();
    }

    private void UpdateStockGraph()
    {
        int companyIndex = 0;

        foreach (var pair in PersistanceDataManager.Instance.StocksInfos)
        {
            if (PersistanceDataManager.Instance.TryGetCompanyMeta(pair.Key, out var companyMeta))
            {
                ShowGraph(pair.Value, companyMeta.Color);
                _companyImages[companyIndex].sprite = companyMeta.Icon;
                _companyNames[companyIndex].text = companyMeta.CompanyName;
                companyIndex++;
            }
            else
            {
                Debug.LogWarning($"SO_Stock não encontrado para '{_playerCompany}'");
            }
        }
    }

    private void CalculateGraphBounds(out int minValue, out int maxValue)
    {
        minValue = int.MaxValue;
        maxValue = int.MinValue;

        foreach (var pair in PersistanceDataManager.Instance?.StocksInfos)
        {
            foreach (int value in pair.Value)
            {
                if (value < minValue) minValue = value;
                if (value > maxValue) maxValue = value;
            }
        }

        int margin = 2;
        minValue -= margin;
        maxValue += margin;
    }


    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }
}
