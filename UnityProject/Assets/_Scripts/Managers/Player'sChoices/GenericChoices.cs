using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenericChoices : MonoBehaviour
{
    [BoxGroup("Response"), HorizontalLine(color: EColor.Green), Header("Screens") ] [SerializeField] private GameObject _responseContainer;
    [BoxGroup("Response")] [SerializeField] private GameObject _firstResponseEmailChoices;
    [BoxGroup("Response")] [SerializeField] private GameObject _wrongfeedbackScreen;
    [BoxGroup("Response")] [SerializeField] private GameObject _confirmResponse;
    [BoxGroup("Response"), HorizontalLine(color: EColor.Green), Header("Buttons")] [SerializeField] private Button _confirmResponseEmailBtn;
    [BoxGroup("Response")] [SerializeField] private Button _rewriteResponseEmailBtn;
    [BoxGroup("Response")] [SerializeField] private Button _confirmWrongFeedbackBtn;
    [BoxGroup("Response")] [SerializeField] private List<Button> _responsesBtn;
    [BoxGroup("Response"), HorizontalLine(color: EColor.Green), Header("Texts")] [SerializeField] private TextMeshProUGUI _responseQuestion;
    
    private HistoryPartState _currentResponseState = HistoryPartState.Part_One;
    private SO_GenericResponse _currentResponse = null;

    private void OnEnable()
    {
        _rewriteResponseEmailBtn.onClick.AddListener(ReturnChoices);
        _confirmWrongFeedbackBtn.onClick.AddListener(ReturnChoices);
    } 

    private void OnDisable()
    {
        _rewriteResponseEmailBtn.onClick.RemoveListener(ReturnChoices);
        _confirmWrongFeedbackBtn.onClick.RemoveListener(ReturnChoices);
    }

    public void OpenResponse(SO_GenericResponse choices)
    {
        _currentResponse = choices;
        _responseQuestion.text = _currentResponse.QuestionText;

        for(int response = 0; response < _currentResponse.Responses.Count; response++)
        {
            int index = response; //necessário guardar um valor fixo pra usar na lambda.
            GenericResponse responseInfos = _currentResponse.Responses[index];
            _responsesBtn[index].onClick.RemoveAllListeners();
            _responsesBtn[index].onClick.AddListener(() => RespondQuestion(responseInfos.IsCorrectAnswer, _currentResponse.ConfirmQuestionText, _currentResponse.WrongFeedbackQuestionText));

            TextMeshProUGUI btnText = _responsesBtn[response].GetComponentInChildren<TextMeshProUGUI>();
            if(btnText) btnText.text = _currentResponse.Responses[response].TextOption;
           
        }
        
        _confirmResponse.SetActive(false);
        _responseContainer.SetActive(true);
        _firstResponseEmailChoices.SetActive(true);
    }

    private void RespondQuestion(bool isCorrectAnswer, string confirmFeedback, string wrongFeedbackQuestionText)
    {
        _firstResponseEmailChoices.SetActive(false);
        _responseQuestion.text = confirmFeedback;
        _confirmResponse.SetActive(true);

        _confirmResponseEmailBtn.onClick.RemoveAllListeners();

        if(isCorrectAnswer) 
            _confirmResponseEmailBtn.onClick.AddListener(CorrectFeedbackChoices);
        else
            _confirmResponseEmailBtn.onClick.AddListener(() => WrongFeedbackChoices(wrongFeedbackQuestionText));
    }

    private void CorrectFeedbackChoices()
    {
        EventManager.CorrectChoice();
        ResponseFeedbackUpdate();
        ReturnChoices();
        gameObject.SetActive(false);
    }

    private void WrongFeedbackChoices(string wrongFeedbackQuestionText)
    {
        _responseQuestion.text = wrongFeedbackQuestionText;
        EventManager.WrongChoice();
        _confirmResponse.SetActive(false);
        _wrongfeedbackScreen.SetActive(true);
    }

    private void ResponseFeedbackUpdate()
    {
        switch(_currentResponseState)
        {
            case HistoryPartState.Part_One:
                EventManager.TicketObjectiveCompleted();
                EventManager.SpawnEmail(EmailType.LORE);
                break;
            case HistoryPartState.Part_Two:
                break;
            case HistoryPartState.Part_Three:
                break;
        }

        _currentResponseState++;
    }

    private void ReturnChoices()
    {
        _confirmResponse.SetActive(false);
        _wrongfeedbackScreen.SetActive(false);
        OpenResponse(_currentResponse);
    }

}
