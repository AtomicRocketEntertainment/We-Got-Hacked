using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

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
    private SiteType _site;
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
    public SiteType Site => _site;
    public List<TicketObjectives> Objectives => _objectives;

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
        _site = infos.CorrectSite;

        _objectives = new List<TicketObjectives>();

        foreach (var obj in infos.Objectives)
        {
            _objectives.Add(obj.Clone());
        }
        
        _currentObjective = GetObjectivesCompletedQuantity();
        
        UnityEngine.Debug.Log("Current Objective " + _currentObjective);
        if(_objectives.Count > 0)
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


        if(_currentObjective > _objectives.Count)
            _currentObjective = _objectives.Count;
    }
}
