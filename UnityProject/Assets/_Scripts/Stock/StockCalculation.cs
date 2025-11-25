using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StockCalculation : MonoBehaviour
{
    [SerializeField] private int _rangeDownOtherCompanyDayOne = 10;
    [SerializeField] private int _rangeUpOtherCompanyDayOne = 20;
    [SerializeField] private int _rangeDownOtherCompanyDayTwo = 15;
    [SerializeField] private int _rangeUpOtherCompanyDayTwo = 10;
    [SerializeField] private int _rangeDownOtherCompanyDayThree = 10;
    [SerializeField] private int _rangeUpOtherCompanyDayThree = 3;

    private CurrentDay _currentDay;
    private int _currentTopCap;
    private int _currentBottomCap;
    private readonly int _addDayOne = 3;
    private readonly int _loseDayOne = 5;
    private readonly int _addDayTwo = 4;
    private readonly int _loseDayTwo = 6;
    private readonly int _addDayThree = 6;
    private readonly int _loseDayThree = 10;
    private readonly int _petroMin = 10;
    private readonly int _petroMinThouth = 20;

    void Awake()
    {
        string[] currentDayNames = SceneManager.GetActiveScene().name.Split('_');

        switch (currentDayNames[0])
        {
            case "DayOne":
                _currentDay = CurrentDay.One;
                _currentTopCap = _rangeUpOtherCompanyDayOne;
                _currentBottomCap = _rangeDownOtherCompanyDayOne;
                break;
            case "DayTwo":
                _currentDay = CurrentDay.Two;
                _currentTopCap = _rangeUpOtherCompanyDayTwo;
                _currentBottomCap = _rangeDownOtherCompanyDayTwo;
                break;
            case "DayThree":
                _currentDay = CurrentDay.Three;
                _currentTopCap = _rangeUpOtherCompanyDayThree;
                _currentBottomCap = _rangeDownOtherCompanyDayThree;
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

            Debug.Log("Valor da petrocais: " + valuePetroCais);

            if (valuePetroCais <= _petroMinThouth)
                EventManager.MakePlayerThink(ThoughtKey.StockValueLow);

            if (valuePetroCais <= _petroMin)
            {
                SceneHandler.Instance.GoToGameOver();
                return;
            }
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
                    valueOtherOne = Random.Range(valuePetroCais - _currentBottomCap, valuePetroCais + _currentTopCap + 1);
                    valueOtherTwo = Random.Range(valuePetroCais - _currentBottomCap, valuePetroCais + _currentTopCap + 1);
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
