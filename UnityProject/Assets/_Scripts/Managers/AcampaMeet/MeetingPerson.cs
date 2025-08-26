using UnityEngine;

public class MeetingPerson
{
    private string _name;
    private Sprite _idle;
    private Sprite _talking;
    private MeetingPersonLines _lines;

    public string Name => _name;

    public MeetingPerson(SO_MeetingPerson infos, MeetingPersonLines lines)
    {
        _name = infos.Name;
        _idle = infos.Idle;
        _talking = infos.Talking;
        _lines = lines;
    }
}
