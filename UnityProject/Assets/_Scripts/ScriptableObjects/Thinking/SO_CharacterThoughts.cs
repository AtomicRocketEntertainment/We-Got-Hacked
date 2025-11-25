using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Character Thought", menuName = "Scriptable Objcts/Thoughts/Character Thought")]
public class SO_CharacterThoughts : ScriptableObject
{
    public CharacterTalking Character;

    [System.Serializable]
    public struct ThoughtEntry
    {
        public ThoughtKey key;
        [TextArea] public string thought;
    }

    public List<ThoughtEntry> thoughts;

    private Dictionary<ThoughtKey, string> _thoughtMap;

    private void OnEnable()
    {
        _thoughtMap = new Dictionary<ThoughtKey, string>();
        foreach (var entry in thoughts)
            _thoughtMap[entry.key] = entry.thought;
    }

    public string GetThought(ThoughtKey key)
    {
        return _thoughtMap.TryGetValue(key, out var value)
            ? $"<b>{Character}:</b> {value}"
            : "[Fala não definida]";
    }
}

public enum CharacterTalking
{ 
    Tiago, Rafael, Raquel, Eduardo
}


public enum ThoughtKey
{
    WrongTimeToWriteEmail,
    WrongTimeCreateTicket,
    TicketWithoutInfo,
    WrongTimeOpenTicket,
    WrongAlertOpen,
    ShutdownWrongSite,
    WrongTimeShutdownSite,
    WrongBackup,
    WrongTimeBackup,
    WrongIPOnRemotinik,
    SendMessageToPks,
    ShouldStartTheMeeting,
    ShouldntSearchOnMalwhere,
    OpenPDFFirstTime,
    OpenHackedWebsite,
    OpenKellyMessage,
    ThinkAboutIPFoundOnMalwhere,
    WrongTimeToSearchOnBinary,
    StockValueLow
}