using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    [BoxGroup("New Email"), HorizontalLine(color: EColor.Blue)] [SerializeField] private GameObject _createContainer;

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

    public void OpenResponse(Email email)
    {
        _responseQuestion.text = email.QuestionText;

        for(int response = 0; response < email.Responses.Count; response++)
        {
            int index = response; //necessário guardar um valor fixo pra usar na lambda.
            EmailResponse responseInfos = email.Responses[index];
            _responsesBtn[index].onClick.RemoveAllListeners();
            _responsesBtn[index].onClick.AddListener(() => RespondEmail(responseInfos.IsCorrectAnswer, responseInfos.EmailText));

            TextMeshProUGUI btnText = _responsesBtn[response].GetComponentInChildren<TextMeshProUGUI>();
            if(btnText) btnText.text = email.Responses[response].TextOption;
           
        }
        
        _createContainer.SetActive(false);
        _confirmResponse.SetActive(false);
        _responseContainer.SetActive(true);
        _firstResponseEmailChoices.SetActive(true);
    }

    private void RespondEmail(bool isCorrectAnswer, string emailResponseText)
    {
        _firstResponseEmailChoices.SetActive(false);
        _confirmResponse.SetActive(true);
        EventManager.ChangeEmailTextContent(emailResponseText);

        _confirmResponseEmailBtn.onClick.RemoveAllListeners();

        if(isCorrectAnswer) 
            _confirmResponseEmailBtn.onClick.AddListener(CorrectFeedbackChoices);
        else
            _confirmResponseEmailBtn.onClick.AddListener(WrongFeedbackChoices);
    }

    private void CorrectFeedbackChoices()
    {
        EventManager.EmailIsAnswered();
        EventManager.CorrectChoice();
        ReturnChoices(false);
        gameObject.SetActive(false);
    }

    private void WrongFeedbackChoices()
    {
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

        _createContainer.SetActive(false);
        _confirmResponse.SetActive(false);
        _wrongfeedbackScreen.SetActive(false);
        _responseContainer.SetActive(true);
        _firstResponseEmailChoices.SetActive(true);
    }

}
