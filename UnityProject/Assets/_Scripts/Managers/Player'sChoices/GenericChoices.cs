using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenericChoices : MonoBehaviour, IChoiceContext
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

    [SerializeField, BoxGroup("State Fluxogram")] private HistoryPartState _currentChoiceState = HistoryPartState.Part_One;
    [SerializeField, BoxGroup("State Fluxogram")] private Character _currentCharacter = Character.None;

    private Dictionary<(Character, HistoryPartState), IChoiceStateHandler> _choiceStateHandlers;
    private SO_GenericResponse _currentResponse = null;

    private void OnEnable()
    {
        _choiceStateHandlers = new();

        IChoiceStateSetup choiceSetup = _currentCharacter switch
        {
            Character.Rafael_Day_One => new Day_One_ChoiceStateSetup_Rafael(),
            Character.Raquel_Day_Two => new Day_Two_ChoiceStateSetupe_Raquel(),
            _ => null
        };

        choiceSetup?.RegisterStates(_choiceStateHandlers);

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
        if(choices == null) return;

        _currentResponse = choices;
        _responseQuestion.text = _currentResponse.QuestionText;

        for (int response = 0; response < _currentResponse.Responses.Count; response++)
        {
            int index = response; //necessário guardar um valor fixo pra usar na lambda.
            GenericResponse responseInfos = _currentResponse.Responses[index];
            _responsesBtn[index].onClick.RemoveAllListeners();
            _responsesBtn[index].onClick.AddListener(() => RespondQuestion(responseInfos, _currentResponse.ConfirmQuestionText, _currentResponse.WrongFeedbackQuestionText, _currentResponse.Responses[index].TextOption));

            TextMeshProUGUI btnText = _responsesBtn[index].GetComponentInChildren<TextMeshProUGUI>();
            if (btnText) btnText.text = _currentResponse.Responses[index].TextOption;
        }
        
        _confirmResponse.SetActive(false);
        _responseContainer.SetActive(true);
        _firstResponseEmailChoices.SetActive(true);
    }

    public void CloseResponse()
    {
        ReturnChoices();
    }

    private void RespondQuestion(GenericResponse responseInfos, string confirmFeedback, string wrongFeedbackQuestionText, string answerText)
    {
        PlayerDataAnswer answerToSave = new PlayerDataAnswer(_currentResponse.QuestionText, answerText, responseInfos.IsCorrectAnswer);
        EventManager.AnswerToSaveIsMaded(answerToSave);

        _firstResponseEmailChoices.SetActive(false);
        _responseQuestion.text = confirmFeedback;
        _confirmResponse.SetActive(true);

        _confirmResponseEmailBtn.onClick.RemoveAllListeners();

        if (responseInfos.IsCorrectAnswer)
            _confirmResponseEmailBtn.onClick.AddListener(() => CorrectFeedbackChoices(responseInfos));
        else
            _confirmResponseEmailBtn.onClick.AddListener(() => WrongFeedbackChoices(wrongFeedbackQuestionText));
    }

    private void CorrectFeedbackChoices(GenericResponse responseInfos)
    {
        if(responseInfos.HasTextToUpdate) //used exclusive on War rooms
            EventManager.SetWrResponse(responseInfos.TextToUpdate);

        EventManager.CorrectChoice();
        EventManager.GenericResponseIsMade(_currentResponse.Index);
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
        if (_choiceStateHandlers.TryGetValue((_currentCharacter, _currentChoiceState), out var handler))
        {
            handler.Handle(this);
        }
        else
        {
            Debug.LogWarning("Nenhum handler para este estado/personagem.");
        }
    }

    private void ReturnChoices()
    {
        _confirmResponse.SetActive(false);
        _wrongfeedbackScreen.SetActive(false);
        OpenResponse(_currentResponse);
    }

    public void ChangeChoiceState(HistoryPartState state)
    {
        _currentChoiceState = state;
    }
}
