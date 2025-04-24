using System;
using System.Collections.Generic;

[Serializable]
public class Email
{
    private string _index;
    private bool _startOpen;
    private EmailType _type;
    private EmailSender _sender;
    private EventDispatcher _disptacherInfo;
    private string _title;
    private string _content;
    private bool _hasResponse;
    private string _questionText;
    private string _confirmQuestionText;
    private string _wrongFeedbackQuestionText;
    private List<EmailResponse> _responses;
    private bool _isAnswered;

    public Email(SO_Email emailInfo)
    {
        _index = emailInfo.Index;
        _startOpen = emailInfo.StartOpen;
        _type = emailInfo.Type;
        _sender = emailInfo.Sender;
        _disptacherInfo = emailInfo.DispatcherInfo;
        _title = emailInfo.Title;
        _content = emailInfo.Content;
        _hasResponse = emailInfo.HasResponse;
        _questionText = emailInfo.QuestionText;
        _confirmQuestionText = emailInfo.ConfirmQuestionText;
        _wrongFeedbackQuestionText = emailInfo.WrongFeedbackQuestionText;

        _responses = new List<EmailResponse>();
        _responses = emailInfo.Responses;
        _isAnswered = false;
        _disptacherInfo.EmailEventSended = false;
        _disptacherInfo.NormalEventSended = false;
    }

    public void AnswerEmail()
    {
        _isAnswered = true;
    }

    public void DispatchEmailEvent()
    {
        _disptacherInfo.EmailEventSended = true;
    }

    public void DispatchNormalEvent()
    {
        _disptacherInfo.NormalEventSended = true;
    }

    public string Index => _index;
    public EmailType Type => _type;
    public bool StartOpen => _startOpen;
    public EmailSender Sender => _sender;
    public EventDispatcher DisptacherInfo => _disptacherInfo;
    public string Title => _title;
    public string Content => _content;
    public bool HasResponse => _hasResponse;
    public string QuestionText => _questionText;
    public string ConfirmQuestionText => _confirmQuestionText;
    public string WrongFeedbackQuestionText => _wrongFeedbackQuestionText;

    public List<EmailResponse> Responses => _responses;
    public bool IsAnswered => _isAnswered;
    
}