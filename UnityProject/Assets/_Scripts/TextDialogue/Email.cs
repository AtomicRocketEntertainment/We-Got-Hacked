[System.Serializable]
public class Email
{
    public string _sender;
    public string _title;
    public string _content;

    public Email(SO_Email emailInfo)
    {
        _sender = emailInfo.Sender;
        _title = emailInfo.Title;
        _content = emailInfo.Content;
    }

    public string Sender => _sender;
    public string Title => _title;
    public string Content => _content;
}