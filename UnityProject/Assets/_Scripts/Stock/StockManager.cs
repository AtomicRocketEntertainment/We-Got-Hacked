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
    [SerializeField] private TextMeshProUGUI[] _companyNames;


    [Header("Graph Infos")]
    [SerializeField] private RectTransform _graphContainer;
    [SerializeField] private RectTransform _yLabelTemplate;
    [SerializeField] private RectTransform _yDashTemplate;
    [SerializeField] private float _xSpaceBetweenDots = 50f;
    [Tooltip("O valor máximo que vai aparecer na altura do gráfico"), SerializeField] private float _yMaximumValue = 150f;
    [Tooltip("O valor minimo que vai aparecer na altura do gráfico"), SerializeField] private float _yMininumValue = 40f;

    [Header("Companys to Show")]
    [SerializeField] private List<SO_Stock> _companys;

    [Header("New's info")]
    [SerializeField] private StockInformationHolder[] _news;
    [SerializeField] private SO_StockNew[] _newsSos;

    private void Awake()
    {
        for(int i = 0; i < _companys.Count; i++)
        {
            ShowGraph(_companys[i].Values, _companys[i].Color, i);
            _companyImages[i].color = _companys[i].Color;
            _companyNames[i].text = _companys[i].CompanyName;
        }
    }

    private void OnEnable()
    {
        EventManager.OnCorrectChoice += CorrectChoice;
        EventManager.OnWrongChoice += WrongChoice;

        for(int i = 0; i < _news.Length; i++)
            _news[i].UpdateNewsInfo(_newsSos[i].Image, _newsSos[i].Header, _newsSos[i].Content);
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
        
        image.sprite = _stockPoint;
        image.color = companyColor;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(_pointSize, _pointSize);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);

        return newPoint;
    }

    private void ShowGraph(List<float> valuesList, Color companyColor, int index)
    {
        float graphHeight = _graphContainer.sizeDelta.y;
        GameObject lastCircleObj = null;


        if(index == 0) //somente uma label e dash
        {
            foreach(float value in valuesList)
            {
                if(value > _yMaximumValue)
                {
                    _yMaximumValue = value;
                }

                if(value < _yMininumValue)
                {
                    _yMininumValue = value;
                }
            }

            _yMaximumValue = _yMaximumValue + ((_yMaximumValue - _yMininumValue) * 0.2f); //aumentar um pouco o valor maximo, caso o valor estoure com a lista.
            _yMininumValue = _yMininumValue - ((_yMaximumValue - _yMininumValue) * 0.2f); //aumentar um pouco o valor maximo, caso o valor estoure com a lista.

            int separatorYCount = 10;

            for(int i = 1; i <= separatorYCount; i++)
            {
                RectTransform labelY = Instantiate(_yLabelTemplate);
                labelY.TryGetComponent(out TextMeshProUGUI tmpro);
                float normalizedValue = i * 1.0f / separatorYCount;

                labelY.SetParent(_graphContainer, false);
                labelY.gameObject.SetActive(true);
                labelY.anchoredPosition = new Vector2(labelY.anchoredPosition.x, normalizedValue * graphHeight); 
                tmpro.text = "R$ " + Mathf.RoundToInt(_yMininumValue + (normalizedValue * (_yMaximumValue - _yMininumValue))).ToString();

                RectTransform dashY = Instantiate(_yDashTemplate);

                dashY.SetParent(_graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(dashY.anchoredPosition.x, 2.0f + (normalizedValue * graphHeight - 5f)); 
            }
        }

        for(int i = 0; i < valuesList.Count; i++)
        {
            float xPosition = _xSpaceBetweenDots + i * _xSpaceBetweenDots;
            float yPosition = (valuesList[i] - _yMininumValue) / (_yMaximumValue - _yMininumValue) * graphHeight;
            GameObject circleObj = CreateCircle(new Vector2(xPosition, yPosition), companyColor);
            
            if(lastCircleObj != null)
            {
                lastCircleObj.TryGetComponent(out RectTransform rectA);
                circleObj.TryGetComponent(out RectTransform rectB);
                CreateDotConnection(rectA.anchoredPosition, rectB.anchoredPosition, companyColor);
            }
            
            lastCircleObj = circleObj;
        }

    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, Color companyColor)
    {
        GameObject dotLine = new GameObject("dotConnection", typeof(Image));
        dotLine.transform.SetParent(_graphContainer, false);

        dotLine.TryGetComponent(out RectTransform rect);
        dotLine.TryGetComponent(out Image image);

        Vector2 direction = (dotPositionA - dotPositionB).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        image.color = companyColor;
        rect.sizeDelta = new Vector2(distance, 2.5f);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.anchoredPosition = (dotPositionA + dotPositionB) * 0.5f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void CorrectChoice()
    {
        Debug.Log("Stock Manager ouviu uma escolha correta");
    }

    private void WrongChoice()
    {
        Debug.Log("Stock Manager ouviu uma escolha errada");
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
