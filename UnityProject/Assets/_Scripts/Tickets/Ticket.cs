using System.Collections.Generic;

public class Ticket
{
    private PlaybookType _playbook;
    private string _iD;
    private string _iPOrigem;
    private string _iPDestiny;
    private string _location;
    private DispostiveInfos _dispositive;
    private string _date;
    private int _riskLevel;
    private List<TicketObjectives> _objectives;
    private int _currentObjective;

    public PlaybookType Playbook => _playbook;
    public string ID => _iD;
    public string IPOrigem => _iPOrigem;
    public string IPDestiny => _iPDestiny;
    public string Location => _location;
    public DispostiveInfos Dispositive => _dispositive;
    public string Date => _date;
    public int RiskLevel => _riskLevel;

    public Ticket(SO_Ticket infos)
    {
        _playbook = infos.Playbook;
        _iD = infos.ID;
        _iPOrigem = infos.IPOrigem;
        _iPDestiny = infos.IPDestiny;
        _location = infos.Location;
        _dispositive = infos.Dispositive;
        _date = infos.Date;
        _riskLevel = infos.RiskLevel;
        _objectives = infos.Objectives;
        _currentObjective = 0;

        _objectives[_currentObjective].ShouldShow = true;
    }

    public void ObjectiveCompleted()
    {
        _objectives[_currentObjective].IsCompleted = true;
        _currentObjective++;

        if(_currentObjective > _objectives.Count)
            _currentObjective = _objectives.Count;
    }
}
