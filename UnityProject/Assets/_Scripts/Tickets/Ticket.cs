using System.Collections.Generic;
public class Ticket
{
    private PlaybookType _playbook;
    private string _iD;
    private string _iPOrigem;
    private string _iPDestiny;
    private string _location;
    private ImpactedDevice _deviceAttacked;
    private AlertOrigin _origin;
    private string _dateDay;
    private string _dateHour;
    private int _riskLevel;
    private SiteType _site;
    private RansomwareInformations _ransomwareInfos;
    private DataLeakInformations _dataLeakInfos;
    private List<TicketObjectives> _objectives;
    private List<TicketLog> _logs;
    private string _siemLog;
    private string _siemLogName;
    private int _currentObjective;

    public PlaybookType Playbook => _playbook;
    public string ID => _iD;
    public string IPOrigem => _iPOrigem;
    public string IPDestiny => _iPDestiny;
    public string Location => _location;
    public ImpactedDevice DeviceAttacked => _deviceAttacked;
    public AlertOrigin Origin => _origin;
    public string DateDay => _dateDay;
    public string DateHour => _dateHour;
    public int RiskLevel => _riskLevel;
    public SiteType Site => _site;
    public RansomwareInformations RansomwareInfos => _ransomwareInfos;
    public DataLeakInformations DataLeakInfos => _dataLeakInfos;

    public List<TicketObjectives> Objectives => _objectives;
    public List<TicketLog> Logs => _logs;
    public string SiemLog => _siemLog;
    public string SiemLogName => _siemLogName;

    public Ticket(SO_Ticket infos)
    {
        _playbook = infos.Playbook;
        _iD = infos.ID;
        _iPOrigem = infos.IPOrigem;
        _iPDestiny = infos.IPDestiny;
        _location = infos.Location;
        _deviceAttacked = infos.DeviceAttacked;
        _origin = infos.Origin;
        _dateDay = infos.DateDay;
        _dateHour = infos.DateHour;
        _riskLevel = infos.RiskLevel;
        _site = infos.CorrectSite;
        _ransomwareInfos = infos.RansomwareInformation;
        _dataLeakInfos = infos.DataLeakInformation;

        _objectives = new List<TicketObjectives>();
        _logs = infos.Loggs;
        _siemLog = infos.SiemLog;
        _siemLogName = infos.SiemLogName;

        foreach (var obj in infos.Objectives)
            _objectives.Add(obj.Clone());

        _currentObjective = GetObjectivesCompletedQuantity();

        if (_objectives.Count > 0)
            _objectives[_currentObjective].ShouldShow = true;
    }

    public int GetObjectivesCompletedQuantity()
    {
        int quantity = 0;
        
        foreach(TicketObjectives objective in _objectives)
        {
            if(objective.IsCompleted)
                quantity++;
        }

        return quantity;
    }

    public void ObjectiveCompleted()
    {
        _objectives[_currentObjective].IsCompleted = true;
        _currentObjective++;

        if (_currentObjective < _objectives.Count)
            _objectives[_currentObjective].ShouldShow = true;


        if(_currentObjective >= _objectives.Count)
            _currentObjective = _objectives.Count;
    }

    public bool IsCompleted => _currentObjective == _objectives.Count && _objectives[_currentObjective - 1].IsCompleted;
}
