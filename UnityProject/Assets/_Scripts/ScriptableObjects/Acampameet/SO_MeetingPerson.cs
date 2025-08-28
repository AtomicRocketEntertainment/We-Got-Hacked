using UnityEngine;

[CreateAssetMenu(fileName = "Meeting Person", menuName = "Scriptable Objcts/Meeting/Person")]
public class SO_MeetingPerson : ScriptableObject
{
    public string Name;
    public Sprite Idle;
    public Sprite Talking;
    public MeetingPersonLines Lines;
}
