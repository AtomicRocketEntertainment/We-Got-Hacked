using UnityEngine;

public class MeetingPerson
{
    private string _name;
    private Sprite _idle;
    private Sprite _talking;
    private MeetingPersonLines _lines;
    private IMeetingPersonInstance _personCardInstance;

    public string Name => _name;
    public Sprite Idle => _idle;

    public MeetingPerson(SO_MeetingPerson infos)
    {
        _name = infos.Name;
        _idle = infos.Idle;
        _talking = infos.Talking;
        this._lines = new MeetingPersonLines(infos.Lines.Lines);
    }

    public void StartTalking()
    {
        Line line = _lines.GetCurrentLine();
        UpCard();
        _personCardInstance.OpenMicAndTalk(line.Text);
        _lines.AvanceLine();
    }

    public void StopTalkin()
    {
        _personCardInstance.CloseMic();
    }

    private void ShowWrResponse(string text)
    {
        UpCard();
        _personCardInstance.OpenMicAndTalk(text);
    }

    private void UpCard() => _personCardInstance.GetTransform().SetAsFirstSibling();

    public bool IsLineToAnswer()
    {
        Line line = _lines.GetCurrentLine();
        return line.IsLineToAnswer;
    }

    public bool ShouldUpdateStream()
    {
        Line line = _lines.GetCurrentLine();
        return line.ShouldUpdateStream;

    }

    public Sprite GetSpriteToShow()
    {
        Line line = _lines.GetCurrentLine();
        return line.IlustrationToShow;
    }

    public void InjectPersonCard(IMeetingPersonInstance cardOnCall) => _personCardInstance = cardOnCall;
    public void SubscribeEvents() => EventManager.OnPlayerSetWrResponse += ShowWrResponse;
    public void UnsubscribeEvents() => EventManager.OnPlayerSetWrResponse -= ShowWrResponse;
}
