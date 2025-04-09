using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Ticket Infos", menuName ="Scriptable Objcts/Objects Info/Ticket Info")]
public class SO_Ticket : ScriptableObject
{
    public PlaybookType Playbook;
    public string ID;
    public string IPOrigem;
    public string IPDestiny;
    public string Location;
    public DispostiveInfos Dispositive;
    public string Date;
    public int RiskLevel;
    public List<TicketObjectives> Objectives;

    [ContextMenu("Gerar random ID")]
    private void GenerateRandomID()
    {
        ID = Random.Range(1000, 9999).ToString();
    }

    [ContextMenu("Gerar random IP")]
    private void GenerateRandomIP()
    {
        int part1 = Random.Range(100, 999);
        int part2 = Random.Range(100, 999);
        int part3 = Random.Range(100, 999);
        IPOrigem = $"{part1}.{part2}.{part3}";

        part1 = Random.Range(100, 999);
        part2 = Random.Range(100, 999);
        part3 = Random.Range(100, 999);
        IPDestiny = $"{part1}.{part2}.{part3}";
    }

    void OnValidate()
    {
        if(RiskLevel > 5 || RiskLevel < 1)
        {
            Debug.LogWarning("Risco não pode ser menor que 1 e maior que 5.");
            RiskLevel = Random.Range(1, 6);
        }
    }
}

public enum PlaybookType
{
    Pichacao, Phishing, Ransomware, DataLeak
}

public enum DispositiveType
{
    MobileAndroid, MobileIOS, DesktopLinux, DesktopWindows, DesktopApple
}

[System.Serializable]
public struct DispostiveInfos
{
    public DispositiveType Type;
    public Sprite Icon;
}

[System.Serializable]
public class TicketObjectives
{
    public string Name;
    public string Text;
    public bool IsCompleted;
    public bool ShouldShow;
}
