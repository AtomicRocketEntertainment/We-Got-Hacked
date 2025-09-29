using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Point Email Map", menuName = "Scriptable Objcts/Email/Point Email Map")]
public class SO_PointEmailMap : ScriptableObject
{

    public List<PointEmailEntry> emails;

    private Dictionary<PointEmailKey, PointEmailEntry> _pointEmailMap;

    private void OnEnable()
    {
        _pointEmailMap = new Dictionary<PointEmailKey, PointEmailEntry>();
        foreach (var entry in emails)
            _pointEmailMap[entry.key] = entry;
    }

    public PointEmailEntry GetEmail(PointEmailKey key)
    {
        _pointEmailMap.TryGetValue(key, out var value);
        return value;
    }
}

[System.Serializable]
public struct PointEmailEntry
{
    public PointEmailKey key;
    public SO_Email email;
    public bool ShouldAdvanceHistory;
    public bool SpawnOnTime;
}

public enum PointEmailKey
{
    OneTimeWrongTicketCreated,
    CorrectTicketAfterWrongCreated,
    FirstApkResponseDayTwoN1,
    SecondApkResponseDayTwoN1,
    ThirdApkResponseDayTwoN1,
    RaquelEmailAboutApkConversationWithTiago,
    LOREA02DAY3,
    LOREA04DAY3,
    LOREA05DAY3

}