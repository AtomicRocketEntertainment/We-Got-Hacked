using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailChoices : MonoBehaviour, IEmailContext
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

    [SerializeField] private HistoryPartState _currentState = HistoryPartState.Part_One;
    [SerializeField] private Character _currentCharacter = Character.None;

    
    private Dictionary<(Character, HistoryPartState), IEmailStateHandler> _stateHandlers;
    private Email _currentEmailToRespond = null;
    private bool _isResponse = false;

    private void OnEnable()
    {
        _stateHandlers = new();

        IEmailStateSetup setup = _currentCharacter switch
        {
            Character.Tiago_Day_One => new TiagoEmailDayOneStateSetup(),
            Character.Rafael_Day_One => new RafaelEmailDayOneStateSetup(),
            Character.Raquel_Day_One => new RaquelEmailDayOneStateSetup(),
            _ => null
        };

        setup?.RegisterStates(_stateHandlers);
        _rewriteResponseEmailBtn.onClick.AddListener(ReturnChoiceWithUpdate);
        _confirmWrongFeedbackBtn.onClick.AddListener(ReturnChoiceWithUpdate);
    } 

    private void OnDisable()
    {
        _rewriteResponseEmailBtn.onClick.RemoveListener(ReturnChoiceWithUpdate);
        _confirmWrongFeedbackBtn.onClick.RemoveListener(ReturnChoiceWithUpdate);
    }

    public void OpenResponse(Email email, bool isResponse)
    {
        if(email == null) return;

        _currentEmailToRespond = email;
        _isResponse = isResponse;
        _responseQuestion.text = _currentEmailToRespond.QuestionText;

        for(int response = 0; response < _currentEmailToRespond.Responses.Count; response++)
        {
            int index = response; //necessário guardar um valor fixo pra usar na lambda.
            EmailResponse responseInfos = _currentEmailToRespond.Responses[index];
            string textToPopulateBtn = _currentEmailToRespond.Responses[index].TextOption;
            _responsesBtn[index].onClick.RemoveAllListeners();
            _responsesBtn[index].onClick.AddListener(() => RespondEmail(responseInfos.IsCorrectAnswer, responseInfos.EmailText, _currentEmailToRespond.ConfirmQuestionText, _currentEmailToRespond.WrongFeedbackQuestionText, textToPopulateBtn));

            TextMeshProUGUI btnText = _responsesBtn[index].GetComponentInChildren<TextMeshProUGUI>();
            if(btnText) btnText.text = textToPopulateBtn;
           
        }
        
        _confirmResponse.SetActive(false);
        _responseContainer.SetActive(true);
        _firstResponseEmailChoices.SetActive(true);
    }

    public void CloseResponse()
    {
        ReturnChoices(false);
    }

    private void RespondEmail(bool isCorrectAnswer, string emailResponseText, string confirmFeedback, string wrongFeedbackQuestionText, string answerText)
    {
        PlayerDataAnswer answerToSave = new PlayerDataAnswer(_currentEmailToRespond.QuestionText, answerText, isCorrectAnswer);
        EventManager.AnswerToSaveIsMaded(answerToSave);

        _firstResponseEmailChoices.SetActive(false);
        _responseQuestion.text = confirmFeedback;
        _confirmResponse.SetActive(true);
        EventManager.ChangeEmailTextContent(emailResponseText);

        _confirmResponseEmailBtn.onClick.RemoveAllListeners();

        if(isCorrectAnswer) 
            _confirmResponseEmailBtn.onClick.AddListener(CorrectFeedbackChoices);
        else
            _confirmResponseEmailBtn.onClick.AddListener(() => WrongFeedbackChoices(wrongFeedbackQuestionText));
    }

    private void CorrectFeedbackChoices()
    {
        if(_isResponse)
            EventManager.EmailIsAnswered(_currentEmailToRespond.Index);
        else
            EventManager.EmailIsWriten(_currentEmailToRespond.Index);

        EventManager.CorrectChoice();
        ResponseFeedbackUpdate();
        ReturnChoices(false);
        gameObject.SetActive(false);
    }

    private void WrongFeedbackChoices(string wrongFeedbackQuestionText)
    {
        _responseQuestion.text = wrongFeedbackQuestionText;
        EventManager.WrongChoice();
        _confirmResponse.SetActive(false);
        _wrongfeedbackScreen.SetActive(true);
    }

    private void ReturnChoiceWithUpdate()
    {
        ReturnChoices(true);
    }

    private void ReturnChoices(bool shouldUpdateContent)
    {
        if(shouldUpdateContent) EventManager.ReturnEmailContent();

        _confirmResponse.SetActive(false);
        _wrongfeedbackScreen.SetActive(false);
        OpenResponse(_currentEmailToRespond, _isResponse);
    }

    private void ResponseFeedbackUpdate()
    {
        EventManager.BlockPlayerWriteEmail();
        
        if (_stateHandlers.TryGetValue((_currentCharacter, _currentState), out var handler))
        {
            handler.Handle(this);
        }
        else
        {
            Debug.LogWarning("Nenhum handler para este estado/personagem.");
        }
    }

    public void ChangeSoftwareState(HistoryPartState state)
    {
        _currentState = state;
    }
}

public enum HistoryPartState
{
    Part_One, Part_Two, Part_Three
}
