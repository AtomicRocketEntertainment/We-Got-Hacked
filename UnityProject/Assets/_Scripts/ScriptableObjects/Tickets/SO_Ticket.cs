using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Ticket Infos", menuName = "Scriptable Objcts/Objects Info/Ticket Info")]
public class SO_Ticket : ScriptableObject
{
    [BoxGroup("Commum Infos")] public PlaybookType Playbook;
    [BoxGroup("Commum Infos")] public string ID;
    [BoxGroup("Commum Infos")] public string IPOrigem;
    [BoxGroup("Commum Infos")] public string IPDestiny;
    [BoxGroup("Commum Infos")] public string Location;
    [BoxGroup("Commum Infos")] public ImpactedDevice DeviceAttacked;
    [BoxGroup("Commum Infos")] public AlertOrigin Origin;
    [BoxGroup("Commum Infos")] public string DateDay;
    [BoxGroup("Commum Infos")] public string DateHour;
    [BoxGroup("Commum Infos")] public int RiskLevel;
    [BoxGroup("Commum Infos")] public List<TicketLog> Loggs;
    [BoxGroup("Commum Infos")] public string SiemLog;
    [BoxGroup("Commum Infos")] public RemotopiaUserLogin RemotopiaUser;

    public List<TicketObjectives> Objectives;

    [BoxGroup("Pichação Infos"), ShowIf(nameof(IsPichacao))][Tooltip("Every ticket is Other site. Just the correct one is Sustentanbilidade")] public SiteType CorrectSite;




    [ContextMenu("Gerar random ID")]
    private void GenerateRandomID()
    {
        ID = Random.Range(1000, 9999).ToString();
    }

    [ContextMenu("Gerar random IP")]
    private void GenerateRandomIP()
    {
        int part1 = Random.Range(1, 255);
        int part2 = Random.Range(1, 255);
        int part3 = Random.Range(1, 255);
        int part4 = Random.Range(1, 255);
        IPOrigem = $"{part1}.{part2}.{part3}.{part4}";

        part1 = Random.Range(1, 255);
        part2 = Random.Range(1, 255);
        part3 = Random.Range(1, 255);
        part4 = Random.Range(1, 255);
        IPDestiny = $"{part1}.{part2}.{part3}.{part4}";
    }

    [ContextMenu("Gerar Hora random")]
    private void GenerateRandomHour()
    {
        int hour = Random.Range(21, 24);
        int minute = Random.Range(00, 60);
        int seconds = Random.Range(00, 60);

        DateHour = $"{hour:D2}:{minute:D2}:{seconds:D2}";
    }

    void OnValidate()
    {
        if (RiskLevel > 5 || RiskLevel < 1)
        {
            Debug.LogWarning("Risco não pode ser menor que 1 e maior que 5.");
            RiskLevel = Random.Range(1, 6);
        }
    }

    private bool IsPichacao() => Playbook == PlaybookType.Pichacao;
    private bool IsRansomware() => Playbook == PlaybookType.Ransomware;

}

public enum PlaybookType
{
    Pichacao, Phishing, Ransomware, DataLeak
}

public enum ImpactedDevice
{
    WindowsServer, Linux, OpenBSD, FreeBSD
}

public enum AlertOrigin
{
    Firewall, EDR, IDS, WAF
}


[System.Serializable]
public class TicketObjectives
{
    public string Name;
    public string Text;
    public bool IsCompleted;
    public bool ShouldShow;

    public TicketObjectives Clone()
    {
        return new TicketObjectives
        {
            Name = this.Name,
            Text = this.Text,
            IsCompleted = this.IsCompleted,
            ShouldShow = this.ShouldShow
        };
    }
}

[System.Serializable]
public class TicketLog
{
    public bool IsCorrect;
    public string Log;
}

public enum Character
{
    None, Tiago_Day_One, Raquel_Day_One, Rafael_Day_One, Tiago_Day_Two, Raquel_Day_Two, Rafael_Day_Two
}
