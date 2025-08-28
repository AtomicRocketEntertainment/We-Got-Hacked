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
        _personCardInstance.OpenMicAndTalk(line.Text);
    }

    public void StopTalkin()
    {
        _personCardInstance.CloseMic();
    }

    public void InjectPersonCard(IMeetingPersonInstance cardOnCall) => _personCardInstance = cardOnCall;
}
