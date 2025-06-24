using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StockCalculation : MonoBehaviour
{
    [SerializeField] private int _rangeDownOtherCompany = 5;
    [SerializeField] private int _rangeUpOtherCompany = 5;
    private CurrentDay _currentDay;
    private int _addDayOne = 3;
    private int _loseDayOne = 5;
    private int _addDayTwo = 4;
    private int _loseDayTwo = 6;
    private int _addDayThree = 6;
    private int _loseDayThree = 10;

    void Awake()
    {
        string[] currentDayNames = SceneManager.GetActiveScene().name.Split('_');

        switch (currentDayNames[0])
        {
            case "DayOne":
                _currentDay = CurrentDay.One;
                break;
            case "DayTwo":
                _currentDay = CurrentDay.Two;
                break;
            case "DayThree":
                _currentDay = CurrentDay.Three;
                break;
        }
    }

    public void Calculate(string playerCompany, bool isCorrectChoice)
    {
        int valuePetroCais = 0;
        int valueOtherOne;
        int valueOtherTwo;
        bool oneOtherGone = false;


        GetValue(out int valueToAdd, out int valueToLose);

        if (PersistanceDataManager.Instance.StocksInfos.TryGetValue(playerCompany, out List<int> values))
        {
            int lastPetroCaisStock = values[values.Count - 1];
            valuePetroCais = isCorrectChoice ? lastPetroCaisStock + valueToAdd : lastPetroCaisStock - valueToLose;
        }

        foreach (var pair in PersistanceDataManager.Instance.StocksInfos)
        {
            string companyName = pair.Key;

            bool isPetro = companyName == playerCompany;
            int valueToSend = 0;

            if (isPetro)
            {
                valueToSend = valuePetroCais;
            }
            else
            {
                do
                {
                    valueOtherOne = Random.Range(valuePetroCais - _rangeDownOtherCompany, valuePetroCais + _rangeUpOtherCompany + 1);
                    valueOtherTwo = Random.Range(valuePetroCais - _rangeDownOtherCompany, valuePetroCais + _rangeUpOtherCompany + 1);
                } while ( valueOtherOne == valueOtherTwo || valueOtherOne == valuePetroCais || valueOtherTwo == valuePetroCais);

                valueToSend = oneOtherGone ? valueOtherOne : valueOtherTwo;
                oneOtherGone = true;
            }

            PersistanceDataManager.Instance?.AddStockValue(companyName, valueToSend);
        }
    }

    private void GetValue(out int valueToAdd, out int valueToLose)
    {
        valueToAdd = 0;
        valueToLose = 0;

        switch (_currentDay)
        {
            case CurrentDay.One:
                valueToAdd = _addDayOne;
                valueToLose = _loseDayOne;
                break;
            case CurrentDay.Two:
                valueToAdd = _addDayTwo;
                valueToLose = _loseDayTwo;
                break;
            case CurrentDay.Three:
                valueToAdd = _addDayThree;
                valueToLose = _loseDayThree;
                break;
        }
    }
}

public enum CurrentDay
{
    One, Two, Three
}
