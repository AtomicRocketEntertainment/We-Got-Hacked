using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailChoices : MonoBehaviour
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
    private Email _currentEmailToRespond = null;
    private bool _isResponse = false;

    private void OnEnable()
    {
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
            _responsesBtn[index].onClick.RemoveAllListeners();
            _responsesBtn[index].onClick.AddListener(() => RespondEmail(responseInfos.IsCorrectAnswer, responseInfos.EmailText, _currentEmailToRespond.ConfirmQuestionText, _currentEmailToRespond.WrongFeedbackQuestionText));

            TextMeshProUGUI btnText = _responsesBtn[response].GetComponentInChildren<TextMeshProUGUI>();
            if(btnText) btnText.text = _currentEmailToRespond.Responses[response].TextOption;
           
        }
        
        _confirmResponse.SetActive(false);
        _responseContainer.SetActive(true);
        _firstResponseEmailChoices.SetActive(true);
    }

    public void CloseResponse()
    {
        ReturnChoices(false);
    }

    private void RespondEmail(bool isCorrectAnswer, string emailResponseText, string confirmFeedback, string wrongFeedbackQuestionText)
    {
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
            EventManager.EmailIsAnswered();
        else
            EventManager.EmailIsWriten();

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
        switch(_currentResponseState)
        {
            case HistoryPartState.Part_One:
                EventManager.SpawnEmail(EmailType.SPAM);
                EventManager.SpawnEmail(EmailType.NEWS);
                break;
            case HistoryPartState.Part_Two:
                break;
            case HistoryPartState.Part_Three:
                break;
        }


        _currentResponseState++;
    }

}

public enum HistoryPartState
{
    Part_One, Part_Two, Part_Three
}
