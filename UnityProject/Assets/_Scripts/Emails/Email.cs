using System.Collections.Generic;

[System.Serializable]
public class Email
{
    private EmailSender _sender;
    private string _title;
    private string _content;
    private bool _hasResponse;
    private string _questionText;
    private List<EmailResponse> _responses;
    private bool _isAnswered;

    public Email(SO_Email emailInfo)
    {
        _sender = emailInfo.Sender;
        _title = emailInfo.Title;
        _content = emailInfo.Content;
        _hasResponse = emailInfo.HasResponse;
        _questionText = emailInfo.QuestionText;

        _responses = new List<EmailResponse>();
        _responses = emailInfo.Responses;
        _isAnswered = false;
    }

    public void AnswerEmail()
    {
        _isAnswered = true;
    }

    public EmailSender Sender => _sender;
    public string Title => _title;
    public string Content => _content;
    public bool HasResponse => _hasResponse;
    public string QuestionText => _questionText;
    public List<EmailResponse> Responses => _responses;
    public bool IsAnswered => _isAnswered;
    
}