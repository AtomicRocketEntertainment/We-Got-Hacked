using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Ticket Infos", menuName = "Scriptable Objcts/Objects Info/Ticket Info")]
public class SO_Ticket : ScriptableObject
{
    [BoxGroup("Tick Task and siemtinel's infos")] public PlaybookType Playbook;
    [BoxGroup("Tick Task and siemtinel's infos")] public string ID;
    [BoxGroup("Tick Task and siemtinel's infos")] public string IPOrigem;
    [BoxGroup("Tick Task and siemtinel's infos")] public string IPDestiny;
    [BoxGroup("Tick Task and siemtinel's infos")] public string Location;
    [BoxGroup("Tick Task and siemtinel's infos")] public ImpactedDevice DeviceAttacked;
    [BoxGroup("Tick Task and siemtinel's infos")] public AlertOrigin Origin;
    [BoxGroup("Tick Task and siemtinel's infos")] public string DateDay;
    [BoxGroup("Tick Task and siemtinel's infos")] public string DateHour;
    [BoxGroup("Tick Task and siemtinel's infos")] public int RiskLevel;

    [BoxGroup("Siemtinel's infos")] public string SiemLog;

    [BoxGroup("Desconex's infos")] public List<TicketLog> Loggs;

    [BoxGroup("Remotopia's infos")] public RemotopiaUserLogin RemotopiaUser;

    [BoxGroup("Phishing Infos"), ShowIf(nameof(IsPichacao))][Tooltip("Every ticket is Other site. Just the correct one is Sustentanbilidade")] public SiteType CorrectSite;

    [BoxGroup("Ransomware Infos")] public RansomwareInformations RansomwareInformation;

    public List<TicketObjectives> Objectives;



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


        switch (Playbook)
        {
            case PlaybookType.Pichacao:
                Origin = AlertOrigin.WAF;
                break;
            case PlaybookType.Ransomware:
                Origin = AlertOrigin.EDR;
                break;
            case PlaybookType.VazamentoDeDados:
                Debug.Log("Ainda sem definido para vazamento de dados");
                //Origin = AlertOrigin.WAF;
                break;
            case PlaybookType.Phishing:
                Origin = AlertOrigin.Antiphishing;
                break;
        }
    }

    private bool IsPichacao() => Playbook == PlaybookType.Pichacao;
    private bool IsRansomware() => Playbook == PlaybookType.Ransomware;

}

public enum PlaybookType
{
    Pichacao, Phishing, Ransomware, VazamentoDeDados
}

public enum ImpactedDevice
{
    WindowsServer, Linux, OpenBSD, FreeBSD
}

public enum AlertOrigin
{
    Firewall, EDR, IDS, WAF, Antiphishing
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

[System.Serializable]
public struct RansomwareInformations
{
    public string RansomwareName;
    public string CriptoWallet;
    public string Hash;
}

public enum Character
{
    None, Tiago_Day_One, Raquel_Day_One, Rafael_Day_One, Tiago_Day_Two, Raquel_Day_Two, Rafael_Day_Two
}
